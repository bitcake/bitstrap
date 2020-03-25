using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( Timer.Duration ) )]
	public sealed class TimerDurationDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );

			var lengthProperty = property.GetMemberProperty<Timer.Duration>( d => d.length );

			EditorGUI.PropertyField( position, lengthProperty, label );
			UnitDrawer.DrawUnit( position, "seconds", EditorStyles.centeredGreyMiniLabel );

			if( lengthProperty.floatValue < Mathf.Epsilon )
			{
				lengthProperty.floatValue = Mathf.Epsilon;
				property.serializedObject.ApplyModifiedProperties();
			}
		}
	}
}