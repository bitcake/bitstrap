using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( HelpBoxAttribute ) )]
	public sealed class HelpBoxDecorator : DecoratorDrawer
	{
		public override float GetHeight()
		{
			return 44.0f;
		}

		public override void OnGUI( Rect position )
		{
			var helpBoxAttribute = attribute as HelpBoxAttribute;
			switch( helpBoxAttribute.messageType )
			{
			default:
			case HelpBoxAttribute.MessageType.None:
				EditorGUI.HelpBox( position, helpBoxAttribute.message, MessageType.None );
				break;

			case HelpBoxAttribute.MessageType.Info:
				EditorGUI.HelpBox( position, helpBoxAttribute.message, MessageType.Info );
				break;

			case HelpBoxAttribute.MessageType.Warning:
				EditorGUI.HelpBox( position, helpBoxAttribute.message, MessageType.Warning );
				break;

			case HelpBoxAttribute.MessageType.Error:
				EditorGUI.HelpBox( position, helpBoxAttribute.message, MessageType.Error );
				break;
			}
		}
	}
}