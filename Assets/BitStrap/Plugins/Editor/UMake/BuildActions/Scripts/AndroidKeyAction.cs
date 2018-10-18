using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public sealed class AndroidKeyAction : UMakeBuildAction
	{
		public string keyStoreName = "user.keystore";
		public string keyStorePassword = "keyStorePassword";
		public string keyAliasName = "aliasName";
		public string keyAliasPassword = "keyAliasPassword";

		private static string ProjectPath
		{
			get
			{
				return Application.dataPath.Remove(Application.dataPath.Length - 6, 6);
			}
		}

		public override void Execute(UMake umake, UMakeTarget target)
		{
			PlayerSettings.keystorePass = keyStorePassword;
			PlayerSettings.keyaliasPass = keyAliasPassword;

			PlayerSettings.Android.keystoreName = ProjectPath + keyStoreName;
			PlayerSettings.Android.keyaliasName = keyAliasName;
		}
	}
}
