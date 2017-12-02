using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Simple singleton class that implements the singleton code design pattern.
	/// Use it like Singleton<MyScript>.Instance.SomeMethod().
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Singleton<T> : MonoBehaviour where T : Object
	{
		private static Option<T> instance = Functional.None;

		/// <summary>
		/// The class's single global instance.
		/// </summary>
		public static Option<T> Instance
		{
			get
			{
				T singleton;
				if( !instance.TryGet( out singleton) )
				{
					singleton = Object.FindObjectOfType<T>();
					instance = singleton;
				}

				return singleton;
			}

			set
			{
				T singleton;
				if( value.TryGet( out singleton ))
					instance = singleton;
			}
		}
	}
}
