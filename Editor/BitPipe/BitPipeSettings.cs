using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    public sealed class BitPipeSettings : ScriptableObject
    {
        public static string persistentSceneName = "P";
        public static string gameplaySceneName = "Gameplay";
        public static string geometrySceneName = "Geometry";
        public static string lightingSceneName = "Lighting";
        public static string audioSceneName = "Audio";

        public static string[] sceneTypeNames = new[]
            { persistentSceneName, gameplaySceneName, geometrySceneName, lightingSceneName, audioSceneName };
    }
}