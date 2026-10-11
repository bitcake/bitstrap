using UnityEngine;

namespace BitStrap.Examples
{
	public sealed class BitStreamExample : MonoBehaviour
	{
		[Header( "Edit the fields and click the buttons to test them!" )]
		[HelpBox( "Be careful. If the bit count is too low, it may corrupt data!", HelpBoxAttribute.MessageType.Warning )]
		[Range( 1, BitStream.IntBitCount )]
		public int intBitCount = 4;

		public int[] intsToAdd = new int[] { 1, 2, 3 };

		[Button]
		public void WriteAndReadInts()
		{
			var bitStream = new BitStream( intsToAdd.Length * intBitCount );

			bitStream.Clear();
			foreach( int i in intsToAdd )
				bitStream.Write( i, intBitCount );

			Debug.Log( bitStream );

			for( int i = 0; i < intsToAdd.Length; i++ )
				Debug.Log( bitStream.ReadInt( intBitCount ) );
		}

		[Button]
		public void WriteAndReadIntsSimulatingNetwork()
		{
			var bitStream = new BitStream( intsToAdd.Length * intBitCount );

			bitStream.Clear();
			foreach( int i in intsToAdd )
				bitStream.Write( i, intBitCount );

			byte[] buffer = bitStream.GetBuffer();
			byte[] bufferCopy = new byte[buffer.Length];
			System.Array.Copy( buffer, bufferCopy, buffer.Length );

			// Simulate copying bytes from network
			bitStream.LoadBuffer( bufferCopy );

			Debug.Log( bitStream );

			for( int i = 0; i < intsToAdd.Length; i++ )
				Debug.Log( bitStream.ReadInt( intBitCount ) );
		}
	}
}