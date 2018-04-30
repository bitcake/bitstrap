using UnityEngine;
using UnityEditor;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( ReferencesBase ), true )]
	public sealed class ReferencesDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
		{
			var referencesBase = SerializedPropertyHelper.GetValue( fieldInfo, property ) as ReferencesBase;
			if( referencesBase.ContainsNullReference )
				return EditorGUIUtility.singleLineHeight * 3.0f;

			return EditorGUIUtility.singleLineHeight * 2.0f;
		}

		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );

			var referencesBase = SerializedPropertyHelper.GetValue( fieldInfo, property ) as ReferencesBase;

			Rect labelRect;
			Rect referenceCountRect = position.Row( 0 )
				.Left( EditorGUIUtility.labelWidth, out labelRect );

			Rect buttonRect;
			Rect rootFolderRect = position.Row( 1 ).Right( -14.0f )
				.Right( 108.0f, out buttonRect );

			if( referencesBase.ContainsNullReference )
			{
				Rect warningRect = position.Row( 2 ).Right( -14.0f );
				GUI.Box( warningRect, GUIContent.none );
				var style = new GUIStyle( EditorStyles.centeredGreyMiniLabel );
				style.normal.textColor = Color.red;
				GUI.Label( warningRect, "Null references found! Please, update the references.", style );
			}

			EditorGUI.LabelField( labelRect, label );

			string referenceCountLabel = string.Format( "{0} references of <{1}>", referencesBase.ReferenceCount, referencesBase.ReferencedType.Name );
			EditorGUI.LabelField( referenceCountRect, referenceCountLabel, EditorStyles.centeredGreyMiniLabel );

			using( LabelWidth.Do( 72.0f ) )
			{
				SerializedProperty rootFolderProperty = property.GetMemberProperty<ReferencesBase>( r => r.rootFolder );
				EditorGUI.PropertyField( rootFolderRect, rootFolderProperty );
			}

			if( GUI.Button( buttonRect, "Update References", EditorStyles.miniButton ) )
			{
				referencesBase.UpdateReferences();
				property.serializedObject.ApplyModifiedProperties();
			}
		}
	}
}