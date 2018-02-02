using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Properties with this attribute are marked as graph node input.
	/// </summary>
	[System.AttributeUsage( System.AttributeTargets.Property )]
	public sealed class InputSlotAttribute : System.Attribute
	{
	}

	/// <summary>
	/// Properties with this attribute are marked as graph node output.
	/// </summary>
	[System.AttributeUsage( System.AttributeTargets.Property )]
	public sealed class OutputSlotAttribute : System.Attribute
	{
	}

	/// <summary>
	/// Defines a graph node.
	/// Inherit from this class to configure custom node behaviour.
	/// </summary>
	public class EditorGraphNode : Node
	{
		/// <summary>
		/// All nodes's input properties.
		/// </summary>
		public List<PropertyInfo> inputs = new List<PropertyInfo>();

		/// <summary>
		/// All nodes's output properties.
		/// </summary>
		public List<PropertyInfo> outputs = new List<PropertyInfo>();

		/// <summary>
		/// Node's custom data.
		/// Here you put your logic node class instance.
		/// </summary>
		public object Data { get; private set; }

		/// <summary>
		/// Initialize the graph node with your logic node class instance.
		/// </summary>
		/// <param name="data"></param>
		public virtual void Initialize( object data )
		{
			Data = data;
			title = Data.GetType().Name;
			position = new Rect( 0.0f, 0.0f, 192.0f, 32.0f );

			SetupNodeSlots();
		}

		/// <summary>
		/// Internal method. Do not touch it.
		/// </summary>
		/// <param name="host"></param>
		public override void NodeUI( GraphGUI host )
		{
			EditorGUI.BeginChangeCheck();

			using( LabelWidth.Do( 84.0f ) )
			{
				OnShowNode( host );
			}

			var editorGraph = graph as EditorGraph;
			if( EditorGUI.EndChangeCheck() && editorGraph != null )
				editorGraph.controller.OnNodeChanged( this );
		}

		/// <summary>
		/// Internal method. Do not touch it.
		/// </summary>
		/// <param name="e"></param>
		public override void InputEdgeChanged( Edge e )
		{
			var editorGraph = graph as EditorGraph;
			if( editorGraph == null || editorGraph.IsCreatingGraph )
				return;

			SetInputEdgePropertyValue( e );
			editorGraph.OnNodeChanged( this );
		}

		/// <summary>
		/// Draw custom node GUI here.
		/// </summary>
		/// <param name="host"></param>
		protected virtual void OnShowNode( GraphGUI host )
		{
		}

		/// <summary>
		/// Helper method to draw a node slot.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="expression"></param>
		protected void DrawSlot( GraphGUI host, Expression<System.Func<object>> expression )
		{
			string slotName;
			if( StaticReflectionHelper.GetMemberName( expression ).TryGet( out slotName ) )
				DrawSlot( host, slotName );
		}

		/// <summary>
		/// Helper method to draw a node slot.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="slotName"></param>
		protected void DrawSlot( GraphGUI host, string slotName )
		{
			Slot slot = this[slotName];
			EditorGUILayout.LabelField( slotName );
			SlotDrawer.Draw( host, slot );
		}

		private void SetInputEdgePropertyValue( Edge e )
		{
			PropertyInfo inputProperty;
			if( !inputs.First( p => p.Name == e.toSlot.name ).TryGet( out inputProperty ) )
				return;

			var outputNode = e.fromSlot.node as EditorGraphNode;
			if( outputNode == null )
				return;

			PropertyInfo outputProperty;
			if( !outputNode.outputs.First( p => p.Name == e.fromSlot.name ).TryGet( out outputProperty ) )
				return;

			object value = null;
			if( graph.Connected( e.fromSlot, e.toSlot ) )
				value = outputProperty.GetValue( outputNode, new object[0] );

			inputProperty.SetValue( this, value, new object[0] );
		}

		private void SetupNodeSlots()
		{
			inputs.Clear();
			outputs.Clear();

			PropertyInfo[] properties = GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );

			foreach( PropertyInfo property in properties )
			{
				if( property.GetCustomAttributes( typeof( InputSlotAttribute ), true ).Length > 0 )
					inputs.Add( property );
				else if( property.GetCustomAttributes( typeof( OutputSlotAttribute ), true ).Length > 0 )
					outputs.Add( property );
			}

			foreach( PropertyInfo property in inputs )
				AddInputSlot( property.Name, property.PropertyType );

			foreach( PropertyInfo property in outputs )
				AddOutputSlot( property.Name, property.PropertyType );
		}
	}
}
