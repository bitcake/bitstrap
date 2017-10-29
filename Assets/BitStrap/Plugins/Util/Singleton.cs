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
		private static Option<T> instance = new None();

		/// <summary>
		/// The class's single global instance.
		/// </summary>
		public static Option<T> Instance
		{
			get
			{
				if( !instance.HasValue )
					instance = Object.FindObjectOfType<T>();

				return instance;
			}

			set
			{
				if( value.HasValue )
					instance = value;
			}
		}
	}
}
