using System.IO;
using BitStrap;
using UnityEditor;
using UnityEngine;

public class CustomAssetModificationProcessor : UnityEditor.AssetModificationProcessor
{
    private const string errorMessage =
        "You cannot Rename or Delete a BitPipe folder normally! Please use the BitPipe Tool Menu for that!";

    private static AssetMoveResult OnWillMoveAsset( string sourcePath, string destinationPath )
    {
        var isFile = File.Exists( sourcePath );
        if( isFile )
            return AssetMoveResult.DidNotMove;

        if( !File.Exists( BitFolderManager.BitFolderJsonPath ) )
            return AssetMoveResult.DidNotMove;

        sourcePath = sourcePath.Replace( "\\", "/" );
        var bitFolder = BitFolderManager.LoadBitFolderFromJson();
        var folderExists = BitFolderManager.CheckFolderPathExists( bitFolder, sourcePath );

        if( folderExists )
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

        if( !File.Exists( BitFolderManager.BitFolderJsonPath ) )
            return AssetDeleteResult.DidNotDelete;

        assetPath = assetPath.Replace( "\\", "/" );
        var bitFolder = BitFolderManager.LoadBitFolderFromJson();
        var folderExists = BitFolderManager.CheckFolderPathExists( bitFolder, assetPath );
        
        Debug.Log( folderExists );

        if( folderExists )
        {
            Debug.LogError( errorMessage );
            return AssetDeleteResult.FailedDelete;
        }

        return AssetDeleteResult.DidNotDelete;
    }
}