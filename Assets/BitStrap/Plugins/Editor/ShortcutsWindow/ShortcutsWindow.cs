using UnityEngine;
using UnityEditor;

namespace BitStrap
{
	public sealed class ShortcutsWindow : EditorWindow
	{
		private const string UndoString = "ShortcutsWindow_Undo";

		private Option<ShortcutsReferences> shortcutsReferences;

		[MenuItem( "Window/BitStrap/Shortcuts Window" )]
		private static void OpenWindow()
		{
			var window = GetWindow<ShortcutsWindow>( typeof( ShortcutsWindow ).Name );
			window.Show();
		}

		private void OnGUI()
		{
			ShortcutsReferences references;
			if( !shortcutsReferences.OrElse( () => AssetDatabaseHelper.FindAssetOfType<ShortcutsReferences>() ).TryGet( out references ) )
			{
				return;
			}

			var indexToDelete = -1;
			for( int i = 0; i < references.references.Length; i++ )
			{
				var r = references.references[i];

				using( Horizontal.Do() )
				{
					if( GUILayout.Button( r.name ) )
					{
						Selection.activeObject = r;
						EditorGUIUtility.PingObject( r );
					}

					GUI.backgroundColor = Color.red;
					if( GUILayout.Button( "X", GUILayout.Width( 32.0f ) ) )
						indexToDelete = i;
					GUI.backgroundColor = Color.white;
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