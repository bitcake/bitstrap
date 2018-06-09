using UnityEngine;
using UnityEditor;

namespace BitStrap
{
	public static class AssetDatabaseHelper
	{
		public static Option<T> FindAssetOfType<T>() where T : Object
		{
			return from guid in AssetDatabase.FindAssets( "t:" + typeof( T ) ).First() select AssetDatabase.LoadAssetAtPath<T>( AssetDatabase.GUIDToAssetPath( guid ) );
		}
	}
}