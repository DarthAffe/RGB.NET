// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
using System;

namespace RGB.NET.Core;

/// <summary>
/// Contains helper-methods and extension for the <see cref="Color"/>-type to work in the XYZ color space.
/// </summary>
public static class XYZColor
{
    #region Getter

    /// <summary>
    /// Gets the X component value (XYZ-color space) of this <see cref="Color"/> in the range [0..95.047].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The X component value of the color.</returns>
    public static float GetX(this in Color color) => color.GetXYZ().x;

    /// <summary>
    /// Gets the Y component value (XYZ-color space) of this <see cref="Color"/> in the range [0..100].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The Y component value of the color.</returns>
    public static float GetY(this in Color color) => color.GetXYZ().y;

    /// <summary>
    /// Gets the Z component value (XYZ-color space) of this <see cref="Color"/> in the range [0..108.883].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The Z component value of the color.</returns>
    public static float GetZ(this in Color color) => color.GetXYZ().z;

    /// <summary>
    /// Gets the X, Y and Z component values (XYZ-color space) of this <see cref="Color"/>.
    /// X in the range [0..95.047].
    /// Y in the range [0..100].
    /// Z in the range [0..108.883].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>A tuple containing the X, Y and Z component value of the color.</returns>
    public static (float x, float y, float z) GetXYZ(this in Color color)
        => CaclulateXYZFromRGB(color.R, color.G, color.B);

    #endregion

    #region Manipulation

    /// <summary>
    /// Adds the specified XYZ values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="x">The X value to add.</param>
    /// <param name="y">The Y value to add.</param>
    /// <param name="z">The Z value to add.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color AddXYZ(this in Color color, float x = 0, float y = 0, float z = 0)
    {
        (float cX, float cY, float cZ) = color.GetXYZ();
        return Create(color.A, cX + x, cY + y, cZ + z);
    }

    /// <summary>
    /// Subtracts the specified XYZ values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="x">The X value to subtract.</param>
    /// <param name="y">The Y value to subtract.</param>
    /// <param name="z">The Z value to subtract.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SubtractXYZ(this in Color color, float x = 0, float y = 0, float z = 0)
    {
        (float cX, float cY, float cZ) = color.GetXYZ();
        return Create(color.A, cX - x, cY - y, cZ - z);
    }

    /// <summary>
    /// Multiplies the specified XYZ values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="x">The X value to multiply.</param>
    /// <param name="y">The Y value to multiply.</param>
    /// <param name="z">The Z value to multiply.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color MultiplyXYZ(this in Color color, float x = 1, float y = 1, float z = 1)
    {
        (float cX, float cY, float cZ) = color.GetXYZ();
        return Create(color.A, cX * x, cY * y, cZ * z);
    }

    /// <summary>
    /// Divides the specified XYZ values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="x">The X value to divide.</param>
    /// <param name="y">The Y value to divide.</param>
    /// <param name="z">The Z value to divide.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color DivideXYZ(this in Color color, float x = 1, float y = 1, float z = 1)
    {
        (float cX, float cY, float cZ) = color.GetXYZ();
        return Create(color.A, cX / x, cY / y, cZ / z);
    }

    /// <summary>
    /// Sets the specified X valueof this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="x">The X value to set.</param>
    /// <param name="y">The Y value to set.</param>
    /// <param name="z">The Z value to set.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SetXYZ(this in Color color, float? x = null, float? y = null, float? z = null)
    {
        (float cX, float cY, float cZ) = color.GetXYZ();
        return Create(color.A, x ?? cX, y ?? cY, z ?? cZ);
    }

    #endregion

    #region Factory

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using XYZ-Values. 
    /// </summary>
    /// <param name="x">The X component value of this <see cref="Color"/>.</param>
    /// <param name="y">The Y component value of this <see cref="Color"/>.</param>
    /// <param name="z">The Z component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(float x, float y, float z)
        => Create(1.0f, x, y, z);

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using alpha and XYZ-Values. 
    /// </summary>
    /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="x">The X component value of this <see cref="Color"/>.</param>
    /// <param name="y">The Y component value of this <see cref="Color"/>.</param>
    /// <param name="z">The Z component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(byte a, float x, float y, float z)
        => Create((float)a / byte.MaxValue, x, y, z);

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using alpha and XYZ-Values. 
    /// </summary>
    /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="x">The X component value of this <see cref="Color"/>.</param>
    /// <param name="y">The Y component value of this <see cref="Color"/>.</param>
    /// <param name="z">The Z component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(int a, float x, float y, float z)
        => Create((float)a / byte.MaxValue, x, y, z);

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using alpha and XYZ-Values. 
    /// </summary>
    /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="x">The X component value of this <see cref="Color"/>.</param>
    /// <param name="y">The Y component value of this <see cref="Color"/>.</param>
    /// <param name="z">The Z component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(float a, float x, float y, float z)
    {
        (float r, float g, float b) = CalculateRGBFromXYZ(x, y, z);
        return new Color(a, r, g, b);
    }

    #endregion

    #region Helper

    internal static (float x, float y, float z) CaclulateXYZFromRGB(float r, float g, float b)
    {
        r = ((r > 0.04045f) ? MathF.Pow(((r + 0.055f) / 1.055f), 2.4f) : (r / 12.92f)) * 100.0f;
        g = ((g > 0.04045f) ? MathF.Pow(((g + 0.055f) / 1.055f), 2.4f) : (g / 12.92f)) * 100.0f;
        b = ((b > 0.04045f) ? MathF.Pow(((b + 0.055f) / 1.055f), 2.4f) : (b / 12.92f)) * 100.0f;

        float x = (r * 0.4124f) + (g * 0.3576f) + (b * 0.1805f);
        float y = (r * 0.2126f) + (g * 0.7152f) + (b * 0.0722f);
        float z = (r * 0.0193f) + (g * 0.1192f) + (b * 0.9505f);

        return (x, y, z);
    }

    internal static (float r, float g, float b) CalculateRGBFromXYZ(float x, float y, float z)
    {
        const float INVERSE_EXPONENT = 1.0f / 2.4f;

        x /= 100.0f;
        y /= 100.0f;
        z /= 100.0f;

        float r = (x * 3.2406f) + (y * -1.5372f) + (z * -0.4986f);
        float g = (x * -0.9689f) + (y * 1.8758f) + (z * 0.0415f);
        float b = (x * 0.0557f) + (y * -0.2040f) + (z * 1.0570f);

        r = ((r > 0.0031308f) ? ((1.055f * (MathF.Pow(r, INVERSE_EXPONENT))) - 0.055f) : (12.92f * r));
        g = ((g > 0.0031308f) ? ((1.055f * (MathF.Pow(g, INVERSE_EXPONENT))) - 0.055f) : (12.92f * g));
        b = ((b > 0.0031308f) ? ((1.055f * (MathF.Pow(b, INVERSE_EXPONENT))) - 0.055f) : (12.92f * b));

        return (r, g, b);
    }

    #endregion
}