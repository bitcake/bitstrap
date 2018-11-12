using System;
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

		private sealed class WorkerData
		{
			public readonly ManualResetEvent allWorkersFinishedSync;
			public readonly WorkerState[] states;

			public BitPickerConfig config;
			public List<BitPickerItem> items;
			public string pattern;
			public int step;
			public int pendingWorkerCount;

			public WorkerData( int workerCount )
			{
				allWorkersFinishedSync = new ManualResetEvent( false );

				states = new WorkerState[workerCount];
				for( var i = 0; i < workerCount; i++ )
					states[i] = new WorkerState( this, i );
			}

			public void Setup( BitPickerConfig config, List<BitPickerItem> items, string pattern )
			{
				allWorkersFinishedSync.Reset();

				this.config = config;
				this.items = items;
				this.pattern = pattern;

				foreach( var state in this.states )
					state.Setup();

				var workerCount = states.Length;
				step = ( items.Count + workerCount - 1 ) / workerCount;
				pendingWorkerCount = workerCount;
			}

			public void Join()
			{
				allWorkersFinishedSync.WaitOne();
			}
		}

		private sealed class WorkerState
		{
			public readonly WorkerData data;
			public readonly int index;
			public readonly List<Result> results;
			public int[] matchMemory;
			public int matchMemoryLength;

			public WorkerState( WorkerData data, int index )
			{
				this.data = data;
				this.index = index;
				this.results = new List<Result>();

				matchMemoryLength = 0;
				matchMemory = null;
			}

			public void Setup()
			{
				results.Clear();
				matchMemoryLength = 0;

				var matchMemoryExpectedCapacity = data.items.Count * FuzzyFinder.ExpectedMaxMatchesPerItem;
				if( matchMemory == null || matchMemoryExpectedCapacity > matchMemory.Length )
					matchMemory = new int[Mathf.NextPowerOfTwo( matchMemoryExpectedCapacity + 1 )];
			}

			public void Finish()
			{
				if( Interlocked.Decrement( ref data.pendingWorkerCount ) == 0 )
					data.allWorkersFinishedSync.Set();
			}

			public void EnsureMatchCapacity()
			{
				if( matchMemoryLength >= matchMemory.Length )
					matchMemory = new int[Mathf.NextPowerOfTwo( matchMemoryLength + 1 )];
			}
		}

		private static readonly WorkerData workerData;

		static BitPickerHelper()
		{
			var workerCount = Mathf.Max( System.Environment.ProcessorCount, 1 );
			workerData = new WorkerData( workerCount );
		}

		public static void GetMatches( BitPickerConfig config, List<BitPickerItem> providedItems, string pattern, List<Result> results )
		{
			UnityEngine.Profiling.Profiler.BeginSample( "bitpicker" );
			workerData.Setup( config, providedItems, pattern );

			foreach( var workerState in workerData.states )
			{
				ThreadPool.QueueUserWorkItem( s =>
				{
					var state = s as WorkerState;
					UnityEngine.Profiling.Profiler.BeginThreadProfiling( "bitpickergroup", "bitpickergroup_" + state.index );
					var startIndex = state.index * state.data.step;
					var endIndex = Mathf.Min( startIndex + state.data.step, state.data.items.Count );

					GetMatchesPartial( state, startIndex, endIndex );

					state.Finish();
					UnityEngine.Profiling.Profiler.EndThreadProfiling();
				}, workerState );
			}

			workerData.Join();

			foreach( var workerState in workerData.states )
			{
				results.AddRange( workerState.results );
				workerState.EnsureMatchCapacity();
			}
			UnityEngine.Profiling.Profiler.EndSample();
		}

		private static void GetMatchesPartial( WorkerState state, int startIndex, int endIndex )
		{
			for( int i = startIndex; i < endIndex; i++ )
			{
				var item = state.data.items[i];

				int nameScore;
				var nameMatches = new Slice<int>( state.matchMemory, state.matchMemoryLength );
				var nameMatched = FuzzyFinder.Match(
					state.data.config.fuzzyFinderConfig,
					item.name,
					state.data.pattern,
					out nameScore,
					ref nameMatches
				);
				state.matchMemoryLength = nameMatches.endIndex;

				int fullNameScore;
				var fullNameMatches = new Slice<int>( state.matchMemory, state.matchMemoryLength );
				var fullNameMatched = FuzzyFinder.Match(
					state.data.config.fuzzyFinderConfig,
					item.fullName,
					state.data.pattern,
					out fullNameScore,
					ref fullNameMatches
				);
				state.matchMemoryLength = fullNameMatches.endIndex;

				if( nameMatched || fullNameMatched )
				{
					var score = Mathf.Max(
						nameScore + state.data.config.scoreConfig.nameMatchBonus,
						fullNameScore
					);

					state.results.Add( new Result( score, i, nameMatches, fullNameMatches ) );
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