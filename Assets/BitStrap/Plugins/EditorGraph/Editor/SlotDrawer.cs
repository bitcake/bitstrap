using UnityEditor.Graphs;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Drawer of graph node slots.
	/// Handles connection and other GUI events.
	/// </summary>
	public static class SlotDrawer
	{
		/// <summary>
		/// Draws a slot given a GraphGUI host.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="slot"></param>
		public static void Draw( GraphGUI host, Slot slot )
		{
			Rect position = GUILayoutUtility.GetLastRect();
			position = GetRect( position, slot, 5.0f );
			Draw( position, host, slot );
		}

		/// <summary>
		/// Calculates a slot Rect for use in Draw().
		/// Use negativePadding to take from default padding and control positioning.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="slot"></param>
		/// <param name="negativePadding"></param>
		/// <returns></returns>
		public static Rect GetRect( Rect position, Slot slot, float negativePadding )
		{
			position.height = 16.0f;

			if( slot.isInputSlot )
			{
				position.x -= negativePadding;
				position.width = Mathf.Min( position.width, 12.0f );
			}
			else if( slot.isOutputSlot )
			{
				position.x += negativePadding;
				position.xMin = position.xMax - Mathf.Min( position.width, 12.0f );
			}

			return position;
		}

		/// <summary>
		/// Draws a slot given a GraphGUI host and a Rect position.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="host"></param>
		/// <param name="slot"></param>
		public static void Draw( Rect position, GraphGUI host, Slot slot )
		{
			if( slot.isInputSlot )
				host.Slot( position, null, slot, false, true, false, EditorHelper.Styles.Input );
			else if( slot.isOutputSlot )
				host.Slot( position, null, slot, true, false, true, EditorHelper.Styles.Output );
		}
	}
}
