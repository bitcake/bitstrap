using System.Collections.Generic;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Interpolates a transform through a Bezier or linear path of control points.
	/// </summary>
	public sealed class TweenPath : MonoBehaviour
	{
		/// <summary>
		/// Use a smooth bezier or linear interpolation.
		/// </summary>
		public bool useBezier = true;

		/// <summary>
		/// The target that should be interpolated.
		/// </summary>
		public Transform target;

		/// <summary>
		/// The parent object that contains all TweenStep.
		/// </summary>
		public Transform pathRoot;

		/// <summary>
		/// Callback called when tween finishes a step.
		/// </summary>
		public readonly SafeAction onFinish = new SafeAction();

		private List<TweenStep> steps = new List<TweenStep>();

		private TweenStep from;
		private TweenStep to;

		private float timer;
		private int currentIndex;
		private int direction;

		/// <summary>
		/// Play tween at this step index and one step further.
		/// </summary>
		/// <param name="index"></param>
		public void StepForward( int index )
		{
			SetupFromIndex( index, 1 );
			timer = 0.0f;
			currentIndex = -1;
		}

		/// <summary>
		/// Play tween at this step index and one step back.
		/// </summary>
		/// <param name="index"></param>
		public void StepBackward( int index )
		{
			SetupFromIndex( index, -1 );
			timer = 0.0f;
			currentIndex = -1;
		}

		/// <summary>
		/// Play tween at this step index to the end.
		/// </summary>
		/// <param name="index"></param>
		public void PlayForward( int index )
		{
			SetupFromIndex( index, 1 );
			timer = 0.0f;
			currentIndex = index;
		}

		/// <summary>
		/// Play tween at this step index to the end (backwards).
		/// </summary>
		/// <param name="index"></param>
		public void PlayBackward( int index )
		{
			SetupFromIndex( index, -1 );
			timer = 0.0f;
			currentIndex = index;
		}

		/// <summary>
		/// Stops the tween wherever it is.
		/// </summary>
		public void Stop()
		{
			enabled = false;
		}

		/// <summary>
		/// Stops the tween and samples it at index and value t (range 0 to 1).
		/// 0 means TweenStep at index, 1 TweenStep at index + 1 and 0.5 means halfway both.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="t"></param>
		public void SampleAt( int index, float t )
		{
			from = steps[Mathf.Clamp( index, 0, steps.Count - 1 )];
			to = steps[Mathf.Clamp( index + 1, 0, steps.Count - 1 )];

			Interpolate( t );
			enabled = false;
		}

		/// <summary>
		/// Change the parent object that contains all the TweenSteps.
		/// </summary>
		/// <param name="root"></param>
		public void SetPathRoot( Transform root )
		{
			pathRoot = root;

			steps.Clear();
			for( int i = 0; i < pathRoot.childCount; i++ )
				steps.Add( pathRoot.GetChild( i ).GetComponent<TweenStep>() );
		}

		private static Vector3 InterpolatePosition( TweenStep from, TweenStep to, float t, bool useBezier, bool forward )
		{
			t = from.positionCurve.Evaluate( Mathf.Clamp01( t ) );

			if( useBezier )
			{
				var p0 = from.transform.position;
				var p3 = to.transform.position;

				var p1 = forward ? p0 + from.tangent : p0 - from.tangent;
				var p2 = forward ? p3 - to.tangent : p3 + to.tangent;

				var p01 = Vector3.Lerp( p0, p1, t );
				var p12 = Vector3.Lerp( p1, p2, t );
				var p23 = Vector3.Lerp( p2, p3, t );

				var p012 = Vector3.Lerp( p01, p12, t );
				var p123 = Vector3.Lerp( p12, p23, t );

				return Vector3.Lerp( p012, p123, t );
			}
			else
			{
				return Vector3.Lerp( from.transform.position, to.transform.position, t );
			}
		}

		private static Quaternion InterpolateRotation( TweenStep from, TweenStep to, float t )
		{
			t = from.rotationCurve.Evaluate( Mathf.Clamp01( t ) );
			return Quaternion.Slerp( from.transform.rotation, to.transform.rotation, t );
		}

		private void Interpolate( float t )
		{
			if( t > 1.0f && currentIndex > -direction && currentIndex < steps.Count - 1 - direction )
			{
				SetupFromIndex( currentIndex + direction, direction );
				t -= 1.0f;
			}

			bool isPlayingForward = direction > 0;
			target.position = InterpolatePosition( from, to, t, useBezier, isPlayingForward );
			target.rotation = InterpolateRotation( from, to, t );
		}

		private void SetupFromIndex( int index, int offset )
		{
			from = steps[Mathf.Clamp( index, 0, steps.Count - 1 )];
			to = steps[Mathf.Clamp( index + offset, 0, steps.Count - 1 )];
			direction = offset;

			from.onEnter.Invoke();
			enabled = true;
		}

		private void Reset()
		{
			pathRoot = transform;
		}

		private void Awake()
		{
			SetPathRoot( pathRoot );
			enabled = false;
		}

		private void Update()
		{
			float t = timer / from.duration;
			Interpolate( t );

			if( timer >= from.duration )
			{
				if( currentIndex > -direction && currentIndex < steps.Count - 1 - direction )
				{
					currentIndex += direction;
					SetupFromIndex( currentIndex, direction );
					timer -= from.duration;
				}
				else
				{
					to.onEnter.Invoke();
					onFinish.Call();

					enabled = false;
				}
			}

			timer += Time.deltaTime;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = new Color( 0.8f, 1.0f, 0.8f, 0.9f );

			for( int i = 0; i < pathRoot.childCount - 1; i++ )
			{
				var from = pathRoot.GetChild( i ).GetComponent<TweenStep>();
				var to = pathRoot.GetChild( i + 1 ).GetComponent<TweenStep>();

				if( from == null || to == null )
					continue;

				const int steps = 32;
				for( int j = 0; j < steps; j++ )
				{
					float t0 = ( ( float ) j ) / steps;
					float t1 = ( j + 1.0f ) / steps;
					Vector3 p0 = InterpolatePosition( from, to, t0, useBezier, true );
					Vector3 p1 = InterpolatePosition( from, to, t1, useBezier, true );

					Gizmos.DrawLine( p0, p1 );
				}
			}
		}
	}
}