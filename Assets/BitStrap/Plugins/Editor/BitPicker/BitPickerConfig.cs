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

		[System.Serializable]
		public sealed class Styling
		{
			public string beforeMatchMarkup = "<b>";
			public string afterMatchMarkup = "</b>";
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

		public FuzzyMatcherConfig fuzzyMatcherConfig;
		public ScoreConfig scoreConfig;
		public Styling stylingConfig;

		[InlineScriptableObject]
		public BitPickerProvider[] providers;
	}
}