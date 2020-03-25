using System.Text;

namespace BitStrap
{
	/// <summary>
	/// Bunch of utility extension methods to the Array class.
	/// Also, it contains some System.Linq like methods that does not generate garbage.
	/// </summary>
	public static class ArrayExtensions
	{
		/// <summary>
		/// Behaves like System.Linq.Count however it does not generate garbage.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static int Count<T>( this T[] collection, System.Predicate<T> predicate )
		{
			if( predicate == null )
				return 0;

			int count = 0;
			for( int i = 0; i < collection.Length; i++ )
			{
				if( predicate( collection[i] ) )
					count++;
			}

			return count;
		}

		/// <summary>
		/// Behaves like System.Linq.All however it does not generate garbage.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static bool All<T>( this T[] collection, System.Predicate<T> predicate )
		{
			if( predicate == null )
				return false;

			for( int i = 0; i < collection.Length; i++ )
			{
				if( !predicate( collection[i] ) )
					return false;
			}

			return true;
		}

		/// <summary>
		/// Behaves like System.Linq.Any however it does not generate garbage.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static bool Any<T>( this T[] collection )
		{
			return collection.Length > 0;
		}

		/// <summary>
		/// Behaves like System.Linq.Any however it does not generate garbage.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static bool Any<T>( this T[] collection, System.Predicate<T> predicate )
		{
			if( predicate == null )
				return false;

			for( int i = 0; i < collection.Length; i++ )
			{
				if( predicate( collection[i] ) )
					return true;
			}

			return false;
		}

		/// <summary>
		/// Behaves like System.Linq.FirstOrDefault however it does not generate garbage.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static Option<T> First<T>( this T[] collection )
		{
			if( collection.Length > 0 )
				return new Option<T>( collection[0] );

			return Functional.None;
		}

		/// <summary>
		/// Behaves like System.Linq.FirstOrDefault however it does not generate garbage.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static Option<T> First<T>( this T[] collection, System.Predicate<T> predicate )
		{
			for( int i = 0; i < collection.Length; i++ )
			{
				if( predicate( collection[i] ) )
					return new Option<T>( collection[i] );
			}

			return Functional.None;
		}

		/// <summary>
		/// Pretty format an array as "[ e1, e2, e3, ..., en ]".
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static string ToStringFull<T>( this T[] collection )
		{
			if( collection == null )
				return "null";
			if( collection.Length <= 0 )
				return "[]";

			StringBuilder sb = new StringBuilder();

			sb.Append( "[ " );

			for( int i = 0; i < collection.Length - 1; i++ )
			{
				sb.Append( collection[i].ToString() );
				sb.Append( ", " );
			}

			sb.Append( collection[collection.Length - 1].ToString() );
			sb.Append( " ]" );

			return sb.ToString();
		}
	}
}
