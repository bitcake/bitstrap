using System.Collections.Generic;
using UnityEngine.Networking;

namespace BitStrap
{
	public interface IWebSerializer
	{
		Result<string, WebError> Serialize( object value );

		Result<T, WebError> Deserialize<T>( string value );

		void OnBeforeRequest( UnityWebRequest request );
	}
}
