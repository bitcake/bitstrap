using UnityEditor;
using UnityEngine;

namespace BitStrap.Examples
{
	/// <summary>
	/// Open this window by navigating in Unity Editor to "Window/BitStrap Examples/Extensions/RectExtensions".
	/// </summary>
	public class RectExtensionsExample : EditorWindow
	{
		private float widthPercentage = 0.4f;
		private float height = 120.0f;

		[MenuItem( "Window/BitStrap Examples/Extensions/RectExtensions" )]
		public static void CreateWindow()
		{
			GetWindow<RectExtensionsExample>().Show();
		}

		private void OnGUI()
		{
			widthPercentage = EditorGUILayout.Slider( "Width %", widthPercentage, -1.0f, 1.0f );

			Rect rect = EditorGUILayout.GetControlRect( true, height );

			GUI.backgroundColor = Color.gray;
			GUI.Box( rect, GUIContent.none );

			Rect lineRect = rect.Row( 1 );

			Rect leftRect = lineRect.Left( lineRect.width * widthPercentage );
			Rect floatedLeftRect = lineRect.Right( lineRect.width * ( -widthPercentage ) );

			GUI.backgroundColor = Color.cyan;
			GUI.Box( leftRect, GUIContent.none );
			EditorGUI.LabelField( leftRect, "Left Rect" );
			EditorGUI.LabelField( floatedLeftRect, "Floated Left Rect" );

			lineRect = rect.Row( 2 );

			Rect rightRect = lineRect.Right( lineRect.width * widthPercentage );
			Rect floatedRightRect = lineRect.Left( lineRect.width * ( -widthPercentage ) );

			GUI.backgroundColor = Color.magenta;
			GUI.Box( rightRect, GUIContent.none );
			EditorGUI.LabelField( rightRect, "Right Rect" );
			EditorGUI.LabelField( floatedRightRect, "Floated Right Rect" );

			lineRect = rect.Row( 3 );
			Rect centerRect = lineRect.Center( lineRect.width * widthPercentage );

			lineRect = rect.Row( 4 );
			Rect floatedCenterRect = lineRect.Center( lineRect.width * ( -widthPercentage ) );

			GUI.backgroundColor = Color.yellow;
			GUI.Box( centerRect, GUIContent.none );
			GUI.Box( floatedCenterRect, GUIContent.none );
			EditorGUI.LabelField( centerRect, "Center Rect" );
			EditorGUI.LabelField( floatedCenterRect, "Floated Center Rect" );

			GUI.backgroundColor = Color.gray;
		}
	}
}