using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace BitStrap
{
    [InitializeOnLoad]
    class SceneOrganizer
    {
        static SceneOrganizer()
        {
            EditorSceneManager.sceneOpened += CheckForPersistentScene;
        }

        static void CheckForPersistentScene( Scene scene, OpenSceneMode mode )
        {
            var activeScene = SceneManager.GetActiveScene();
            var parentDirName = Directory.GetParent( scene.path )?.Name;
            var scenePathDir = Directory.GetParent( scene.path )?.FullName;
            scenePathDir = StringHelper.AbsoluteToRelativePath( scenePathDir );

            var pureSceneName = StringHelper.RemoveSuffixFromString( scene.name );

            var splitScene = scene.name.Split( "_" );
            if( !String.Equals( splitScene.Last(), BitPipeSettings.persistentSceneName,
                StringComparison.OrdinalIgnoreCase ) )
                return;

            if( pureSceneName != parentDirName )
            {
                var bitFolder = BitFolderManager.LoadBitFolderFromJson();
                var folderPath = BitFolderManager.GetBitPipeFolderPath( bitFolder, "Scenes" );

                if( !AssetDatabase.IsValidFolder( Path.Join( folderPath, pureSceneName ) ) )
                    AssetDatabase.CreateFolder( folderPath, pureSceneName );

                AssetDatabase.MoveAsset( scene.path, Path.Join( folderPath, pureSceneName, scene.name + ".unity" ) );
                scenePathDir = Directory.GetParent( scene.path )?.FullName;
                scenePathDir = StringHelper.AbsoluteToRelativePath( scenePathDir );
            }

            var sceneTypesList = BitPipeSettings.sceneTypeNames.ToList();
            foreach( var sceneType in BitPipeSettings.sceneTypeNames )
            {
                var matchingAssets = AssetDatabase.FindAssets( $"{pureSceneName}_{sceneType} t:scene" );
                foreach( var assetGUID in matchingAssets )
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath( assetGUID );
                    var filename = Path.GetFileNameWithoutExtension( assetPath );
                    var filenameExt = Path.GetFileName( assetPath );

                    var assetPathDir = Directory.GetParent( assetPath )?.FullName;
                    assetPathDir = StringHelper.AbsoluteToRelativePath( assetPathDir );

                    if( filename == $"{pureSceneName}_{sceneType}" )
                    {
                        sceneTypesList.Remove( sceneType );
                        if( assetPathDir != scenePathDir )
                            AssetDatabase.MoveAsset( Path.Combine( assetPathDir, filenameExt ),
                                Path.Combine( scenePathDir, filenameExt ) );

                        if( !CheckActiveScene( filename ) )
                            EditorSceneManager.OpenScene( Path.Combine( scenePathDir, filenameExt ),
                                OpenSceneMode.Additive );
                    }
                }
            }

            foreach( var remainingSceneType in sceneTypesList )
            {
                var newScene = EditorSceneManager.NewScene( NewSceneSetup.EmptyScene, NewSceneMode.Additive );
                newScene.name = $"{pureSceneName}_{remainingSceneType}";
                EditorSceneManager.SaveScene( newScene, Path.Combine( scenePathDir, newScene.name + ".unity" ) );
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SceneManager.SetActiveScene( activeScene );
        }

        private static bool CheckActiveScene( string filename )
        {
            var numOpenScenes = SceneManager.sceneCount;
            for( int i = 0; i < numOpenScenes; i++ )
            {
                var sceneAt = SceneManager.GetSceneAt( i );
                if( sceneAt.name == filename )
                    return true;
            }

            return false;
        }
    }
}