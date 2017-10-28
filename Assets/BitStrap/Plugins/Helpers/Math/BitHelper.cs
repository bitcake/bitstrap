using System.Text;

namespace BitStrap
{
    /// <summary>
    /// Helper class for working with the bits inside an int.
    /// </summary>
    public static class BitHelper
    {
        private const int IntSize = 32;
        private readonly static int[] randIntsArray = new int[IntSize];
        private readonly static bool[] bitsArray = new bool[IntSize];
        private readonly static StringBuilder stringBuilder = new StringBuilder( IntSize );
        private readonly static FastRandom fastRandom = new FastRandom();

        /// <summary>
        /// Prety formats an int's bits.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToBinaryString( this int v )
        {
            stringBuilder.Length = 0;

            for( int i = 0; i < IntSize; i++ )
            {
                int mask = 1 << i;
                bitsArray[i] = ( v & mask ) != 0;
            }

            for( int i = IntSize - 1; i >= 0; i-- )
            {
                stringBuilder.Append( bitsArray[i] ? '1' : '0' );
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Shuffles an int's bits given a shuffle key.
        /// Also, offset can be used so you can guarantee that you have bits
        /// to shuffle (e.g. num is greater than zero).
        /// </summary>
        /// <param name="num"></param>
        /// <param name="key"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int ShuffleBits( int num, int key, int offset = 0 )
        {
            fastRandom.SetSeed( key );

            num += offset;

            for( int i = 0; i < IntSize; i++ )
            {
                int result = fastRandom.GetNextInt();
                num = SwapBits( num, i, result % IntSize );
            }

            return num;
        }

        /// <summary>
        /// Unshuffles an int's bits given the same shuffle key that was used to shuffle it.
        /// Also remember to use the same offset so you can correctly restore the original value.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="key"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int UnshuffleBits( int num, int key, int offset = 0 )
        {
            fastRandom.SetSeed( key );

            for( int i = 0; i < IntSize; i++ )
            {
                randIntsArray[i] = fastRandom.GetNextInt();
            }

            for( int i = IntSize - 1; i >= 0; i-- )
            {
                int result = randIntsArray[i];
                num = SwapBits( num, i, result % IntSize );
            }

            return num - offset;
        }

        /// <summary>
        /// Swap two bits inside an int.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="indexA"></param>
        /// <param name="indexB"></param>
        /// <returns></returns>
        public static int SwapBits( int num, int indexA, int indexB )
        {
            int maskA = 1 << indexA;
            int maskB = 1 << indexB;

            bool a = ( num & maskA ) != 0;
            bool b = ( num & maskB ) != 0;

            num = a ? ( num | maskB ) : ( num & ( ~maskB ) );
            num = b ? ( num | maskA ) : ( num & ( ~maskA ) );

            return num;
        }
    }
}
