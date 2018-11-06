using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace BitStrap
{
	public sealed class BitPickerWindow : EditorWindow
	{
		private static class Consts
		{
			public const string SearchControlName = "BitPickerSearch";
			public const int PatternFontSize = 24;
			public const int ResultsFontSize = 20;
			public const int MaxResults = 10;
			public const float WindowHeightOffset = 200.0f;

			public static readonly Vector2 WindowSize = new Vector2( 800.0f, 320.0f );
			public static readonly Color SelectionColor = new Color32( 0, 122, 255, 255 );
		}

		private struct Result
		{
			public string match;
			public int score;
		}

		private static bool editorReloaded = true;

		private BitPickerConfig config;
		private bool initialized = false;
		private string pattern = "";
		private List<Result> results = new List<Result>();
		private string[] allAssetPathsCache;
		private int selectedResult = 0;

		private GUIStyle patternStyle;
		private GUIStyle resultStyle;

		[MenuItem( "Window/BitStrap/BitPicker %k" )]
		public static void Open()
		{
			editorReloaded = false;

			BitPickerConfig config;
			if( !BitPickerConfig.Instance.TryGet( out config ) )
				return;

			var window = EditorWindow.GetWindow<BitPickerWindow>( true, "BitPicker", true );
			window.config = config;

			window.Show();
			window.position = new Rect(
				( Screen.currentResolution.width - Consts.WindowSize.x ) * 0.5f,
				( Screen.currentResolution.height - Consts.WindowSize.y ) * 0.5f - Consts.WindowHeightOffset,
				Consts.WindowSize.x,
				Consts.WindowSize.y
			);

			//PopupWindow.Show( rect, new FuzzyFinderWindow() );
		}

		public void Init()
		{
			allAssetPathsCache = AssetDatabase.GetAllAssetPaths();

			patternStyle = new GUIStyle( GUI.skin.textField );
			patternStyle.fontSize = Consts.PatternFontSize;

			resultStyle = new GUIStyle( GUI.skin.label );
			resultStyle.fontSize = Consts.ResultsFontSize;
		}

		public void OnGUI()
		{
			if( editorReloaded )
			{
				Close();
				return;
			}

			if( !initialized )
				Init();

			// Events
			{
				var resultsCount = Mathf.Min( Consts.MaxResults, results.Count );

				var currentEvent = Event.current;
				if( currentEvent.type == EventType.KeyDown )
				{
					switch( currentEvent.keyCode )
					{
					case KeyCode.Escape:
						Close();
						break;
					case KeyCode.UpArrow:
						selectedResult = ( selectedResult - 1 + resultsCount ) % resultsCount;
						currentEvent.Use();
						break;
					case KeyCode.DownArrow:
						selectedResult = ( selectedResult + 1 ) % resultsCount;
						currentEvent.Use();
						break;
					case KeyCode.Return:
						OnSelectResult( results[selectedResult] );
						break;
					}
				}
			}

			// Pattern match
			{
				EditorGUI.BeginChangeCheck();

				GUI.SetNextControlName( Consts.SearchControlName );
				var patternRect = GUILayoutUtility.GetRect( GUIContent.none, patternStyle );
				pattern = EditorGUI.TextField( patternRect, pattern, patternStyle );

				if( EditorGUI.EndChangeCheck() )
				{
					selectedResult = 0;
					results.Clear();
					if( pattern.Length > 0 )
					{
						foreach( var path in allAssetPathsCache )
						{
							int score;
							if( FuzzyFinder.Match( config.fuzzyFinderConfig, pattern, path, out score ) )
							{
								results.Add( new Result
								{
									match = path,
									score = score,
								} );
							}
						}

						results.Sort( ( a, b ) => b.score - a.score );
					}
				}
			}

			// Show Results
			{
				var resultsCount = Mathf.Min( Consts.MaxResults, results.Count );
				for( var i = 0; i < resultsCount; i++ )
				{
					var result = results[i];

					var asset = AssetDatabase.LoadAssetAtPath<Object>( result.match );
					var assetTexture = AssetPreview.GetMiniThumbnail( asset );

					var content = new GUIContent( result.match );
					var resultRect = GUILayoutUtility.GetRect( content, resultStyle );

					if( selectedResult == i )
						EditorGUI.DrawRect( resultRect, Consts.SelectionColor );

					var textureRect = resultRect;
					textureRect.width = textureRect.height;
					GUI.Label( textureRect, assetTexture );

					resultRect.xMin += textureRect.width;
					if( GUI.Button( resultRect, content, resultStyle ) )
						OnSelectResult( results[selectedResult] );
				}
			}

			if( !initialized )
			{
				EditorGUI.FocusTextInControl( Consts.SearchControlName );
				initialized = true;
			}
		}

		private void OnLostFocus()
		{
			Close();
		}

		private void OnSelectResult( Result result )
		{
			Close();

			var asset = AssetDatabase.LoadAssetAtPath<Object>( result.match );
			if( asset != null )
			{
				EditorGUIUtility.PingObject( asset );
				Selection.activeObject = asset;
				//AssetDatabase.OpenAsset( asset );
			}
		}
	}
}