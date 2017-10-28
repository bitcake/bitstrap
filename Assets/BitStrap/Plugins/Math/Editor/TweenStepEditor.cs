using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomEditor( typeof( TweenStep ) )]
	public class TweenStepEditor : Editor
	{
		private void OnSceneGUI()
		{
			var tweenStep = target as TweenStep;

			var position = tweenStep.transform.position;
			var handlePosition = Handles.PositionHandle( position + tweenStep.tangent, Quaternion.identity );
			tweenStep.tangent = handlePosition - position;

			Handles.color = new Color( 0.4f, 0.9f, 0.4f, 0.9f );
			Handles.DrawLine( position, position + tweenStep.tangent );
			float size = 0.15f * HandleUtility.GetHandleSize( position + tweenStep.tangent );
			Handles.SphereHandleCap( 0, position + tweenStep.tangent, Quaternion.identity, size, EventType.Repaint );
		}
	}
}
