using UnityEngine;
using System.Collections;


/// <summary>
/// Bunch of utility methods to the float class.
/// </summary>
public static class FloatExtensions
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
    public static float MapRange(this float value, float min1, float max1, float min2, float max2)
    {
        return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
    }
}

