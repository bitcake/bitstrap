using UnityEngine;

namespace BitStrap.Examples
{
	public class IndirectAnimationParametersExample : MonoBehaviour
	{
		[AnimatorField( "animator" )]
		public BoolAnimationParameter boolParameter;
		[AnimatorField( "animator" )]
		public IntAnimationParameter intParameter;
		[AnimatorField( "animator" )]
		public FloatAnimationParameter floatParameter;
		[AnimatorField( "animator" )]
		public TriggerAnimationParameter trigger;

		public Animator animator;
	}
}