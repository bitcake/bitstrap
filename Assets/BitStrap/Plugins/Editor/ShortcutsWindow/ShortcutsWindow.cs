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

		[MenuItem( "Window/BitStrap/Shortcuts Window" )]
		private static void OpenWindow()
		{
			var window = GetWindow<ShortcutsWindow>( "ShortcutsWindow" );
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
			const float width = 80.0f;
			const float height = 80.0f;

			var currentEvent = Event.current;
			var eventType = currentEvent.type;
			var clickCount = currentEvent.clickCount;
			var mousePosition = currentEvent.mousePosition;

			var columnCount = Mathf.Max( Mathf.FloorToInt( areaRect.width / width ), 1 );

			for( int i = 0; i < references.assets.Length; i++ )
			{
				var a = references.assets[i];
				if( a == null )
					continue;

				var previewTexture = AssetPreview.GetMiniThumbnail( a );
				var x = ( i % columnCount ) * width + padding * 0.5f;
				var y = ( i / columnCount ) * height + padding * 0.5f;

				var totalRect = new Rect( x, y, width, height );
				var totalRectAbsolute = new Rect( totalRect );
				totalRectAbsolute.position = position.position;

				if( clickCount > 0 && totalRect.Contains( mousePosition ) )
				{
					Selection.activeObject = a;
					Repaint();

					if( clickCount > 1 )
						AssetDatabase.OpenAsset( a );
				}

				bool isSelected = Selection.Contains( a );
				if( isSelected )
					EditorGUI.DrawRect( totalRect, Colors.SelectionColor );

				var previewRect = new Rect( x + ( width - previewTexture.width ) * 0.5f, y, width, previewTexture.height );
				GUI.Label( previewRect, previewTexture );

				var labelRect = new Rect( x, y + previewTexture.height, width, EditorGUIUtility.singleLineHeight );
				var labelStyle = isSelected ? EditorStyles.whiteLabel : EditorStyles.label;
				GUI.Label( labelRect, a.name, labelStyle );
			}
			GUILayout.EndArea();

			// Delete
			if( eventType == EventType.KeyUp && currentEvent.keyCode == KeyCode.Delete )
			{
				Undo.RecordObject( references, UndoString );
				for( int i = 0; i < references.assets.Length; i++ )
				{
					var a = references.assets[i];
					if( Selection.Contains( a ) )
						ArrayUtility.RemoveAt( ref references.assets, i );
				}

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
					Undo.RecordObject( references, UndoString );

					foreach( var o in DragAndDrop.objectReferences )
					{
						if( !string.IsNullOrEmpty( AssetDatabase.GetAssetPath( o ) ) && !ArrayUtility.Contains( references.assets, o ) )
							ArrayUtility.Add( ref references.assets, o );
					}
				}

				Event.current.Use();
			}
		}
	}
}