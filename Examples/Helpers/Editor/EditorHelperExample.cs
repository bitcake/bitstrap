using UnityEditor;
using UnityEngine;

namespace BitStrap.Examples
{
	/// <summary>
	/// Open this window by navigating in Unity Editor to "Window/BitStrap Examples/Extensions/EditorHelper".
	/// </summary>
	public class EditorHelperExample : EditorWindow
	{
		private string searchText = "";
		private GenericMenu dropDownMenu;

		[MenuItem( "Window/BitStrap Examples/Helpers/EditorHelper" )]
		public static void CreateWindow()
		{
			GetWindow<EditorHelperExample>().Show();
		}

		private void OnEnable()
		{
			dropDownMenu = new GenericMenu();

			dropDownMenu.AddItem( new GUIContent( "None" ), false, () => { } );
			dropDownMenu.AddSeparator( "" );
			dropDownMenu.AddItem( new GUIContent( "Option 1" ), false, () => Debug.Log( "Selected 1" ) );
			dropDownMenu.AddItem( new GUIContent( "Option 2" ), false, () => Debug.Log( "Selected 2" ) );
		}

		private void OnGUI()
		{
			using( Horizontal.Do( EditorStyles.toolbar ) )
			{
				if( EditorHelper.DropDownButton( "Drop Down Button", EditorStyles.toolbarDropDown ) )
					dropDownMenu.DropDown( EditorHelper.DropDownRect );
				GUILayout.FlexibleSpace();
			}

			EditorGUILayout.LabelField( "Selection Style", EditorHelper.Styles.Selection );
			EditorGUILayout.LabelField( "PreDrop Style", EditorHelper.Styles.PreDrop );
			EditorGUILayout.LabelField( "Plus Style", EditorHelper.Styles.Plus );
			EditorGUILayout.LabelField( "Minus Style", EditorHelper.Styles.Minus );
			EditorGUILayout.LabelField( "Warning Style", EditorHelper.Styles.Warning, GUILayout.Height( 24.0f ) );

			using( LabelWidth.Do( 256.0f ) )
			{
				EditorGUILayout.IntField( "This is a 256 width label", 0 );
			}

			EditorHelper.BeginBoxHeader();
			EditorGUILayout.LabelField( "Awesome Box" );
			EditorHelper.EndBoxHeaderBeginContent();

			EditorGUILayout.LabelField( "Box contents..." );

			Rect position = EditorHelper.Rect( 4.0f );
			EditorGUI.DrawRect( position, Color.gray );

			GUI.tooltip = "This is a tooltip";
			EditorGUILayout.LabelField( EditorHelper.Label( "This label has a tooltip" ) );

			EditorHelper.EndBox();

			searchText = EditorHelper.SearchField( searchText );
		}
	}
}
