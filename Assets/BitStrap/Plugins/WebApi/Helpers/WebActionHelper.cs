using System.Reflection;
using UnityEngine;

namespace BitStrap
{
	public class WebHeadersAttribute : System.Attribute
	{
		public readonly string[] headerNames;

		public WebHeadersAttribute( params string[] headerNames )
		{
			this.headerNames = headerNames;
		}
	}

	public class WebActionAttribute : System.Attribute
	{
		public readonly WebMethod httpMethod;
		public readonly string[] paramNames;

		public WebActionAttribute( WebMethod httpMethod, params string[] paramNames )
		{
			this.httpMethod = httpMethod;
			this.paramNames = paramNames;
		}
	}

	public static class WebActionHelper
	{
		public static void InitWebActionField( FieldInfo field, IWebController controller )
		{
			string actionName = field.GetAttribute<WebUrlAttribute>( false ).Match(
				some: a => a.url,
				none: () => field.Name
			);

			WebMethod actionMethod;
			string[] paramNames;

			WebActionAttribute attribute;
			if( !field.GetAttribute<WebActionAttribute>( false ).TryGet( out attribute ) )
				return;

			actionMethod = attribute.httpMethod;
			paramNames = attribute.paramNames;

			string[] headerNames = field.GetAttribute<WebHeadersAttribute>( false ).Match(
				some: a => a.headerNames,
				none: () => new string[0]
			);

			var action = System.Activator.CreateInstance( field.FieldType, actionName, actionMethod, headerNames, paramNames, controller ) as IWebAction;
			if( action != null )
				field.SetValue( controller, action );
		}
	}

}