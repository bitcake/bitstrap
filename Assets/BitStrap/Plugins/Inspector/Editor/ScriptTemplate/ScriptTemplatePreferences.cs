using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Draws the preferences for editor script templates (<see cref="ScriptTemplatePreference"/>).
	/// This class also provides access to the defined templates as well as other settings from
	/// its static properties.
	/// To use script templates go to Edit -> Preferences -> BitStrap.
	/// </summary>
	[InitializeOnLoadAttribute]
	public static class ScriptTemplatePreferences
	{
		public static EditorPrefProperty<string> ScriptTemplateDefaultPath = new EditorPrefString( "ScriptTemplate_DefaultFilePath", "" );
		public static EditorPrefProperty<bool> ScriptTemplateUseWindowsLineEnding = new EditorPrefBool( "ScriptTemplate_UseWindowsLineEnding", true );

		private static ScriptTemplatePreference cSharpScriptTemplate = new ScriptTemplatePreference
			( "C# Script", "ScriptTemplate_CSharpScript",
@"using UnityEngine;

public sealed class #SCRIPTNAME# : MonoBehaviour
{
}
" );

		private static ScriptTemplatePreference cSharpEditorScriptTemplate = new ScriptTemplatePreference
			( "C# EditorScript", "ScriptTemplate_CSharpEditorScript",
@"using UnityEditor;
using UnityEngine;

public sealed class #SCRIPTNAME# : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
	}
}
" );

		private static Vector2 scroll = Vector2.zero;

		public static string CSharpScriptDefaultCode
		{
			get { return cSharpScriptTemplate.TemplateCode; }
		}

		public static string CSharpEditorScriptDefaultCode
		{
			get { return cSharpEditorScriptTemplate.TemplateCode; }
		}

		static ScriptTemplatePreferences()
		{
			BitStrapPreferences.RegisterPreference( OnPreferencesGUI );
		}

		public static void OnPreferencesGUI()
		{
			using( BoxGroup.Do( ref scroll, "Script Templates" ) )
			{
				bool windowsLineEnding = EditorGUILayout.Toggle( "Use windows line ending format", ScriptTemplateUseWindowsLineEnding.Value );

				if( windowsLineEnding != ScriptTemplateUseWindowsLineEnding.Value )
				{
					ScriptTemplateUseWindowsLineEnding.Value = windowsLineEnding;
					cSharpScriptTemplate.UpdateLineEnding();
					cSharpEditorScriptTemplate.UpdateLineEnding();
				}
				cSharpScriptTemplate.OnPreferencesGUI();
				EditorGUILayout.Space();
				cSharpEditorScriptTemplate.OnPreferencesGUI();
			}
		}

		public static void SaveDefaultPathFromFilePath( string filePath )
		{
			ScriptTemplateDefaultPath.Value = filePath.Substring( 0, filePath.LastIndexOf( "/", System.StringComparison.Ordinal ) );
		}
	}
}