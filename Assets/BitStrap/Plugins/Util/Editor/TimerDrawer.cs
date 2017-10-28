using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( Timer ) )]
	public class TimerDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );
			var lengthProperty = property.GetMemberProperty<Timer>( t => t.length );

			EditorGUI.PropertyField( position, lengthProperty, label );
			GUI.Label( position.Right( 52.0f ), "seconds", EditorStyles.centeredGreyMiniLabel );

			if( lengthProperty.floatValue < 0.0f )
			{
				lengthProperty.floatValue = 0.0f;
				property.serializedObject.ApplyModifiedProperties();
			}
		}
	}
}