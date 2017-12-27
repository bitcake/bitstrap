using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BitStrap
{
	public struct WebError
	{
		public string message;
		public long httpCode;

		public override string ToString()
		{
			return message;
		}
	}

	public sealed class WebApi : MonoBehaviour
	{
		public static WebApi Instance { get; private set; }

		public string url;
		public bool verboseMode;

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

			if( verboseMode )
				Debug.LogFormat( "*[WebApi.Request]* [{0}] \"{1}\"\n{2}", action.Method, url, actionData.values.ToStringFull() );

			UnityWebRequest httpRequest;

			var result = WebApiHelper.CreateRequest( url, action, actionData, serializer );
			if( !result.Ok.TryGet( out httpRequest ) )
			{
				request.RespondToResult( this, result.Select( r => "" ) );
				yield break;
			}

#if UNITY_5
			yield return httpRequest.Send();

			bool success = !httpRequest.isError;
#else
	#if UNITY_2017_1
			yield return httpRequest.Send();
	#else
			yield return httpRequest.SendWebRequest();
	#endif

			bool success = !httpRequest.isNetworkError && !httpRequest.isHttpError;
#endif

			string text;
			if( success )
				text = httpRequest.downloadHandler.text;
			else
				text = httpRequest.error;

			if( verboseMode )
				Debug.LogFormat( "*[WebApi.Response]* [{0}] \"{1}\"\n{2}", action.Method, url, text );

			if( success )
				request.RespondToResult( this, new Result<string, WebError>( text ) );
			else
				request.RespondToResult(this, new Result<string, WebError>( new WebError
				{
					message = text,
					httpCode = httpRequest.responseCode
				} ) );
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
