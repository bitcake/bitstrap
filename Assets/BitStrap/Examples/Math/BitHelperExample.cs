using UnityEngine;

namespace BitStrap.Examples
{
	public class BitHelperExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!", order = 1 )]
		[HelpBox( "Key must be the same to restore original on Unshuffle!", HelpBoxAttribute.MessageType.Warning, order = 2 )]
		public int integer = 7;

		public int key = 3;

		[ReadOnly]
		public int shuffledBitsInteger = 7;

		[Button]
		public void ShowIntegerBits()
		{
			Debug.LogFormat( "Integer bits: \"{0}\"", integer.ToBinaryString() );
		}

		[Button]
		public void ShuffleIntegerBits()
		{
			shuffledBitsInteger = BitHelper.ShuffleBits( integer, key );
			Debug.LogFormat( "Integer was \"{0}\". Now is \"{1}\"", integer.ToBinaryString(), shuffledBitsInteger.ToBinaryString() );
		}

		[Button]
		public void UnshuffleIntegerBits()
		{
			integer = BitHelper.UnshuffleBits( shuffledBitsInteger, key );
			Debug.LogFormat( "Suffled integer was \"{0}\". Now is \"{1}\"", shuffledBitsInteger.ToBinaryString(), integer.ToBinaryString() );
		}
	}
}
