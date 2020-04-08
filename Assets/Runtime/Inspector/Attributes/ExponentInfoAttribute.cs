using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Put this attribute above a number field (int, long, float, double)
	/// It adds a small label to the right of the property field.
	/// This small label shows the number in scientific notation
	///
	/// <code>
	/// <para>// This results in 1E+09</para>
	/// <para>[ExponentInfo]</para>
	/// <para>public float largeNumber = 1000000000f;</para>
	/// </code>
	/// </summary>
	[System.AttributeUsage( System.AttributeTargets.Field, AllowMultiple = true )]
	public sealed class ExponentInfoAttribute : PropertyAttribute
	{
		public GUIStyle style = GUI.skin.label;
	}
}