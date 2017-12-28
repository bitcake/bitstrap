using UnityEngine;
using UnityEngine.Networking;

namespace BitStrap
{
	public static class UnityWebRequestHelper
	{
		public static AsyncOperation SendWebRequest( UnityWebRequest webRequest )
		{
#if UNITY_5 || UNITY_2017_1
			return webRequest.Send();
#else
			return webRequest.SendWebRequest();
#endif
		}

		public static bool IsSuccess( UnityWebRequest webRequest )
		{
#if UNITY_5
			return !webRequest.isError;
#else
			return !webRequest.isNetworkError && !webRequest.isHttpError;
#endif
		}
	}
}