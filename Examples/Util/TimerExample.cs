using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class TimerExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public Timer.Duration timerDuration = new Timer.Duration( 4.0f );
		private Timer timer = new Timer();
		private SafeAction timerAction = new SafeAction();

		[ReadOnly]
		public float remainingTime = 0.0f;
		[ReadOnly]
		public float progress = 0.0f;
		[ReadOnly]
		public bool isRunning = false;

		[Button]
		public void StartTimer()
		{
			if( Application.isPlaying )
			{
				timer.Start();
				Debug.Log( "Timer was started!" );
			}
			else
			{
				Debug.LogWarning( "In order to see Timer working, please enter Play mode." );
			}
		}

		[Button]
		public void StopTimer()
		{
			if( Application.isPlaying )
			{
				timer.Stop();
				Debug.Log( "Timer was stopped!" );
			}
			else
			{
				Debug.LogWarning( "In order to see Timer working, please enter Play mode." );
			}
		}

		private void OnTimer()
		{
			Debug.Log( "Timer has finished!" );
		}

		private void Awake()
		{
			timerAction.Register( OnTimer );
		}

		private void Update()
		{
			timer.Update( timerDuration ).TryCall( timerAction );

			remainingTime = timerDuration.length - timer.elapsedTime;
			progress = timer.GetProgress( timerDuration );
			isRunning = timer.isRunning;
		}
	}
}