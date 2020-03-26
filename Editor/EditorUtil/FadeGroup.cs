using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public struct FadeGroup : System.IDisposable
	{
		public readonly bool visible;

		public static FadeGroup Do( float value )
		{
			var visible = EditorGUILayout.BeginFadeGroup( value );
			return new FadeGroup( visible );
		}

		private FadeGroup( bool visible )
		{
			this.visible = visible;
		}

		public void Dispose()
		{
			EditorGUILayout.EndFadeGroup();
		}
	}
}