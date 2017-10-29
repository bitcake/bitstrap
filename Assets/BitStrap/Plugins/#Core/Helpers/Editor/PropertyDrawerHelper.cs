using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Collection of helper methods when coding a PropertyDrawer editor.
	/// </summary>
	public static class PropertyDrawerHelper
	{
		/// <summary>
		/// If the target property has a [Tooltip] attribute, load it into its label.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="label"></param>
		public static void LoadAttributeTooltip( PropertyDrawer self, GUIContent label )
		{
			var tooltipAttribute = from a in self.fieldInfo.GetCustomAttributes( typeof( TooltipAttribute ), true ).First() select a as TooltipAttribute;

			label.tooltip = tooltipAttribute.Match(
				some: a => a.tooltip,
				none: () => "" );
		}
	}
}
