using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( ModifiableInt ) )]
	[CustomPropertyDrawer( typeof( ModifiableFloat ) )]
	public sealed class ModifiableDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );

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

			bool modified;

			using( LabelWidth.Do( 56.0f ) )
			using( IndentLevel.Do( 0 ) )
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.PropertyField( originalPosition, originalValue, new GUIContent( "Original" ) );
				modified = EditorGUI.EndChangeCheck();

				using( DisabledGroup.Do( true ) )
				{
					EditorGUI.PropertyField( modifiedPosition, modifiedValue, new GUIContent( "Modified" ) );
				}
			}

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
