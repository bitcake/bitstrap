using System.Collections.Generic;
using System.Text;

namespace BitStrap
{
	/// <summary>
	/// Bunch of utility extension methods to the generic HashSet class.
	/// These methods are intended to be System.Ling substitues as they do not generate garbage.
	/// </summary>
	public static class HashSetExtensions
	{
		public struct Iterator<T>
		{
			private HashSet<T>.Enumerator enumerator;

			public T Current
			{
				get { return enumerator.Current; }
			}

			public Iterator( HashSet<T> collection )
			{
				enumerator = collection.GetEnumerator();
			}

			public Iterator<T> GetEnumerator()
			{
				return this;
			}

			public bool MoveNext()
			{
				return enumerator.MoveNext();
			}
		}

		/// <summary>
		/// Use this method to iterate a HashSet in a foreach loop but with no garbage
		/// </summary>
		/// <example>
		/// foreach( var element in myHashSet.Iter() )
		/// {
		///     // code goes here...
		/// }
		/// </example>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static Iterator<T> Iter<T>( this HashSet<T> collection )
		{
			return new Iterator<T>( collection );
		}

		/// <summary>
		/// Behaves like System.Linq.Count however it does not generate garbage.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static int Count<T>( this HashSet<T> collection, System.Predicate<T> predicate )
		{
			if( predicate == null )
				return 0;

			int count = 0;
			for( var enumerator = collection.GetEnumerator(); enumerator.MoveNext(); )
			{
				if( predicate( enumerator.Current ) )
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
		public static bool All<T>( this HashSet<T> collection, System.Predicate<T> predicate )
		{
			if( predicate == null )
				return false;

			for( var enumerator = collection.GetEnumerator(); enumerator.MoveNext(); )
			{
				if( !predicate( enumerator.Current ) )
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
		public static bool Any<T>( this HashSet<T> collection )
		{
			return collection.Count > 0;
		}

		/// <summary>
		/// Behaves like System.Linq.Any however it does not generate garbage.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static bool Any<T>( this HashSet<T> collection, System.Predicate<T> predicate )
		{
			if( predicate == null )
				return false;

			for( var enumerator = collection.GetEnumerator(); enumerator.MoveNext(); )
			{
				if( predicate( enumerator.Current ) )
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
		public static Option<T> First<T>( this HashSet<T> collection )
		{
			var enumerator = collection.GetEnumerator();
			if( enumerator.MoveNext() )
				return new Option<T>( enumerator.Current );

			return Functional.None;
		}

		/// <summary>
		/// Behaves like System.Linq.FirstOrDefault however it does not generate garbage.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static Option<T> First<T>( this HashSet<T> collection, System.Predicate<T> predicate )
		{
			for( var enumerator = collection.GetEnumerator(); enumerator.MoveNext(); )
			{
				if( predicate( enumerator.Current ) )
					return new Option<T>( enumerator.Current );
			}

			return Functional.None;
		}

		/// <summary>
		/// Pretty format an hashset as "{ e1, e2, e3, ..., en }".
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static string ToStringFull<T>( this HashSet<T> collection )
		{
			if( collection == null )
				return "null";
			if( collection.Count <= 0 )
				return "{}";

			StringBuilder sb = new StringBuilder();

			sb.Append( "{ " );

			for( var enumerator = collection.GetEnumerator(); enumerator.MoveNext(); )
			{
				sb.Append( enumerator.Current.ToString() );
				sb.Append( ", " );
			}

			sb.Remove( sb.Length - 2, 2 );
			sb.Append( " }" );

			return sb.ToString();
		}
	}
}