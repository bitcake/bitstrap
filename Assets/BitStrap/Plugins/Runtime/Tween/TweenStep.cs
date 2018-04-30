using UnityEngine;
using UnityEngine.Events;

namespace BitStrap
{
	public class TweenStep : MonoBehaviour
	{
		public float duration = 1.0f;
		public AnimationCurve positionCurve = AnimationCurve.Linear( 0.0f, 0.0f, 1.0f, 1.0f );
		public AnimationCurve rotationCurve = AnimationCurve.Linear( 0.0f, 0.0f, 1.0f, 1.0f );

		public Vector3 tangent = Vector3.right;

		public UnityEvent onEnter;
	}
}