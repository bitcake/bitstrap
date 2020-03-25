using System.Collections.Generic;
using UnityEditor.Graphs;

namespace BitStrap
{
	/// <summary>
	/// Handles drawing the graph GUI.
	/// </summary>
	public sealed class EditorGraphGUI : GraphGUI
	{
		private string pasteboard;

		/// <summary>
		/// Internal method. Do not touch it.
		/// </summary>
		/// <param name="n"></param>
		public override void NodeGUI( Node n )
		{
			SelectNode( n );
			n.NodeUI( this );
			DragNodes();
		}

		protected override void CopyNodesToPasteboard()
		{
			var selectedData = new HashSet<object>();

			foreach( Node node in selection.Iter() )
			{
				var editorNode = node as EditorGraphNode;
				if( editorNode != null )
					selectedData.Add( editorNode.Data );
			}

			pasteboard = EditorGraphSerializer.Serialize( selectedData );
		}

		protected override void PasteNodesFromPasteboard()
		{
			var editorGraph = graph as EditorGraph;
			if( editorGraph == null || string.IsNullOrEmpty( pasteboard ) )
				return;

			object[] data;
			if( EditorGraphSerializer.Deserialize<object[]>( pasteboard ).TryGet( out data ) )
				editorGraph.OnCopiedNodes( data );
		}

		protected override void DuplicateNodesThroughPasteboard()
		{
			CopyNodesToPasteboard();
			PasteNodesFromPasteboard();
		}
	}
}
