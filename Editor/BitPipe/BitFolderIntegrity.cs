using System;
using System.IO;
using BitStrap;
using UnityEditor;
using UnityEngine;

public class CustomAssetModificationProcessor : AssetModificationProcessor
{
    private const string errorMessage =
        "You cannot Rename or Delete a BitPipe folder normally! Please use the BitPipe Tool Menu for that!";

    private static AssetMoveResult OnWillMoveAsset( string sourcePath, string destinationPath )
    {
        var isFile = File.Exists( sourcePath );
        if( isFile )
            return AssetMoveResult.DidNotMove;

        if (!File.Exists( BitFolderManager.BitFolderJsonPath ))
            return AssetMoveResult.DidNotMove;
        
        var dirName = Path.GetFileName( sourcePath );
        var bitFolder = BitFolderManager.LoadBitFolderFromJson();
        var folderNameExists = BitFolderManager.CheckFolderNameExists( bitFolder, dirName );

        if( folderNameExists )
        {
            Debug.LogError( errorMessage );
            return AssetMoveResult.FailedMove;
        }

        return AssetMoveResult.DidNotMove;
    }

    private static AssetDeleteResult OnWillDeleteAsset( string assetPath, RemoveAssetOptions options )
    {
        var isFile = File.Exists( assetPath );
        if( isFile )
            return AssetDeleteResult.DidNotDelete;
        
        if (!File.Exists( BitFolderManager.BitFolderJsonPath ))
            return AssetDeleteResult.DidDelete;

        var dirName = Path.GetFileName( assetPath );
        var bitFolder = BitFolderManager.LoadBitFolderFromJson();
        var folderNameExists = BitFolderManager.CheckFolderNameExists( bitFolder, dirName );

        if( folderNameExists )
        {
            Debug.LogError( errorMessage );
            return AssetDeleteResult.FailedDelete;
        }

        return AssetDeleteResult.DidNotDelete;
    }
}