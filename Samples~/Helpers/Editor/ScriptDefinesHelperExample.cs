using UnityEditor;

namespace BitStrap.Examples
{
	/// <summary>
	/// Open this window by navigating in Unity Editor to "Window/BitStrap Examples/Extensions/ScriptDefinesHelper".
	/// </summary>
	public class ScriptDefinesHelperExample : EditorWindow
	{
		private const string defineSymbol = "TEST_DEFINE_SYMBOL";
		private bool? defined;

		[MenuItem( "Window/BitStrap Examples/Helpers/ScriptDefinesHelper" )]
		public static void CreateWindow()
		{
			GetWindow<ScriptDefinesHelperExample>().Show();
		}

		private void OnGUI()
		{
			if( !defined.HasValue )
			{
				defined = ScriptDefinesHelper.IsDefined( EditorUserBuildSettings.selectedBuildTargetGroup, defineSymbol );
			}

			EditorGUILayout.HelpBox( "Please check \"Player Settings\" > \"Other Settings\" > \"Scripting Define Symbols\"\nto see which symbols are defined.", MessageType.Info );

			EditorGUI.BeginChangeCheck();
			defined = EditorGUILayout.Toggle( "Test symbol is defined", defined.Value );
			if( EditorGUI.EndChangeCheck() )
			{
				ScriptDefinesHelper.SetDefined( EditorUserBuildSettings.selectedBuildTargetGroup, defineSymbol, defined.Value );
			}
		}
	}
}
