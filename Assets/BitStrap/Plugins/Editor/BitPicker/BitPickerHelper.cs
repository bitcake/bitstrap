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
	}
}