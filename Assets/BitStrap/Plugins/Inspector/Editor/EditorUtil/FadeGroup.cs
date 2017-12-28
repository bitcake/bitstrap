using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public struct FadeGroup : System.IDisposable
	{
		public readonly bool visible;

		public FadeGroup( float value )
		{
			visible = EditorGUILayout.BeginFadeGroup( value );
		}

		public void Dispose()
		{
			EditorGUILayout.EndFadeGroup();
		}
	}
}