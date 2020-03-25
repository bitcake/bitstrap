using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Tween the field of view of Cameras.
	/// </summary>
	public sealed class TweenFov : Tween
	{
		/// <summary>
		/// The target Camera
		/// </summary>
		public Camera target;

		/// <summary>
		/// Lerp from this field of view value
		/// </summary>
		public float from = 30.0f;

		/// <summary>
		/// Lerp to this field of view value
		/// </summary>
		public float to = 60.0f;

		private float targetFrom;
		private float targetTo;

		/// <summary>
		/// Play tween forward
		/// </summary>
		[Button]
		public override void PlayForward()
		{
			Play( from, to );
		}

		/// <summary>
		/// Same as PlayForward() but with inverted direction
		/// </summary>
		[Button]
		public override void PlayBackward()
		{
			Play( to, from );
		}

		/// <summary>
		/// Sample the tween at t [0..1]
		/// </summary>
		/// <param name="t"></param>
		public override void SampleAt( float t )
		{
			t = curve.Evaluate( t );
			target.fieldOfView = Mathf.Lerp( targetFrom, targetTo, t );
		}

		private void Play( float from, float to )
		{
			targetFrom = from;
			targetTo = to;

			timer.Start();
			enabled = true;
		}
	}
}