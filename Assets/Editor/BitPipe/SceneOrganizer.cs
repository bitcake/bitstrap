using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BitStrap
{
    [InitializeOnLoad]
    class SceneOrganizer
    {
        private const string BitPipeSettingsPath = "Assets/Editor/BitPipe/BitPipeSettings.asset";
        private static BitPipeSettings bitPipeSettings;

        
        static SceneOrganizer()
        {
            EditorSceneManager.sceneOpened += CheckForPersistentScene;
        }

        static void CheckForPersistentScene( Scene scene, OpenSceneMode mode )
        {
            EnsureData();
            
            if( !File.Exists( BitFolderManager.BitFolderJsonPath ) )
            {
                Debug.LogWarning( "Please use BitPipe to create your Folder Structure and organize Scene files" );
                return;
            }
            
            var activeScene = SceneManager.GetActiveScene();
            var parentDirName = Directory.GetParent( scene.path )?.Name;
            var scenePathDir = Directory.GetParent( scene.path )?.FullName;
            scenePathDir = StringHelper.AbsoluteToRelativePath( scenePathDir );

            var pureSceneName = StringHelper.RemoveSuffixFromString( scene.name );

            var splitScene = scene.name.Split('_');
            if( !String.Equals( splitScene.Last(), bitPipeSettings.persistentSceneName,
                StringComparison.OrdinalIgnoreCase ) )
                return;

            if( pureSceneName != parentDirName )
            {
                var bitFolder = BitFolderManager.LoadBitFolderFromJson();
                var folderExists = BitFolderManager.GetBitPipeFolderPath( bitFolder, "Scenes", out var outFolderPath );

                if( !folderExists )
                {
                    Debug.LogError( $"Cannot automatically organize the Scene: {activeScene.name}! <Scenes> folder not found, please create one using BitPipe" );
                    return;
                }
                
                if( !AssetDatabase.IsValidFolder( Path.Combine( outFolderPath, pureSceneName ) ) )
                    AssetDatabase.CreateFolder( outFolderPath, pureSceneName );

                AssetDatabase.MoveAsset( scene.path, Path.Combine( outFolderPath, pureSceneName, scene.name + ".unity" ) );
                scenePathDir = Directory.GetParent( scene.path )?.FullName;
                scenePathDir = StringHelper.AbsoluteToRelativePath( scenePathDir );
            }

            var sceneTypesList = bitPipeSettings.GetSceneTypesToCreate();
            foreach( var sceneType in bitPipeSettings.GetSceneTypesToCreate() )
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

        private static void EnsureData()
        {
            if (bitPipeSettings != null) 
                return;
			
            bitPipeSettings = AssetDatabase.LoadAssetAtPath<BitPipeSettings>(BitPipeSettingsPath);
            if (bitPipeSettings == null)
            {
                var debug = ScriptableObject.CreateInstance<BitPipeSettings>(  );
                AssetDatabase.CreateAsset(debug, BitPipeSettingsPath);
                AssetDatabase.SaveAssets();
                bitPipeSettings =
                    AssetDatabase.LoadAssetAtPath<BitPipeSettings>(BitPipeSettingsPath);
            }
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