using UnityEngine;

namespace BitStrap.Examples
{
	public class GameObjectExtensionsExample : MonoBehaviour
	{
		[Button]
		public void GetComponentInParentIncludingInactive()
		{
			transform.parent.gameObject.SetActive( false );
			Debug.LogFormat( "Found component '{0}'", gameObject.GetComponentInParent<Rigidbody>( true ) );
		}
	}
}
