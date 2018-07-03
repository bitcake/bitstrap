using UnityEngine;
using UnityEditor;

namespace BitStrap
{
	public sealed class ShortcutsWindow : EditorWindow
	{
		private static class Colors
		{
			public static readonly Color SelectionColor = new Color32( 24, 118, 206, 255 );
		}

		private const string UndoString = "ShortcutsWindow_Undo";
		private const string DragString = "ShortcutsWindow_Drag";

		private Option<ShortcutsReferences> shortcutsReferences;

		[MenuItem( "Window/BitStrap/Shortcuts Window %&s" )]
		private static void OpenWindow()
		{
			var window = GetWindow<ShortcutsWindow>( "Shortcuts" );
			window.Show();
		}

		private void OnGUI()
		{
			ShortcutsReferences references;
			if( !shortcutsReferences.OrElse( () => AssetDatabaseHelper.FindAssetOfType<ShortcutsReferences>() ).TryGet( out references ) )
				return;

			var areaRect = new Rect( position );
			areaRect.position = Vector2.zero;
			GUILayout.BeginArea( areaRect );

			const float padding = 8.0f;
			const float size = 80.0f;

			var currentEvent = Event.current;
			var eventType = currentEvent.type;
			var clickCount = currentEvent.clickCount;
			var holdingControl = currentEvent.control;
			var mousePosition = currentEvent.mousePosition;

			var clickedItem = false;
			var columnCount = Mathf.Max( Mathf.FloorToInt( areaRect.width / size ), 1 );

			references.assets.RemoveAll( a => a == null );

			for( int i = 0; i < references.assets.Count; i++ )
			{
				var a = references.assets[i];
				if( a == null )
					continue;

				var previewTexture = AssetPreview.GetAssetPreview( a );
				if( previewTexture == null )
					previewTexture = AssetPreview.GetMiniThumbnail( a );

				var x = ( i % columnCount ) * size + padding * 0.5f;
				var y = ( i / columnCount ) * size + padding * 0.5f;

				var totalRect = new Rect( x, y, size, size );

				// Click item
				if( clickCount > 0 && totalRect.Contains( mousePosition ) )
				{
					clickedItem = true;

					if( clickCount == 1 && holdingControl )
					{
						var selected = Selection.objects;
						if( !ArrayUtility.Contains( selected, a ) )
						{
							ArrayUtility.Add( ref selected, a );
							Selection.objects = selected;
						}
					}
					else
					{
						Selection.activeObject = a;
					}

					if( clickCount > 1 )
						AssetDatabase.OpenAsset( a );

					Repaint();
				}

				bool isSelected = Selection.Contains( a );
				if( isSelected )
					EditorGUI.DrawRect( totalRect, Colors.SelectionColor );

				// Preview texture
				var maxPreviewTextureSize = size - EditorGUIUtility.singleLineHeight;
				var previewTextureSize = Mathf.Min( previewTexture.height, maxPreviewTextureSize );

				var previewRect = new Rect( x + ( size - previewTextureSize ) * 0.5f, y, size, previewTextureSize );
				GUI.Label( previewRect, previewTexture );

				// Centered label
				var labelStyle = isSelected ? EditorStyles.whiteLabel : EditorStyles.label;
				var labelContent = new GUIContent( a.name );
				var labelTextSize = labelStyle.CalcSize( labelContent );
				labelTextSize.x = Mathf.Min( labelTextSize.x, size );

				var labelRect = new Rect( x + ( size - labelTextSize.x ) * 0.5f, y + previewTextureSize, size, EditorGUIUtility.singleLineHeight );
				GUI.Label( labelRect, labelContent, labelStyle );
			}
			GUILayout.EndArea();

			// Clear selection
			if( clickCount > 0 && !clickedItem )
			{
				Selection.objects = new Object[0];
				Repaint();
			}

			// Delete
			if( eventType == EventType.KeyUp && currentEvent.keyCode == KeyCode.Delete )
			{
				Undo.RecordObject( references, UndoString );
				for( int i = 0; i < references.assets.Count; i++ )
				{
					var a = references.assets[i];
					if( Selection.Contains( a ) )
						references.assets.RemoveAt( i );
				}
				EditorUtility.SetDirty( references );

				Repaint();
			}

			// Drag and drop
			if( eventType == EventType.MouseDrag )
			{
				var selected = Selection.objects;
				if( selected.Length > 0 )
				{
					DragAndDrop.PrepareStartDrag();
					DragAndDrop.objectReferences = selected;
					DragAndDrop.StartDrag( DragString );
					Event.current.Use();
				}
			}
			else if( eventType == EventType.DragUpdated || eventType == EventType.DragPerform )
			{
				bool anyFromProject = DragAndDrop.objectReferences.Any( o => !string.IsNullOrEmpty( AssetDatabase.GetAssetPath( o ) ) );
				if( anyFromProject )
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

				if( eventType == EventType.DragPerform )
				{
					DragAndDrop.AcceptDrag();

					var x = Mathf.FloorToInt( mousePosition.x / size );
					var y = Mathf.FloorToInt( mousePosition.y / size );
					var index = y * columnCount + x;

					Undo.RecordObject( references, UndoString );

					foreach( var o in DragAndDrop.objectReferences )
					{
						if( string.IsNullOrEmpty( AssetDatabase.GetAssetPath( o ) ) )
							continue;

						if( references.assets.Contains( o ) )
							references.assets.Remove( o );

						if( index >= references.assets.Count )
							references.assets.Add( o );
						else
							references.assets.Insert( index, o );
						index++;
					}

					EditorUtility.SetDirty( references );
				}

				Event.current.Use();
			}
		}
	}
}