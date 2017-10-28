using System.Collections;
using System.Text;

namespace BitStrap
{
	/// <summary>
	/// Serialize bools, ints and floats with bit precision.
	/// Good for critical network data compressing.
	/// </summary>
	public sealed class BitStream
	{
		public const int ByteSize = 8;
		public const int IntBitCount = 31;

		private readonly BitArray bitBuffer;
		private readonly byte[] byteBuffer;

		private int position;

		/// <summary>
		/// Constructs a new BitStream given a bit count for the buffer.
		/// </summary>
		/// <param name="bitBufferSize"></param>
		public BitStream( int bitBufferSize )
		{
			bitBuffer = new BitArray( bitBufferSize );

			int byteBufferSize = bitBufferSize / ByteSize;
			if( bitBufferSize % ByteSize > 0 )
				byteBufferSize += 1;

			byteBuffer = new byte[byteBufferSize];
			position = 0;
		}

		/// <summary>
		/// Clear the stream from previous values.
		/// </summary>
		public void Clear()
		{
			position = 0;
		}

		/// <summary>
		/// Get access to the stream buffer.
		/// Send this over the network.
		/// </summary>
		/// <returns></returns>
		public byte[] GetBuffer()
		{
			bitBuffer.CopyTo( byteBuffer, 0 );
			return byteBuffer;
		}

		/// <summary>
		/// Loads some buffer's bits to read from them.
		/// </summary>
		public void LoadBuffer( byte[] buffer )
		{
			int byteCount = buffer.Length;
			int bitCount = byteCount * ByteSize;

			if( bitCount > bitBuffer.Length )
				bitCount = bitBuffer.Length;

			position = bitCount;

			for( int i = 0; i < byteCount; i++ )
			{
				for( int j = ByteSize - 1; j >= 0; j-- )
				{
					int bitIndex = i * ByteSize + j;
					if( bitIndex >= bitCount )
						break;

					bitBuffer.Set( bitIndex, ( ( ( buffer[i] >> j ) & 1 ) > 0 ) );
				}
			}
		}

		/// <summary>
		/// Reads a bool from the stream.
		/// Reads a single bit.
		/// </summary>
		/// <returns></returns>
		public bool ReadBool()
		{
			position--;
			return bitBuffer.Get( bitBuffer.Length - 1 - position );
		}

		/// <summary>
		/// Reads an int from the stream.
		/// Reads <paramref name="bitCount"/> bits.
		/// </summary>
		/// <param name="bitCount"></param>
		/// <returns></returns>
		public int ReadInt( int bitCount )
		{
			int value = 0;
			for( int i = bitCount - 1; i >= 0; i-- )
			{
				if( ReadBool() )
					value += ( 1 << i );
			}

			return value;
		}

		/// <summary>
		/// Reads a float from the stream.
		/// Reads <paramref name="bitCount"/> bits.
		/// Then they're converted from an int fixed point to float.
		/// </summary>
		/// <param name="bitCount"></param>
		/// <param name="fixedPointScale"></param>
		/// <returns></returns>
		public float ReadFloat( int bitCount, int fixedPointScale )
		{
			int fixedValue = ReadInt( bitCount );
			return ( ( float ) fixedValue ) / fixedPointScale;
		}

		/// <summary>
		/// Writes a bool to the stream.
		/// Stores a single bit.
		/// </summary>
		/// <param name="b"></param>
		public void Write( bool b )
		{
			bitBuffer.Set( position, b );
			position++;
		}

		/// <summary>
		/// Writes a int to the stream.
		/// Stores <paramref name="bitCount"/> bits.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="bitCount"></param>
		/// <returns></returns>
		public bool Write( int value, int bitCount )
		{
			bool clamped = bitCount > IntBitCount;
			if( clamped )
				bitCount = IntBitCount;

			for( int i = bitCount - 1; i >= 0; i-- )
			{
				bool isOne = ( ( value >> i ) & 1 ) > 0;
				Write( isOne );
			}

			return !clamped;
		}

		/// <summary>
		/// Writes a fload to the stream.
		/// First it converts the float to a fixed point number stored as an int.
		/// Then it's stored with <paramref name="bitCount"/> bits.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="bitCount"></param>
		/// <param name="fixedPointScale"></param>
		/// <returns></returns>
		public bool Write( float value, int bitCount, int fixedPointScale )
		{
			int fixedValue = ( int ) ( value * fixedPointScale );
			return Write( fixedValue, bitCount );
		}

		/// <summary>
		/// String containing 1s and 0s representing the stream bits.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder( position );
			for( int i = 0; i < bitBuffer.Length; i++ )
				sb.Append( bitBuffer.Get( i ) ? "1" : "0" );
			return sb.ToString();
		}
	}
}