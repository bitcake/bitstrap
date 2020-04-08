using UnityEngine;
using UnityEngine.Events;

namespace BitStrap
{
	/// <summary>
	/// This class will listen to Trigger2D events and redirect them to explicit callbacks.
	/// </summary>
	public class Trigger2dListener : MonoBehaviour
	{
		[System.Serializable]
		public class CollisionEvent : UnityEvent<Collider2D, bool>
		{
		}

		public CollisionEvent onCollide = new CollisionEvent();

		private void OnTriggerEnter2D( Collider2D collider )
		{
			onCollide.Invoke( collider, true );
		}

		private void OnTriggerExit2D( Collider2D collider )
		{
			onCollide.Invoke( collider, false );
		}
	}
}