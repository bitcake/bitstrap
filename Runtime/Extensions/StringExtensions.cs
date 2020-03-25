using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// Bunch of utility extension methods to the string class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string to a DateTime in the format: yyyy-MM-dd HH:mm:ss zzz
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static System.DateTime ToDateTime( this string self )
        {
            try
            {
                return System.DateTime.ParseExact( self, "yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture );
            }
            catch( System.Exception )
            {
                return System.DateTime.MinValue;
            }
        }

        /// <summary>
        /// It's like string.Split() however it splits "CamelCase" words.
        /// </summary>
        /// <example>
        /// string text = "CamelCase";
        /// text.SeparateCamelCase(); // => text = "Camel Case"
        /// </example>
        /// <param name="self"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string SeparateCamelCase( this string self, string separator = " " )
        {
            return Regex.Replace( self, "(?<=[a-z])([A-Z])", separator + "$1" ).Trim();
        }

        /// <summary>
        /// Calculates how different two strings are.
        ///
        /// Algorithm source: http://en.wikibooks.org/wiki/Algorithm_implementation/Strings/Levenshtein_distance#C.23
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>The number of modifications needed to make source equals target.
        /// Zero means they are the same</returns>
        public static int Distance( this string source, string target )
        {
            if( string.IsNullOrEmpty( source ) )
            {
                if( string.IsNullOrEmpty( target ) )
                    return 0;
                return target.Length;
            }

            if( string.IsNullOrEmpty( target ) )
                return source.Length;

            if( source.Length > target.Length )
            {
                var temp = target;
                target = source;
                source = temp;
            }

            var m = target.Length;
            var n = source.Length;
            var distance = new int[2, m + 1];

            // Initialize the distance 'matrix'
            for( var j = 1; j <= m; j++ )
                distance[0, j] = j;

            var currentRow = 0;
            for( var i = 1; i <= n; ++i )
            {
                currentRow = i & 1;
                distance[currentRow, 0] = i;
                var previousRow = currentRow ^ 1;

                for( var j = 1; j <= m; j++ )
                {
                    var cost = ( target[j - 1] == source[i - 1] ? 0 : 1 );
                    distance[currentRow, j] = Mathf.Min(
                        Mathf.Min(
                            distance[previousRow, j] + 1,
                            distance[currentRow, j - 1] + 1 ),
                        distance[previousRow, j - 1] + cost );
                }
            }

            return distance[currentRow, m];
        }
    }
}
