using System.Text;
using UnityEngine;

namespace BitStrap
{
	public static class Slice
	{
		public static void ForceAdd<T>( ref Slice<T> self, T element )
		{
			if( self.endIndex >= self.array.Length )
			{
				var newArray = new T[Mathf.NextPowerOfTwo( self.array.Length + 1 )];
				System.Array.Copy( self.array, newArray, self.array.Length );

				self = new Slice<T>( newArray, self.startIndex, self.endIndex );
			}

			self.Add( element );
		}

		public static void SoftCopy<T>( Slice<T> source, ref Slice<T> destination )
		{
			var count = Mathf.Min( source.Count, destination.array.Length - destination.startIndex );
			System.Array.Copy( source.array, source.startIndex, destination.array, destination.startIndex, count );
			destination.Count = count;
		}
	}

	public struct Slice<T>
	{
		public readonly T[] array;
		public int startIndex;
		public int endIndex;

		public int Count
		{
			get { return endIndex - startIndex; }
			set { endIndex = startIndex + value; }
		}

		public T Get( int index )
		{
			return array[startIndex + index];
		}

		public void Set( int index, T value )
		{
			array[startIndex + index] = value;
		}

		public Slice( T[] array, int startIndex, int endIndex )
		{
			this.array = array;
			this.startIndex = startIndex;
			this.endIndex = endIndex;
		}

		public Slice( T[] array, int startIndex )
		{
			this.array = array;
			this.startIndex = startIndex;
			this.endIndex = startIndex;
		}

		public bool Add( T element )
		{
			if( endIndex < array.Length )
			{
				array[endIndex] = element;
				endIndex++;

				return true;
			}

			return false;
		}

		public string ToStringFull()
		{
			if( array == null )
				return "null";
			if( array.Length <= 0 )
				return "[]";

			var sb = new StringBuilder();

			sb.Append( "[ " );

			for( var i = startIndex; i < endIndex - 1; i++ )
			{
				sb.Append( array[i].ToString() );
				sb.Append( ", " );
			}

			sb.Append( array[endIndex - 1].ToString() );
			sb.Append( " ]" );

			return sb.ToString();
		}
	}
}