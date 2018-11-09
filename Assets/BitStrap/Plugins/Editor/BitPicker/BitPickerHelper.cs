using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BitStrap
{
	public static class BitPickerHelper
	{
		public struct Result
		{
			readonly public int score;
			readonly public int itemIndex;
			readonly public List<int> nameMatches;
			readonly public List<int> fullNameMatches;

			public Result( int score, int itemIndex, List<int> nameMatches, List<int> fullNameMatches )
			{
				this.score = score;
				this.itemIndex = itemIndex;
				this.nameMatches = nameMatches;
				this.fullNameMatches = fullNameMatches;
			}
		}

		public static void GetMatches( BitPickerConfig config, List<BitPickerItem> providedItems, string pattern, List<Result> results )
		{
			for( int i = 0; i < providedItems.Count; i++ )
			{
				var item = providedItems[i];

				int nameScore;
				var nameMatches = new List<int>();
				var nameMatched = FuzzyFinder.Match(
					config.fuzzyFinderConfig,
					item.name,
					pattern,
					out nameScore,
					nameMatches
				);

				int fullNameScore;
				var fullNameMatches = new List<int>();
				var fullNameMatched = FuzzyFinder.Match(
					config.fuzzyFinderConfig,
					item.fullName,
					pattern,
					out fullNameScore,
					fullNameMatches
				);

				if( nameMatched || fullNameMatched )
				{
					var score = Mathf.Max(
						nameScore + config.scoreConfig.nameMatchBonus,
						fullNameScore
					);

					results.Add( new Result( score, i, nameMatches, fullNameMatches ) );
				}
			}
		}

		public static string RemoveArgs( string pattern )
		{
			var index = pattern.LastIndexOf( ':' );
			if( index < 0 )
				return pattern;

			return pattern.Substring( 0, index );
		}

		public static string GetArgs( string pattern )
		{
			var index = pattern.LastIndexOf( ':' );
			if( index < 0 || index >= pattern.Length )
				return "";

			return pattern.Substring( index + 1 );
		}

		public static void HighlightMatches( string text, List<int> matches, StringBuilder stringBuilder )
		{
			var beforeMarkup = "<b>";
			var afterMarkup = "</b>";
			var markupLength = beforeMarkup.Length + afterMarkup.Length;

			var offset = stringBuilder.Length;

			stringBuilder.Append( text );

			for( int i = 0; i < matches.Count; i++ )
			{
				var match = matches[i];
				stringBuilder.Insert( offset + match + i * markupLength + 1, afterMarkup );
				stringBuilder.Insert( offset + match + i * markupLength, beforeMarkup );
			}
		}

		public static Vector2 GetStyleLayoutSize( GUIStyle style, GUIContent content )
		{
			var size = style.CalcSize( content );
			size.x += style.margin.horizontal;
			size.y += style.margin.vertical;

			return size;
		}
	}
}