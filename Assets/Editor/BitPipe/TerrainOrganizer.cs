using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace BitStrap
{
    [InitializeOnLoad]
    class TerrainOrganizer
    {
        static TerrainOrganizer()
        {
            EditorSceneManager.sceneOpened += TerrainOrganizerSceneOpenedCallback;
            EditorApplication.hierarchyChanged += TerrainOrganizerHierarchyChangeCallback;
        }

        private static void TerrainOrganizerSceneOpenedCallback( Scene scene, OpenSceneMode mode )
        {
            var terrains = Object.FindObjectsOfType<Terrain>();
            TerrainRenamer( terrains );
        }

        private static void TerrainOrganizerHierarchyChangeCallback()
        {
            var terrains = Object.FindObjectsOfType<Terrain>();
            TerrainRenamer( terrains );
        }

        private static void TerrainRenamer( Terrain[] terrains )
        {
            if (terrains == null || terrains.Length == 0)
                return;
            
            var activeScene = SceneManager.GetActiveScene();
            foreach( var terrain in terrains )
            {
                if( terrain.terrainData == null)
                    continue;
                
                if( terrain.terrainData.name == "New Terrain" )
                {
                    var terrainOriginalPath = AssetDatabase.GetAssetPath( terrain.terrainData );
                    var terrainTargetName = StringHelper.RemoveSuffixFromString( activeScene.name ) + "_Terrain.asset";
                    var activeScenePath = Path.GetDirectoryName(activeScene.path);

                    var resourcesFolder = BitPipeHelper.CreateResourcesFolder( activeScenePath );

                    var terrainNewPath = Path.Combine(resourcesFolder, terrainTargetName);
                    terrainNewPath = AssetDatabase.GenerateUniqueAssetPath(terrainNewPath);
                    
                    AssetDatabase.MoveAsset( terrainOriginalPath, terrainNewPath );
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }
    }
}