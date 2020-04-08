using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// Bunch of utility extension methods to the Color class.
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// Compress a color object to an int.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int Encode( Color self )
        {
            Color32 color32 = self;

            int c = 0;

            c |= color32.a << 24;
            c |= color32.r << 16;
            c |= color32.g << 8;
            c |= color32.b;

            return c;
        }

        /// <summary>
        /// Decompress a int into a color object.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color Decode( int color )
        {
            byte colorAlpha = ( byte ) ( ( color >> 24 ) & 0xFF );
            byte colorRed = ( byte ) ( ( color >> 16 ) & 0xFF );
            byte colorGreen = ( byte ) ( ( color >> 8 ) & 0xFF );
            byte colorBlue = ( byte ) ( color & 0xFF );

            return new Color32( colorRed, colorGreen, colorBlue, colorAlpha );
        }
    }
}
