using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public sealed class IncrementVersionBuildAction : UMakeBuildAction
	{
		public char separator = '.';

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

			EditorUtility.SetDirty( umake );
		}
	}
}