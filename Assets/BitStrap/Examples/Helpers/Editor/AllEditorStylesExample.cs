using UnityEditor;
using UnityEngine;

namespace BitStrap.Examples
{
	/// <summary>
	/// Open this window by navigating in Unity Editor to "Window/BitStrap Examples/Extensions/AllEditorStyles".
	/// </summary>
	public class AllEditorStylesExample : EditorWindow
	{
		public bool showStyles;

		[MenuItem( "Window/BitStrap Examples/Helpers/AllEditorStyles" )]
		public static void CreateWindow()
		{
			GetWindow<AllEditorStylesExample>().Show();
		}

		private void OnGUI()
		{
			using( Horizontal.Do( EditorStyles.toolbar ) )
			{
				showStyles = GUILayout.Toggle( showStyles, "Styles", EditorStyles.toolbarButton );
				showStyles = !GUILayout.Toggle( !showStyles, "Icons", EditorStyles.toolbarButton );
			}

			if( showStyles )
				EditorHelper.DrawAllStyles();
			else
				EditorHelper.DrawAllIcons();
		}
	}
}
