using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class NumberRangeExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public IntRange intRange = new IntRange( 10, 20 );

		public FloatRange floatRange = new FloatRange( -1.0f, 1.0f );
		public DoubleRange doubleRange = new DoubleRange( -1.0, 1.0 );

		[Range( 0.0f, 1.0f )]
		public float lerpT = 0.5f;

		[Button]
		public void GetMaxOfIntBounds()
		{
			Debug.LogFormat( "Int Bounds Max: {0}", intRange.Max );
		}

		[Button]
		public void GetMinOfIntBounds()
		{
			Debug.LogFormat( "Int Bounds Max: {0}", intRange.Min );
		}

		[Button]
		public void LerpFloatBoundsAtT()
		{
			Debug.LogFormat( "Lerp Float Bounds at T: {0}", floatRange.Lerp( lerpT ) );
		}
	}
}
