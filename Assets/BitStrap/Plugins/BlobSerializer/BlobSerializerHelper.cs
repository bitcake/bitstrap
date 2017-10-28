using System.Text;
using UnityEngine;

namespace BitStrap
{
	public static class BlobSerializerHelper
	{
		public static string SerializeBlobCollection( BlobCollection collection )
		{
			var builder = new StringBuilder();
			foreach( Blob blob in collection.blobs )
			{
				foreach( BlobField value in blob.fields )
				{
					string stringValue;
					if( value.value.TryGet( out stringValue ) )
					{
						builder.Append( value.name );
						builder.Append( "=" );
						builder.AppendLine( stringValue );
					}
				}

				builder.AppendLine( "-" );
			}

			return builder.ToString();
		}

		public static BlobCollection DeserializeBlobCollection( string blob )
		{
			var collection = new BlobCollection();
			string[] lines = blob.Split( '\n' );

			ParseBlobLines( collection, lines );

			return collection;
		}

		private static void ParseBlobLines( BlobCollection collection, string[] lines )
		{
			Blob blob = new Blob();
			collection.blobs.Add( blob );

			for( int i = 0; i < lines.Length; i++ )
			{
				string line = lines[i];
				if( string.IsNullOrEmpty( line ) )
					continue;

				if( line.StartsWith( "-" ) )
				{
					blob = new Blob();
					collection.blobs.Add( blob );
				}
				else
				{
					int separatorIndex = line.IndexOf( '=' );
					if( separatorIndex >= 0 )
					{
						string name = line.Substring( 0, separatorIndex );
						string value = line.Substring( separatorIndex + 1 );

						blob.fields.Add( new BlobField( name, value ) );
					}
				}
			}
		}
	}
}