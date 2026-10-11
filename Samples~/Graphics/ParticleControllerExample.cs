using UnityEngine;

namespace BitStrap.Examples
{
	public class ParticleControllerExample : MonoBehaviour
	{
		// Control a ParticleSystem without generating garbage!
		public ParticleController particleController;

		[Button]
		public void Play()
		{
			if( Application.isPlaying )
			{
				particleController.Play();
			}
			else
			{
				Debug.LogWarning( "In order to see ParticleController working, please enter Play mode." );
			}
		}

		[Button]
		public void Stop()
		{
			if( Application.isPlaying )
			{
				particleController.Stop();
			}
			else
			{
				Debug.LogWarning( "In order to see ParticleController working, please enter Play mode." );
			}
		}
	}
}
