using UnityEngine;

namespace BitStrap.Examples
{
	public class ColorHelperExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		public Color color = new Color( 1.0f, 0.0f, 0.0f );

		[ReadOnly]
		public int encodedColor;

		[Button]
		public void EncodeToInteger()
		{
			encodedColor = ColorHelper.Encode( color );
			Debug.Log( encodedColor );
		}

		[Button]
		public void DecodeToInteger()
		{
			color = ColorHelper.Decode( encodedColor );
			Debug.LogFormat( "rgba = ({0}, {1}, {2}, {3})", color.r, color.g, color.b, color.a );
		}
	}
}
