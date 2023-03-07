using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Utility class to ease getting a reference to a component when you don't have a direct way to do it.
	/// E.g. getting a player component located on a different object (in the same hierarchy) than where a collision event happened.
	/// </summary>
	public sealed class ComponentReference : MonoBehaviour
	{
		/// <summary>
		/// The component referenced.
		/// </summary>
		public Component reference;

		/// <summary>
		/// Get the component reference given the gameobject where the ComponentReference is located.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static T Get<T>( GameObject obj ) where T : MonoBehaviour
		{
			var componentReference = obj.GetComponent<ComponentReference>();
			return Get( componentReference, obj ) as T;
		}

		/// <summary>
		/// Get the component reference given a component whose gameobject has the ComponentReference.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="component"></param>
		/// <returns></returns>
		public static T Get<T>( Component component ) where T : MonoBehaviour
		{
			var componentReference = component.GetComponent<ComponentReference>();
			return Get( componentReference, component ) as T;
		}

		private static Component Get( ComponentReference componentReference, object obj )
		{
			return ( obj != null && componentReference != null ) ? componentReference.reference : null;
		}
	}
}