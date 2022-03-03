using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class FolderStructureMaintainer : AssetPostprocessor
{
    private const string jsonPath = "Assets/Editor/BitPipe/project_structure.json";
    
    private static void OnPostprocessAllAssets( string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths )
    {
        foreach( var asset in importedAssets )
        {
            // Checks if asset is a file or directory
            FileAttributes attr = File.GetAttributes( asset );
            if( ( attr & FileAttributes.Directory ) == FileAttributes.Directory )
            {
                Debug.Log( $"Asset que foi modificado {asset}" );
                var jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>( jsonPath );
                var bitFolder = JsonUtility.FromJson<BitFolder>( jsonAsset.text );
                if( CompareFolders( bitFolder, "Assets", asset ) )
                {
                    Debug.Log( "Renomeiandow" );
                    break;
                }
            }
        }
    }

    static bool CompareFolders( BitFolder folder, string parentPath, string dirToCompare )
    {
        var path = parentPath + "/" + folder.folderName;
        // Debug.Log( $"Path is the same? {path == dirToCompare}" );
        if(!Directory.Exists( path ))
        {
            // Debug.LogError( "YOU CANNOT MOVE OR RENAME THIS FOLDER, BITCH" );
            Debug.LogError( $"Modified Asset Path: {dirToCompare} | Correct Path: {path} " );
            FileUtil.ReplaceDirectory( dirToCompare, path );
            FileUtil.DeleteFileOrDirectory( dirToCompare + ".meta" );
            FileUtil.DeleteFileOrDirectory( dirToCompare );
            AssetDatabase.Refresh();
            return true;
        }

        foreach( var childFolder in folder.childFolders )
        {
            if( CompareFolders( childFolder, path, dirToCompare ) )
                return true;
        }
        return false;
    }
}