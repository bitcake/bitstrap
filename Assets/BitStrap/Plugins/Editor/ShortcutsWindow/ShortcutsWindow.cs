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
			const float margin = 4.0f;

			var clickCount = Event.current.clickCount;
			var mousePosition = Event.current.mousePosition;

			for( int i = 0; i < references.references.Length; i++ )
			{
				var r = references.references[i];

				var previewTexture = AssetPreview.GetMiniThumbnail( r );
				var width = previewTexture.width + padding;
				var previewHeight = previewTexture.height;
				var labelHeight = EditorGUIUtility.singleLineHeight;
				var x = i * ( width + margin ) + padding * 0.5f;
				var y = 0.0f;

				var totalRect = new Rect( x, y, width, previewHeight + labelHeight );
				var totalRectAbsolute = new Rect( totalRect );
				totalRectAbsolute.position = position.position;

				if( clickCount > 0 && totalRect.Contains( mousePosition ) )
				{
					Selection.activeObject = r;
					Repaint();
				}

				bool isSelected = Selection.Contains( r );
				if( isSelected )
					EditorGUI.DrawRect( totalRect, Colors.SelectionColor );

				var previewRect = new Rect( x, y, width, previewHeight );
				GUI.Label( previewRect, previewTexture );

				var labelRect = new Rect( x, y + previewHeight, width, labelHeight );
				var labelStyle = isSelected ? EditorStyles.whiteLabel : EditorStyles.label;
				GUI.Label( labelRect, r.name, labelStyle );
			}
			GUILayout.EndArea();
			return;

			var indexToDelete = -1;
			for( int i = 0; i < references.references.Length; i++ )
			{
				var r = references.references[i];
				var previewTexture = AssetPreview.GetMiniThumbnail( r );
				var width = previewTexture.width;

				using( Horizontal.Do( GUILayout.Width( width ) ) )
				{
					using( Vertical.Do() )
					{
						GUILayout.Label( previewTexture );
						GUILayout.Label( r.name, GUILayout.Width( width ) );

						//*
						if( GUILayout.Button( previewTexture, EditorStyles.label ) )
						{
							Selection.activeObject = r;
							EditorGUIUtility.PingObject( r );
						}
						//*/
					}

					//*
					GUI.backgroundColor = Color.red;
					if( GUILayout.Button( "X", GUILayout.Width( 32.0f ) ) )
						indexToDelete = i;
					GUI.backgroundColor = Color.white;
					//*/
				}
			}

			if( indexToDelete >= 0 )
			{
				Undo.RecordObject( references, UndoString );
				ArrayUtility.RemoveAt( ref references.references, indexToDelete );
			}

			// Drag and drop
			var eventType = Event.current.type;
			if( eventType == EventType.DragUpdated || eventType == EventType.DragPerform )
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
						if( !string.IsNullOrEmpty( AssetDatabase.GetAssetPath( o ) ) )
							ArrayUtility.Add( ref references.references, o );
					}
				}

				Event.current.Use();
			}
		}
	}
}