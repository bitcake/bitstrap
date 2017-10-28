using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BitStrap
{
	public sealed class JsonWebSerializer : IWebSerializer
	{
		Option<T> IWebSerializer.Deserialize<T>( string value )
		{
			try
			{
				return JsonUtility.FromJson<T>( value );
			}
			catch( System.Exception e )
			{
				Debug.LogException( e );
				return new None();
			}
		}

		Option<string> IWebSerializer.Serialize( object value )
		{
			try
			{
				return JsonUtility.ToJson( value, false );
			}
			catch( System.Exception e )
			{
				Debug.LogException( e );
				return new None();
			}
		}

		void IWebSerializer.OnBeforeRequest( UnityWebRequest request )
		{
			request.SetRequestHeader( "Content-Type", "application/json" );
		}
	}
}
