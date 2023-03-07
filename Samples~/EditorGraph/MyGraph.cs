using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BitStrap.Examples
{
	/// <summary>
	/// Open the graph editor window by navigating in Unity Editor to "Window/BitStrap Examples/EditorGraph".
	/// </summary>
	public sealed class MyGraph : ScriptableObject, ISerializationCallbackReceiver
	{
		public List<MyGraphNode> nodes = new List<MyGraphNode>();

		[SerializeField]
		[HideInInspector]
		private string serialized;

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			nodes = EditorGraphSerializer.Deserialize<MyGraphNode[]>( serialized ).Match(
				some: a => a.ToList(),
				none: () => new List<MyGraphNode>()
			);
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			serialized = EditorGraphSerializer.Serialize( nodes.ToArray() );
		}
	}
}
