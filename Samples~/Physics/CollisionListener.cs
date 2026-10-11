using UnityEngine;
using UnityEngine.Events;

namespace BitStrap
{
	/// <summary>
	/// This class will listen to Collision events and redirect them to explicit callbacks.
	/// </summary>
	public class CollisionListener : MonoBehaviour
	{
		[System.Serializable]
		public class CollisionEvent : UnityEvent<Collision, bool>
		{
		}

		public CollisionEvent onCollide = new CollisionEvent();

		private void OnCollisionEnter( Collision collision )
		{
			onCollide.Invoke( collision, true );
		}

		private void OnCollisionExit( Collision collision )
		{
			onCollide.Invoke( collision, false );
		}
	}
}