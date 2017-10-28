using System.IO;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    public static class ScriptableObjectCreator
    {
        [MenuItem( "Assets/Create/Scriptable Object Instance", true )]
        public static bool ValidateCreateScriptableObjectInstance()
        {
            var monoScript = Selection.activeObject as MonoScript;
            return monoScript != null && typeof( ScriptableObject ).IsAssignableFrom( monoScript.GetClass() );
        }

        [MenuItem( "Assets/Create/Scriptable Object Instance" )]
        public static void CreateScriptableObjectInstance()
        {
            var script = Selection.activeObject as MonoScript;
            if( !ValidateCreateScriptableObjectInstance() || script == null )
                return;

            var instance = ScriptableObject.CreateInstance( script.GetClass() );
            string path = AssetDatabase.GetAssetPath( script );
            path = Path.ChangeExtension( path, ".asset" );

            AssetDatabase.CreateAsset( instance, path );
            AssetDatabase.SaveAssets();
        }
    }
}
