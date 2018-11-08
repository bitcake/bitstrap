using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BitStrap
{
	public static class BitPickerHelper
	{
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

		public static void HighlightMatches( string text, List<byte> matches, StringBuilder stringBuilder )
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
	}
}