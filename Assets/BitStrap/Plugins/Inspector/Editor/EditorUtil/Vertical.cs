using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public struct Vertical : System.IDisposable
	{
		public readonly Rect rect;

		public Vertical( params GUILayoutOption[] options )
		{
			rect = EditorGUILayout.BeginVertical( options );
		}

		public Vertical( GUIStyle style, params GUILayoutOption[] options )
		{
			rect = EditorGUILayout.BeginVertical( style, options );
		}

		public void Dispose()
		{
			EditorGUILayout.EndVertical();
		}
	}
}