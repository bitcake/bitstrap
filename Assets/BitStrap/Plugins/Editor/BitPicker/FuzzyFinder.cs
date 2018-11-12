using UnityEngine;
using System.Collections.Generic;

namespace BitStrap
{
	// Based on https://github.com/forrestthewoods/lib_fts/blob/master/code/fts_fuzzy_match.h
	public static class FuzzyFinder
	{
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

		public static bool Match( FuzzyFinderConfig config, string text, string pattern, out int score, ref Slice<int> matches )
		{
			score = 100;

			if( pattern.Length == 0 || text.Length == 0 )
				return false;

			var lastMatch = 0;
			for( ; lastMatch < text.Length && char.ToLower( text[lastMatch] ) != char.ToLower( pattern[0] ); lastMatch++ )
				continue;

			// First letter scoring
			if( lastMatch == 0 )
				score += config.firstLetterBonus;
			else
				score += Mathf.Max( config.leadingLetterPenalty * lastMatch, config.maxLeadingLetterPenalty );

			matches.Add( lastMatch );

			var patternIndex = 1;
			for( var i = lastMatch + 1; i < text.Length && patternIndex < pattern.Length; i++ )
			{
				var patternChar = pattern[patternIndex];

				// Search ahead for camel case or separator
				var betterIndex = i;
				for( ; betterIndex < text.Length; betterIndex++ )
				{
					if( char.ToLower( text[betterIndex] ) == char.ToLower( patternChar ) )
					{
						if( char.IsLower( text[betterIndex - 1] ) && char.IsUpper( text[betterIndex] ) ) // Camel Case
						{
							score += config.camelBonus;

							lastMatch = betterIndex;
							matches.Add( betterIndex );
							patternIndex++;

							break;
						}
						else if( config.separators.IndexOf( text[betterIndex - 1] ) >= 0 ) // After Separator
						{
							score += config.separatorBonus;

							lastMatch = betterIndex;
							matches.Add( betterIndex );
							patternIndex++;

							break;
						}
					}
				}
				// Found camel case or separator
				if( betterIndex < text.Length )
				{
					score += ( betterIndex - i ) * config.unmatchedLetterPenalty;
					i = betterIndex;
					continue;
				}

				if( char.ToLower( text[i] ) == char.ToLower( patternChar ) )
				{
					if( i == lastMatch + 1 ) // Consecutive
					{
						score += config.consecutiveBonus;

						lastMatch = i;
						matches.Add( i );
						patternIndex++;
					}
				}
				else
				{
					score += config.unmatchedLetterPenalty;
				}
			}

			if( patternIndex < pattern.Length )
			{
				score = int.MinValue;
				matches.Count = 0;
				return false;
			}

			return true;
		}

		public static bool MatchEx( FuzzyFinderConfig config, string text, string pattern, out int score, ref Slice<int> matches, ref Slice<int> tempMatches )
		{
			tempMatches.Count = 0;

			var recursionCount = 0;
			score = MatchExRecursive( config, text, 0, pattern, 0, ref matches, ref tempMatches, 0, ref recursionCount );

			return score > int.MinValue;
		}

		public static int MatchExRecursive( FuzzyFinderConfig config, string text, int textIndex, string pattern, int patternIndex, ref Slice<int> matches, ref Slice<int> tempMatches, int tempMatchesStartIndex, ref int recursionCount )
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
					var recursiveScore = MatchExRecursive(
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
				var calculatedScore = CalculateScoreEx( config, text, ref tempMatches );
				if( calculatedScore > score )
				{
					score = calculatedScore;
					Slice.SoftCopy( tempMatches, ref matches );
				}
			}

			tempMatches.endIndex = tempMatchesStartIndex;

			return score;
		}

		private static int CalculateScoreEx( FuzzyFinderConfig config, string text, ref Slice<int> tempMatches )
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