using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;


namespace BitStrap
{
    public class BitPipeWindow : EditorWindow
    {
        [NonSerialized] private BitFolder bitFolder;
        [NonSerialized] private TextAsset jsonAsset;

        private SerializedObject so;

        [MenuItem( "Window/BitStrap/BitPipe" )]
        public static void OpenFolderStructure() => GetWindow<BitPipeWindow>();

        private void OnEnable()
        {
            so = new SerializedObject( this );
            bitFolder = BitFolderManager.LoadBitFolderFromJson();
        }

        private void DrawBitFolder( BitFolder bitFolderToDraw, string path )
        {
            var originalBitFolderName = bitFolderToDraw.folderName;
            if( originalBitFolderName is null )
            {
                originalBitFolderName = "";
                bitFolderToDraw.locked = false;
            }
            var constructedPath = Path.Combine( path, originalBitFolderName );

            using( Horizontal.Do() )
            {
                bitFolderToDraw.isExpanded =
                    EditorGUILayout.Toggle( bitFolderToDraw.isExpanded, EditorStyles.foldoutHeader,
                        GUILayout.Width( 16 ) );

                bool directoryExists = Directory.Exists( constructedPath );
                bool disabled = directoryExists && bitFolderToDraw.locked;

                // TextField for typing the name of the folders
                using( DisabledGroup.Do( disabled ) )
                {
                    EditorGUI.BeginChangeCheck();

                    var newFolderName = EditorGUILayout.DelayedTextField( originalBitFolderName );

                    // If player has renamed an existing directory here in BitPipe, rename the folder as well
                    if( EditorGUI.EndChangeCheck() )
                    {
                        // Remove Whitespaces in case a smart guy tries to do it
                        newFolderName = newFolderName.Replace( " ", "" );

                        if( string.IsNullOrWhiteSpace( newFolderName ) )
                            newFolderName = "NewFolder";

                        if( BitFolderManager.CheckFolderNameExists( bitFolder, newFolderName ) )
                        {
                            if(bitFolderToDraw.parentFolder.childFolders.Any(f => f.folderName == newFolderName))
                            {
                                bitFolderToDraw.folderName = originalBitFolderName;
                                bitFolderToDraw.locked = true;
                                return;
                            }                            
                        }

                        bitFolderToDraw.folderName = newFolderName;

                        var destPath = Path.Combine( path, bitFolderToDraw.folderName );
                        if( !directoryExists )
                            return;

                        var windowTitle = "Are you sure?";
                        var message =
                            "This will replace\n" +
                            $"{constructedPath} with {destPath}!\n" +
                            $"You will DESTROY all files inside {destPath}, proceed?";

                        if( Directory.Exists( destPath ) )
                            if( !EditorUtility.DisplayDialog( windowTitle, message, "Yes", "No" ) )
                            {
                                bitFolderToDraw.folderName = originalBitFolderName;
                                bitFolderToDraw.locked = true;
                                return;
                            }

                        FileUtil.ReplaceDirectory( constructedPath, destPath );
                        FileUtil.DeleteFileOrDirectory( constructedPath + ".meta" );
                        FileUtil.DeleteFileOrDirectory( constructedPath );
                        AssetDatabase.Refresh();
                        bitFolderToDraw.locked = true;
                    }
                }

                if( directoryExists )
                {
                    if( GUILayout.Button( new GUIContent( "R", "Rename this Pipeline Folder" ),
                        GUILayout.Width( 25 ) ) )
                    {
                        bitFolderToDraw.locked = !bitFolderToDraw.locked;
                    }
                }

                if( GUILayout.Button( new GUIContent( "+", "Create new Pipeline Folder" ), GUILayout.Width( 25 ) ) )
                {
                    bitFolderToDraw.childFolders.Add( new BitFolder() { folderName = "NewFolder" } );
                }

                if( GUILayout.Button( new GUIContent( "-", "Delete this Pipeline Folder" ), GUILayout.Width( 25 ) ) )
                {
                    if( EditorUtility.DisplayDialog(
                        "Are you sure?",
                        $"Do you REALLY want to delete {originalBitFolderName}?",
                        "Yes", "No" ) )
                    {
                        bitFolderToDraw.markedForDeletion = true;
                        if( directoryExists )
                        {
                            FileUtil.DeleteFileOrDirectory( constructedPath + ".meta" );
                            FileUtil.DeleteFileOrDirectory( constructedPath );
                            AssetDatabase.Refresh();
                        }
                    }
                }
            }

            EditorGUI.indentLevel++;

            if( bitFolderToDraw.isExpanded )
            {
                foreach( var folder in bitFolderToDraw.childFolders )
                {
                    DrawBitFolder( folder, constructedPath );
                }
            }


            for( var index = bitFolderToDraw.childFolders.Count - 1; index >= 0; index-- )
            {
                var folder = bitFolderToDraw.childFolders[index];
                if( folder.markedForDeletion )
                    bitFolderToDraw.childFolders.Remove( folder );
            }

            EditorGUI.indentLevel--;
        }

        private void OnGUI()
        {
            so.Update();
            EditorGUILayout.LabelField( "BitPipe", EditorStyles.largeLabel );
            EditorGUI.BeginChangeCheck();
            DrawBitFolder( bitFolder, "Assets/" );
            if( EditorGUI.EndChangeCheck() )
            {
                UpdateJson();
            }

            // if( !BitFolderManager.CheckFolderNameExists( bitFolder, "Scenes" ) )
            // {
            //     GUILayout.Space( 20 );
            //     EditorGUILayout.HelpBox( "You need a Folder with the name \'Scenes\' for BitPipe to work correctly!",
            //         MessageType.Warning, true );
            // }

            GUILayout.Space( 20 );
            if( GUILayout.Button( "Generate Folders", GUILayout.Height( 35 ) ) )
            {
                try
                {
                    GenerateFolders( bitFolder, Application.dataPath );
                    AssetDatabase.Refresh();
                }
                catch( IOException )
                {
                    Debug.LogError(
                        $"You're using invalid characters in a BitPipe Folder, stupid. Please rename it before trying again..." );
                }
            }

            so.ApplyModifiedProperties();
        }

        private void GenerateFolders( BitFolder bitFolderToGenerate, string parentPath )
        {
            var path = Path.Combine( parentPath, bitFolderToGenerate.folderName );
            // Debug.Log( "Generated Path: " + path );
            if( !Directory.Exists( path ) )
                Directory.CreateDirectory( path );

            foreach( var childFolder in bitFolderToGenerate.childFolders )
            {
                GenerateFolders( childFolder, path );
            }
        }

        private void UpdateJson()
        {
            SortBitFolders( bitFolder );
            File.WriteAllText( BitFolderManager.BitFolderJsonPath, JsonUtility.ToJson( bitFolder, true ) );
            // EditorUtility.SetDirty( jsonAsset );
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset( BitFolderManager.BitFolderJsonPath );
        }

        private void SortBitFolders( BitFolder bitFolderToSort )
        {
            List<BitFolder> sortedFolders = bitFolderToSort.childFolders.OrderBy( o => o.folderName ).ToList();
            bitFolderToSort.childFolders = sortedFolders;

            foreach( var childFolder in bitFolderToSort.childFolders )
            {
                SortBitFolders( childFolder );
            }
        }
    }
}