using System.Reflection;

namespace BitStrap
{
	public static class TypeExtensions
	{
		public static bool IsByValue( this System.Type self )
		{
#if NETFX_CORE
			return WinRTLegacy.TypeExtensions.IsValueType( self );
#else
			return self.IsValueType;
#endif
		}

#if NETFX_CORE
		public static Option<T> GetAttribute<T>( this System.Type type, bool inherit ) where T : System.Attribute
		{
			var attribute = CustomAttributeExtensions.GetCustomAttribute<T>( type.GetTypeInfo(), inherit );
			if( attribute != null )
				return new Option<T>( attribute );
			return Option<T>.None;
		}
#endif
	}
}