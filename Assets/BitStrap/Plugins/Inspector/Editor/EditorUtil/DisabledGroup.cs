using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public struct DisabledGroup : System.IDisposable
	{
		public DisabledGroup( bool disabled )
		{
			EditorGUI.BeginDisabledGroup( disabled );
		}

		public void Dispose()
		{
			EditorGUI.EndDisabledGroup();
		}
	}
}