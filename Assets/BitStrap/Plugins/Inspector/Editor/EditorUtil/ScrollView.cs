using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public struct ScrollView : System.IDisposable
	{
		public ScrollView( ref Vector2 scrollPosition, params GUILayoutOption[] options )
		{
			scrollPosition = EditorGUILayout.BeginScrollView( scrollPosition, options );
		}

		public ScrollView( ref Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options )
		{
			scrollPosition = EditorGUILayout.BeginScrollView( scrollPosition, alwaysShowHorizontal, alwaysShowVertical, options );
		}

		public ScrollView( ref Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options )
		{
			scrollPosition = EditorGUILayout.BeginScrollView( scrollPosition, horizontalScrollbar, verticalScrollbar, options );
		}

		public ScrollView( ref Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options )
		{
			scrollPosition = EditorGUILayout.BeginScrollView( scrollPosition, style, options );
		}

		public ScrollView( ref Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options )
		{
			scrollPosition = EditorGUILayout.BeginScrollView( scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background, options );
		}

		public void Dispose()
		{
			EditorGUILayout.EndScrollView();
		}
	}
}