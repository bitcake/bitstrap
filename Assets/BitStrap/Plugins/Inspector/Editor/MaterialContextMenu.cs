using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public static class MaterialContextMenu
	{
		[MenuItem( "CONTEXT/Material/Select Material" )]
		public static void SelectMaterial( MenuCommand command )
		{
			Selection.activeObject = command.context;
		}

		[MenuItem( "CONTEXT/Material/Find Materials with Shader" )]
		public static void FindMaterialsWithMaterialShader( MenuCommand command )
		{
			var targetMaterial = command.context as Material;
			TryFindMaterialsWithShader( targetMaterial != null ? targetMaterial.shader : null );
		}

		[MenuItem( "CONTEXT/Shader/Find Materials with Shader" )]
		public static void FindMaterialsWithShader( MenuCommand command )
		{
			var targetShader = command.context as Shader;
			TryFindMaterialsWithShader( targetShader );
		}

		public static void TryFindMaterialsWithShader( Shader shader )
		{
			if( !FindMaterialsWithShader( shader ) )
				Debug.Log( "NO MATERIALS FOUND" );
		}

		public static bool FindMaterialsWithShader( Shader shader )
		{
			if( shader == null )
				return false;

			string[] allMaterialGuids = AssetDatabase.FindAssets( "t:Material" );
			if( allMaterialGuids.Length == 0 )
				return false;

			Debug.Log( string.Concat( "SEARCH FOR SHADER: \"", shader, "\"" ) );

			foreach( string materialGuid in allMaterialGuids )
			{
				string materialPath = AssetDatabase.GUIDToAssetPath( materialGuid );
				var material = AssetDatabase.LoadAssetAtPath<Material>( materialPath );
				if( material != null && material.shader == shader )
					Debug.Log( string.Concat( "MATERIAL: \"", material, "\"" ), material );
			}

			Resources.UnloadUnusedAssets();

			return true;
		}
	}
}
