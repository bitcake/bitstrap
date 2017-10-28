using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( LayerSelectorAttribute ) )]
	public sealed class LayerSelectorDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			if( property.propertyType == SerializedPropertyType.Integer && !EditorApplication.isPlaying )
			{
				EditorGUI.BeginProperty( position, label, property );
				property.intValue = EditorGUI.LayerField( position, label, property.intValue );
				EditorGUI.EndProperty();
			}
			else
			{
				EditorGUI.PropertyField( position, property, label );
			}
		}
	}
}