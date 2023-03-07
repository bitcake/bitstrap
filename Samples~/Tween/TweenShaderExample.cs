using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class TweenShaderExample : MonoBehaviour
	{
		private TweenShader tween;

		[Button]
		public void PlayForward()
		{
			if( Application.isPlaying )
			{
				tween.PlayForward();
			}
			else
			{
				Debug.LogWarning( "In order to see TweenShader working, please enter Play mode." );
			}
		}

		[Button]
		public void PlayBackward()
		{
			if( Application.isPlaying )
			{
				tween.PlayBackward();
			}
			else
			{
				Debug.LogWarning( "In order to see TweenShader working, please enter Play mode." );
			}
		}

		private void Awake()
		{
			tween = GetComponent<TweenShader>();
		}
	}
}
