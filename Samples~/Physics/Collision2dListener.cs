using UnityEngine;
using UnityEngine.Events;

namespace BitStrap
{
	/// <summary>
	/// This class will listen to Collision2D events and redirect them to explicit callbacks.
	/// </summary>
	public class Collision2dListener : MonoBehaviour
	{
		[System.Serializable]
		public class CollisionEvent : UnityEvent<Collision2D, bool>
		{
		}

		public CollisionEvent onCollide = new CollisionEvent();

		private void OnCollisionEnter2D( Collision2D collision )
		{
			onCollide.Invoke( collision, true );
		}

		private void OnCollisionExit2D( Collision2D collision )
		{
			onCollide.Invoke( collision, false );
		}
	}
}