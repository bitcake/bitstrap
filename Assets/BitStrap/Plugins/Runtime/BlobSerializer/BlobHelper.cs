using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace BitStrap
{
	public static class BlobHelper
	{
		private const BindingFlags FieldFlags = BindingFlags.Instance | BindingFlags.Public;

		public static Option<object> RestoreObject( BlobCollection collection, int index, System.Type type )
		{
			try
			{
				Blob blob = collection.blobs[index];
				if( blob.reference != null )
					return blob.reference;

				if( type.IsArray )
					blob.reference = System.Activator.CreateInstance( type, blob.fields.Count );
				else
					blob.reference = System.Activator.CreateInstance( type );

				for( int i = 0; i < blob.fields.Count; i++ )
				{
					BlobField value = blob.fields[i];

					string stringValue;
					if( !value.value.TryGet( out stringValue ) )
						continue;

					if( type.IsArray )
					{
						var array = blob.reference as System.Array;
						object elementValue;
						if( ParseBlobValue( collection, type.GetElementType(), stringValue ).TryGet( out elementValue ) )
							array.SetValue( elementValue, i );
					}
					else
					{
						FieldInfo field = type.GetField( value.name, FieldFlags );
						object fieldValue;
						if( ParseBlobValue( collection, field.FieldType, stringValue ).TryGet( out fieldValue ) )
							field.SetValue( blob.reference, fieldValue );
					}
				}

				return blob.reference;
			}
			catch( System.Exception e )
			{
				Debug.LogException( e );
				return Functional.None;
			}
		}

		private static Option<object> ParseBlobValue( BlobCollection collection, System.Type type, string stringValue )
		{
			if( IsValueType( type ) )
			{
				stringValue = WWW.UnEscapeURL( stringValue );
				return System.Convert.ChangeType( stringValue, type );
			}
			else
			{
				int referenceIndex = int.Parse( stringValue );
				return RestoreObject( collection, referenceIndex, type );
			}
		}

		public static int TrackObject( BlobCollection collection, object obj )
		{
			int index;
			if( !collection.FindBlobIndex( obj ).TryGet( out index ) )
			{
				var blob = new Blob();
				blob.reference = obj;

				collection.blobs.Add( blob );
				index = collection.blobs.Count - 1;

				FillBlob( collection, blob, obj );
			}

			return index;
		}

		private static void FillBlob( BlobCollection collection, Blob blob, object obj )
		{
			var type = obj.GetType();

			if( type.IsArray )
			{
				var array = obj as System.Array;
				for( int i = 0; i < array.Length; i++ )
				{
					object element = array.GetValue( i );
					blob.fields.Add( CreateBlobValue( collection, "", element ) );
				}
			}
			else
			{
				foreach( var field in type.GetFields( FieldFlags ) )
				{
					object value = field.GetValue( obj );
					blob.fields.Add( CreateBlobValue( collection, field.Name, value ) );
				}
			}
		}

		private static BlobField CreateBlobValue( BlobCollection collection, string name, object value )
		{
			if( value == null )
			{
				return new BlobField( name, Functional.None );
			}
			else if( IsValueType( value.GetType() ) )
			{
				string stringValue = WWW.EscapeURL( value.ToString() );
				return new BlobField( name, stringValue );
			}
			else
			{
				int referenceIndex = TrackObject( collection, value );
				return new BlobField( name, referenceIndex.ToString() );
			}
		}

		private static bool IsValueType( System.Type type )
		{
			return type.IsValueType || type == typeof( string );
		}
	}
}