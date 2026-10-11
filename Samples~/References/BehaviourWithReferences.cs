using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class BehaviourWithReferences : MonoBehaviour
	{
		[System.Serializable]
		public sealed class GameObjectReferences : References<GameObject>
		{
		}

		public GameObjectReferences gameObjectReferences;

		[Button]
		public void ListAllReferences()
		{
			Debug.LogFormat( "{0} references found", gameObjectReferences.references.Length );

			foreach( var reference in gameObjectReferences.references )
				Debug.Log( reference.name );
		}
	}
}