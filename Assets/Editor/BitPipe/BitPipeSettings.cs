using System.Collections.Generic;
using UnityEngine;

namespace BitStrap
{
    public sealed class BitPipeSettings : ScriptableObject
    {
        public static string sPersistentSceneName = "P";
        public static string sGameplaySceneName = "Gameplay";
        public static string sGeometrySceneName = "Geometry";
        public static string sLightingSceneName = "Lighting";
        public static string sAudioSceneName = "Audio";

        public bool createPersistentScene = true;
        public string persistentSceneName = "P";
        public bool createGameplayScene;
        public string gameplaySceneName = "Gameplay";
        public bool createGeometryScene;
        public string geometrySceneName = "Geometry";
        public bool createLightingScene;
        public string lightingSceneName = "Lighting";
        public bool createAudioScene;
        public string audioSceneName = "Audio";

        public static string[] sceneTypeNames = new[]
            { sPersistentSceneName, sGameplaySceneName, sGeometrySceneName, sLightingSceneName, sAudioSceneName };

        public List<string> GetSceneTypesToCreate()
        {
            List<string> sceneTypes = new List<string>();

            if( createPersistentScene )
                sceneTypes.Add( persistentSceneName );
            if( createGameplayScene )
                sceneTypes.Add( gameplaySceneName );
            if( createGeometryScene )
                sceneTypes.Add( geometrySceneName );
            if( createLightingScene )
                sceneTypes.Add( lightingSceneName );
            if( createAudioScene )
                sceneTypes.Add( audioSceneName );

            return sceneTypes;
        }
    }
}