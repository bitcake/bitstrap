using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Use this attribute to make a field appear as read only in the inspector.
	///
	/// <code>
	/// <para>[ReadOnly]</para>
	/// <para>public int myIntField</para>
	/// </code>
	/// </summary>
	[System.AttributeUsage( System.AttributeTargets.Field, AllowMultiple = false )]
	public class ReadOnlyAttribute : PropertyAttribute
	{
		public bool onlyInPlaymode = false;
	}
}
