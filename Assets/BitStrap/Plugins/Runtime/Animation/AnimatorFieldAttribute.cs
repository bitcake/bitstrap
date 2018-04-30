namespace BitStrap
{
	/// <summary>
	/// Lets you use AnimationParameters even when it's MonoBehaviour does not have a sibling Animator component.
	/// </summary>
	[System.AttributeUsage( System.AttributeTargets.Field, AllowMultiple = false, Inherited = true )]
	public sealed class AnimatorFieldAttribute : System.Attribute
	{
		public string animatorFieldName;

		public AnimatorFieldAttribute( string animatorFieldName )
		{
			this.animatorFieldName = animatorFieldName;
		}
	}
}