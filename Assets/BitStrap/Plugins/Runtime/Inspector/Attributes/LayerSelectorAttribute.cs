using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// Layer helper class.
    /// </summary>
    public static class LayerHelper
    {
        /// <summary>
        /// Calculates the layer mask from a layer index.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        public static int GetMask( int layerIndex )
        {
            return 1 << layerIndex;
        }

        /// <summary>
        /// Calculates the bit mask of a set of layer indexes.
        /// </summary>
        /// <param name="layerIndexes"></param>
        /// <returns></returns>
        public static int GetMask( int[] layerIndexes )
        {
            int layerMask = 0;
            foreach( int layerIndex in layerIndexes )
            {
                layerMask |= GetMask( layerIndex );
            }

            return layerMask;
        }
    }

    /// <summary>
    /// Put this attribute above an int field and it will draw like a layer picker in the inspector.
    /// </summary>
    [System.AttributeUsage( System.AttributeTargets.Field, AllowMultiple = false )]
    public class LayerSelectorAttribute : PropertyAttribute
    {
    }
}
