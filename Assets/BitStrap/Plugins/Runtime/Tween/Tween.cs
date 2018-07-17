using UnityEngine;
using UnityEngine.Events;

namespace BitStrap
{
	public abstract class Tween : MonoBehaviour
	{
		public Timer timer = new Timer();
		public Timer.Duration duration = new Timer.Duration( 1.0f );
		public AnimationCurve curve = AnimationCurve.EaseInOut( 0.0f, 0.0f, 1.0f, 1.0f );

		public UnityEvent onFinish;

		public abstract void PlayForward();

		public abstract void PlayBackward();

		public abstract void SampleAt( float t );


		[Button]
		public void Stop()
		{
			enabled = false;
		}

		private void Reset()
		{
			enabled = false;
		}

		private void Awake()
		{
			if( enabled )
				PlayForward();
		}

		private void Update()
		{
			if( timer.Update( duration ) )
			{
				SampleAt( 1.0f );
				Stop();

				onFinish.Invoke();
			}

			SampleAt( timer.GetProgress( duration ) );
		}
	}
}