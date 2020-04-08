using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public struct Vertical : System.IDisposable
	{
		public readonly Rect rect;

		public static Vertical Do( params GUILayoutOption[] options )
		{
			var rect = EditorGUILayout.BeginVertical( options );
			return new Vertical( rect );
		}

		public static Vertical Do( GUIStyle style, params GUILayoutOption[] options )
		{
			var rect = EditorGUILayout.BeginVertical( style, options );
			return new Vertical( rect );
		}

		private Vertical( Rect rect )
		{
			this.rect = rect;
		}

		public void Dispose()
		{
			EditorGUILayout.EndVertical();
		}
	}
}