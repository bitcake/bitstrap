using System.IO;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public sealed class SteamUploadBuildAction : UMakeBuildAction
	{
		private const string steamCmdArgFormat = "+login {0} {1} +run_app_build_http \"{2}\" +quit";
		public string contentSubFolder = "windows_content/x86";
		public string buildScript = "app_build_<steam_appid>.vdf";
		public bool skipSteamContentCopy = false;

		private EditorPrefString steamSdkPath = new EditorPrefString( "UMakeSteam_SteamSdkPath" );
		private EditorPrefString steamUsername = new EditorPrefString( "UMakeSteam_Username" );
		private EditorPrefString steamPassword = new EditorPrefString( "UMakeSteam_Password" );

		public override void Execute( UMake umake, UMakeTarget target )
		{
			try
			{
				string buildPath = UMake.GetBuildPath();
				string steamBuildScript = buildScript;
				string sdkPath = steamSdkPath.Value;
				string username = steamUsername.Value;
				string password = steamPassword.Value;
				bool skipCopy = skipSteamContentCopy;

				if( UMakeCli.IsInCli )
				{
					UMakeCli.Args.TryGetValue( "path", out buildPath );
					UMakeCli.Args.TryGetValue( "script", out steamBuildScript );
					UMakeCli.Args.TryGetValue( "steam-sdk", out sdkPath );
					UMakeCli.Args.TryGetValue( "steam-username", out username );
					UMakeCli.Args.TryGetValue( "steam-password", out password );

					string skipCopyStringValue;
					UMakeCli.Args.TryGetValue( "skip-steam-content-copy", out skipCopyStringValue );
					bool.TryParse( skipCopyStringValue, out skipCopy );
				}

				if( !Directory.Exists( sdkPath ) )
				{
					Debug.LogFormat( "SteamSDK \"{0}\" not found.", sdkPath );
					return;
				}

				string steamCmdPath = Path.Combine( sdkPath, "tools/ContentBuilder/builder/steamcmd.exe" ); ;
				if( !File.Exists( steamCmdPath ) )
				{
					Debug.LogFormat( "SteamCMD \"{0}\" not found.", steamCmdPath );
					return;
				}

				if( !skipCopy && !CopyContent( sdkPath, target, umake.version, buildPath ) )
				{
					Debug.Log( "Could not copy content to Steam folder." );
					return;
				}

				var uploaderProcess = new System.Diagnostics.Process();
				uploaderProcess.StartInfo.FileName = steamCmdPath;
				uploaderProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName( Path.GetDirectoryName( steamCmdPath ) );
				uploaderProcess.StartInfo.Arguments = string.Format( steamCmdArgFormat, username, password, steamBuildScript );

				if( UMakeCli.IsInCli )
				{
					uploaderProcess.StartInfo.UseShellExecute = false;
					uploaderProcess.StartInfo.RedirectStandardOutput = true;
					uploaderProcess.OutputDataReceived += ( sender, msg ) =>
					{
						if( msg != null )
							Debug.Log( msg.Data );
					};
				}

				uploaderProcess.Start();
				Debug.LogFormat( "Executing SteamCMD \"{0}\"...", steamCmdPath );

				if( UMakeCli.IsInCli )
				{
					uploaderProcess.BeginOutputReadLine();
					uploaderProcess.WaitForExit();
				}

				uploaderProcess.Close();
			}
			catch( System.Exception e )
			{
				Debug.Log( "Upload to Steam failed." );
				Debug.LogException( e );
			}
		}

		public override void OnInspectorGUI( UMakeBuildActionEditor editor )
		{
			base.OnInspectorGUI( editor );

			using( BoxGroup.Do( "Shared Settings" ) )
			{
				using( Horizontal.Do() )
				{
					steamSdkPath.Value = EditorGUILayout.TextField( "Steam SDK Folder", steamSdkPath.Value );
					if( GUILayout.Button( "Change", GUILayout.Width( 64.0f ) ) )
					{
						string path = EditorUtility.OpenFolderPanel( "Steam SDK Folder Path", steamSdkPath.Value, "" );
						if( !string.IsNullOrEmpty( path ) )
							steamSdkPath.Value = path;
					}
				}

				steamUsername.Value = EditorGUILayout.TextField( "Steam Username", steamUsername.Value );
				steamPassword.Value = EditorGUILayout.PasswordField( "Steam Password", steamPassword.Value );
			}

			GUILayout.FlexibleSpace();
		}

		private bool CopyContent( string sdkPath, UMakeTarget umakeTarget, string version, string buildPath )
		{
			string contentFolderPath = Path.Combine( sdkPath, "tools/ContentBuilder/content" );

			if( !Directory.Exists( contentFolderPath ) )
			{
				Debug.LogFormat( "Content folder \"{0}\" does not exist.", contentFolderPath );
				return false;
			}

			UMakeTarget.Path targetPath = umakeTarget.GetTargetPath( version, buildPath );

			if( !Directory.Exists( targetPath.directoryPath ) )
			{
				Debug.LogFormat( "Target path \"{0}\" does not exist. Did you forget to build?", targetPath.directoryPath );
				return false;
			}

			string uploadFolderPath = Path.Combine( contentFolderPath, contentSubFolder );
			if( Directory.Exists( uploadFolderPath ) )
				Directory.Delete( uploadFolderPath, true );

			FileSystemHelper.CopyDirectory( targetPath.directoryPath, uploadFolderPath );

			Debug.LogFormat( "Build files copied to \"{0}\".", uploadFolderPath );
			return true;
		}
	}
}
