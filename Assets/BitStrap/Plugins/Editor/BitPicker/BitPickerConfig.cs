using UnityEngine;
using BitStrap;

namespace BitStrap
{
	public sealed class BitPickerConfig : ScriptableObject
	{
		private static Option<BitPickerConfig> instance;
		public static Option<BitPickerConfig> Instance
		{
			get
			{
				instance = AssetDatabaseHelper.FindAssetOfType<BitPickerConfig>();
				return instance;
			}
		}

		public FuzzyFinderConfig fuzzyFinderConfig;
	}
}