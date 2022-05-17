using System;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Helps with optimization, caching the index of the parameter name for you automatically
	/// Helps with type-safe animation parameters
	/// Has a nice editor that allows you to choose a parameter from a dropdown menu in the inspector, based on the animator of the current object
	/// </summary>
	public abstract class AnimationParameter
	{
		public string name;

		[NonSerialized]
		private bool cached = false;
		[NonSerialized]
		private int index;
		
		public int Index
		{
			get
			{
				if (!cached)
				{
					index = Animator.StringToHash(name);
					cached = true;
				}
				
				return index;
			}
		}
	}
}
