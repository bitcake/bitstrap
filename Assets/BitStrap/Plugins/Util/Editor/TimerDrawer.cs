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
			var timer = SerializedPropertyHelper.GetValue( fieldInfo, property ) as Timer;

			if( timer != null && timer.IsRunning )
			{
				GUI.color = Color.cyan;
				Rect progressPosition = position.Right( -EditorGUIUtility.labelWidth );
				progressPosition.width *= timer.Progress;
				GUI.Box( progressPosition, GUIContent.none );
				GUI.color = Color.white;
			}

			EditorGUI.PropertyField( position, lengthProperty, label );
			UnitDrawer.DrawUnit( position, "seconds", new None() );

			if( lengthProperty.floatValue < 0.0f )
			{
				lengthProperty.floatValue = 0.0f;
				property.serializedObject.ApplyModifiedProperties();
			}
		}
	}
}