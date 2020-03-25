using System.Runtime.Serialization.Formatters;

namespace BitStrap
{
	/// <summary>
	/// Handles serialization of a graph object.
	/// </summary>
	public static class EditorGraphSerializer
	{
		/// <summary>
		/// Serialize graph like data.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string Serialize( object data )
		{
			return BlobSerializer.Serialize( data );
		}

		/// <summary>
		/// Deserialize graph like json.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="text"></param>
		/// <returns></returns>
		public static Option<T> Deserialize<T>( string text )
		{
			return BlobSerializer.Deserialize<T>( text );
		}
	}
}
