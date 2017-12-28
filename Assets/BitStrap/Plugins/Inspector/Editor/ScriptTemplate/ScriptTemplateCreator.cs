using System;
using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityEditor.ProjectWindowCallback
{
	internal sealed class CreateScriptAssetAction : EndNameEditAction
	{
		public override void Action( int instanceId, string path, string source )
		{
			BitStrap.ScriptTemplateCreator.WriteScript( path, source, true );
		}
	}
}

namespace BitStrap
{
	public sealed class ScriptModificationProcessor : UnityEditor.AssetModificationProcessor
	{
		private static string defaultBehaviourTemplate = null;

		public static void OnWillCreateAsset( string path )
		{
			string metaPath = path;

			path = path.Replace( ".meta", "" );
			if( !path.EndsWith( ".cs" ) || !File.Exists( path ) || File.Exists( metaPath ) )
				return;

			if( defaultBehaviourTemplate == null )
			{
				string templatePath = EditorApplication.applicationContentsPath;
				templatePath = Path.Combine( templatePath, "Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs.txt" );
				if( File.Exists( templatePath ) )
					defaultBehaviourTemplate = File.ReadAllText( templatePath );
			}

			if( defaultBehaviourTemplate == null )
				return;

			string className = Path.GetFileNameWithoutExtension( path );
			string defaultBehaviourSource = defaultBehaviourTemplate.Replace( "#SCRIPTNAME#", className );
			defaultBehaviourSource = defaultBehaviourSource.Replace( "#NOTRIM#", "" );

			if( string.Equals( defaultBehaviourSource, File.ReadAllText( path ) ) )
				ScriptTemplateCreator.WriteScript( path, ScriptTemplatePreferences.CSharpScriptDefaultCode, false );
		}
	}

	public static class ScriptTemplateCreator
	{
		private static bool justCreatedCustomScript = false;

		private static Texture2D ScriptIcon
		{
			get { return EditorGUIUtility.IconContent( "cs Script Icon" ).image as Texture2D; }
		}

		public static void WriteScript( string path, string source, bool isCustomScript )
		{
			if( !isCustomScript && justCreatedCustomScript )
				return;

			justCreatedCustomScript = isCustomScript;

			string className = Path.GetFileNameWithoutExtension( path );
			source = source.Replace( "#CLASSNAME#", className );
			source = source.Replace( "#SCRIPTNAME#", className );

			int currentYear = DateTime.Today.Year;
			source = source.Replace( "#YEAR#", currentYear.ToString() );

			File.WriteAllText( path, source );

			AssetDatabase.ImportAsset( path );
			Object o = AssetDatabase.LoadAssetAtPath( path, typeof( Object ) );
			ProjectWindowUtil.ShowCreatedAsset( o );
		}

		[MenuItem( "Assets/Create/C# Editor Script", false, 81 )]
		public static void CreateCSharpEditorScript()
		{
			string path = GetNewScriptPath( "NewEditor" );
			CreateScript( path, ScriptTemplatePreferences.CSharpEditorScriptDefaultCode );
		}

		private static string GetNewScriptPath( string scriptName )
		{
			string path = "Assets/";
			if( Selection.activeObject != null )
			{
				path = AssetDatabase.GetAssetPath( Selection.activeObject );
				if( !AssetDatabase.IsValidFolder( path ) )
					path = Path.GetDirectoryName( path );
			}

			return Path.Combine( path, string.Concat( scriptName, ".cs" ) );
		}

		private static void CreateScript( string path, string source )
		{
			var createScriptAssetAction = ScriptableObject.CreateInstance<CreateScriptAssetAction>();
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists( 0, createScriptAssetAction, path, ScriptIcon, source );
		}
	}
}