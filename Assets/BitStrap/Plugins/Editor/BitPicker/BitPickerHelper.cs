using System.Collections.Generic;
using System.Text;
using System.Threading;
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

		private sealed class WorkerData
		{
			public readonly ManualResetEvent manualResetEvent;
			public readonly BitPickerConfig config;
			public readonly List<BitPickerItem> items;
			public readonly string pattern;
			public readonly WorkerState[] states;
			public readonly int step;
			public int pendingWorkerCount;

			public WorkerData( BitPickerConfig config, List<BitPickerItem> providedItems, string pattern, int workerCount )
			{
				this.manualResetEvent = new ManualResetEvent( false );
				this.config = config;
				this.items = providedItems;
				this.pattern = pattern;

				this.states = new WorkerState[workerCount];
				for( var i = 0; i < workerCount; i++ )
					states[i] = new WorkerState( this, i );

				this.step = ( providedItems.Count + workerCount - 1 ) / workerCount;
				this.pendingWorkerCount = workerCount;
			}
		}

		private sealed class WorkerState
		{
			public readonly WorkerData data;
			public readonly int index;
			public readonly List<Result> results;

			public WorkerState( WorkerData data, int index )
			{
				this.data = data;
				this.index = index;
				this.results = new List<Result>();
			}
		}

		public static void GetMatches( BitPickerConfig config, List<BitPickerItem> providedItems, string pattern, List<Result> results )
		{
			var workerCount = System.Environment.ProcessorCount;
			if( workerCount <= 1 )
			{
				GetMatchesPartial(
					config,
					providedItems,
					0,
					providedItems.Count,
					pattern,
					results
				);

				return;
			}

			var data = new WorkerData( config, providedItems, pattern, workerCount );

			for( var i = 0; i < workerCount; i++ )
			{
				ThreadPool.QueueUserWorkItem( s =>
				{
					var state = s as WorkerState;
					var startIndex = state.index * state.data.step;
					var endIndex = Mathf.Min( startIndex + state.data.step, state.data.items.Count );

					GetMatchesPartial(
						state.data.config,
						state.data.items,
						startIndex,
						endIndex,
						state.data.pattern,
						state.results
					);

					if( Interlocked.Decrement( ref state.data.pendingWorkerCount ) == 0 )
						state.data.manualResetEvent.Set();
				}, data.states[i] );
			}

			data.manualResetEvent.WaitOne();

			for( var i = 0; i < workerCount; i++ )
				results.AddRange( data.states[i].results );
		}

		private static void GetMatchesPartial( BitPickerConfig config, List<BitPickerItem> providedItems, int startIndex, int endIndex, string pattern, List<Result> results )
		{
			for( int i = startIndex; i < endIndex; i++ )
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