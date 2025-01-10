using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace BitStrap.Examples
{
	/// <summary>
	/// Open the graph editor window by navigating in Unity Editor to "Window/BitStrap Examples/EditorGraph".
	/// </summary>
	public sealed class MyGraphNodeController : EditorGraphNode
	{
		public MyGraphNode Node { get { return Data as MyGraphNode; } }

		[InputSlot]
		public MyGraphNode Input
		{
			get { return Node.input; }
			set { Node.input = value; }
		}

		[OutputSlot]
		public MyGraphNode Output { get { return Node; } }

		public override void Initialize( object nodeObject )
		{
			base.Initialize( nodeObject );

			position.x = Node.x;
			position.y = Node.y;
		}

		public override void OnDrag()
		{
			Node.x = ( int ) position.x;
			Node.y = ( int ) position.y;
		}

		protected override void OnShowNode( GraphGUI host )
		{
			color = Styles.Color.Orange;

			DrawOutputSlot( host );
			DrawSlot( host, () => Input );

			Node.value = EditorGUILayout.IntField( "Value", Node.value );
		}

		private void DrawOutputSlot( GraphGUI host )
		{
			Slot slot = this["Output"];

			Rect titlePosition = new Rect( 0.0f, 6.0f, position.width, 16.0f );
			titlePosition = SlotDrawer.GetRect( titlePosition, slot, 1.0f );

			SlotDrawer.Draw( titlePosition, host, slot );
		}
	}
}
