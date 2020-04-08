using System.IO;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Draws and syncs a script template.
	/// Script templates are used within the context menu Assets -> Create -> [SomeScript].
	/// Exposing those templates in the editor enables an easy access with the possibility
	/// to store different templates as text files and store them in the project.
	/// To use script templates go to Edit -> Preferences -> BitStrap.
	/// </summary>
	public sealed class ScriptTemplatePreference
	{
		private string propertyName;
		private EditorPrefProperty<string> templateCode;
		private bool unfolded;

		public string TemplateCode
		{
			get { return templateCode.Value; }
		}

		public ScriptTemplatePreference( string propertyName, string editorPrefKey, string defaultTemplateCode )
		{
			this.propertyName = propertyName;
			this.templateCode = new EditorPrefString( editorPrefKey, defaultTemplateCode );
		}

		public void OnPreferencesGUI()
		{
			unfolded = EditorGUILayout.Foldout( unfolded, new GUIContent( propertyName ) );
			if( !unfolded )
			{
				return;
			}
			templateCode.Value = EditorGUILayout.TextArea( templateCode.Value );

			using( Horizontal.Do() )
			{
				if( GUILayout.Button( "Load from file" ) )
				{
					string filePath = EditorUtility.OpenFilePanelWithFilters( "Load script template file for " + propertyName, ScriptTemplatePreferences.ScriptTemplateDefaultPath.Value, new[] { "FileType", "txt,cs" } );
					if( !filePath.Equals( string.Empty ) )
					{
						ScriptTemplatePreferences.SaveDefaultPathFromFilePath( filePath );
						templateCode.Value = File.ReadAllText( filePath );
						UpdateLineEnding();
					}
				}

				if( GUILayout.Button( "Save to file" ) )
				{
					string filePath = EditorUtility.SaveFilePanel( "Save script template", ScriptTemplatePreferences.ScriptTemplateDefaultPath.Value, propertyName + "Template", "txt" );
					if( !filePath.Equals( string.Empty ) )
					{
						ScriptTemplatePreferences.SaveDefaultPathFromFilePath( filePath );
						UpdateLineEnding();
						WriteToFile( templateCode.Value, filePath );
						AssetDatabase.Refresh();
					}
				}

				if( GUILayout.Button( "Reset" ) )
				{
					GUI.FocusControl( null );
					templateCode.DeleteKey();
				}
			}
		}

		/// <summary>
		/// Update the line ending to the current line ending settings.
		/// Line endings: \r\n for Windows, \n for Unix
		/// </summary>
		public void UpdateLineEnding()
		{
			var tempString = templateCode.Value;
			tempString = tempString.Replace( "\r\n", "\n" );
			if( ScriptTemplatePreferences.ScriptTemplateUseWindowsLineEnding.Value )
			{
				tempString = tempString.Replace( "\n", "\r\n" );
			}
			templateCode.Value = tempString;
		}

		private void WriteToFile( string s, string filename )
		{
			using( StreamWriter sw = new StreamWriter( filename, false ) )
			{
				sw.Write( s );
			}
		}
	}
}