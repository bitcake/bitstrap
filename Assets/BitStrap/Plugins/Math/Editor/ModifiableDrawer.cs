using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( ModifiableInt ) )]
	[CustomPropertyDrawer( typeof( ModifiableFloat ) )]
	public class ModifiableDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			// Bugged Unity... hacks :(
			if( !property.type.StartsWith( "Modifiable" ) )
				return;

			Rect labelPosition = position.Left( EditorGUIUtility.labelWidth );
			Rect fieldsPosition = position.Right( -EditorGUIUtility.labelWidth );
			Rect originalPosition = fieldsPosition.Left( fieldsPosition.width * 0.5f );
			Rect modifiedPosition = fieldsPosition.Right( -fieldsPosition.width * 0.5f );

			EditorGUI.LabelField( labelPosition, label );

			SerializedProperty originalValue = property.GetMemberProperty<ModifiableInt>( m => m.OriginalValue );
			SerializedProperty modifiedValue = property.GetMemberProperty<ModifiableInt>( m => m.ModifiedValue );

			EditorHelper.BeginChangeLabelWidth( 56.0f );
			EditorHelper.BeginChangeIndentLevel( 0 );

			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField( originalPosition, originalValue, new GUIContent( "Original" ) );
			bool modified = EditorGUI.EndChangeCheck();

			EditorGUI.BeginDisabledGroup( true );
			EditorGUI.PropertyField( modifiedPosition, modifiedValue, new GUIContent( "Modified" ) );
			EditorGUI.EndDisabledGroup();

			EditorHelper.EndChangeIndentLevel();
			EditorHelper.EndChangeLabelWidth();

			if( modified )
			{
				originalValue.serializedObject.ApplyModifiedProperties();
				modifiedValue.serializedObject.ApplyModifiedProperties();

				var modifiable = SerializedPropertyHelper.GetValue( fieldInfo, property ) as IModifiable;
				modifiable.UpdateModifiedValues();

				originalValue.serializedObject.Update();
				modifiedValue.serializedObject.Update();
			}
		}
	}
}
