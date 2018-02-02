using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public sealed class Horizontal : System.IDisposable
	{
		public readonly Rect rect;

		public static Horizontal Do( params GUILayoutOption[] options )
		{
			var rect = EditorGUILayout.BeginHorizontal( options );
			return new Horizontal( rect );
		}

		public static Horizontal Do( GUIStyle style, params GUILayoutOption[] options )
		{
			var rect = EditorGUILayout.BeginHorizontal( style, options );
			return new Horizontal( rect );
		}

		private Horizontal( Rect rect )
		{
			this.rect = rect;
		}

		public void Dispose()
		{
			EditorGUILayout.EndHorizontal();
		}
	}
}