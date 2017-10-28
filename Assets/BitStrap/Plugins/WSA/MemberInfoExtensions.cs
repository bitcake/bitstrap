using System.Reflection;
using UnityEngine;

namespace BitStrap
{
	public static class MemberInfoExtensions
	{
#if NETFX_CORE
		public static Option<T> GetAttribute<T>( this MemberInfo member, bool inherit ) where T : System.Attribute
		{
			var attribute = CustomAttributeExtensions.GetCustomAttribute<T>( member, inherit );
			if( attribute != null )
				return new Option<T>( attribute );
			return Option<T>.None;
		}
#endif
	}
}