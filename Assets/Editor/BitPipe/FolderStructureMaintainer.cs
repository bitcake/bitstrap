using System.IO;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    public class FolderStructureMaintainer : AssetPostprocessor
    {
        private const string jsonPath = "Assets/Editor/BitPipe/project_structure.json";

        
        private void OnPreprocessAsset()
        {
            Debug.Log( assetPath );
            // Checks if asset is a file or directory
            FileAttributes attr = File.GetAttributes( assetPath );
            
            if( ( attr & FileAttributes.Directory ) == FileAttributes.Directory )
            {
                var jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>( jsonPath );
                var bitFolder = JsonUtility.FromJson<BitFolder>( jsonAsset.text );
                CompareFolders( bitFolder, "Assets", assetPath );
            }
        }


        static void CompareFolders( BitFolder bitFolder, string parentPath, string modifiedAssetPath )
        {
            var folderPath = Path.Join( parentPath, bitFolder.folderName );

            if( !Directory.Exists( folderPath ) )
            {
                if( FolderStructure.isRenamingWithBitPipe )
                {
                    FolderStructure.isRenamingWithBitPipe = false;
                    return;
                }
                Undo.PerformUndo();
                Debug.LogError( "This folder is part of this Project's Pipeline, you cannot rename it! Please use BitPipe if you REALLY want to do it." );
                return;
            }
            foreach( var childFolder in bitFolder.childFolders )
                CompareFolders( childFolder, folderPath, modifiedAssetPath );
        }
    }
    
}
