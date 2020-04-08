using UnityEngine;
using System.Collections.Generic;

namespace BitStrap
{
	// Based on https://github.com/forrestthewoods/lib_fts/blob/master/code/fts_fuzzy_match.h
	public static class FuzzyMatcher
	{
		public const int MinScore = int.MinValue;
		public const int ExpectedMaxMatchesPerItem = 16;

		public static bool IsMatch( string text, string pattern )
		{
			var patternIndex = 0;
			for( var i = 0; i < text.Length && patternIndex < pattern.Length; i++ )
			{
				if( char.ToLower( text[i] ) == char.ToLower( pattern[patternIndex] ) )
					patternIndex++;
			}

			return patternIndex == pattern.Length;
		}

		public static int GetMatches( FuzzyMatcherConfig config, string text, string pattern, ref Slice<int> matches, ref Slice<int> tempMatches )
		{
			if( !IsMatch( text, pattern ) )
				return MinScore;

			tempMatches.Count = 0;

			var recursionCount = 0;
			return MatchRecursive( config, text, 0, pattern, 0, ref matches, ref tempMatches, 0, ref recursionCount );
		}

		public static int MatchRecursive( FuzzyMatcherConfig config, string text, int textIndex, string pattern, int patternIndex, ref Slice<int> matches, ref Slice<int> tempMatches, int tempMatchesStartIndex, ref int recursionCount )
		{
			if( recursionCount >= config.recursionLimit )
				return MinScore;
			recursionCount++;

			var patternLength = pattern.Length;
			var textLength = text.Length;

			if( patternIndex == patternLength || textIndex == textLength )
				return MinScore;

			var score = MinScore;

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
						ref tempMatches,
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
				var calculatedScore = CalculateScore( config, text, ref tempMatches );
				if( calculatedScore > score )
				{
					score = calculatedScore;
					Slice.SoftCopy( tempMatches, ref matches );
				}
			}

			tempMatches.endIndex = tempMatchesStartIndex;

			return score;
		}

		private static int CalculateScore( FuzzyMatcherConfig config, string text, ref Slice<int> tempMatches )
		{
			var score = 100;

			score += Mathf.Max( config.leadingLetterPenalty * tempMatches.Get( 0 ), config.maxLeadingLetterPenalty );
			score += config.unmatchedLetterPenalty * ( text.Length - tempMatches.Count );

			for( var i = tempMatches.startIndex; i < tempMatches.endIndex; i++ )
			{
				var index = tempMatches.array[i];

				if( i > tempMatches.startIndex )
				{
					var previousIndex = tempMatches.array[i - 1];

					// Sequential
					if( index == ( previousIndex + 1 ) )
						score += config.consecutiveBonus;
				}

				if( index > 0 )
				{
					var previousChar = text[index - 1];
					var currentChar = text[index];
					if( char.IsLower( previousChar ) && char.IsUpper( currentChar ) )
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