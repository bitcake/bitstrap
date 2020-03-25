using UnityEngine;
using System.Collections;


/// <summary>
/// Bunch of utility methods to the int class.
/// </summary>
public static class IntExtensions
{

    /// <summary>
    /// Maps value from original range to new range
    /// </summary>
    /// <param name="value">value to map</param>
    /// <param name="min1">original range min</param>
    /// <param name="max1">original range max</param>
    /// <param name="min2">new range min</param>
    /// <param name="max2">new range max</param>
    /// <returns>value in new range</returns>
    public static int MapRange(this int value, int min1, int max1, int min2, int max2)
    {
        return (int)Mathf.Round( (value - min1) / ((float)max1 - min1) * (max2 - min2) + min2 );
    }
}

