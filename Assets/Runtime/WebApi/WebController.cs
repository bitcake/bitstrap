using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BitStrap
{
	[System.AttributeUsage( System.AttributeTargets.Class | System.AttributeTargets.Field | System.AttributeTargets.Method, AllowMultiple = false, Inherited = true )]
	public sealed class WebUrlAttribute : System.Attribute
	{
		public readonly string url;

		public WebUrlAttribute( string uri )
		{
			this.url = uri;
		}
	}

	public interface IWebController
	{
		string Name { get; }
		WebApi Api { get; }

		void Setup( WebApi api );
	}

	public class WebController<T> : IWebController where T : class, IWebController, new()
	{
		public static T Instance
		{
			get { return WebApi.Instance.Controller<T>(); }
		}

		public string Name { get; private set; }
		public WebApi Api { get; private set; }

		public WebController()
		{
			Init();
		}

		void IWebController.Setup( WebApi api )
		{
			Api = api;
		}

		private void Init()
		{
			Name = GetType().GetAttribute<WebUrlAttribute>( true ).Match(
				some: attribute =>
				{
					return attribute.url;
				},
				none: () =>
				{
					const string stripFromName = "Controller";

					string name = GetType().Name;
					if( name.EndsWith( stripFromName ) )
						name = name.Substring( 0, name.LastIndexOf( stripFromName ) );

					return name;
				}
			);

			FieldInfo[] fields = GetType().GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );

			foreach( var field in fields )
			{
				if( typeof( IWebAction ).IsAssignableFrom( field.FieldType ) )
					WebActionHelper.InitWebActionField( field, this );
			}
		}
	}
}
