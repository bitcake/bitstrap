using System.Collections;
using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// Bunch of Vector math methods that you won't find neither in Vector3 or Vector2.
    /// </summary>
    public static class VectorHelper
    {
        /// <summary>
        /// Rotates a unit vector towards target.
        /// </summary>
        /// <param name="current">Unit vector to be rotated</param>
        /// <param name="target">Target rotation</param>
        /// <param name="maxRadiansDelta"></param>
        /// <returns></returns>
        public static Vector2 UnitRotateTowards( Vector2 current, Vector2 target, float maxRadiansDelta )
        {
            float currentAngle = Mathf.Atan2( current.y, current.x ) * Mathf.Rad2Deg;
            float targetAngle = Mathf.Atan2( target.y, target.x ) * Mathf.Rad2Deg;

            float angle = Mathf.MoveTowardsAngle( currentAngle, targetAngle, maxRadiansDelta * Mathf.Rad2Deg ) * Mathf.Deg2Rad;

            return new Vector2( Mathf.Cos( angle ), Mathf.Sin( angle ) );
        }

        /// <summary>
        /// Check if a vector is between two other coplanar vectors.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <returns></returns>
        public static bool IsBetweenVectors( Vector3 direction, Vector3 lowerBound, Vector3 upperBound )
        {
            return IsOnVectorSide( direction, lowerBound, upperBound ) && IsOnVectorSide( direction, upperBound, lowerBound );
        }

        /// <summary>
        /// Check if a vector is to the side of another one given a coplanar pivot vector.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="vector"></param>
        /// <param name="sidePivot"></param>
        /// <returns></returns>
        public static bool IsOnVectorSide( Vector3 direction, Vector3 vector, Vector3 sidePivot )
        {
            return Vector3.Dot( Vector3.Cross( vector, direction ), Vector3.Cross( vector, sidePivot ) ) > 0.0f;
        }

        /// <summary>
        /// Returns true if a vector's magnitude is very small.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static bool IsZero( this Vector3 vec )
        {
            return vec.sqrMagnitude <= 0.001f;
        }

        /// <summary>
        /// Returns true if a vector's magnitude is very small.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static bool IsZero( this Vector2 vec )
        {
            return vec.sqrMagnitude <= 0.001f;
        }
    }
}
