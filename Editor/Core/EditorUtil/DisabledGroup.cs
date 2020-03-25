using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public struct DisabledGroup : System.IDisposable
	{
		public static DisabledGroup Do( bool disabled )
		{
			EditorGUI.BeginDisabledGroup( disabled );
			return new DisabledGroup();
		}

		public void Dispose()
		{
			EditorGUI.EndDisabledGroup();
		}
	}
}