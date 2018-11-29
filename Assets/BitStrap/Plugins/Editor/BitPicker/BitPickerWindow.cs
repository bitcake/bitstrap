using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;

namespace BitStrap
{
	public struct BitPickerItem
	{
		public static readonly Texture2D EmptyIcon = new Texture2D( 4, 4 );

		public readonly BitPickerProvider provider;
		public readonly string name;
		public readonly string fullName;

		public readonly object data;
		public Texture2D icon;

		public BitPickerItem( BitPickerProvider provider, string name, string fullName, object data )
		{
			this.provider = provider;
			this.name = name;
			this.fullName = fullName;

			this.data = data;
			this.icon = null;
		}
	}

	public sealed class BitPickerWindow : EditorWindow
	{
		private static class Consts
		{
			public const string SearchControlName = "BitPickerSearch";
			public const string DragTitle = "BitPickerDrag";
			public const int PatternFontSize = 18;
			public const int MaxResults = 10;
			public const float WindowHeightOffset = 109.0f;

			public static readonly float WindowWidth = 600.0f;
			public static readonly Color SelectionColor = new Color32( 62, 95, 150, 255 );
			public static readonly Color InterleavedBackgroundColor = new Color( 0.0f, 0.0f, 0.0f, 0.1f );
		}

		private static bool editorReloaded = true;

		private BitPickerConfig config;
		private bool initialized = false;
		private string pattern = "";
		private List<BitPickerItem> providedItems = new List<BitPickerItem>( 2048 );
		private List<BitPickerHelper.Result> results = new List<BitPickerHelper.Result>( 1024 );
		private int selectedResultIndex = 0;
		private int viewResultIndex = 0;

		private GUIStyle patternStyle;
		private GUIStyle nameStyle;
		private GUIStyle fullNameStyle;
		private GUIStyle[] sourceStyles;

		private StringBuilder contentCache = new StringBuilder();
		private float patternStyleHeightCache;
		private float nameStyleHeightCache;

		[MenuItem( "Window/BitStrap/BitPicker %," )]
		public static void Open()
		{
			editorReloaded = false;

			EditorWindow.FocusWindowIfItsOpen<BitPickerWindow>();
			if( EditorWindow.focusedWindow is BitPickerWindow )
				return;

			BitPickerConfig config;
			if( !BitPickerConfig.Instance.TryGet( out config ) )
				return;

			var window = ScriptableObject.CreateInstance<BitPickerWindow>();
			window.config = config;

			window.ShowPopup();
			window.position = new Rect(
				( Screen.currentResolution.width - Consts.WindowWidth ) * 0.5f,
				Consts.WindowHeightOffset,
				Consts.WindowWidth,
				EditorGUIUtility.singleLineHeight
			);

			EditorWindow.FocusWindowIfItsOpen<BitPickerWindow>();
		}

		public void TryInit()
		{
			if( initialized )
				return;
			initialized = true;

			patternStyle = new GUIStyle( EditorStyles.textField );
			patternStyle.fontSize = Consts.PatternFontSize;
			patternStyle.margin = new RectOffset( 0, 0, 0, 0 );

			nameStyle = new GUIStyle( EditorStyles.largeLabel );
			nameStyle.alignment = TextAnchor.MiddleLeft;
			nameStyle.richText = true;

			fullNameStyle = new GUIStyle( EditorStyles.label );
			fullNameStyle.alignment = TextAnchor.MiddleLeft;
			fullNameStyle.richText = true;

			sourceStyles = new GUIStyle[] {
				GUI.skin.GetStyle( "sv_label_0" ),
				GUI.skin.GetStyle( "sv_label_1" ),
				GUI.skin.GetStyle( "sv_label_2" ),
				GUI.skin.GetStyle( "sv_label_3" ),
				GUI.skin.GetStyle( "sv_label_4" ),
				GUI.skin.GetStyle( "sv_label_5" ),
				GUI.skin.GetStyle( "sv_label_6" ),
				GUI.skin.GetStyle( "sv_label_7" ),
			};

			patternStyleHeightCache = BitPickerHelper.GetStyleLayoutSize( patternStyle, GUIContent.none ).y + 1;
			nameStyleHeightCache = BitPickerHelper.GetStyleLayoutSize( nameStyle, GUIContent.none ).y + 1;
			minSize = new Vector2( position.size.x, patternStyleHeightCache );

			foreach( var provider in config.providers )
				provider.Provide( providedItems );
		}

		private void SelectResultAt( int newSelected )
		{
			var resultCount = results.Count;
			selectedResultIndex = ( newSelected % resultCount + resultCount ) % resultCount;

			if( selectedResultIndex < viewResultIndex )
				viewResultIndex = selectedResultIndex;
			else if( selectedResultIndex >= viewResultIndex + Consts.MaxResults )
				viewResultIndex = selectedResultIndex - Consts.MaxResults + 1;
		}

		public void OnGUI()
		{
			if( editorReloaded || EditorWindow.focusedWindow != this )
			{
				Close();
				return;
			}

			TryInit();

			// Events
			{
				var currentEvent = Event.current;

				if( currentEvent.type == EventType.KeyDown )
				{
					switch( currentEvent.keyCode )
					{
					case KeyCode.Escape:
						Close();
						break;
					case KeyCode.UpArrow:
						SelectResultAt( selectedResultIndex - 1 );
						currentEvent.Use();
						break;
					case KeyCode.DownArrow:
						SelectResultAt( selectedResultIndex + 1 );
						currentEvent.Use();
						break;
					case KeyCode.Return:
						if( currentEvent.control || currentEvent.shift )
							PingItem( providedItems[results[selectedResultIndex].itemIndex] );
						else
							SelectItem( providedItems[results[selectedResultIndex].itemIndex] );

						currentEvent.Use();
						break;
					}
				}
				else if( currentEvent.type == EventType.ScrollWheel )
				{
					var scrollDelta = currentEvent.delta.y > 0.0f ? 1 : -1;
					SelectResultAt( selectedResultIndex + scrollDelta );
					currentEvent.Use();
				}
			}

			// Pattern matching
			{
				EditorGUI.BeginChangeCheck();

				GUI.SetNextControlName( Consts.SearchControlName );
				var patternRect = GUILayoutUtility.GetRect( GUIContent.none, patternStyle );
				var newPattern = EditorGUI.TextField( patternRect, pattern, patternStyle );

				if( EditorGUI.EndChangeCheck() && newPattern != pattern )
				{
					pattern = newPattern;
					selectedResultIndex = 0;
					viewResultIndex = 0;
					results.Clear();

					if( pattern.Length > 0 )
					{
						var patternWithoutArgs = BitPickerHelper.RemoveArgs( pattern );
						BitPickerHelper.PrepareToGetMatches( config, providedItems, patternWithoutArgs );
					}
				}

				if( Event.current.type == EventType.Layout && BitPickerHelper.GetMatchesPartial( results ) )
				{
					selectedResultIndex = 0;
					viewResultIndex = 0;
					results.Sort( ( a, b ) => b.score - a.score );
					Repaint();
				}
			}

			// Show Results
			{
				var currentEvent = Event.current;
				var resultsViewCount = Mathf.Min( Consts.MaxResults, results.Count );

				var windowRect = position;
				windowRect.height = patternStyleHeightCache + resultsViewCount * nameStyleHeightCache;
				position = windowRect;
				minSize = windowRect.size;

				for( var i = viewResultIndex; i < viewResultIndex + resultsViewCount; i++ )
				{
					var result = results[i];
					var item = providedItems[result.itemIndex];
					if( item.icon == null )
						item.icon = item.provider.GetItemIcon( item );

					contentCache.Length = 0;
					if( config.scoreConfig.showScores )
					{
						contentCache.Append( result.score );
						contentCache.Append( " - " );
					}

					BitPickerHelper.HighlightMatches( config, item.name, result.nameMatches, contentCache );
					var nameContent = new GUIContent( contentCache.ToString() );

					contentCache.Length = 0;
					BitPickerHelper.HighlightMatches( config, item.fullName, result.fullNameMatches, contentCache );
					var fullNameContent = new GUIContent( contentCache.ToString() );

					var nameSize = BitPickerHelper.GetStyleLayoutSize( nameStyle, nameContent );

					var resultRect = GUILayoutUtility.GetRect( position.width, nameSize.y );

					var sourceContent = new GUIContent( item.provider.GetProvisionSource() );
					var sourceStyle = GetSourceStyle( sourceContent.text );
					var sourceSize = BitPickerHelper.GetStyleLayoutSize( sourceStyle, sourceContent );

					if( selectedResultIndex == i )
						EditorGUI.DrawRect( resultRect, Consts.SelectionColor );
					else if( i % 2 == 1 )
						EditorGUI.DrawRect( resultRect, Consts.InterleavedBackgroundColor );

					Rect iconRect;
					Rect sourceRect;
					Rect nameRect;
					Rect fullNameRect;

					resultRect
						.Left( resultRect.height, out iconRect )
						.Left( nameSize.x, out nameRect )
						.Right( sourceSize.x, out sourceRect )
						.Expand( out fullNameRect );

					sourceRect = sourceRect.CenterVertically( sourceSize.y );

					if( item.icon != BitPickerItem.EmptyIcon )
						GUI.Label( iconRect, item.icon );

					GUI.Label( sourceRect, sourceContent, sourceStyle );
					GUI.Label( fullNameRect, fullNameContent, fullNameStyle );

					if( currentEvent.type == EventType.MouseDrag && resultRect.Contains( currentEvent.mousePosition ) )
					{
						var dragReferences = item.provider.GetItemDragReferences( item );
						if( dragReferences != null )
						{
							DragAndDrop.PrepareStartDrag();
							DragAndDrop.objectReferences = dragReferences;
							DragAndDrop.StartDrag( Consts.DragTitle );
							currentEvent.Use();
						}
					}

					if( GUI.Button( nameRect, nameContent, nameStyle ) )
						SelectItem( item );
				}
			}

			EditorGUI.FocusTextInControl( Consts.SearchControlName );
		}

		private void OnLostFocus2()
		{
			Close();
			DestroyImmediate( this );
		}

		private void PingItem( BitPickerItem item )
		{
			if( item.provider != null )
				item.provider.OnPingItem( item );
		}

		private void SelectItem( BitPickerItem item )
		{
			Close();

			if( item.provider != null )
				item.provider.OnOpenItem( item, pattern );
		}

		private GUIStyle GetSourceStyle( string source )
		{
			var hash = source.GetHashCode();
			var count = sourceStyles.Length;
			var index = ( ( hash % count ) + count ) % count;
			return sourceStyles[index];
		}
	}
}