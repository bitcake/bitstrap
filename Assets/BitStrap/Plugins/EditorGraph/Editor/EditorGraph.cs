using System.Collections.Generic;
using UnityEditor.Graphs;

namespace BitStrap
{
	/// <summary>
	/// An editor graph.
	/// </summary>
	public sealed class EditorGraph : Graph
	{
		/// <summary>
		/// The graph's controller.
		/// </summary>
		public EditorGraphController controller;

		/// <summary>
		/// Is the graph currently being created?
		/// </summary>
		public bool IsCreatingGraph { get; set; }

		/// <summary>
		/// Internal method. Do not touch it.
		/// </summary>
		public override void RemoveNode( Node node, bool destroyNode = false )
		{
			NotifyRemovedNode( node );
			base.RemoveNode( node, destroyNode );
		}

		/// <summary>
		/// Internal method. Do not touch it.
		/// </summary>
		public override void RemoveNodes( List<Node> nodesToRemove, bool destroyNodes = false )
		{
			foreach( Node node in nodesToRemove.Iter() )
				NotifyRemovedNode( node );
			base.RemoveNodes( nodesToRemove, destroyNodes );
		}

		/// <summary>
		/// Internal method. Do not touch it.
		/// </summary>
		public void OnNodeChanged( EditorGraphNode node )
		{
			if( controller != null )
				controller.OnNodeChanged( node );
		}

		/// <summary>
		/// Internal method. Do not touch it.
		/// </summary>
		public override bool CanConnect( Slot fromSlot, Slot toSlot )
		{
			bool sameNode = fromSlot.node == toSlot.node;
			bool compatibleTypes = toSlot.dataType.IsAssignableFrom( fromSlot.dataType );
			return !sameNode && compatibleTypes && base.CanConnect( fromSlot, toSlot );
		}

		/// <summary>
		/// Internal method. Do not touch it.
		/// </summary>
		public void OnCopiedNodes( object[] data )
		{
			if( controller != null )
				controller.OnCopiedNodes( data );
		}

		private void NotifyRemovedNode( Node node )
		{
			var editorNode = node as EditorGraphNode;
			if( editorNode != null && controller != null )
				controller.OnNodeRemoved( editorNode );
		}
	}
}
