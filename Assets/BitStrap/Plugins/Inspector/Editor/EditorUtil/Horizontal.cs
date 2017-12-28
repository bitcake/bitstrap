using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public struct Horizontal : System.IDisposable
	{
		public readonly Rect rect;

		public Horizontal( params GUILayoutOption[] options )
		{
			rect = EditorGUILayout.BeginHorizontal( options );
		}

		public Horizontal( GUIStyle style, params GUILayoutOption[] options )
		{
			rect = EditorGUILayout.BeginHorizontal( style, options );
		}

		public void Dispose()
		{
			EditorGUILayout.EndHorizontal();
		}
	}
}