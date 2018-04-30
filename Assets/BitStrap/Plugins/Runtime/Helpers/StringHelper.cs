using System.Collections.Generic;

namespace BitStrap
{
    /// <summary>
    /// Bunch of helper methods to work with the string class.
    /// </summary>
    public static class StringHelper
    {
        private class IndexComparer : IEqualityComparer<Index>
        {
            public bool Equals( Index x, Index y )
            {
                return x.Equals( y );
            }

            public int GetHashCode( Index x )
            {
                return x.GetHashCode();
            }
        }

        private struct Index
        {
            public readonly int number;
            public readonly string format;
            private readonly int hashCode;

            public Index( string format, int number )
            {
                this.number = number;
                this.format = format;
                this.hashCode = number * format.GetHashCode();
            }

            public bool Equals( Index other )
            {
                return number == other.number && format == other.format;
            }

            public override int GetHashCode()
            {
                return hashCode;
            }

            public string GetString()
            {
                return string.Format( format, number );
            }
        }

        private static Dictionary<Index, string> stringTable = new Dictionary<Index, string>( new IndexComparer() );

        /// <summary>
        /// Given a string format and a number, returns its string representation.
        /// It's better to use this method than just number.ToString() since,
        /// in the long term, it does not generate string garbage.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string Format( string format, int number )
        {
            Index ui = new Index( format, number );

            if( !stringTable.ContainsKey( ui ) )
                stringTable[ui] = ui.GetString();

            return stringTable[ui];
        }

        /// <summary>
        /// The same as StringHelper.Format() but as an int extension method.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToStringSmart( this int n, string format = "{0}" )
        {
            return Format( format, n );
        }
    }
}
