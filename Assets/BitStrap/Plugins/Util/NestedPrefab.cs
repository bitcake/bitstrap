using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Simple nested prefabs achieved by instantiating a list of prefabs
	/// at runtime and parenting them inside of "attachTo" transform.
	/// </summary>
	public class NestedPrefab : MonoBehaviour
	{
		[SerializeField]
		private Transform attachTo;

		[SerializeField]
		private GameObject[] prefabs;

		private void Awake()
		{
			if( attachTo == null )
				attachTo = transform;

			foreach( GameObject prefab in prefabs )
			{
				if( prefab != null )
					Create.Prefab( prefab, attachTo );
			}
		}
	}
}
