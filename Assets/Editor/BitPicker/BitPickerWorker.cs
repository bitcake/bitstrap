using UnityEngine;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace BitStrap
{
	public sealed class BitPickerWorkerGroup
	{
		public readonly ManualResetEvent allWorkersFinishedSync;
		public readonly BitPickerWorker[] workers;

		public BitPickerConfig config;
		public List<BitPickerItem> items;
		public string pattern;
		public int step;
		public int pendingWorkerCount;

		private bool hasWorkLeft;

		public BitPickerWorkerGroup( int workerCount )
		{
			allWorkersFinishedSync = new ManualResetEvent( false );

			workers = new BitPickerWorker[workerCount];
			for( var i = 0; i < workerCount; i++ )
				workers[i] = new BitPickerWorker( this, i );

			hasWorkLeft = false;
		}

		public void SetupForItems( BitPickerConfig config, List<BitPickerItem> items, string pattern )
		{
			hasWorkLeft = true;

			this.config = config;
			this.items = items;
			this.pattern = pattern;

			var workerCount = workers.Length;
			step = ( items.Count + workerCount - 1 ) / workerCount;

			foreach( var state in this.workers )
				state.Setup();
		}

		public bool GetMatchesPartialSync( List<BitPickerHelper.Result> results )
		{
			if( !hasWorkLeft )
				return false;

			pendingWorkerCount = workers.Length;
			allWorkersFinishedSync.Reset();

			foreach( var worker in workers )
			{
				ThreadPool.QueueUserWorkItem(
					w => ( w as BitPickerWorker ).GetMatchesPartial(),
					worker
				);
			}

			allWorkersFinishedSync.WaitOne();

			hasWorkLeft = false;
			foreach( var worker in workers )
			{
				results.AddRange( worker.results );
				worker.EnsureMatchCapacity();

				if( worker.IsIncomplete() )
					hasWorkLeft = true;
			}

			return true;
		}
	}

	public sealed class BitPickerWorker
	{
		public readonly BitPickerWorkerGroup data;
		public readonly int workerIndex;
		public readonly List<BitPickerHelper.Result> results;
		public readonly Stopwatch stopwatch;
		public int[] matchMemory;
		public int matchMemoryLength;

		public Slice<int> tempMaches;
		public int currentIndex;
		public int endIndex;

		public BitPickerWorker( BitPickerWorkerGroup data, int workerIndex )
		{
			this.data = data;
			this.workerIndex = workerIndex;

			results = new List<BitPickerHelper.Result>();
			stopwatch = new Stopwatch();

			matchMemoryLength = 0;
			matchMemory = null;

			tempMaches = new Slice<int>( new int[FuzzyMatcher.ExpectedMaxMatchesPerItem * 10], 0 );
		}

		public bool IsIncomplete()
		{
			return currentIndex < endIndex;
		}

		public void Setup()
		{
			matchMemoryLength = 0;

			var matchMemoryExpectedCapacity = data.items.Count * FuzzyMatcher.ExpectedMaxMatchesPerItem;
			if( matchMemory == null || matchMemoryExpectedCapacity > matchMemory.Length )
				matchMemory = new int[Mathf.NextPowerOfTwo( matchMemoryExpectedCapacity + 1 )];

			currentIndex = workerIndex * data.step;
			endIndex = Mathf.Min( currentIndex + data.step, data.items.Count );
		}

		public void GetMatchesPartial()
		{
			results.Clear();

			stopwatch.Reset();
			stopwatch.Start();

			for( ; currentIndex < endIndex && stopwatch.ElapsedMilliseconds <= 16; currentIndex++ )
			{
				var item = data.items[currentIndex];

				var nameMatches = new Slice<int>( matchMemory, matchMemoryLength );
				var nameScore = FuzzyMatcher.GetMatches(
					data.config.fuzzyMatcherConfig,
					item.name,
					data.pattern,
					ref nameMatches,
					ref tempMaches
				);
				matchMemoryLength = nameMatches.endIndex;

				var fullNameMatches = new Slice<int>( matchMemory, matchMemoryLength );
				var fullNameScore = FuzzyMatcher.GetMatches(
					data.config.fuzzyMatcherConfig,
					item.fullName,
					data.pattern,
					ref fullNameMatches,
					ref tempMaches
				);
				matchMemoryLength = fullNameMatches.endIndex;

				if( nameScore > FuzzyMatcher.MinScore || fullNameScore > FuzzyMatcher.MinScore )
				{
					var score = Mathf.Max(
						nameScore + data.config.scoreConfig.nameMatchBonus,
						fullNameScore
					);

					results.Add( new BitPickerHelper.Result(
						score,
						currentIndex,
						nameMatches,
						fullNameMatches
					) );
				}
			}

			stopwatch.Stop();

			if( Interlocked.Decrement( ref data.pendingWorkerCount ) == 0 )
				data.allWorkersFinishedSync.Set();
		}

		public void EnsureMatchCapacity()
		{
			if( matchMemoryLength >= matchMemory.Length )
				matchMemory = new int[Mathf.NextPowerOfTwo( matchMemoryLength + 1 )];
		}
	}
}