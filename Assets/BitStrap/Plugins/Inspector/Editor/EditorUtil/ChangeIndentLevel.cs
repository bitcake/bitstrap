using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public struct ChangeIndentLevel : System.IDisposable
	{
		private readonly int savedIndentLevel;

		public ChangeIndentLevel( int indentLevel )
		{
			savedIndentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = indentLevel;
		}

		public void Dispose()
		{
			EditorGUI.indentLevel = savedIndentLevel;
		}
	}
}