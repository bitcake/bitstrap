using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Put this attribute above any field
	/// It adds a small label to the right of the property field.
	/// This small label can be used to add information about the assumed unit.
	///
	/// <code>
	/// <para>[Unit( "m/s2" )]</para>
	/// <para>public float gravity = 9.80665f;</para>
	/// </code>
	/// </summary>
	[System.AttributeUsage( System.AttributeTargets.Field, AllowMultiple = true )]
	public sealed class UnitAttribute : PropertyAttribute
	{
		public string text;
		public GUIStyle style = GUI.skin.label;

		public UnitAttribute( string label )
		{
			text = label;
		}
	}
}