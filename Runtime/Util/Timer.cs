using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Timer utility class. Allows you to receive a callback after a certain
	/// amount of time has elapsed.
	/// </summary>
	public struct Timer
	{
		[System.Serializable]
		public struct Duration
		{
			public float length;

			public Duration( float length )
			{
				this.length = Mathf.Max( length, Mathf.Epsilon );
			}
		}

		public struct Latency
		{
			public readonly float length;

			public Latency( float latency )
			{
				this.length = latency;
			}

			public static implicit operator bool( Latency self )
			{
				return self.length >= 0.0f;
			}

			public Latency TryCall( SafeAction onTimer )
			{
				if( length >= 0.0f )
					onTimer.Call();

				return this;
			}
		}

		public bool isRunning;
		public float elapsedTime;

		/// <summary>
		/// Return a 0.0 to 1.0 number where 1.0 means the timer completed and is now stopped.
		/// </summary>
		public float GetProgress( Duration duration )
		{
			return Mathf.Clamp01( elapsedTime / duration.length );
		}

		/// <summary>
		/// You need to manually call this at your script Update() method for the timer to work properly.
		/// Uses Time.deltaTime for delta time.
		/// </summary>
		public Latency Update( Duration duration )
		{
			return Update( duration, Time.deltaTime );
		}

		/// <summary>
		/// You need to manually call this at your script Update() method for the timer to work properly.
		/// </summary>
		/// <param name="deltaTime"></param>
		public Latency Update( Duration duration, float deltaTime )
		{
			if( !isRunning )
				return new Latency( -1.0f );

			elapsedTime += deltaTime;

			float latency = elapsedTime - duration.length;
			isRunning = latency < 0.0f;

			return new Latency( latency );
		}

		/// <summary>
		/// Stop the timer and its counter.
		/// </summary>
		public void Stop()
		{
			isRunning = false;
		}

		/// <summary>
		/// Start the timer and play its counter.
		/// </summary>
		public void Start()
		{
			elapsedTime = 0.0f;
			isRunning = true;
		}

		/// <summary>
		/// Start the timer at 'timeOffset' and play its counter.
		/// </summary>
		public void StartAt( Latency latency )
		{
			elapsedTime = latency.length;
			isRunning = true;
		}
	}
}
