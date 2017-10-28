using System.Collections.Generic;
using UnityEngine.Networking;

namespace BitStrap
{
	public interface IWebSerializer
	{
		Option<string> Serialize( object value );

		Option<T> Deserialize<T>( string value );

		void OnBeforeRequest( UnityWebRequest request );
	}
}
