namespace BitStrap
{
	/// <summary>
	/// Fast and simple pseudo random generator class that allows you to reset its seed.
	/// Used mainly inside SecureInt, but you can also use it whenever you need a garbage-free 
	/// random generator where you can set a specific seed.
	/// </summary>
	public class FastRandom
    {
        private const float REAL_UNIT_INT = 1.0f / ( 1.0f + int.MaxValue );

        private uint x;
        private uint y;
        private uint z;
        private uint w;

        public FastRandom()
        {
            SetSeed( System.Environment.TickCount );
        }

        /// <summary>
        /// Get the next pseudo random int.
        /// </summary>
        /// <returns></returns>
        public int GetNextInt()
        {
            uint t = x ^ ( x << 11 );

            x = y;
            y = z;
            z = w;
            w = ( w ^ ( w >> 19 ) ) ^ ( t ^ ( t >> 8 ) );

            return ( int ) ( 0x7FFFFFFF & w );
        }

        /// <summary>
        /// Get the next pseudo random float.
        /// </summary>
        /// <returns></returns>
        public float GetNextFloat()
        {
            return REAL_UNIT_INT * GetNextInt();
        }

        /// <summary>
        /// Reset the RNG seed.
        /// </summary>
        /// <param name="seed"></param>
        public void SetSeed( int seed )
        {
            x = ( uint ) ( ( seed * 1431655781 ) + ( seed * 1183186591 ) + ( seed * 622729787 ) + ( seed * 338294347 ) );

            y = 842502087;
            z = 3579807591;
            w = 273326509;
        }
    }
}
