using UnityEngine;

namespace BitStrap
{
	[System.Serializable]
	public sealed class FuzzyMatcherConfig
	{
		[Range( 1, 10 )]
		public int recursionLimit = 8;

		public string separators = " /\\_-.";
		public int consecutiveBonus = 30; // bonus for adjacent matches
		public int separatorBonus = 30; // bonus if match occurs after a separator
		public int camelBonus = 30; // bonus if match is uppercase and prev is lower
		public int firstLetterBonus = 15; // bonus if the first letter is matched

		public int leadingLetterPenalty = -5; // penalty applied for every letter in str before the first match
		public int maxLeadingLetterPenalty = -15; // maximum penalty for leading letters
		public int unmatchedLetterPenalty = -1; // penalty for every letter that doesn't matter
	}
}