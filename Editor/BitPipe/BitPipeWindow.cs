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
        private const string JsonRelativePath = "Assets/project_structure.json";
        [NonSerialized] public BitFolder bitFolder;
        [NonSerialized] private TextAsset jsonAsset;

        public static bool isRenamingWithBitPipe;

        private SerializedObject so;

        [MenuItem( "Window/BitStrap/BitPipe" )]
        public static void OpenFolderStructure() => GetWindow<BitPipeWindow>();

        private void OnEnable()
        {
            so = new SerializedObject( this );
            bitFolder = BitFolderManager.LoadBitFolderFromJson();
        }

        private void DrawBitFolder( BitFolder bitFolder, string path )
        {
            string bitFolderFolderName = bitFolder.folderName;
            if( bitFolderFolderName == null )
                bitFolderFolderName = "";

            var constructedPath = Path.Combine( path, bitFolderFolderName );

            using( Horizontal.Do() )
            {
                bitFolder.isExpanded =
                    EditorGUILayout.Toggle( bitFolder.isExpanded, EditorStyles.foldoutHeader, GUILayout.Width( 16 ) );


                bool directoryExists = Directory.Exists( Path.Combine( constructedPath ) );
                bool disabled = directoryExists && bitFolderFolderName != "" && bitFolder.locked;

                if( directoryExists )
                {
                    if( GUILayout.Button( new GUIContent( "R", "Rename this Pipeline Folder" ),
                        GUILayout.Width( 25 ) ) )
                    {
                        bitFolder.locked = !bitFolder.locked;
                    }
                }

                // TextField for typing the name of the folders
                using( DisabledGroup.Do( disabled ) )
                {
                    EditorGUI.BeginChangeCheck();

                    bitFolder.folderName = EditorGUILayout.DelayedTextField( bitFolderFolderName );

                    // If player has renamed an existing directory here in BitPipe, rename the folder as well
                    if( EditorGUI.EndChangeCheck() && directoryExists )
                    {
                        isRenamingWithBitPipe = true;
                        
                        // Remove Whitespaces in case a smart guy tries to do it
                        if( !string.IsNullOrEmpty( bitFolder.folderName ) )
                            bitFolder.folderName = bitFolder.folderName.Replace( " ", "" );

                        var destPath = Path.Combine( path, bitFolder.folderName );
                        if( Directory.Exists( destPath ) )
                            if( EditorUtility.DisplayDialog(
                                "Are you sure?",
                                $"This will replace\n{constructedPath} with {destPath}!\nYou will DESTROY all files inside {destPath}, proceed?",
                                "Yes", "No" ) )
                            {
                                FileUtil.ReplaceDirectory( constructedPath, destPath );
                            }
                            else
                                return;
                        else
                            FileUtil.MoveFileOrDirectory( constructedPath, destPath );

                        FileUtil.DeleteFileOrDirectory( constructedPath + ".meta" );
                        FileUtil.DeleteFileOrDirectory( constructedPath );
                        AssetDatabase.Refresh();
                        bitFolder.locked = true;
                    }
                }


                if( GUILayout.Button( new GUIContent( "+", "Create new Pipeline Folder" ), GUILayout.Width( 25 ) ) )
                {
                    bitFolder.childFolders.Add( new BitFolder() );
                }

                if( GUILayout.Button( new GUIContent( "-", "Delete this Pipeline Folder" ), GUILayout.Width( 25 ) ) )
                {
                    if( EditorUtility.DisplayDialog(
                        "Are you sure?",
                        $"Do you REALLY want to delete {bitFolderFolderName}?",
                        "Yes", "No" ) )
                    {
                        bitFolder.markedForDeletion = true;
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

            if( bitFolder.isExpanded )
            {
                foreach( var folder in bitFolder.childFolders )
                {
                    DrawBitFolder( folder, constructedPath );
                }
            }


            for( var index = bitFolder.childFolders.Count - 1; index >= 0; index-- )
            {
                var folder = bitFolder.childFolders[index];
                if( folder.markedForDeletion )
                    bitFolder.childFolders.Remove( folder );
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

            if(!BitFolderManager.CheckFolderNameExists( bitFolder, "Scenes" ))
            {
                GUILayout.Space( 20 );
                EditorGUILayout.HelpBox( "You need a Folder with the name \'Scenes\' for BitPipe to work correctly!",
                    MessageType.Warning, true );
            }
            
            GUILayout.Space( 20 );
            if( GUILayout.Button( "Generate Folders", GUILayout.Height( 35 ) ) )
            {
                GenerateFolders( bitFolder, Application.dataPath );
                AssetDatabase.Refresh();
            }

            so.ApplyModifiedProperties();
        }

        private void GenerateFolders( BitFolder bitFolder, string parentPath )
        {
            var path = Path.Combine( parentPath, bitFolder.folderName );
            // Debug.Log( "Generated Path: " + path );
            if( !Directory.Exists( path ) )
                Directory.CreateDirectory( path );

            foreach( var childFolder in bitFolder.childFolders )
            {
                GenerateFolders( childFolder, path );
            }
        }

        private void UpdateJson()
        {
            SortBitFolders( bitFolder );

            File.WriteAllText( AssetDatabase.GetAssetPath( jsonAsset ), JsonUtility.ToJson( bitFolder, true ) );
            EditorUtility.SetDirty( jsonAsset );
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset( JsonRelativePath );
        }

        private void SortBitFolders( BitFolder bitFolder )
        {
            List<BitFolder> sortedFolders = bitFolder.childFolders.OrderBy( o => o.folderName ).ToList();
            bitFolder.childFolders = sortedFolders;

            foreach( var childFolder in bitFolder.childFolders )
            {
                SortBitFolders( childFolder );
            }
        }
    }
}