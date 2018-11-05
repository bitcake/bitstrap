using UnityEngine;
using System.Collections.Generic;

// Based on https://github.com/forrestthewoods/lib_fts/blob/master/code/fts_fuzzy_match.h
public static class FuzzyFinder
{
	private const int RecursionLimit = 8;

	private const string Separators = " /\\_-";
	private const int SequentialBonus = 15; // bonus for adjacent matches
	private const int SeparatorBonus = 30; // bonus if match occurs after a separator
	private const int CamelBonus = 30; // bonus if match is uppercase and prev is lower
	private const int FirstLetterBonus = 15; // bonus if the first letter is matched

	private const int LeadingLetterPenalty = -5; // penalty applied for every letter in str before the first match
	private const int MaxLeadingLetterPenalty = -15; // maximum penalty for leading letters
	private const int UnmatchedLetterPenalty = -1; // penalty for every letter that doesn't matter

	public static bool Match( string pattern, string str, out int score )
	{
		int recursionCount = 0;
		return MatchRecursive( pattern, 0, str, 0, out score, 0, new List<byte>(), new List<byte>(), ref recursionCount );
	}

	private static void Copy<T>( List<T> destination, List<T> source, int count )
	{
		destination.Clear();
		for( int i = 0; i < count; i++ )
			destination.Add( source[i] );
	}

	private static bool MatchRecursive( string pattern, int patternIndex, string str, int strIndex, out int outScore, int strBeginIndex, List<byte> srcMatches, List<byte> matches, ref int recursionCount )
	{
		outScore = int.MinValue;

		// Count recursions
		++recursionCount;
		if( recursionCount >= RecursionLimit )
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
				if( first_match )
				{
					Copy( matches, srcMatches, matches.Count );
					first_match = false;
				}

				// Recursive call that "skips" this match
				var recursiveMatches = new List<byte>();
				int recursiveScore;
				if( MatchRecursive( pattern, patternIndex, str, strIndex + 1, out recursiveScore, strBeginIndex, matches, recursiveMatches, ref recursionCount ) )
				{
					// Pick best recursive score
					if( !recursiveMatch || recursiveScore > bestRecursiveScore )
					{
						Copy( bestRecursiveMatches, recursiveMatches, recursiveMatches.Count );
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
			int penalty = LeadingLetterPenalty * matches[0];
			if( penalty < MaxLeadingLetterPenalty )

				penalty = MaxLeadingLetterPenalty;
			outScore += penalty;

			// Apply unmatched penalty
			int unmatched = ( int ) ( strIndex - strBeginIndex ) - matches.Count;
			outScore += UnmatchedLetterPenalty * unmatched;

			// Apply ordering bonuses
			for( int i = 0; i < matches.Count; ++i )
			{
				byte currIdx = matches[i];

				if( i > 0 )
				{
					byte prevIdx = matches[i - 1];

					// Sequential
					if( currIdx == ( prevIdx + 1 ) )
						outScore += SequentialBonus;
				}

				// Check for bonuses based on neighbor character value
				if( currIdx > 0 )
				{
					// Camel case
					char neighbor = str[strBeginIndex + currIdx - 1];
					char curr = str[strBeginIndex + currIdx];
					if( char.IsLower( neighbor ) && char.IsUpper( curr ) )
						outScore += CamelBonus;

					// Separator
					if( Separators.IndexOf( neighbor ) >= 0 )
						outScore += SeparatorBonus;
				}
				else
				{
					// First letter
					outScore += FirstLetterBonus;
				}
			}
		}

		// Return best result
		if( recursiveMatch && ( !matched || bestRecursiveScore > outScore ) )
		{
			// Recursive score is better than "this"
			Copy( matches, bestRecursiveMatches, bestRecursiveMatches.Count );
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
}
