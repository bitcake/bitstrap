using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BitStrap
{
	public sealed class WebApi : MonoBehaviour
	{
		public static WebApi Instance { get; private set; }

		public string Url;
		public bool VerboseMode;

		[System.NonSerialized]
		public IWebSerializer serializer;

		private readonly Dictionary<System.Type, IWebController> controllers = new Dictionary<System.Type, IWebController>();

		public T Controller<T>() where T : class, IWebController, new()
		{
			IWebController controller;
			if( !controllers.TryGetValue( typeof( T ), out controller ) )
			{
				controller = new T();
				controller.Setup( this );
				controllers.Add( typeof( T ), controller );
			}

			return controller as T;
		}

		internal void MakeRequest( IWebAction action, WebActionData actionData, IWebRequest request )
		{
			StartCoroutine( MakeRequestCoroutine( action, actionData, request ) );
		}

		private IEnumerator MakeRequestCoroutine( IWebAction action, WebActionData actionData, IWebRequest request )
		{
			string url = WebApiHelper.BuildUrl( action, actionData );

			if( VerboseMode )
				Debug.LogFormat( "*[WebApi.Request]* [{0}] \"{1}\"\n{2}", action.Method, url, actionData.values.ToStringFull() );

			UnityWebRequest httpRequest;
			if( !WebApiHelper.CreateRequest( url, action, actionData, serializer ).TryGet( out httpRequest ) )
				yield break;

#if UNITY_5
			yield return httpRequest.Send();

			bool success = !httpRequest.isError;
#else
			yield return httpRequest.SendWebRequest();

			bool success = !httpRequest.isNetworkError && !httpRequest.isHttpError;
#endif

			string text;

			if( success )
				text = httpRequest.downloadHandler.text;
			else
				text = httpRequest.error;

			if( VerboseMode )
				Debug.LogFormat( "*[WebApi.Response]* [{0}] \"{1}\"\n{2}", action.Method, url, text );

			request.RespondToResult( success, text );
		}

		private void Awake()
		{
			if( enabled )
				Instance = this;

			serializer = new JsonWebSerializer();
		}

		private void OnEnable()
		{
			if( enabled )
				Instance = this;
		}
	}
}
