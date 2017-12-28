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
	}

	public sealed class WebAction<T> : IWebAction
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

		public Promise<Result<T, WebError>> Request( params object[] values )
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

			var responsePromise = new Promise<Result<T, WebError>>();
			Controller.Api.MakeRequest( this, data, responsePromise );

			data = new WebActionData();

			return responsePromise;
		}

		public Result<T, WebError> ConvertResult( Result<string, WebError> result )
		{
			if( typeof( T ) == typeof( string ) )
				return result.Select( text => ( T ) ( object ) text );

			return result.AndThen( text => Controller.Api.serializer.Deserialize<T>( text ) );
		}
	}
}
