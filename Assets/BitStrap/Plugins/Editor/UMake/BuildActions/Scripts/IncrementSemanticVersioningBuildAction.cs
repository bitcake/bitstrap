using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Version of <see cref="IncrementVersionBuildAction"/> but based on Semantic Versioning(https://semver.org/)
	/// </summary>
	public sealed class IncrementSemanticVersioningBuildAction : UMakeBuildAction
	{
		public enum SemanticVersionType
		{
			Major,
			Minor,
			Patch,
			Dev
		}

		public char separator = '.';
		public SemanticVersionType increaseVersionType = SemanticVersionType.Patch;
		public bool updateApplicationVersion = false;

		public override void Execute(UMake umake, UMakeTarget target)
		{
			Undo.RecordObject(umake, "UMakeBuildAction");

			string[] parts = umake.version.Split(separator);
			int version = 1;
			int index = (int)increaseVersionType;

			if (index >= parts.Length)
			{
				string[] versions = new string[(index + 1) - parts.Length];
				for(int i = 0; i < versions.Length; i++)
				{
					versions[i] = "0";
				}
				ArrayUtility.AddRange(ref parts, versions);
			}
			else if (int.TryParse(parts[index], out version))
			{
				version += 1;
			}

            for(int i = index +1; i < parts.Length;)
            {
                ArrayUtility.RemoveAt(ref parts, i);
            }

			parts[index] = version.ToString();
			umake.version = string.Join(separator.ToString(), parts);

			if (updateApplicationVersion)
			{
				PlayerSettings.bundleVersion = umake.version;
				Debug.Log("The application is now with version " + Application.version);
			}
			EditorUtility.SetDirty(umake);
		}
	}
}
