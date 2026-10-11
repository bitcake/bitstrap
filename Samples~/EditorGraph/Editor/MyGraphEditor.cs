using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace BitStrap.Examples
{
	/// <summary>
	/// Open the graph editor window by navigating in Unity Editor to "Window/BitStrap Examples/EditorGraph".
	/// </summary>
	public sealed class MyGraphController : EditorGraphController
	{
		public MyGraph Target
		{
			get { return target; }
		}

		private MyGraph target;

		public void SetTarget( MyGraph target )
		{
			this.target = target;
			UpdateGraph();
			CenterGraph();
		}

		public override void OnNodeRemoved( EditorGraphNode node )
		{
			if( target == null )
				return;

			target.nodes.Remove( node.Data as MyGraphNode );
		}

		public override void OnNodeChanged( EditorGraphNode node )
		{
			if( target == null )
				return;
		}

		public override void OnCopiedNodes( object[] data )
		{
			if( target == null )
				return;

			Undo.RecordObject( target, "EditorGraph.CopyNodes" );
			int offset = 64;

			foreach( object d in data )
			{
				var node = d as MyGraphNode;
				if( node == null )
					continue;

				node.x += offset;
				node.y += offset;

				target.nodes.Add( node );
			}

			UpdateGraph();
			EditorUtility.SetDirty( target );
		}

		protected override void OnCreateGraph()
		{
			if( target == null )
				return;

			MapNodeType<MyGraphNode, MyGraphNodeController>();

			foreach( var node in target.nodes )
				AddNode( node );
		}

		protected override void OnToolbarGUI()
		{
			if( GUILayout.Button( "Add Node", EditorStyles.toolbarButton ) )
			{
				Undo.RecordObject( target, "EditorGraph.AddNode" );

				target.nodes.Add( new MyGraphNode() );
				UpdateGraph();
				EditorUtility.SetDirty( target );
			}

			GUILayout.FlexibleSpace();

			EditorGUI.BeginChangeCheck();
			target = EditorGUILayout.ObjectField( target, typeof( MyGraph ), false ) as MyGraph;
			if( EditorGUI.EndChangeCheck() )
			{
				SetTarget( target );
			}
		}
	}

	/// <summary>
	/// Open the graph editor window by navigating in Unity Editor to "Window/BitStrap Examples/EditorGraph".
	/// </summary>
	public sealed class MyGraphWindow : EditorWindow
	{
		public MyGraphController controller = new MyGraphController();

		[MenuItem( "Window/BitStrap Examples/EditorGraph" )]
		public static MyGraphWindow ShowWindow()
		{
			var window = GetWindow<MyGraphWindow>( "MyGraphEditor" );
			window.Show();
			return window;
		}

		private void OnGUI()
		{
			if( controller.Target != null )
				Undo.RecordObject( controller.Target, "EditGraph" );

			controller.OnGUI( this );
		}

		/// <summary>
		/// Add the possibility to open the asset by just double clicking a my graph scriptable object
		/// </summary>
		/// <returns>True if the clicked item was of the type <see cref="MyGraph"/></returns>
		[OnOpenAsset( 0 )]
		public static bool OnOpenGraphAsset( int instanceID, int line )
		{
			UnityEngine.Object obj = EditorUtility.InstanceIDToObject( instanceID );
			System.Type type = obj.GetType();
			if( type == typeof( MyGraph ) )
			{
				var window = GetWindow<MyGraphWindow>( "MyGraphEditor" );
				window.controller.SetTarget( obj as MyGraph );
				window.Show();
				return true;
			}
			return false;
		}
	}

	[CustomEditor( typeof( MyGraph ) )]
	public sealed class MyGraphEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			if( GUILayout.Button( "Open Graph Window" ) )
			{
				var window = MyGraphWindow.ShowWindow();
				window.controller.SetTarget( target as MyGraph );
			}
		}
	}
}
