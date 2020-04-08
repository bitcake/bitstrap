using System;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// An Int Animation Parameter
	/// </summary>
	[Serializable]
	public class IntAnimationParameter : AnimationParameter
	{
		/// <summary>
		/// Sets the value of the selected parameter, based on value
		/// </summary>
		/// <param name="animator"></param>
		/// <param name="value"></param>
		public void Set( Animator animator, int value )
		{
			if( animator.isInitialized )
				animator.SetInteger( Index, value );
		}
	}
}
