using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Timer utility class. Allows you to receive a callback after a certain
	/// amount of time has elapsed.
	/// </summary>
	[System.Serializable]
	public class Timer
	{
		/// <summary>
		/// The timer's length in seconds.
		/// </summary>
		public float length = 1.0f;

		/// <summary>
		/// Callback that gets called when "length" seconds has elapsed.
		/// </summary>
		public SafeAction onTimer = new SafeAction();

		private float elapsedTime = -1.0f;

		/// <summary>
		/// The countdown time in seconds.
		/// </summary>
		public float RemainingTime
		{
			get { return elapsedTime < 0.0f ? 0.0f : Mathf.Max( length - elapsedTime, 0.0f ); }
		}

		/// <summary>
		/// Return a 0.0 to 1.0 number where 1.0 means the timer completed and is now stopped.
		/// </summary>
		public float Progress
		{
			get { return elapsedTime < 0.0f ? 1.0f : Mathf.Clamp01( elapsedTime / length ); }
		}

		/// <summary>
		/// Is the timer countdown running?
		/// </summary>
		public bool IsRunning
		{
			get { return elapsedTime >= 0.0f; }
		}

		public Timer()
		{
		}

		public Timer( float length )
		{
			this.length = length;
		}

		/// <summary>
		/// You need to manually call this at your script Update() method for the timer to work properly.
		/// Uses Time.unscaledDeltaTime for delta time.
		/// </summary>
		public void OnUpdate()
		{
			OnUpdate( Time.unscaledDeltaTime );
		}

		/// <summary>
		/// You need to manually call this at your script Update() method for the timer to work properly.
		/// </summary>
		/// <param name="deltaTime"></param>
		public void OnUpdate( float deltaTime )
		{
			if( elapsedTime >= 0.0f )
				elapsedTime += deltaTime;

			if( elapsedTime >= length )
			{
				elapsedTime = -1.0f;
				onTimer.Call();
			}
		}

		/// <summary>
		/// Stop the timer and its counter.
		/// </summary>
		public void Stop()
		{
			elapsedTime = -1.0f;
		}

		/// <summary>
		/// Start the timer and play its counter.
		/// </summary>
		public void Start()
		{
			elapsedTime = 0.0f;
		}
	}
}