using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Bunch of EditorGUI[Layout] helper methods to ease your Unity custom editor development.
	/// </summary>
	public static class EditorHelper
	{
		private class ObjectNameComparer : IComparer<Object>
		{
			public readonly static ObjectNameComparer Instance = new ObjectNameComparer();

			int IComparer<Object>.Compare( Object a, Object b )
			{
				return System.String.Compare( a.name, b.name, System.StringComparison.OrdinalIgnoreCase );
			}
		}

		/// <summary>
		/// Collection of some cool and useful GUI styles.
		/// </summary>
		public static class Styles
		{
			public static GUIStyle Header
			{
				get { return GUI.skin.GetStyle( "HeaderLabel" ); }
			}

			public static GUIStyle Selection
			{
				get { return GUI.skin.GetStyle( "MeTransitionSelectHead" ); }
			}

			public static GUIStyle PreDrop
			{
				get { return GUI.skin.GetStyle( "TL SelectionButton PreDropGlow" ); }
			}

			public static GUIStyle SearchTextField
			{
				get { return GUI.skin.GetStyle( "SearchTextField" ); }
			}

			public static GUIStyle SearchCancelButtonEmpty
			{
				get { return GUI.skin.GetStyle( "SearchCancelButtonEmpty" ); }
			}

			public static GUIStyle SearchCancelButton
			{
				get { return GUI.skin.GetStyle( "SearchCancelButton" ); }
			}

			public static GUIStyle Plus
			{
				get { return GUI.skin.GetStyle( "OL Plus" ); }
			}

			public static GUIStyle Minus
			{
				get { return GUI.skin.GetStyle( "OL Minus" ); }
			}

			public static GUIStyle Input
			{
				get { return GUI.skin.GetStyle( "flow shader in 0" ); }
			}

			public static GUIStyle Output
			{
				get { return GUI.skin.GetStyle( "flow shader out 0" ); }
			}

			public static GUIStyle Warning
			{
				get { return GUI.skin.GetStyle( "CN EntryWarn" ); }
			}
		}

		private static string searchField = "";
		private static Vector2 scroll = Vector2.zero;
		private static Texture[] unityIcons = null;

		private static GUIStyle boxStyle = null;

		/// <summary>
		/// The drop down button stored Rect. For use with GenericMenu
		/// </summary>
		public static Rect DropDownRect { get; private set; }

		private static GUIStyle BoxStyle
		{
			get
			{
				if( boxStyle == null )
				{
					boxStyle = EditorStyles.helpBox;

					boxStyle.padding.left = 1;
					boxStyle.padding.right = 1;
					boxStyle.padding.top = 4;
					boxStyle.padding.bottom = 8;

					boxStyle.margin.left = 16;
					boxStyle.margin.right = 16;
				}

				return boxStyle;
			}
		}

		/// <summary>
		/// Begins drawing a box.
		/// Draw its header here.
		/// </summary>
		public static void BeginBoxHeader()
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginVertical( BoxStyle );
			EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );
		}

		/// <summary>
		/// Ends drawing the box header.
		/// Draw its contents here.
		/// </summary>
		public static void EndBoxHeaderBeginContent()
		{
			EndBoxHeaderBeginContent( Vector2.zero );
		}

		/// <summary>
		/// Ends drawing the box header.
		/// Draw its contents here (scroll version).
		/// </summary>
		/// <param name="scroll"></param>
		/// <returns></returns>
		public static Vector2 EndBoxHeaderBeginContent( Vector2 scroll )
		{
			EditorGUILayout.EndHorizontal();
			GUILayout.Space( 1.0f );
			return EditorGUILayout.BeginScrollView( scroll );
		}

		/// <summary>
		/// Begins drawing a box with a label header.
		/// </summary>
		/// <param name="label"></param>
		public static void BeginBox( string label )
		{
			BeginBoxHeader();
			Rect rect = GUILayoutUtility.GetRect( GUIContent.none, GUI.skin.label );
			rect.y -= 2.0f;
			rect.height += 2.0f;
			EditorGUI.LabelField( rect, Label( label ), Styles.Header );
			EndBoxHeaderBeginContent();
		}

		/// <summary>
		/// Begins drawing a box with a label header (scroll version).
		/// </summary>
		/// <param name="scroll"></param>
		/// <param name="label"></param>
		/// <returns></returns>
		public static Vector2 BeginBox( Vector2 scroll, string label )
		{
			BeginBoxHeader();
			EditorGUILayout.LabelField( Label( label ), Styles.Header );
			return EndBoxHeaderBeginContent( scroll );
		}

		/// <summary>
		/// Finishes drawing the box.
		/// </summary>
		/// <returns></returns>
		public static bool EndBox()
		{
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
			return EditorGUI.EndChangeCheck();
		}

		/// <summary>
		/// Reserves a Rect in a layout setup given a style.
		/// </summary>
		/// <param name="style"></param>
		/// <returns></returns>
		public static Rect Rect( GUIStyle style )
		{
			return GUILayoutUtility.GetRect( GUIContent.none, style );
		}

		/// <summary>
		/// Reserves a Rect with an explicit height in a layout.
		/// </summary>
		/// <param name="height"></param>
		/// <returns></returns>
		public static Rect Rect( float height )
		{
			return GUILayoutUtility.GetRect( 0.0f, height, GUILayout.ExpandWidth( true ) );
		}

		/// <summary>
		/// Returns a GUIContent containing a label and the tooltip defined in GUI.tooltip.
		/// </summary>
		/// <param name="label"></param>
		/// <returns></returns>
		public static GUIContent Label( string label )
		{
			return new GUIContent( label, GUI.tooltip );
		}

		/// <summary>
		/// Draws a drop down button and stores its Rect in DropDownRect variable.
		/// </summary>
		/// <param name="label"></param>
		/// <param name="style"></param>
		/// <returns></returns>
		public static bool DropDownButton( string label, GUIStyle style )
		{
			var content = new GUIContent( label );
			DropDownRect = GUILayoutUtility.GetRect( content, style );
			return GUI.Button( DropDownRect, content, style );
		}

		/// <summary>
		/// Draws a search field like those of Project window.
		/// </summary>
		/// <param name="search"></param>
		/// <returns></returns>
		public static string SearchField( string search )
		{
			using( Horizontal.Do() )
			{
				search = EditorGUILayout.TextField( search, Styles.SearchTextField );

				GUIStyle buttonStyle = Styles.SearchCancelButtonEmpty;
				if( !string.IsNullOrEmpty( search ) )
					buttonStyle = Styles.SearchCancelButton;

				if( GUILayout.Button( GUIContent.none, buttonStyle ) )
					search = "";
			}

			return search;
		}

		/// <summary>
		/// Draws a delayed search field like those of Project window.
		/// </summary>
		/// <param name="search"></param>
		/// <returns></returns>
		public static string DelayedSearchField( string search )
		{
			using( Horizontal.Do() )
			{
				search = EditorGUILayout.DelayedTextField( search, Styles.SearchTextField );

				GUIStyle buttonStyle = Styles.SearchCancelButtonEmpty;
				if( !string.IsNullOrEmpty( search ) )
					buttonStyle = Styles.SearchCancelButton;

				if( GUILayout.Button( GUIContent.none, buttonStyle ) )
					search = "";
			}

			return search;
		}

		/// <summary>
		/// This is a debug method that draws all Unity styles found in GUI.skin.customStyles
		/// together with its name, so you can later use some specific style.
		/// </summary>
		public static void DrawAllStyles()
		{
			searchField = SearchField( searchField );

			string searchLower = searchField.ToLower( CultureInfo.InvariantCulture );
			EditorGUILayout.Space();

			using( ScrollView.Do( ref scroll ) )
			{
				foreach( GUIStyle style in GUI.skin.customStyles )
				{
					if( string.IsNullOrEmpty( searchField ) ||
						style.name.ToLower( CultureInfo.InvariantCulture ).Contains( searchLower ) )
					{
						using( Horizontal.Do() )
						{
							EditorGUILayout.TextField( style.name, EditorStyles.label );
							GUILayout.Label( style.name, style );
						}
					}
				}
			}
		}

		/// <summary>
		/// This is a debug method that draws all Unity icons
		/// together with its name, so you can later use them.
		/// </summary>
		public static void DrawAllIcons()
		{
			if( unityIcons == null )
			{
				unityIcons = Resources.FindObjectsOfTypeAll<Texture>();
				System.Array.Sort( unityIcons, ObjectNameComparer.Instance );
			}

			searchField = SearchField( searchField );

			string searchLower = searchField.ToLower( CultureInfo.InvariantCulture );
			EditorGUILayout.Space();

			using( ScrollView.Do( ref scroll ) )
			{
				foreach( Texture texture in unityIcons )
				{
					if( texture == null || texture.name == "" )
						continue;

					if( !AssetDatabase.GetAssetPath( texture ).StartsWith( "Library/" ) )
						continue;

					if( string.IsNullOrEmpty( searchField ) ||
						texture.name.ToLower( CultureInfo.InvariantCulture ).Contains( searchLower ) )
					{
						using( Horizontal.Do() )
						{
							EditorGUILayout.TextField( texture.name, EditorStyles.label );
							GUILayout.Label( new GUIContent( texture ) );
						}
					}
				}
			}
		}
	}
}
