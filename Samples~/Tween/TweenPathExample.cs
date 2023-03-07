using UnityEngine;

namespace BitStrap.Examples
{
	public class TweenPathExample : MonoBehaviour
	{
		[Header( "Click play and click buttons to make the box lerp the path." )]
		[Header( "Also, try editing 'TweenPath/StepX's transform and tangent." )]
		public TweenPath tweenPath;

		private int currentIndex = 0;
		private bool canAdvance = true;

		[Button]
		public void StepForward()
		{
			if( canAdvance )
			{
				if( currentIndex < tweenPath.transform.childCount - 1 )
					MoveForward();
				else
					ResetPosition();
			}
		}

		[Button]
		public void StepBackward()
		{
			if( canAdvance )
			{
				if( currentIndex > 0 )
					MoveBackward();
				else
					ResetPosition();
			}
		}

		[Button]
		public void PlayForward()
		{
			if( currentIndex < tweenPath.transform.childCount - 1 )
				FinishForward();
			else
				ResetPosition();
		}

		[Button]
		public void PlayBackward()
		{
			if( currentIndex > 0 )
				FinishBackward();
			else
				ResetPosition();
		}

		[Button]
		public void ResetTween()
		{
			ResetPosition();
		}

		private void Awake()
		{
			tweenPath.onFinish.Register( OnFinishTween );
		}

		private void MoveForward()
		{
			tweenPath.StepForward( currentIndex );
			currentIndex++;
			canAdvance = false;
		}

		private void MoveBackward()
		{
			tweenPath.StepBackward( currentIndex );
			currentIndex--;
			canAdvance = false;
		}

		private void FinishForward()
		{
			tweenPath.PlayForward( currentIndex );
			currentIndex = tweenPath.transform.childCount - 1;
			canAdvance = false;
		}

		private void FinishBackward()
		{
			tweenPath.PlayBackward( currentIndex );
			currentIndex = 0;
			canAdvance = false;
		}

		private void ResetPosition()
		{
			currentIndex = 0;
			tweenPath.SampleAt( currentIndex, 0.0f );
			canAdvance = true;
		}

		private void OnFinishTween()
		{
			canAdvance = true;
		}
	}
}