using UnityEngine;
using UnityEngine.Networking;

namespace BitStrap
{
	public interface IWebRequest
	{
		void RespondToResult( WebApi api, Result<string, WebError> result );
	}

	public sealed class WebRequest<T> : IWebRequest
	{
		private readonly IWebAction action;
		private readonly SafeAction<Result<string, WebError>> onResponse = new SafeAction<Result<string, WebError>>();

		public WebRequest( IWebAction action, ref WebActionData data )
		{
			this.action = action;
			action.Controller.Api.MakeRequest( action, data, this );
		}

		public WebRequest<T> OnResponse( System.Action<Result<T, WebError>> callback )
		{
			onResponse.Register( action.ConvertRequestCallback( callback ) );
			return this;
		}

		public WebRequest<T> OnRawResponse( System.Action<Result<string, WebError>> callback )
		{
			onResponse.Register( callback );
			return this;
		}

		void IWebRequest.RespondToResult( WebApi api, Result<string, WebError> result )
		{
			onResponse.Call( result );

			WebError error;
			if( api.verboseMode && result.Error.TryGet( out error ) )
				Debug.LogErrorFormat( "Response error: \"{0}\"", error );
		}
	}
}
