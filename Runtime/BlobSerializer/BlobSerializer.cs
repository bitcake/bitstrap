using UnityEngine;

namespace BitStrap
{
	public static class BlobSerializer
	{
		public static Option<T> Deserialize<T>( string blob )
		{
			BlobCollection collection = BlobSerializerHelper.DeserializeBlobCollection( blob );
			
			return from obj in BlobHelper.RestoreObject( collection, 0, typeof( T ) ) select ( T ) obj;
		}

		public static string Serialize<T>( T value )
		{
			var collection = new BlobCollection();
			BlobHelper.TrackObject( collection, value );

			return BlobSerializerHelper.SerializeBlobCollection( collection );
		}
	}
}