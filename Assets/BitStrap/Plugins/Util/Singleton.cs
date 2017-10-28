using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Simple singleton class that implements the singleton code design pattern.
	/// Use it by inheriting from this class, using T as the class itself.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T instance = null;
		private static bool applicationQuitting = false;

		/// <summary>
		/// The class's single instance.
		/// </summary>
		public static T Instance
		{
			get { return GetInstance( true ); }
		}

		/// <summary>
		/// Executes the callback passing the instance if there is one.
		/// </summary>
		/// <param name="callback"></param>
		public static void RequireInstance( System.Action<T> callback )
		{
			T inst = GetInstance( false );
			if( inst != null && callback != null )
				callback( inst );
		}

		/// <summary>
		/// Returns the class's single instance.
		/// </summary>
		/// <param name="warnIfNotFound">Triggers a warning if the instance is missing
		/// and the application is not quitting</param>
		/// <returns>The single instance of the type T or null if it is not existent</returns>
		public static T GetInstance( bool warnIfNotFound )
		{
			if( instance == null )
			{
				instance = Object.FindObjectOfType<T>();
				if( instance == null && warnIfNotFound && !applicationQuitting )
					OnInstanceNotFound();
			}

			return instance;
		}

		protected void ForceSingletonInstance()
		{
			instance = this as T;
		}

		protected virtual void OnDestroy()
		{
			instance = null;
		}

		protected virtual void OnApplicationQuit()
		{
			applicationQuitting = true;
		}

		private static void OnInstanceNotFound()
		{
			Debug.LogWarningFormat( "Didn't find an object of type {0}!", typeof( T ).Name );
		}
	}
}
