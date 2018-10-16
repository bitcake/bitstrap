
using UnityEditor;

namespace BitStrap
{
    public sealed class IncrementBundleVersion : UMakeBuildAction
    {
        public override void Execute(UMake umake, UMakeTarget target)
        {
            string bundleVersion = PlayerSettings.bundleVersion;
           
            if (target.buildTarget == BuildTarget.Android)
            {
                PlayerSettings.Android.bundleVersionCode++;
            }
        }
    }
}