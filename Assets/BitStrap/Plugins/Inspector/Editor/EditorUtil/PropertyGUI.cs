using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public struct PropertyGUI : System.IDisposable
	{
		public PropertyGUI( Rect totalPosition, GUIContent label, SerializedProperty property )
		{
			EditorGUI.BeginProperty( totalPosition, label, property );
		}

		public void Dispose()
		{
			EditorGUI.EndProperty();
		}
	}
}