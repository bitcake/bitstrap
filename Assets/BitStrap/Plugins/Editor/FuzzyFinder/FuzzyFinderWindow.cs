using UnityEngine;
using UnityEditor;

namespace BitStrap
{
	public sealed class FuzzyFinderWindow : EditorWindow
	{
		private bool needControlFocus = false;
		private string pattern = "";
		private string[] results = new string[0];

		[MenuItem( "Window/BitStrap/Fuzzy Finder %k" )]
		public static void Open()
		{
			var window = GetWindow<FuzzyFinderWindow>( true, "Fuzzy Find", true );

			var position = window.position;
			position.x = ( Screen.currentResolution.width - position.width ) * 0.5f;
			position.y = ( Screen.currentResolution.height - position.height ) * 0.5f;
			window.position = position;

			window.needControlFocus = true;

			window.Show();
		}

		private void OnGUI()
		{
			const string ControlName = "FuzzyFinderSearch";

			var currentEvent = Event.current;
			if( currentEvent.type == EventType.KeyDown )
			{
				switch( currentEvent.keyCode )
				{
				case KeyCode.Escape:
					Close();
					break;
				}
			}

			EditorGUI.BeginChangeCheck();

			using( Horizontal.Do() )
			{
				GUI.SetNextControlName( ControlName );
				pattern = EditorGUILayout.TextField( pattern, EditorHelper.Styles.SearchTextField );

				var buttonStyle = string.IsNullOrEmpty( pattern ) ?
					EditorHelper.Styles.SearchCancelButtonEmpty :
					EditorHelper.Styles.SearchCancelButton;

				if( GUILayout.Button( GUIContent.none, buttonStyle ) )
					pattern = "";
			}

			if( EditorGUI.EndChangeCheck() )
			{
				if( pattern.Length > 0 )
					results = AssetDatabase.FindAssets( pattern );
				else
					results = new string[0];
			}

			const int MaxResults = 5;

			for( var i = 0; i < MaxResults && i < results.Length; i++ )
			{
				var result = results[i];
				var path = AssetDatabase.GUIDToAssetPath( result );
				EditorGUILayout.LabelField( path );
			}

			if( needControlFocus )
			{
				EditorGUI.FocusTextInControl( ControlName );
				needControlFocus = false;
			}
		}
	}
}