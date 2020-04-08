using UnityEngine;
using UnityEngine.Events;

namespace BitStrap
{
	/// <summary>
	/// This class will listen to Trigger events and redirect them to explicit callbacks.
	/// </summary>
	public class TriggerListener : MonoBehaviour
	{
		[System.Serializable]
		public class CollisionEvent : UnityEvent<Collider, bool>
		{
		}

		public CollisionEvent onCollide = new CollisionEvent();

		private void OnTriggerEnter( Collider collider )
		{
			onCollide.Invoke( collider, true );
		}

		private void OnTriggerExit( Collider collider )
		{
			onCollide.Invoke( collider, false );
		}
	}
}