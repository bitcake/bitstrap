using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BitStrap
{
	public sealed class JsonWebSerializer : IWebSerializer
	{
		Result<string, WebError> IWebSerializer.Serialize( object value )
		{
			try
			{
				return JsonUtility.ToJson( value, false );
			}
			catch( System.Exception e )
			{
				return new WebError( WebError.Type.Serialization, string.Format( "Could not serialize \"{0}\". Exception: {1}", value.GetType().Name, e ), Functional.None );
			}
		}

		Result<T, WebError> IWebSerializer.Deserialize<T>( string value )
		{
			try
			{
				return JsonUtility.FromJson<T>( value );
			}
			catch( System.Exception e )
			{
				return new WebError( WebError.Type.Serialization, string.Format( "Could not deserialize \"{0}\" into a {1}. Exception:\n{2}", value, typeof( T ).Name, e ), Functional.None );
			}
		}

		void IWebSerializer.OnBeforeRequest( UnityWebRequest request )
		{
			request.SetRequestHeader( "Content-Type", "application/json" );
		}
	}
}
