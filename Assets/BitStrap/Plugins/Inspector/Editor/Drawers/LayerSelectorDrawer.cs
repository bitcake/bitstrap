using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( LayerSelectorAttribute ) )]
	public sealed class LayerSelectorDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );

			if( property.propertyType == SerializedPropertyType.Integer && !EditorApplication.isPlaying )
			{
				using( Property.Do( position, label, property ) )
				{
					property.intValue = EditorGUI.LayerField( position, label, property.intValue );
				}
			}
			else
			{
				EditorGUI.PropertyField( position, property, label );
			}
		}
	}
}