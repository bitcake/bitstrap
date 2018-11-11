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

		[InlineScriptableObject]
		public BitPickerProvider[] providers;

		public string debugText;
		public string debugPattern;
		[Button]
		public void Test()
		{
			var matches = new Slice<int>( new int[16], 14 );
			int score;
			var matched = FuzzyFinder.Match( fuzzyFinderConfig, debugText, debugPattern, out score, ref matches );
			if( matched )
			{
				var sb = new System.Text.StringBuilder();
				BitPickerHelper.HighlightMatches( debugText, matches, sb );
				Debug.LogFormat( "MATCHED: {0} => [{1}] {2}", matches.ToStringFull(), score, sb.ToString() );
			}
			else
			{
				Debug.Log( "NO MATCH" );
			}
		}
	}
}