using System.Reflection;

namespace BitStrap
{
	/// <summary>
	/// Complementary methods to the System.Reflection classes.
	/// </summary>
	public static class ReflectionHelper
	{
		/// <summary>
		/// Returns the value of all fields of type.
		/// </summary>
		/// <typeparam name="TField"></typeparam>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static TField[] GetFieldValuesOfType<TField>( object instance )
		{
			return GetFieldValuesOfType<TField>( instance, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
		}

		/// <summary>
		/// Returns the value of all fields of type.
		/// </summary>
		/// <typeparam name="TField"></typeparam>
		/// <param name="instance"></param>
		/// <param name="bindingFlags"></param>
		/// <returns></returns>
		public static TField[] GetFieldValuesOfType<TField>( object instance, BindingFlags bindingFlags )
		{
			FieldInfo[] fields = instance.GetType().GetFields( bindingFlags );
			int size = fields.Count( f => typeof( TField ).IsAssignableFrom( f.FieldType ) );
			TField[] values = new TField[size];

			int i = 0;
			foreach( var field in fields )
			{
				if( typeof( TField ).IsAssignableFrom( field.FieldType ) )
					values[i++] = ( TField ) field.GetValue( instance );
			}

			return values;
		}

		/// <summary>
		/// Returns all FieldInfos inside a type that are assignable from TField.
		/// </summary>
		/// <typeparam name="TField"></typeparam>
		/// <param name="type"></param>
		/// <returns></returns>
		public static FieldInfo[] GetFieldsOfType<TField>( System.Type type )
		{
			return GetFieldsOfType<TField>( type, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
		}

		/// <summary>
		/// Returns all FieldInfos inside a type that are assignable from TField.
		/// </summary>
		/// <typeparam name="TField"></typeparam>
		/// <param name="type"></param>
		/// <param name="bindingFlags"></param>
		/// <returns></returns>
		public static FieldInfo[] GetFieldsOfType<TField>( System.Type type, BindingFlags bindingFlags )
		{
			FieldInfo[] allFields = type.GetFields( bindingFlags );
			int size = allFields.Count( f => typeof( TField ).IsAssignableFrom( f.FieldType ) );
			FieldInfo[] fields = new FieldInfo[size];

			int i = 0;
			foreach( var field in allFields )
			{
				if( typeof( TField ).IsAssignableFrom( field.FieldType ) )
					fields[i++] = field;
			}

			return fields;
		}

#if !NETFX_CORE
		/// <summary>
		/// Returns the first type's custom attribute that is of type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		/// <param name="inherit"></param>
		/// <returns></returns>
		public static Option<T> GetAttribute<T>( this System.Type type, bool inherit ) where T : System.Attribute
		{
			return from a in type.GetCustomAttributes( typeof( T ), inherit ).First() select a as T;
		}

		/// <summary>
		/// Returns the first type's custom attribute that is of type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		/// <param name="inherit"></param>
		/// <returns></returns>
		public static Option<T> GetAttribute<T>( this MemberInfo member, bool inherit ) where T : System.Attribute
		{
			return from a in member.GetCustomAttributes( typeof( T ), inherit ).First() select a as T;
		}
#endif
	}
}