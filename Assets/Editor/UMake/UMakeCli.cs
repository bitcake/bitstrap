using System.Runtime.Serialization;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public static class UMakeCli
	{
		public sealed class CliErrorException : System.Exception
		{
			public CliErrorException( string message ) : base( message )
			{
			}

			public CliErrorException() : base()
			{
			}

			public CliErrorException( SerializationInfo info, StreamingContext context ) : base( info, context )
			{
			}

			public CliErrorException( string message, System.Exception innerException ) : base( message, innerException )
			{
			}
		}

		public static bool IsInCli = false;
		public static readonly Dictionary<string, string> Args = new Dictionary<string, string>();

		public static void GetPref()
		{
			GetArgs();

			string prefKey;
			Args.TryGetValue( "key", out prefKey );

			string prefValue = EditorPrefs.GetString( prefKey, "" );

			Debug.LogFormat( "GETTING EDITOR PREF: '{0}' = '{1}'", prefKey, prefValue );
		}

		public static void SetPref()
		{
			GetArgs();

			string prefKey;
			Args.TryGetValue( "key", out prefKey );

			string prefValue;
			Args.TryGetValue( "value", out prefValue );

			EditorPrefs.SetString( prefKey, prefValue );

			Debug.LogFormat( "SETTING EDITOR PREF: '{0}' = '{1}'", prefKey, prefValue );
		}

		private static void ExecuteOnTarget( System.Action<UMake, UMakeTarget> callback )
		{
			if( callback == null )
				return;

			GetArgs();

			string targetName;
			Args.TryGetValue( "target", out targetName );

			UMakeTarget target;
			if( !UMake.GetTarget( targetName ).TryGet( out target ) )
				throw new CliErrorException( string.Format( "Could not find target '{0}'.", targetName ) );

			UMake umake;
			if( UMake.Get().TryGet( out umake ) )
				callback( umake, target );
		}

		public static void Build()
		{
			ExecuteOnTarget( ( umake, target ) =>
			{
				string buildPath;
				Args.TryGetValue( "path", out buildPath );

				Debug.LogFormat( "\n\nBuilding for target: '{0}' at '{1}'.\n\n", target.name, buildPath );

				target.Build( umake, buildPath);
			} );
		}

		public static void PostBuild()
		{
			ExecuteOnTarget( ( umake, target ) =>
			{
				target.ExecutePostBuildActions( umake );
			} );
		}

		public static void BuildAndPostBuild()
		{
			Build();
			PostBuild();
		}

		private static void GetArgs()
		{
			string[] allArgs = System.Environment.GetCommandLineArgs();

			if( allArgs == null )
				throw new CliErrorException( "No args provided." );

			foreach( var arg in allArgs )
			{
				int index = arg.IndexOf( ':' );
				if( index >= 0 )
					Args[arg.Substring( 0, index )] = arg.Substring( index + 1 );
			}

			IsInCli = true;
		}
	}
}