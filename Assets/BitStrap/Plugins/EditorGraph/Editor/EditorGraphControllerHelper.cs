using UnityEditor.Graphs;

namespace BitStrap
{
	/// <summary>
	/// Helper methods to deal with an editor graph.
	/// </summary>
	public static class EditorGraphControllerHelper
	{
		/// <summary>
		/// Automatiaclly connect nodes from its serialized state.
		/// </summary>
		/// <param name="graph"></param>
		public static void ConnectNodes( Graph graph )
		{
			foreach( Node n in graph.nodes )
			{
				var outputNode = n as EditorGraphNode;
				if( outputNode == null )
					continue;

				foreach( var property in outputNode.outputs )
				{
					object value = property.GetValue( outputNode, new object[0] );
					Slot inputSlot = FindInputSlot( graph, value );
					Slot outputSlot = outputNode[property.Name];

					if( inputSlot != null && outputSlot != null )
						graph.Connect( outputSlot, inputSlot );
				}
			}
		}

		private static Slot FindInputSlot( Graph graph, object outputValue )
		{
			foreach( Node n in graph.nodes )
			{
				var inputNode = n as EditorGraphNode;
				if( inputNode == null )
					continue;

				foreach( var property in inputNode.inputs )
				{
					object value = property.GetValue( inputNode, new object[0] );
					if( value == outputValue )
						return inputNode[property.Name];
				}
			}

			return null;
		}
	}
}
