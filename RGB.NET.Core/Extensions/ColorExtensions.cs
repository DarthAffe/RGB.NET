using System;

namespace RGB.NET.Core;

/// <summary>
/// Offers some extensions and helper-methods for <see cref="Color"/> related things.
/// </summary>
public static class ColorExtensions
{
    #region Methods

    /// <summary>
    /// Calculates the distance between the two specified colors using the redmean algorithm.
    /// For more infos check https://www.compuphase.com/cmetric.htm
    /// </summary>
    /// <param name="color1">The start color of the distance calculation.</param>
    /// <param name="color2">The end color fot the distance calculation.</param>
    /// <returns>The redmean distance between the two specified colors.</returns>
    public static double DistanceTo(this in Color color1, in Color color2)
    {
        (_, byte r1, byte g1, byte b1) = color1.GetRGBBytes();
        (_, byte r2, byte g2, byte b2) = color2.GetRGBBytes();

        long rmean = (r1 + r2) / 2;
        long r = r1 - r2;
        long g = g1 - g2;
        long b = b1 - b2;
        return Math.Sqrt((((512 + rmean) * r * r) >> 8) + (4 * g * g) + (((767 - rmean) * b * b) >> 8));
    }

    #endregion
}