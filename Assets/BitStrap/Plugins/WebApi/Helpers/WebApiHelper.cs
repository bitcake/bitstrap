using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace BitStrap
{
	public static class WebApiHelper
	{
		public static string BuildUrl( IWebAction action, WebActionData actionData )
		{
			var controller = action.Controller;
			var api = controller.Api;

			var uriBuilder = new StringBuilder();
			uriBuilder.Append( api.Url );

			if( !api.Url.EndsWith( "/" ) )
				uriBuilder.Append( '/' );

			uriBuilder.Append( controller.Name );

			if( !string.IsNullOrEmpty( controller.Name ) )
				uriBuilder.Append( '/' );

			uriBuilder.Append( action.Name );

			if( !string.IsNullOrEmpty( action.Name ) )
				uriBuilder.Append( '/' );

			// Url params
			object[] values = actionData.values;
			for( int i = 0; i < values.Length; i++ )
			{
				if( string.IsNullOrEmpty( action.ParamNames[i] ) )
					continue;

				string encodedValue = WWW.EscapeURL( System.Convert.ToString( values[i] ) );

				if( !string.IsNullOrEmpty( encodedValue ) )
				{
					uriBuilder.Append( encodedValue );
					uriBuilder.Append( '/' );
				}
			}

			// Remove last '/'
			uriBuilder.Remove( uriBuilder.Length - 1, 1 );

			if( values.Length > 0 )
			{
				// Get params
				if( action.Method == WebMethod.GET )
				{
					uriBuilder.Append( '?' );

					for( int i = 0; i < values.Length; i++ )
					{
						uriBuilder.Append( WWW.EscapeURL( action.ParamNames[i] ) );
						uriBuilder.Append( '=' );
						uriBuilder.Append( WWW.EscapeURL( System.Convert.ToString( values[i] ) ) );
						uriBuilder.Append( '&' );
					}

					if( uriBuilder.Length > 0 )
						uriBuilder.Remove( uriBuilder.Length - 1, 1 );
				}
			}

			return uriBuilder.ToString();
		}

		public static Option<UnityWebRequest> CreateRequest( string url, IWebAction action, WebActionData actionData, IWebSerializer serializer )
		{
			UnityWebRequest request;

			if( action.Method == WebMethod.POST )
			{
				string postData = "";
				object[] values = actionData.values;

				var form = new Dictionary<string, object>();
				for( int i = 0; i < values.Length; i++ )
					form.Add( action.ParamNames[i], values[i] );

				if( !serializer.Serialize( form ).TryGet( out postData ) )
					return new None();

				request = UnityWebRequest.Post( url, postData );
			}
			else
			{
				request = UnityWebRequest.Get( url );
			}

			serializer.OnBeforeRequest( request );

			return request;
		}
	}
}