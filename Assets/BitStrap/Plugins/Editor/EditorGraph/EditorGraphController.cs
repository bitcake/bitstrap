using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// The graph controller.
	/// Here goes the main GUI logic.
	/// Inherit from it to define your custom logic.
	/// </summary>
	public class EditorGraphController
	{
		private EditorGraph graph;
		private EditorGraphGUI graphGUI;

		private Dictionary<System.Type, System.Type> typeNodeMap = new Dictionary<System.Type, System.Type>();
		private bool graphNeedsUpdate = false;

		/// <summary>
		/// Called when a node is removed.
		/// </summary>
		/// <param name="node"></param>
		public virtual void OnNodeRemoved( EditorGraphNode node )
		{
		}

		/// <summary>
		/// Called when a node is changed.
		/// Mainly edge connection events.
		/// </summary>
		/// <param name="node"></param>
		public virtual void OnNodeChanged( EditorGraphNode node )
		{
		}

		/// <summary>
		/// Called when nodes were copied.
		/// </summary>
		/// <param name="data"></param>
		public virtual void OnCopiedNodes( object[] data )
		{
		}

		/// <summary>
		/// Map a custom EditorGraphNode to your logic graph node type.
		/// Everything else is handled automatically.
		/// </summary>
		/// <typeparam name="TNodeObject"></typeparam>
		/// <typeparam name="TEditorGraphNode"></typeparam>
		public void MapNodeType<TNodeObject, TEditorGraphNode>() where TEditorGraphNode : EditorGraphNode
		{
			typeNodeMap.Add( typeof( TNodeObject ), typeof( TEditorGraphNode ) );
		}

		/// <summary>
		/// Add a logic node to the graph.
		/// </summary>
		/// <param name="nodeObject"></param>
		/// <returns></returns>
		public EditorGraphNode AddNode( object nodeObject )
		{
			System.Type nodeType;
			if( !typeNodeMap.TryGetValue( nodeObject.GetType(), out nodeType ) )
				return null;

			var node = ScriptableObject.CreateInstance( nodeType ) as EditorGraphNode;
			if( node == null )
				return null;

			node.Initialize( nodeObject );

			graph.AddNode( node );

			return node;
		}

		/// <summary>
		/// Mark the graph to recalculate node connections.
		/// </summary>
		public void UpdateGraph()
		{
			graphNeedsUpdate = true;
		}

		/// <summary>
		/// Center the graph GUI.
		/// </summary>
		public void CenterGraph()
		{
			if( graphGUI != null )
				graphGUI.CenterGraph();
		}

		/// <summary>
		/// Draw the graph given a host EditorWindow.
		/// </summary>
		/// <param name="host"></param>
		public void OnGUI( EditorWindow host )
		{
			OnGUI( host, host.position );
		}

		/// <summary>
		/// Draw the graph given a host EditorWindow at a Rect position.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="position"></param>
		public void OnGUI( EditorWindow host, Rect position )
		{
			if( graph == null || graphGUI == null )
				Initialize();

			if( graphNeedsUpdate )
				UpdateGraphImmediate();

			using( Horizontal.Do( EditorStyles.toolbar ) )
			{
				OnToolbarGUI();
			}

			float toolbarHeight = EditorGUIUtility.singleLineHeight + 1.0f;

			graphGUI.BeginGraphGUI( host, new Rect( 0, toolbarHeight, position.width, position.height - toolbarHeight ) );
			graphGUI.OnGraphGUI();
			graphGUI.EndGraphGUI();
		}

		/// <summary>
		/// Called when the graph is bein created.
		/// </summary>
		protected virtual void OnCreateGraph()
		{
		}

		/// <summary>
		/// Called when drawing the graph toolbar GUI.
		/// </summary>
		protected virtual void OnToolbarGUI()
		{
		}

		/// <summary>
		/// Called when initializing the graph controller.
		/// </summary>
		protected virtual void Initialize()
		{
			graph = ScriptableObject.CreateInstance<EditorGraph>();
			graphGUI = ScriptableObject.CreateInstance<EditorGraphGUI>();
			graphGUI.graph = graph;

			UpdateGraphImmediate();
			CenterGraph();
		}

		private void UpdateGraphImmediate()
		{
			try
			{
				graph.IsCreatingGraph = true;
				graph.controller = this;

				graph.Clear( true );
				typeNodeMap.Clear();

				OnCreateGraph();
				EditorGraphControllerHelper.ConnectNodes( graph );

				graphNeedsUpdate = false;
			}
			finally
			{
				graph.IsCreatingGraph = false;
			}
		}
	}
}
