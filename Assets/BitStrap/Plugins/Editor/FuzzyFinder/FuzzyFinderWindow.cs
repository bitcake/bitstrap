using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace BitStrap
{
	public sealed class FuzzyFinderWindow : EditorWindow
	{
		private struct Result
		{
			public string match;
			public int score;
		}

		private bool needControlFocus = false;
		private string pattern = "";
		private List<Result> results = new List<Result>();
		private string[] allAssetPathsCache;

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

			if( allAssetPathsCache == null )
				allAssetPathsCache = AssetDatabase.GetAllAssetPaths();

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

			const int MaxResults = 5;

			if( EditorGUI.EndChangeCheck() )
			{
				results.Clear();
				if( pattern.Length > 0 )
				{
					foreach( var path in allAssetPathsCache )
					{
						int score;
						if( FuzzyFinder.Match( pattern, path, out score ) )
						{
							results.Add( new Result
							{
								match = path,
								score = score,
							} );
						}
					}
				}
			}

			for( var i = 0; i < MaxResults && i < results.Count; i++ )
			{
				var result = results[i];
				EditorGUILayout.LabelField( result.score + " " + result.match );
			}

			if( needControlFocus )
			{
				EditorGUI.FocusTextInControl( ControlName );
				needControlFocus = false;
			}
		}
	}
}