using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BitStrap
{
	public sealed class WebError
	{
		public enum Type
		{
			Request,
			Serialization
		}

		public readonly Type type;
		public readonly string message;
		public readonly Option<long> httpCode;

		public WebError( Type type, string message, Option<long> httpCode )
		{
			this.type = type;
			this.message = message;
			this.httpCode = httpCode;
		}

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

		internal void MakeRequest<T>( WebAction<T> action, WebActionData actionData, Promise<Result<T, WebError>> responsePromise )
		{
			StartCoroutine( MakeRequestCoroutine<T>( action, actionData, responsePromise ) );
		}

		private IEnumerator MakeRequestCoroutine<T>( WebAction<T> action, WebActionData actionData, Promise<Result<T, WebError>> responsePromise )
		{
			string url = WebApiHelper.BuildUrl( action, actionData );

			if( verboseMode )
				Debug.LogFormat( "*[WebApi.Request]* [{0}] \"{1}\"\n{2}", action.Method, url, actionData.values.ToStringFull() );

			UnityWebRequest webRequest;

			var httpRequestResult = WebApiHelper.CreateRequest( url, action, actionData, serializer );
			if( !httpRequestResult.Ok.TryGet( out webRequest ) )
			{
				WebApiHelper.RespondToResult( this, action, httpRequestResult.Select( r => "" ), responsePromise );
				yield break;
			}

			yield return UnityWebRequestHelper.SendWebRequest( webRequest );
			bool success = UnityWebRequestHelper.IsSuccess( webRequest );

			string text;
			if( success )
				text = webRequest.downloadHandler.text;
			else
				text = webRequest.error;

			if( verboseMode )
				Debug.LogFormat( "*[WebApi.Response]* [{0}] \"{1}\"\n{2}", action.Method, url, text );

			if( success )
				WebApiHelper.RespondToResult(
					this,
					action,
					new Result<string, WebError>( text ),
					responsePromise );
			else
				WebApiHelper.RespondToResult(
					this,
					action,
					new Result<string, WebError>( new WebError( WebError.Type.Request, text, webRequest.responseCode ) ),
					responsePromise );
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
