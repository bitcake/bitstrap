using UnityEngine;
using UnityEditor;

namespace BitStrap
{
	public struct ChangeLabelWidth : System.IDisposable
	{
		private readonly float savedLabelWidth;

		public ChangeLabelWidth( float labelWidth )
		{
			savedLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = labelWidth;
		}

		public void Dispose()
		{
			EditorGUIUtility.labelWidth = savedLabelWidth;
		}
	}
}