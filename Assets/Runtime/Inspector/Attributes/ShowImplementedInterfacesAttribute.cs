using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Show in inspector all the interfaces this component implements.
	/// </summary>
	[System.AttributeUsage( System.AttributeTargets.Field, AllowMultiple = false )]
	public sealed class ShowImplementedInterfacesAttribute : PropertyAttribute
	{
		public System.Type type;

		public ShowImplementedInterfacesAttribute( System.Type type )
		{
			this.type = type;
		}
	}
}