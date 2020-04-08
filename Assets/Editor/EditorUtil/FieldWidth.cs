using UnityEngine;
using UnityEditor;

namespace BitStrap
{
	public struct FieldWidth : System.IDisposable
	{
		private readonly float savedFieldWidth;

		public static FieldWidth Do( float fieldWidth )
		{
			var savedFieldWidth = EditorGUIUtility.fieldWidth;
			EditorGUIUtility.fieldWidth = fieldWidth;

			return new FieldWidth( savedFieldWidth );
		}

		private FieldWidth( float savedFieldWidth )
		{
			this.savedFieldWidth = savedFieldWidth;
		}

		public void Dispose()
		{
			EditorGUIUtility.fieldWidth = savedFieldWidth;
		}
	}
}