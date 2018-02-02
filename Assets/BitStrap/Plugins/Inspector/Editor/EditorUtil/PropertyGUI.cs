using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public struct PropertyGUI : System.IDisposable
	{
		public static PropertyGUI Do( Rect totalPosition, GUIContent label, SerializedProperty property )
		{
			EditorGUI.BeginProperty( totalPosition, label, property );
			return new PropertyGUI();
		}

		public void Dispose()
		{
			EditorGUI.EndProperty();
		}
	}
}