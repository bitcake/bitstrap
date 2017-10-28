using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( RequireInterfaceAttribute ) )]
	public sealed class RequireInterfaceDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );

			var requireInterfaceAttribute = attribute as RequireInterfaceAttribute;

			if( requireInterfaceAttribute == null )
			{
				base.OnGUI( position, property, label );
				return;
			}

			EditorGUI.BeginChangeCheck();
			var objectValue = EditorGUI.ObjectField( position, label, property.objectReferenceValue, requireInterfaceAttribute.interfaceType, true );
			if( EditorGUI.EndChangeCheck() )
			{
				property.objectReferenceValue = objectValue;
				property.serializedObject.ApplyModifiedProperties();
			}
		}
	}
}