using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;
using Application = UnityEngine.Application;


namespace BitStrap
{
    [System.Serializable]
    public class BitFolder
    {
        public string folderName;
        [NonSerialized] public bool isExpanded = true;
        [NonSerialized] public bool locked = true;
        [NonSerialized] public bool markedForDeletion;
        public List<BitFolder> childFolders = new();
    }

    public class FolderStructure : EditorWindow
    {
        private const string jsonPath = "Editor/BitPipe/project_structure.json";
        private const string jsonRelativePath = "Assets/" + jsonPath;
        [NonSerialized] public BitFolder bitFolder;
        [NonSerialized] private TextAsset jsonAsset;

        public static bool isRenamingWithBitPipe;

        private SerializedObject so;

        [MenuItem( "Window/BitStrap/BitPipe" )]
        public static void OpenFolderStructure() => GetWindow<FolderStructure>();

        private void OnEnable()
        {
            so = new SerializedObject( this );
            jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>( jsonRelativePath );
            if( jsonAsset == null )
            {
                File.WriteAllText( Path.Combine( Application.dataPath, jsonPath ), "{}" );
                AssetDatabase.ImportAsset( jsonRelativePath );
                jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>( jsonRelativePath );
            }

            bitFolder = JsonUtility.FromJson<BitFolder>( jsonAsset.text );
        }

        private void DrawBitFolder( BitFolder bitFolder, string path )
        {
            EditorGUILayout.BeginHorizontal();
            bitFolder.isExpanded =
                EditorGUILayout.Toggle( bitFolder.isExpanded, EditorStyles.foldoutHeader, GUILayout.Width( 16 ) );

            if( bitFolder.folderName == null )
                bitFolder.folderName = "";
            var constructedPath = Path.Combine( path, bitFolder.folderName );
            bool directoryExists = Directory.Exists( Path.Combine( constructedPath ) );
            bool disabled = directoryExists && bitFolder.folderName != "" && bitFolder.locked;

            if( directoryExists )
            {
                if( GUILayout.Button( new GUIContent( "R", "Rename this Pipeline Folder" ), GUILayout.Width( 25 ) ) )
                {
                    bitFolder.locked = !bitFolder.locked;
                }
            }

            // TextField for typing the name of the folders
            using( DisabledGroup.Do( disabled ) )
            {
                EditorGUI.BeginChangeCheck();
                bitFolder.folderName = EditorGUILayout.DelayedTextField( bitFolder.folderName );
                // If player has renamed an existing directory here in BitPipe, rename the folder as well
                if( EditorGUI.EndChangeCheck() && directoryExists )
                {
                    // Remove Whitespaces in case a smart guy tries to do it
                    if( !string.IsNullOrEmpty( bitFolder.folderName ) )
                        bitFolder.folderName = bitFolder.folderName.Replace( " ", "" );
                    isRenamingWithBitPipe = true;
                    FileUtil.MoveFileOrDirectory( constructedPath, Path.Combine(path, bitFolder.folderName) );
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
                    $"Do you REALLY want to delete {bitFolder.folderName}?",
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

            EditorGUILayout.EndHorizontal();
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
            AssetDatabase.ImportAsset( jsonRelativePath );
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