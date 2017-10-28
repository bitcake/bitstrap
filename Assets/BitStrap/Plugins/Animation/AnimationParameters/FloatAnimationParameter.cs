using System;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// A Float Animation Parameter
	/// </summary>
	[Serializable]
	public class FloatAnimationParameter : AnimationParameter
	{
		/// <summary>
		/// Sets the value of the selected parameter, based on value
		/// </summary>
		/// <param name="animator"></param>
		/// <param name="value"></param>
		public void Set( Animator animator, float value )
		{
			if( animator.isInitialized )
				animator.SetFloat( Index, value );
		}
	}
}
