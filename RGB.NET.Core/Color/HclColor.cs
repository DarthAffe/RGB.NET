// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
using System;

namespace RGB.NET.Core;

/// <summary>
/// Contains helper-methods and extension for the <see cref="Color"/>-type to work in the Hcl color space.
/// </summary>
public static class HclColor
{
    #region Getter

    /// <summary>
    /// Gets the H component value (Hcl-color space) of this <see cref="Color"/> in the range [0..360].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The H component value of the color. </returns>
    public static float GetHclH(this in Color color) => color.GetHcl().h;

    /// <summary>
    /// Gets the c component value (Hcl-color space) of this <see cref="Color"/> in the range [0..1].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The c component value of the color. </returns>
    public static float GetHclC(this in Color color) => color.GetHcl().c;

    /// <summary>
    /// Gets the l component value (Hcl-color space) of this <see cref="Color"/> in the range [0..1].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The l component value of the color. </returns>
    public static float GetHclL(this in Color color) => color.GetHcl().l;

    /// <summary>
    /// Gets the H, c and l component values (Hcl-color space) of this <see cref="Color"/>.
    /// H in the range [0..360].
    /// c in the range [0..1].
    /// l in the range [0..1].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>A tuple containing the H, c and l component value of the color.</returns>
    public static (float h, float c, float l) GetHcl(this in Color color)
        => CalculateHclFromRGB(color.R, color.G, color.B);

    #endregion

    #region Manipulation

    /// <summary>
    /// Adds the specified Hcl values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="h">The H value to add.</param>
    /// <param name="c">The c value to add.</param>
    /// <param name="l">The l value to add.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color AddHcl(this in Color color, float h = 0, float c = 0, float l = 0)
    {
        (float cH, float cC, float cL) = color.GetHcl();
        return Create(color.A, cH + h, cC + c, cL + l);
    }

    /// <summary>
    /// Subtracts the specified Hcl values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="h">The H value to subtract.</param>
    /// <param name="c">The c value to subtract.</param>
    /// <param name="l">The l value to subtract.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SubtractHcl(this in Color color, float h = 0, float c = 0, float l = 0)
    {
        (float cH, float cC, float cL) = color.GetHcl();
        return Create(color.A, cH - h, cC - c, cL - l);
    }

    /// <summary>
    /// Multiplies the specified Hcl values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="h">The H value to multiply.</param>
    /// <param name="c">The c value to multiply.</param>
    /// <param name="l">The l value to multiply.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color MultiplyHcl(this in Color color, float h = 1, float c = 1, float l = 1)
    {
        (float cH, float cC, float cL) = color.GetHcl();
        return Create(color.A, cH * h, cC * c, cL * l);
    }

    /// <summary>
    /// Divides the specified Hcl values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="h">The H value to divide.</param>
    /// <param name="c">The c value to divide.</param>
    /// <param name="l">The l value to divide.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color DivideHcl(this in Color color, float h = 1, float c = 1, float l = 1)
    {
        (float cH, float cC, float cL) = color.GetHcl();
        return Create(color.A, cH / h, cC / c, cL / l);
    }

    /// <summary>
    /// Sets the specified X value of this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="h">The H value to set.</param>
    /// <param name="c">The c value to set.</param>
    /// <param name="l">The l value to set.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SetHcl(this in Color color, float? h = null, float? c = null, float? l = null)
    {
        (float cH, float cC, float cL) = color.GetHcl();
        return Create(color.A, h ?? cH, c ?? cC, l ?? cL);
    }

    #endregion

    #region Factory

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using Hcl-Values. 
    /// </summary>
    /// <param name="h">The H component value of this <see cref="Color"/>.</param>
    /// <param name="c">The c component value of this <see cref="Color"/>.</param>
    /// <param name="l">The l component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(float h, float c, float l)
        => Create(1.0f, h, c, l);

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using alpha and Hcl-Values. 
    /// </summary>
    /// <param name="alpha">The alphc component value of this <see cref="Color"/>.</param>
    /// <param name="h">The H component value of this <see cref="Color"/>.</param>
    /// <param name="c">The c component value of this <see cref="Color"/>.</param>
    /// <param name="l">The l component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(byte alpha, float h, float c, float l)
        => Create((float)alpha / byte.MaxValue, h, c, l);

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using alpha and Hcl-Values. 
    /// </summary>
    /// <param name="alpha">The alphc component value of this <see cref="Color"/>.</param>
    /// <param name="h">The H component value of this <see cref="Color"/>.</param>
    /// <param name="c">The c component value of this <see cref="Color"/>.</param>
    /// <param name="l">The l component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(int alpha, float h, float c, float l)
        => Create((float)alpha / byte.MaxValue, h, c, l);

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using alpha and Hcl-Values. 
    /// </summary>
    /// <param name="alpha">The alphc component value of this <see cref="Color"/>.</param>
    /// <param name="h">The H component value of this <see cref="Color"/>.</param>
    /// <param name="c">The c component value of this <see cref="Color"/>.</param>
    /// <param name="l">The l component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(float alpha, float h, float c, float l)
    {
        (float r, float g, float b) = CalculateRGBFromHcl(h, c, l);
        return new Color(alpha, r, g, b);
    }

    #endregion

    #region Helper

    private static (float h, float c, float l) CalculateHclFromRGB(float r, float g, float b)
    {
        const float RADIANS_DEGREES_CONVERSION = 180.0f / MathF.PI;

        // ReSharper disable once InconsistentNaming - b is used above
        (float l, float a, float _b) = LabColor.CalculateLabFromRGB(r, g, b);

        float h, c;
        if (r.EqualsInTolerance(g) && r.EqualsInTolerance(b)) //DarthAffe 26.02.2021: The cumulated rounding errors are big enough to cause problems in that case
        {
            h = 0;
            c = 0;
        }
        else
        {
            h = MathF.Atan2(_b, a);
            if (h >= 0) h *= RADIANS_DEGREES_CONVERSION;
            else h = 360 - (-h * RADIANS_DEGREES_CONVERSION);

            c = MathF.Sqrt((a * a) + (_b * _b));
        }

        return (h, c, l);
    }

    private static (float r, float g, float b) CalculateRGBFromHcl(float h, float c, float l)
    {
        const float DEGREES_RADIANS_CONVERSION = MathF.PI / 180.0f;

        h *= DEGREES_RADIANS_CONVERSION;
        float a = c * MathF.Cos(h);
        float b = c * MathF.Sin(h);

        return LabColor.CalculateRGBFromLab(l, a, b);
    }

    #endregion
}