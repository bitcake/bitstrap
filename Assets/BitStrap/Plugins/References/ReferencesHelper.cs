using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BitStrap
{
#if UNITY_EDITOR
	public static class ReferencesHelper
	{
		public static T[] GetReferencesInProject<T>() where T : Object
		{
			return GetReferencesInFolder<T>( "Assets" );
		}

		public static T[] GetReferencesInFolder<T>( Object folder ) where T : Object
		{
			string folderPath = AssetDatabase.GetAssetPath( folder );
			return GetReferencesInFolder<T>( folderPath );
		}

		private static T[] GetReferencesInFolder<T>( string folderPath ) where T : Object
		{
			string filter = string.Concat( "t:", typeof( T ).Name );
			string[] foldersToSearch = new string[] { folderPath };
			string[] assetGuids = AssetDatabase.FindAssets( filter, foldersToSearch );

			T[] references = new T[assetGuids.Length];
			for( int i = 0; i < references.Length; i++ )
			{
				string assetPath = AssetDatabase.GUIDToAssetPath( assetGuids[i] );
				references[i] = AssetDatabase.LoadAssetAtPath<T>( assetPath );
			}

			return references;
		}
	}
#endif
}