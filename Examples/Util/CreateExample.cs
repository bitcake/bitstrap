using UnityEngine;

namespace BitStrap.Examples
{
	public class CreateExample : MonoBehaviour
	{
		public GameObject prefab;

		[Button]
		public void CreateDummyBehaviourInstanceAsChild()
		{
			if( Application.isPlaying )
			{
				// Instantiates a script in a GameObject
				Create.Behaviour<DummyBehaviour>( transform );
			}
			else
			{
				Debug.LogWarning( "In order to see Create working, please enter Play mode." );
			}
		}

		[Button]
		public void CreatePrefabInstanceAsChild()
		{
			if( Application.isPlaying )
			{
				// Instantiates a prefab
				Create.Prefab( prefab, transform );
			}
			else
			{
				Debug.LogWarning( "In order to see Create working, please enter Play mode." );
			}
		}
	}
}
