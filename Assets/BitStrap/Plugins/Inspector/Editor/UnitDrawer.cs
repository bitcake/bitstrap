using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( UnitAttribute ) )]
	public sealed class UnitDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			UnitAttribute labelAttribute = attribute as UnitAttribute;
			EditorGUI.PropertyField( position, property, label );
			GUI.Label( position.Right( labelAttribute.width + 2f ), labelAttribute.label, labelAttribute.labelStyle );
		}
	}
}