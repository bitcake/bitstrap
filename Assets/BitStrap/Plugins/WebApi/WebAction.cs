using System.Collections.Generic;
using UnityEngine;

namespace BitStrap
{
	public sealed class WrongNumberOfParamsException : System.Exception
	{
		private string actionName;
		private string controllerName;

		public override string Message
		{
			get
			{
				string format = "Wrong number of params on WebAction \"{0}\" from WebController \"{1}\".";
				return string.Format( format, actionName, controllerName );
			}
		}

		public WrongNumberOfParamsException( IWebAction action, IWebController controller )
		{
			actionName = action.GetType().Name;
			controllerName = controller.GetType().Name;
		}
	}

	public struct WebActionData
	{
		public object[] values;
		public Option<string[]> headers;
	}

	public enum WebMethod
	{
		GET,
		POST
	}

	public interface IWebAction
	{
		WebMethod Method { get; }
		string Name { get; }
		string[] HeaderNames { get; }
		string[] ParamNames { get; }
		IWebController Controller { get; }

		System.Action<Result<string, WebError>> ConvertRequestCallback<A>( System.Action<Result<A, WebError>> callback );
	}

	public sealed class WebAction : WebAction<None>, IWebAction
	{
		public WebAction( string name, WebMethod httpMethod, string[] headerNames, string[] argNames, IWebController controller ) : base( name, httpMethod, headerNames, argNames, controller ) { }

		System.Action<Result<string, WebError>> IWebAction.ConvertRequestCallback<A>( System.Action<Result<A, WebError>> callback )
		{
			return result => callback( result.Select( text => default( A ) ) );
		}
	}

	public class WebAction<T> : IWebAction
	{
		public string Name { get; private set; }
		public WebMethod Method { get; private set; }
		public string[] HeaderNames { get; private set; }
		public string[] ParamNames { get; private set; }
		public IWebController Controller { get; private set; }

		private WebActionData data = new WebActionData();

		public WebAction( string name, WebMethod actionMethod, string[] headerNames, string[] paramNames, IWebController controller )
		{
			Name = name;
			Method = actionMethod;
			HeaderNames = headerNames;
			ParamNames = paramNames;
			Controller = controller;
		}

		public WebAction<T> Headers( params string[] headerValues )
		{
			data.headers = headerValues;
			return this;
		}

		public WebRequest<T> Request( params object[] values )
		{
			if( values == null || ParamNames.Length != values.Length )
				throw new WrongNumberOfParamsException( this, Controller );

			int headerCount = data.headers.Match(
				 some: h => h.Length,
				 none: () => 0
			);

			if( HeaderNames.Length != headerCount )
				throw new WrongNumberOfParamsException( this, Controller );

			data.values = values;

			var request = new WebRequest<T>( this, ref data );
			data = new WebActionData();

			return request;
		}

		System.Action<Result<string, WebError>> IWebAction.ConvertRequestCallback<A>( System.Action<Result<A, WebError>> callback )
		{
			return result => callback( result.AndThen( text => Controller.Api.serializer.Deserialize<A>( text ) ) );
		}
	}
}
