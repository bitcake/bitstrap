using UnityEngine;
using BitStrap;

namespace BitStrap
{
	public sealed class BitPickerConfig : ScriptableObject
	{
		[System.Serializable]
		public sealed class ScoreConfig
		{
			public bool showScores = false;
			public int nameMatchBonus = 15;
		}

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
		public ScoreConfig scoreConfig;
		public float scrollSensitivity = 1.0f;

		[InlineScriptableObject]
		public BitPickerProvider[] providers;
	}
}