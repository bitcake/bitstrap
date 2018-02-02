using UnityEngine;

namespace BitStrap
{
	public struct BoxGroup : System.IDisposable
	{
		public static BoxGroup Do( string label )
		{
			EditorHelper.BeginBox( label );
			return new BoxGroup();
		}

		public static BoxGroup Do( ref Vector2 scroll, string label )
		{
			scroll = EditorHelper.BeginBox( scroll, label );
			return new BoxGroup();
		}

		public void Dispose()
		{
			EditorHelper.EndBox();
		}
	}
}