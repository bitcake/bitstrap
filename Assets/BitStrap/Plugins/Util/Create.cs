using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Bunch of prefab utility methods.
	/// Allows easy instantiation of prefabs and scripts.
	/// It is also better than regular Instantiate as it copies the prefab transform.
	/// </summary>
	public static class Create
	{
		/// <summary>
		/// Instantiates a prefab and possibly attach it to a parent transform.
		/// </summary>
		/// <param name="prefab"></param>
		/// <param name="parent"></param>
		/// <returns></returns>
		public static GameObject Prefab( GameObject prefab, Option<Transform> parent = new Option<Transform>() )
		{
			GameObject go = Object.Instantiate( prefab );

			Transform p;
			if( parent.TryGet( out p ) )
				go.transform.SetParent( p, false );

			go.transform.localPosition = prefab.transform.localPosition;
			go.transform.localRotation = prefab.transform.localRotation;
			go.transform.localScale = prefab.transform.localScale;

			return go;
		}

		/// <summary>
		/// Instantiates a script and possibly attach it to a parent transform.
		/// This will create a new GameObject and then add the script component to it.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parent"></param>
		/// <returns></returns>
		public static T Behaviour<T>( Option<Transform> parent = new Option<Transform>() ) where T : MonoBehaviour
		{
			GameObject go = new GameObject( typeof( T ).Name );

			Transform p;
			if( parent.TryGet( out p ) )
				go.transform.SetParent( p, false );

			go.transform.localPosition = Vector3.zero;
			go.transform.localRotation = Quaternion.identity;
			go.transform.localScale = Vector3.one;

			return go.AddComponent<T>();
		}

		/// <summary>
		/// Instantiates a script prefab and possibly attach it to a parent transform.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="behaviourPrefab"></param>
		/// <param name="parent"></param>
		/// <returns></returns>
		public static T Behaviour<T>( T behaviourPrefab, Option<Transform> parent = new Option<Transform>() ) where T : MonoBehaviour
		{
			GameObject go = Object.Instantiate( behaviourPrefab.gameObject );

			Transform p;
			if( parent.TryGet( out p ) )
				go.transform.SetParent( p, false );

			go.transform.localPosition = behaviourPrefab.transform.localPosition;
			go.transform.localRotation = behaviourPrefab.transform.localRotation;
			go.transform.localScale = behaviourPrefab.transform.localScale;

			return go.GetComponent<T>();
		}
	}
}
