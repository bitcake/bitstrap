using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Use it on a UnityEngine.Object reference to restrict its assignment.
	/// </summary>
	[System.AttributeUsage( System.AttributeTargets.Field, AllowMultiple = false )]
	public sealed class RequireInterfaceAttribute : PropertyAttribute
	{
		public System.Type interfaceType;

		public RequireInterfaceAttribute( System.Type interfaceType )
		{
			this.interfaceType = interfaceType;
		}
	}
}