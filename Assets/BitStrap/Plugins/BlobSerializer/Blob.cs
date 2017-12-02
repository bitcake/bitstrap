using System.Collections.Generic;

namespace BitStrap
{
	public sealed class BlobCollection
	{
		public readonly List<Blob> blobs = new List<Blob>();

		public Option<int> FindBlobIndex( object reference )
		{
			for( int i = 0; i < blobs.Count; i++ )
			{
				if( blobs[i].reference == reference )
					return i;
			}

			return Functional.None;
		}
	}

	public sealed class Blob
	{
		public readonly List<BlobField> fields = new List<BlobField>();
		public object reference;
	}

	public sealed class BlobField
	{
		public readonly string name;
		public readonly Option<string> value;

		public BlobField( string name, Option<string> value )
		{
			this.name = name;
			this.value = value;
		}
	}
}