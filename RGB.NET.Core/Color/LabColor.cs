// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
using System;

namespace RGB.NET.Core;

/// <summary>
/// Contains helper-methods and extension for the <see cref="Color"/>-type to work in the Lab color space.
/// </summary>
public static class LabColor
{
    #region Getter

    /// <summary>
    /// Gets the L component value (Lab-color space) of this <see cref="Color"/> in the range [0..100].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The L component value of the color.</returns>
    public static float GetLabL(this in Color color) => color.GetLab().l;

    /// <summary>
    /// Gets the a component value (Lab-color space) of this <see cref="Color"/> in the range [0..1].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The a component value of the color.</returns>
    public static float GetLabA(this in Color color) => color.GetLab().a;

    /// <summary>
    /// Gets the b component value (Lab-color space) of this <see cref="Color"/> in the range [0..1].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The b component value of the color.</returns>
    public static float GetLabB(this in Color color) => color.GetLab().b;

    /// <summary>
    /// Gets the L, a and b component values (Lab-color space) of this <see cref="Color"/>.
    /// L in the range [0..100].
    /// a in the range [0..1].
    /// b in the range [0..1].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>A tuple containing the L, a and b component value of the color.</returns>
    public static (float l, float a, float b) GetLab(this in Color color)
        => CalculateLabFromRGB(color.R, color.G, color.B);

    #endregion

    #region Manipulation

    /// <summary>
    /// Adds the specified Lab values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="l">The L value to add.</param>
    /// <param name="a">The a value to add.</param>
    /// <param name="b">The b value to add.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color AddLab(this in Color color, float l = 0, float a = 0, float b = 0)
    {
        (float cL, float cA, float cB) = color.GetLab();
        return Create(color.A, cL + l, cA + a, cB + b);
    }

    /// <summary>
    /// Subtracts the specified Lab values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="l">The L value to subtract.</param>
    /// <param name="a">The a value to subtract.</param>
    /// <param name="b">The b value to subtract.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SubtractLab(this in Color color, float l = 0, float a = 0, float b = 0)
    {
        (float cL, float cA, float cB) = color.GetLab();
        return Create(color.A, cL - l, cA - a, cB - b);
    }

    /// <summary>
    /// Multiplies the specified Lab values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="l">The L value to multiply.</param>
    /// <param name="a">The a value to multiply.</param>
    /// <param name="b">The b value to multiply.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color MultiplyLab(this in Color color, float l = 1, float a = 1, float b = 1)
    {
        (float cL, float cA, float cB) = color.GetLab();
        return Create(color.A, cL * l, cA * a, cB * b);
    }

    /// <summary>
    /// Divides the specified Lab values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="l">The L value to divide.</param>
    /// <param name="a">The a value to divide.</param>
    /// <param name="b">The b value to divide.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color DivideLab(this in Color color, float l = 1, float a = 1, float b = 1)
    {
        (float cL, float cA, float cB) = color.GetLab();
        return Create(color.A, cL / l, cA / a, cB / b);
    }

    /// <summary>
    /// Sets the specified X valueof this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="l">The L value to set.</param>
    /// <param name="a">The a value to set.</param>
    /// <param name="b">The b value to set.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SetLab(this in Color color, float? l = null, float? a = null, float? b = null)
    {
        (float cL, float cA, float cB) = color.GetLab();
        return Create(color.A, l ?? cL, a ?? cA, b ?? cB);
    }

    #endregion

    #region Factory

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using Lab-Values. 
    /// </summary>
    /// <param name="l">The L component value of this <see cref="Color"/>.</param>
    /// <param name="a">The a component value of this <see cref="Color"/>.</param>
    /// <param name="b">The b component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(float l, float a, float b)
        => Create(1.0f, l, a, b);

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using alpha and Lab-Values. 
    /// </summary>
    /// <param name="alpha">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="l">The L component value of this <see cref="Color"/>.</param>
    /// <param name="a">The a component value of this <see cref="Color"/>.</param>
    /// <param name="b">The b component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(byte alpha, float l, float a, float b)
        => Create((float)alpha / byte.MaxValue, l, a, b);

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using alpha and Lab-Values. 
    /// </summary>
    /// <param name="alpha">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="l">The L component value of this <see cref="Color"/>.</param>
    /// <param name="a">The a component value of this <see cref="Color"/>.</param>
    /// <param name="b">The b component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(int alpha, float l, float a, float b)
        => Create((float)alpha / byte.MaxValue, l, a, b);

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using alpha and Lab-Values. 
    /// </summary>
    /// <param name="alpha">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="l">The L component value of this <see cref="Color"/>.</param>
    /// <param name="a">The a component value of this <see cref="Color"/>.</param>
    /// <param name="b">The b component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(float alpha, float l, float a, float b)
    {
        // ReSharper disable once InconsistentNaming - b is used above
        (float r, float g, float _b) = CalculateRGBFromLab(l, a, b);
        return new Color(alpha, r, g, _b);
    }

    #endregion

    #region Helper

    internal static (float l, float a, float b) CalculateLabFromRGB(float r, float g, float b)
    {
        (float x, float y, float z) = XYZColor.CaclulateXYZFromRGB(r, g, b);
        return CaclulateLabFromXYZ(x, y, z);
    }

    internal static (float r, float g, float b) CalculateRGBFromLab(float l, float a, float b)
    {
        (float x, float y, float z) = CalculateXYZFromLab(l, a, b);
        return XYZColor.CalculateRGBFromXYZ(x, y, z);
    }

    private static (float l, float a, float b) CaclulateLabFromXYZ(float x, float y, float z)
    {
        const float ONETHRID = 1.0f / 3.0f;
        const float FACTOR2 = 16.0f / 116.0f;

        x /= 95.047f;
        y /= 100.0f;
        z /= 108.883f;

        x = ((x > 0.008856f) ? (MathF.Pow(x, ONETHRID)) : ((7.787f * x) + FACTOR2));
        y = ((y > 0.008856f) ? (MathF.Pow(y, ONETHRID)) : ((7.787f * y) + FACTOR2));
        z = ((z > 0.008856f) ? (MathF.Pow(z, ONETHRID)) : ((7.787f * z) + FACTOR2));

        float l = (116.0f * y) - 16.0f;
        float a = 500.0f * (x - y);
        float b = 200.0f * (y - z);

        return (l, a, b);
    }

    private static (float x, float y, float z) CalculateXYZFromLab(float l, float a, float b)
    {
        const float FACTOR2 = 16.0f / 116.0f;

        float y = (l + 16.0f) / 116.0f;
        float x = (a / 500.0f) + y;
        float z = y - (b / 200.0f);

        float powX = MathF.Pow(x, 3.0f);
        float powY = MathF.Pow(y, 3.0f);
        float powZ = MathF.Pow(z, 3.0f);

        x = ((powX > 0.008856f) ? (powX) : ((x - FACTOR2) / 7.787f));
        y = ((powY > 0.008856f) ? (powY) : ((y - FACTOR2) / 7.787f));
        z = ((powZ > 0.008856f) ? (powZ) : ((z - FACTOR2) / 7.787f));

        return (x * 95.047f, y * 100.0f, z * 108.883f);
    }

    #endregion
}