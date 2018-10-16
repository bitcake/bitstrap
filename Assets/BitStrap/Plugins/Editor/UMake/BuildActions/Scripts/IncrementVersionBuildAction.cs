using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public sealed class IncrementVersionBuildAction : UMakeBuildAction
	{
		public char separator = '.';
        public bool updateApplicationVersion = false;

		public override void Execute( UMake umake, UMakeTarget target )
		{
			Undo.RecordObject( umake, "UMakeBuildAction" );

			string[] parts = umake.version.Split( separator );
			int minVersion;

			if( int.TryParse( parts[parts.Length - 1], out minVersion ) )
			{
				minVersion += 1;
				parts[parts.Length - 1] = minVersion.ToString();

				umake.version = string.Join( separator.ToString(), parts );
			}

			if ( updateApplicationVersion )
			{
				PlayerSettings.bundleVersion = umake.version;
				Debug.Log( "The application is now with version " + Application.version );
			}
			EditorUtility.SetDirty( umake );
		}
	}
}