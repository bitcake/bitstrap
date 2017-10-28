using UnityEngine;

namespace BitStrap.Examples
{
	public class NumberBoundsExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public IntBounds intBounds = new IntBounds( 10, 20 );

		public FloatBounds floatBounds = new FloatBounds( -1.0f, 1.0f );

		[Range( 0.0f, 1.0f )]
		public float lerpT = 0.5f;

		[Button]
		public void GetMaxOfIntBounds()
		{
			Debug.LogFormat( "Int Bounds Max: {0}", intBounds.Max );
		}

		[Button]
		public void GetMinOfIntBounds()
		{
			Debug.LogFormat( "Int Bounds Max: {0}", intBounds.Min );
		}

		[Button]
		public void LerpFloatBoundsAtT()
		{
			Debug.LogFormat( "Lerp Float Bounds at T: {0}", floatBounds.Lerp( lerpT ) );
		}
	}
}
