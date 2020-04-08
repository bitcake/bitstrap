using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public struct IndentLevel : System.IDisposable
	{
		private readonly int savedIndentLevel;

		public static IndentLevel Do( int indentLevel )
		{
			var savedIndentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = indentLevel;

			return new IndentLevel( savedIndentLevel );
		}

		private IndentLevel( int savedIndentLevel )
		{
			this.savedIndentLevel = savedIndentLevel;
		}

		public void Dispose()
		{
			EditorGUI.indentLevel = savedIndentLevel;
		}
	}
}