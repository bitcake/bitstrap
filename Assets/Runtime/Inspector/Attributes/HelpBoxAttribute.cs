using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// Use this attribute to draw a EditorGUI.HelpBox above your field.
    ///
    /// <code>
    /// <para>[HelpBox( "This is a warning", HelpBox.MessageType.Warning )]</para>
    /// <para>public int myIntField</para>
    /// </code>
    /// </summary>
    [System.AttributeUsage( System.AttributeTargets.Field, AllowMultiple = true )]
    public class HelpBoxAttribute : PropertyAttribute
    {
        public enum MessageType
        {
            None,
            Info,
            Warning,
            Error
        }

        public string message;
        public MessageType messageType;

        public HelpBoxAttribute( string message, MessageType messageType = MessageType.None )
        {
            this.message = message;
            this.messageType = messageType;
        }
    }
}
