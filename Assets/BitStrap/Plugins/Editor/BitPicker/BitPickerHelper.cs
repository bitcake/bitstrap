using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BitStrap
{
	public static class BitPickerHelper
	{
		public struct Result
		{
			public readonly int score;
			public readonly int itemIndex;
			public readonly Slice<int> nameMatches;
			public readonly Slice<int> fullNameMatches;

			public Result( int score, int itemIndex, Slice<int> nameMatches, Slice<int> fullNameMatches )
			{
				this.score = score;
				this.itemIndex = itemIndex;
				this.nameMatches = nameMatches;
				this.fullNameMatches = fullNameMatches;
			}
		}

		private static readonly BitPickerWorkerGroup workerGroup;

		static BitPickerHelper()
		{
			var workerCount = Mathf.Max( System.Environment.ProcessorCount, 1 );
			workerGroup = new BitPickerWorkerGroup( workerCount );
		}

		public static void PrepareToGetMatches( BitPickerConfig config, List<BitPickerItem> providedItems, string pattern )
		{
			workerGroup.SetupForItems( config, providedItems, pattern );
		}

		public static bool GetMatchesPartial( List<Result> results )
		{
			return workerGroup.GetMatchesPartialSync( results );
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

		public static void HighlightMatches( BitPickerConfig config, string text, Slice<int> matches, StringBuilder stringBuilder )
		{
			var markupLength =
				config.stylingConfig.beforeMatchMarkup.Length +
				config.stylingConfig.afterMatchMarkup.Length;

			var offset = stringBuilder.Length;
			stringBuilder.Append( text );

			var matchGroupCount = 0;
			for( var i = matches.startIndex; i < matches.endIndex; )
			{
				var match = matches.array[i];

				var startIndex = i;
				i++;
				for( ; i < matches.endIndex && matches.array[i] == matches.array[i - 1] + 1; i++ )
					continue;

				stringBuilder.Insert(
					offset + match + matchGroupCount * markupLength + ( i - startIndex ),
					config.stylingConfig.afterMatchMarkup
				);

				stringBuilder.Insert(
					offset + match + matchGroupCount * markupLength,
					config.stylingConfig.beforeMatchMarkup
				);

				matchGroupCount++;
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