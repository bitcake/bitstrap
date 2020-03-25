using System;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// An Animation Trigger
	/// </summary>
	[Serializable]
	public class TriggerAnimationParameter : AnimationParameter
	{
		/// <summary>
		/// Activates the selected trigger on the animator
		/// </summary>
		/// <param name="animator"></param>
		public void Set( Animator animator )
		{
			if( animator.isInitialized )
				animator.SetTrigger( Index );
		}
	}
}
