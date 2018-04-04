using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( UnitAttribute ) )]
	public sealed class UnitDrawer : PropertyDrawer
	{
		public static void DrawUnit( Rect fieldPosition, string unitText, GUIStyle unitStyle )
		{
			var content = new GUIContent( unitText );
			float labelWidth = unitStyle.CalcSize( content ).x;

			GUI.Label( fieldPosition.Right( labelWidth + 2.0f ), content, unitStyle );
		}

		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );
			EditorGUI.PropertyField( position, property, label );

			var unitAttribute = attribute as UnitAttribute;
			DrawUnit( position, unitAttribute.text, unitAttribute.style );
		}
	}
}