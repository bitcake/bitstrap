using UnityEngine;
using BitStrap;

namespace BitStrap
{
	public sealed class BitPickerConfig : ScriptableObject
	{
		[System.Serializable]
		public sealed class ScoreConfig
		{
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

		public string debugPattern;
		public string debugText;

		[Button]
		public void Test()
		{
			
		}


		public FuzzyFinderConfig fuzzyFinderConfig;
		public ScoreConfig scoreConfig;
		public bool showScores = false;

		[InlineScriptableObject]
		public BitPickerProvider[] providers;
	}
}