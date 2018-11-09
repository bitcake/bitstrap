using UnityEngine;
using System.Collections.Generic;

namespace BitStrap
{
	// Based on https://github.com/forrestthewoods/lib_fts/blob/master/code/fts_fuzzy_match.h
	public static class FuzzyFinder
	{
		[System.ThreadStatic]
		private static List<int> tempMatches;

		public static bool Match( FuzzyFinderConfig config, string text, string pattern, out int score, List<int> matches )
		{
			if( tempMatches == null )
				tempMatches = new List<int>();

			var recursionCount = 0;

			score = MatchRecursive( config, text, 0, pattern, 0, matches, tempMatches, 0, ref recursionCount );

			return score > int.MinValue;
		}

		public static int MatchRecursive( FuzzyFinderConfig config, string text, int textIndex, string pattern, int patternIndex, List<int> matches, List<int> tempMatches, int tempMatchesStartIndex, ref int recursionCount )
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
						matches,
						tempMatches,
						tempMatches.Count,
						ref recursionCount
					);

					if( recursiveScore > score )
						score = recursiveScore;

					patternIndex++;
					tempMatches.Add( textIndex );
				}

				textIndex++;
			}

			if( patternIndex == patternLength )
			{
				var calculatedScore = CalculateScore( config, text, tempMatches );
				if( calculatedScore > score )
				{
					score = calculatedScore;
					Copy( matches, tempMatches );
				}
			}

			tempMatches.RemoveRange( tempMatchesStartIndex, tempMatches.Count - tempMatchesStartIndex );

			return score;
		}

		private static int CalculateScore( FuzzyFinderConfig config, string text, List<int> matches )
		{
			var score = 0;

			score += Mathf.Max( config.leadingLetterPenalty * matches[0], config.maxLeadingLetterPenalty );
			score += config.unmatchedLetterPenalty * ( text.Length - matches.Count );

			for( var i = 0; i < matches.Count; i++ )
			{
				var index = matches[i];

				if( i > 0 )
				{
					var previousIndex = matches[i - 1];

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

		public static bool MatchOld( FuzzyFinderConfig config, string pattern, string text, out int score, List<byte> matches )
		{
			matches.Clear();
			int recursionCount = 0;
			return MatchRecursiveOld( config, pattern, 0, text, 0, out score, 0, new List<byte>(), matches, ref recursionCount );
		}

		private static bool MatchRecursiveOld( FuzzyFinderConfig config, string pattern, int patternIndex, string str, int strIndex, out int outScore, int strBeginIndex, List<byte> srcMatches, List<byte> matches, ref int recursionCount )
		{
			outScore = int.MinValue;

			// Count recursions
			++recursionCount;
			if( recursionCount >= config.recursionLimit )
				return false;

			// Detect end of strings
			if( patternIndex == pattern.Length || strIndex == str.Length )
				return false;

			// Recursion params
			bool recursiveMatch = false;
			var bestRecursiveMatches = new List<byte>();
			int bestRecursiveScore = 0;

			// Loop through pattern and str looking for a match
			bool first_match = true;
			while( patternIndex != pattern.Length && strIndex != str.Length )
			{
				// Found match
				if( char.ToLower( pattern[patternIndex] ) == char.ToLower( str[strIndex] ) )
				{
					// "Copy-on-Write" srcMatches into matches
					if( first_match && srcMatches.Count > 0 )
					{
						Copy( matches, srcMatches );
						first_match = false;
					}

					// Recursive call that "skips" this match
					var recursiveMatches = new List<byte>();
					int recursiveScore;
					if( MatchRecursiveOld( config, pattern, patternIndex, str, strIndex + 1, out recursiveScore, strBeginIndex, matches, recursiveMatches, ref recursionCount ) )
					{
						// Pick best recursive score
						if( !recursiveMatch || recursiveScore > bestRecursiveScore )
						{
							Copy( bestRecursiveMatches, recursiveMatches );
							bestRecursiveScore = recursiveScore;
						}
						recursiveMatch = true;
					}

					// Advance
					matches.Add( ( byte ) ( strIndex - strBeginIndex ) );
					++patternIndex;
				}
				++strIndex;
			}

			// Determine if full pattern was matched
			bool matched = patternIndex == pattern.Length ? true : false;

			// Calculate score
			if( matched )
			{
				strIndex = str.Length;

				// Initialize score
				outScore = 100;

				// Apply leading letter penalty
				int penalty = config.leadingLetterPenalty * matches[0];
				if( penalty < config.maxLeadingLetterPenalty )
					penalty = config.maxLeadingLetterPenalty;
				outScore += penalty;

				// Apply unmatched penalty
				int unmatched = ( int ) ( strIndex - strBeginIndex ) - matches.Count;
				outScore += config.unmatchedLetterPenalty * unmatched;

				// Apply ordering bonuses
				for( int i = 0; i < matches.Count; ++i )
				{
					byte currIdx = matches[i];

					if( i > 0 )
					{
						byte prevIdx = matches[i - 1];

						// Sequential
						if( currIdx == ( prevIdx + 1 ) )
							outScore += config.sequentialBonus;
					}

					// Check for bonuses based on neighbor character value
					if( currIdx > 0 )
					{
						// Camel case
						char neighbor = str[strBeginIndex + currIdx - 1];
						char curr = str[strBeginIndex + currIdx];
						if( char.IsLower( neighbor ) && char.IsUpper( curr ) )
							outScore += config.camelBonus;

						// Separator
						if( config.separators.IndexOf( neighbor ) >= 0 )
							outScore += config.separatorBonus;
					}
					else
					{
						// First letter
						outScore += config.firstLetterBonus;
					}
				}
			}

			// Return best result
			if( recursiveMatch && ( !matched || bestRecursiveScore > outScore ) )
			{
				// Recursive score is better than "this"
				Copy( matches, bestRecursiveMatches );
				outScore = bestRecursiveScore;
				return true;
			}
			else if( matched )
			{
				// "this" score is better than recursive
				return true;
			}
			else
			{
				// no match
				return false;
			}
		}

		private static void Copy<T>( List<T> destination, List<T> source )
		{
			destination.Clear();
			var count = source.Count;
			for( var i = 0; i < count; i++ )
				destination.Add( source[i] );
		}
	}
}