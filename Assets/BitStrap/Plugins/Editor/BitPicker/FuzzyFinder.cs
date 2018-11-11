using UnityEngine;
using System.Collections.Generic;

namespace BitStrap
{
	// Based on https://github.com/forrestthewoods/lib_fts/blob/master/code/fts_fuzzy_match.h
	public static class FuzzyFinder
	{
		public const int MeanMaxMatchesPerItem = 16;

		[System.ThreadStatic]
		private static Slice<int> tempMatches;

		public static bool Match( FuzzyFinderConfig config, string text, string pattern, out int score, ref Slice<int> matches )
		{
			if( tempMatches.array == null )
				tempMatches = new Slice<int>( new int[config.recursionLimit * MeanMaxMatchesPerItem], 0 );

			tempMatches.Count = 0;

			var recursionCount = 0;
			score = MatchRecursive( config, text, 0, pattern, 0, ref matches, 0, ref recursionCount );

			return score > int.MinValue;
		}

		public static int MatchRecursive( FuzzyFinderConfig config, string text, int textIndex, string pattern, int patternIndex, ref Slice<int> matches, int tempMatchesStartIndex, ref int recursionCount )
		{
			if( recursionCount >= config.recursionLimit )
				return int.MinValue;
			recursionCount++;

			var patternLength = pattern.Length;
			var textLength = text.Length;

			if( patternIndex == patternLength || textIndex == textLength )
				return int.MinValue;

			var score = int.MinValue;

			while( patternIndex != patternLength && textIndex != textLength )
			{
				if( char.ToLower( pattern[patternIndex] ) == char.ToLower( text[textIndex] ) )
				{
					var recursiveScore = MatchRecursive(
						config,
						text,
						textIndex + 1,
						pattern,
						patternIndex,
						ref matches,
						tempMatches.endIndex,
						ref recursionCount
					);

					if( recursiveScore > score )
						score = recursiveScore;

					patternIndex++;
					Slice.ForceAdd( ref tempMatches, textIndex );
				}

				textIndex++;
			}

			if( patternIndex == patternLength )
			{
				var calculatedScore = CalculateScore( config, text );
				if( calculatedScore > score )
				{
					score = calculatedScore;
					Slice.SoftCopy( tempMatches, ref matches );
				}
			}

			tempMatches.endIndex = tempMatchesStartIndex;

			return score;
		}

		private static int CalculateScore( FuzzyFinderConfig config, string text )
		{
			var score = 100;

			score += Mathf.Max( config.leadingLetterPenalty * tempMatches.Get( 0 ), config.maxLeadingLetterPenalty );
			score += config.unmatchedLetterPenalty * ( text.Length - tempMatches.endIndex );

			for( var i = tempMatches.startIndex; i < tempMatches.endIndex; i++ )
			{
				var index = tempMatches.array[i];

				if( i > tempMatches.startIndex )
				{
					var previousIndex = tempMatches.array[i - 1];

					// Sequential
					if( index == ( previousIndex + 1 ) )
						score += config.sequentialBonus;
				}

				if( index > 0 )
				{
					var previousChar = text[index - 1];
					var currentChat = text[index];
					if( char.IsLower( previousChar ) && char.IsUpper( currentChat ) )
						score += config.camelBonus;

					if( config.separators.IndexOf( previousChar ) >= 0 )
						score += config.separatorBonus;
				}
				else
				{
					score += config.firstLetterBonus;
				}
			}

			return score;
		}
	}
}