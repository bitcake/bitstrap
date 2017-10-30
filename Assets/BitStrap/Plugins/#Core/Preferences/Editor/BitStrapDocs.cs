using UnityEngine;
using UnityEditor;

namespace BitStrap
{
	public static class BitStrapDocs
	{
		public const string docsUrl = "https://github.com/bitcake/bitstrap/wiki";

		[MenuItem( "Window/BitStrap/Open Web Documentation", false, 0 )]
		public static void OpenDocs()
		{
			Application.OpenURL( docsUrl );
		}
	}
}