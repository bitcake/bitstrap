using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class BlobSerializerExample : MonoBehaviour
	{
		public sealed class OtherData
		{
			public string anotherText = "- hey hey = bye bye";

			public override string ToString()
			{
				return string.Format( "anotherText='{0}'", anotherText );
			}
		}

		public sealed class ExampleData
		{
			public int integer = 17;
			public float number = 34.8f;
			public bool boolean = true;
			public string text = "hello\nworld";
			public string[] abc = new string[] { "a", "b", "c" };

			public OtherData[] otherData;
			public ExampleData otherExample;
		}

		[Button]
		public void Example()
		{
			var others = new OtherData[] { new OtherData(), new OtherData() };
			var data = new ExampleData();
			data.otherData = others;
			data.otherExample = new ExampleData();
			data.otherExample.otherData = others;
			data.otherExample.otherExample = data;

			string blob = BlobSerializer.Serialize( data );
			Debug.Log( "Blob:\n" + blob );

			ExampleData deserializedData;
			if( BlobSerializer.Deserialize<ExampleData>( blob ).TryGet( out deserializedData ) )
			{
				Debug.LogFormat( "integer = {0}", deserializedData.integer );
				Debug.LogFormat( "number = {0}", deserializedData.number );
				Debug.LogFormat( "boolean = {0}", deserializedData.boolean );
				Debug.LogFormat( "text = {0}", deserializedData.text );
				Debug.LogFormat( "abc = {0}", deserializedData.abc.ToStringFull() );

				Debug.LogFormat( "other = {0}", deserializedData.otherData.ToStringFull() );

				Debug.LogFormat( "OHTER integer = {0}", deserializedData.otherExample.integer );
				Debug.LogFormat( "OHTER number = {0}", deserializedData.otherExample.number );
				Debug.LogFormat( "OHTER boolean = {0}", deserializedData.otherExample.boolean );
				Debug.LogFormat( "OHTER text = {0}", deserializedData.otherExample.text );
				Debug.LogFormat( "OHTER abc = {0}", deserializedData.otherExample.abc.ToStringFull() );

				Debug.LogFormat( "OHTER other = {0}", deserializedData.otherExample.otherData.ToStringFull() );
			}
		}
	}
}