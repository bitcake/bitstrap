using UnityEngine;

namespace BitStrap
{
	public struct BoxGroup : System.IDisposable
	{
		public BoxGroup( string label )
		{
			EditorHelper.BeginBox( label );
		}

		public BoxGroup( ref Vector2 scroll, string label )
		{
			scroll = EditorHelper.BeginBox( scroll, label );
		}

		public void Dispose()
		{
			EditorHelper.EndBox();
		}
	}
}