using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( TagSelectorAttribute ) )]
	public sealed class TagSelectorDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );

			if( property.propertyType == SerializedPropertyType.String )
			{
				using( Property.Do( position, label, property ) )
				{
					property.stringValue = EditorGUI.TagField( position, label, property.stringValue );
				}
			}
			else
			{
				EditorGUI.PropertyField( position, property, label );
			}
		}
	}
}