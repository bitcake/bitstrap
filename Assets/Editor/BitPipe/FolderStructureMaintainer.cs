using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class FolderStructureMaintainer : AssetPostprocessor
{
    private const string jsonPath = "Assets/Editor/BitPipe/project_structure.json";
    
    private static void OnPostprocessAllAssets( string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths )
    {
        foreach( var asset in movedAssets )
        {
            // Checks if asset is a file or directory
            FileAttributes attr = File.GetAttributes( asset );
            if( ( attr & FileAttributes.Directory ) == FileAttributes.Directory )
            {
                // Debug.Log( $"Asset que foi modificado {asset}" );
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

    static bool CompareFolders( BitFolder bitFolder, string parentPath, string modifiedAssetPath )
    {
        var constructedBitfolderPath = parentPath + "/" + bitFolder.folderName;
        var leafModifiedAssetPath = modifiedAssetPath.Split( "/" ).Last();
        
        if(!Directory.Exists( constructedBitfolderPath ))
        {
            // The Hacking below is because Unity isn't reliable
            // Sometimes it'll accuse the Child folders as the ones being renamed
            
            Debug.Log( $"Folder Name: {bitFolder.folderName} and Lead Dir: {leafModifiedAssetPath}" );
            
            var splitParentPath = constructedBitfolderPath.Split( "/" );
            var splitModifiedAssetPath = modifiedAssetPath.Split( "/" );
            List<String> finalWrongPath = new List<string>();
            List<String> finalCorrectPath = new List<string>();
            string constructedFinalWrongPath = null;
            string constructedFinalCorrectPath = null;
            
            if(bitFolder.folderName == leafModifiedAssetPath)
            {
                for( int i = 0; i < splitParentPath.Length; i++ )
                {
                    if( splitParentPath[i] == splitModifiedAssetPath[i] )
                    {
                        finalWrongPath.Add( splitModifiedAssetPath[i] );
                        finalCorrectPath.Add( splitParentPath[i] );
                    }
                    else
                    {
                        finalWrongPath.Add( splitModifiedAssetPath[i] );
                        finalCorrectPath.Add( splitParentPath[i] );
                        constructedFinalWrongPath = string.Join( "/", finalWrongPath.ToArray() );
                        constructedFinalCorrectPath= string.Join( "/", finalCorrectPath.ToArray() );
                        break;
                    }
                }
                
                Debug.LogError( $" Correct Path: {constructedFinalCorrectPath} | Wrong Path: {constructedFinalWrongPath} " );
                RenameAndDeleteDirs( constructedFinalWrongPath, constructedFinalCorrectPath );
                return true;
            }
        }

        foreach( var childFolder in bitFolder.childFolders )
        {
            if( CompareFolders( childFolder, constructedBitfolderPath, modifiedAssetPath ) )
                return true;
        }
        return false;
    }

    private static void RenameAndDeleteDirs( string constructedFinalWrongPath, string constructedFinalCorrectPath )
    {
        FileUtil.MoveFileOrDirectory( constructedFinalWrongPath, constructedFinalCorrectPath );
        FileUtil.DeleteFileOrDirectory( constructedFinalWrongPath + ".meta" );
        FileUtil.DeleteFileOrDirectory( constructedFinalWrongPath );
        AssetDatabase.Refresh();
    }
}