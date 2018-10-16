
using UnityEditor;

namespace BitStrap
{
	public sealed class IncrementBundleVersion : UMakeBuildAction
	{
		public override void Execute( UMake umake, UMakeTarget target )
		{
			string bundleVersion = PlayerSettings.bundleVersion;

			if ( target.buildTarget == BuildTarget.Android )
			{
				PlayerSettings.Android.bundleVersionCode++;
			}
			else if ( target.buildTarget == BuildTarget.iOS )
			{
				int buildNumber = 0;

				if ( int.TryParse( PlayerSettings.iOS.buildNumber, out buildNumber ) )
				{
					buildNumber++;
					PlayerSettings.iOS.buildNumber = buildNumber.ToString();
				}
			}
		}
	}
}