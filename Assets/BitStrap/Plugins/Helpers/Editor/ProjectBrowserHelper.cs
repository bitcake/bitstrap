using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    public static class HierarchyQuickCreate
    {
        /// <summary>
        /// Instantiate all prefabs found in project by the search string "findString".
        /// </summary>
        /// <param name="findString"></param>
        public static void InstantiatePrefab( string findString )
        {
            string[] assets = AssetDatabase.FindAssets( findString );

            if( assets.Length > 0 )
            {
                string asset = assets[0];
                if( !string.IsNullOrEmpty( asset ) )
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>( AssetDatabase.GUIDToAssetPath( asset ) );
                    if( prefab != null )
                    {
                        PrefabUtility.InstantiatePrefab( prefab );
                    }
                }
            }
        }
    }

    public static class ProjectBrowserFilters
    {
        /// <summary>
        /// Selects all assets in project window found by the search string "findString".
        /// </summary>
        /// <param name="findString"></param>
        /// <param name="searchFolders"></param>
        public static void SelectAssets( string findString, params string[] searchFolders )
        {
            string[] assetIds;
            if( searchFolders.Length > 0 )
                assetIds = AssetDatabase.FindAssets( findString, searchFolders );
            else
                assetIds = AssetDatabase.FindAssets( findString );

            Object[] assets = new Object[assetIds.Length];
            for( int i = 0; i < assetIds.Length; i++ )
            {
                assets[i] = AssetDatabase.LoadAssetAtPath( AssetDatabase.GUIDToAssetPath( assetIds[i] ), typeof( Object ) );
            }

            Selection.objects = assets;
        }
    }

    /// <summary>
    /// Bunch of helper methods to work with the project window.
    /// </summary>
    public static class ProjectBrowserHelper
    {
        private static System.Type projectBrowserType;
        private static MethodInfo setSearchMethod;
        private static object[] setSearchMethodArgs = new object[] { null };

        private static string[] projecBrowserTypeNames = new string[] {
            "UnityEditor.ProjectBrowser",
            "UnityEditor.ProjectWindow",
            "UnityEditor.ObjectBrowser"
        };

        /// <summary>
        /// Returns true if it was possible to reflect the SetSearch method
        /// from the project window class.
        /// </summary>
        public static bool HasSearchImplementation
        {
            get { return projectBrowserType != null && setSearchMethod != null; }
        }

        static ProjectBrowserHelper()
        {
            Assembly editorAssembly = Assembly.GetAssembly( typeof( EditorWindow ) );
            foreach( string typeName in projecBrowserTypeNames )
            {
                projectBrowserType = editorAssembly.GetType( typeName );

                if( projectBrowserType != null )
                {
                    setSearchMethod = projectBrowserType.GetMethod( "SetSearch", new System.Type[] { typeof( string ) } );

                    if( setSearchMethod != null )
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Apply a search in the project window.
        /// </summary>
        /// <param name="filter"></param>
        public static void SetSearch( string filter )
        {
            if( HasSearchImplementation )
            {
                EditorWindow projectBrowserInstance = EditorWindow.GetWindow( projectBrowserType );

                if( projectBrowserInstance != null )
                {
                    setSearchMethodArgs[0] = filter;
                    setSearchMethod.Invoke( projectBrowserInstance, setSearchMethodArgs );
                }
            }
        }
    }
}
