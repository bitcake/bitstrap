using UnityEngine;

namespace BitStrap.Examples
{
	public class ComponentExtensionsExample : MonoBehaviour
	{
		[Button]
		public void GetComponentInParentIncludingInactive()
		{
			transform.parent.gameObject.SetActive( false );
			Debug.LogFormat( "Found component '{0}'", this.GetComponentInParent<Rigidbody>( true ) );
		}
	}
}
