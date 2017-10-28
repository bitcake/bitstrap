using UnityEngine;
using UnityEngine.Networking;

namespace BitStrap
{
	public interface IWebRequest
	{
		void RespondToResult( bool success, string text );
	}

	public sealed class WebRequest<T> : IWebRequest
	{
		private readonly IWebAction action;
		private readonly SafeAction<string> onResponse = new SafeAction<string>();
		private readonly SafeAction<string> onError = new SafeAction<string>();

		public WebRequest( IWebAction action, ref WebActionData data )
		{
			this.action = action;
			action.Controller.Api.MakeRequest( action, data, this );
		}

		public WebRequest<T> OnResponse( System.Action<T> callback )
		{
			onResponse.Register( action.ConvertRequestCallback( callback ) );
			return this;
		}

		public WebRequest<T> OnRawResponse( System.Action<string> callback )
		{
			onResponse.Register( callback );
			return this;
		}

		public WebRequest<T> OnError( System.Action<string> callback )
		{
			onError.Register( callback );
			return this;
		}

		void IWebRequest.RespondToResult( bool success, string text )
		{
			if( success )
				onResponse.Call( text );
			else if( onError.Count > 0 )
				onError.Call( text );
			else
				Debug.LogErrorFormat( "Response error: \"{0}\"", text );
		}
	}
}
