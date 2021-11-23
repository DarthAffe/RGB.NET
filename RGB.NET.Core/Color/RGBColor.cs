// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
using System;

namespace RGB.NET.Core;

/// <summary>
/// Contains helper-methods and extension for the <see cref="Color"/>-type to work in the RGB color space.
/// </summary>
public static class RGBColor
{
    #region Getter

    /// <summary>
    /// Gets the A component value of this <see cref="Color"/> as byte in the range [0..255].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The A component value of the color.</returns>
    public static byte GetA(this in Color color) => color.A.GetByteValueFromPercentage();

    /// <summary>
    /// Gets the R component value of this <see cref="Color"/> as byte in the range [0..255].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The R component value of the color.</returns>
    public static byte GetR(this in Color color) => color.R.GetByteValueFromPercentage();

    /// <summary>
    /// Gets the G component value of this <see cref="Color"/> as byte in the range [0..255].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The G component value of the color.</returns>
    public static byte GetG(this in Color color) => color.G.GetByteValueFromPercentage();

    /// <summary>
    /// Gets the B component value of this <see cref="Color"/> as byte in the range [0..255].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The B component value of the color.</returns>
    public static byte GetB(this in Color color) => color.B.GetByteValueFromPercentage();

    /// <summary>
    /// Gets the A, R, G and B component value of this <see cref="Color"/> as byte in the range [0..255].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>A tuple containing the A, R, G and B component value of the color.</returns>
    public static (byte a, byte r, byte g, byte b) GetRGBBytes(this in Color color)
        => (color.GetA(), color.GetR(), color.GetG(), color.GetB());

    /// <summary>
    /// Gets the A, R, G and B component value of this <see cref="Color"/> as percentage in the range [0..1].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>A tuple containing the A, R, G and B component value of the color.</returns>
    public static (float a, float r, float g, float b) GetRGB(this in Color color)
        => (color.A, color.R, color.G, color.B);

    #endregion

    #region Manipulation

    #region Add

    /// <summary>
    /// Adds the specified RGB values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="r">The red value to add.</param>
    /// <param name="g">The green value to add.</param>
    /// <param name="b">The blue value to add.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color AddRGB(this in Color color, int r = 0, int g = 0, int b = 0)
        => new(color.A, color.GetR() + r, color.GetG() + g, color.GetB() + b);

    /// <summary>
    /// Adds the specified RGB-percent values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="r">The red value to add.</param>
    /// <param name="g">The green value to add.</param>
    /// <param name="b">The blue value to add.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color AddRGB(this in Color color, float r = 0, float g = 0, float b = 0)
        => new(color.A, color.R + r, color.G + g, color.B + b);

    /// <summary>
    /// Adds the specified alpha value to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="a">The alpha value to add.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color AddA(this in Color color, int a)
        => new(color.GetA() + a, color.R, color.G, color.B);

    /// <summary>
    /// Adds the specified alpha-percent value to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="a">The alpha value to add.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color AddA(this in Color color, float a)
        => new(color.A + a, color.R, color.G, color.B);

    #endregion

    #region Subtract

    /// <summary>
    /// Subtracts the specified RGB values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="r">The red value to subtract.</param>
    /// <param name="g">The green value to subtract.</param>
    /// <param name="b">The blue value to subtract.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SubtractRGB(this in Color color, int r = 0, int g = 0, int b = 0)
        => new(color.A, color.GetR() - r, color.GetG() - g, color.GetB() - b);

    /// <summary>
    /// Subtracts the specified RGB values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="r">The red value to subtract.</param>
    /// <param name="g">The green value to subtract.</param>
    /// <param name="b">The blue value to subtract.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SubtractRGB(this in Color color, float r = 0, float g = 0, float b = 0)
        => new(color.A, color.R - r, color.G - g, color.B - b);

    /// <summary>
    /// Subtracts the specified alpha value to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="a">The alpha value to subtract.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SubtractA(this in Color color, int a)
        => new(color.GetA() - a, color.R, color.G, color.B);

    /// <summary>
    /// Subtracts the specified alpha-percent value to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="aPercent">The alpha value to subtract.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SubtractA(this in Color color, float aPercent)
        => new(color.A - aPercent, color.R, color.G, color.B);

    #endregion

    #region Multiply

    /// <summary>
    /// Multiplies the specified RGB values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="r">The red value to multiply.</param>
    /// <param name="g">The green value to multiply.</param>
    /// <param name="b">The blue value to multiply.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color MultiplyRGB(this in Color color, float r = 1, float g = 1, float b = 1)
        => new(color.A, color.R * r, color.G * g, color.B * b);

    /// <summary>
    /// Multiplies the specified alpha value to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="a">The alpha value to multiply.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color MultiplyA(this in Color color, float a)
        => new(color.A * a, color.R, color.G, color.B);

    #endregion

    #region Divide

    /// <summary>
    /// Divides the specified RGB values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="r">The red value to divide.</param>
    /// <param name="g">The green value to divide.</param>
    /// <param name="b">The blue value to divide.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color DivideRGB(this in Color color, float r = 1, float g = 1, float b = 1)
        => new(color.A, color.R / r, color.G / g, color.B / b);

    /// <summary>
    /// Divides the specified alpha value to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="a">The alpha value to divide.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color DivideA(this in Color color, float a)
        => new(color.A / a, color.R, color.G, color.B);

    #endregion

    #region Set

    /// <summary>
    /// Sets the specified RGB value of this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="r">The red value to set.</param>
    /// <param name="g">The green value to set.</param>
    /// <param name="b">The blue value to set.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SetRGB(this in Color color, byte? r = null, byte? g = null, byte? b = null)
        => new(color.A, r ?? color.GetR(), g ?? color.GetG(), b ?? color.GetB());

    /// <summary>
    /// Sets the specified RGB value of this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="r">The red value to set.</param>
    /// <param name="g">The green value to set.</param>
    /// <param name="b">The blue value to set.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SetRGB(this in Color color, int? r = null, int? g = null, int? b = null)
        => new(color.A, r ?? color.GetR(), g ?? color.GetG(), b ?? color.GetB());

    /// <summary>
    /// Sets the specified RGB value of this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="r">The red value to set.</param>
    /// <param name="g">The green value to set.</param>
    /// <param name="b">The blue value to set.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SetRGB(this in Color color, float? r = null, float? g = null, float? b = null)
        => new(color.A, r ?? color.R, g ?? color.G, b ?? color.B);

    /// <summary>
    /// Sets the specified alpha value of this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="a">The alpha value to set.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SetA(this in Color color, int a) => new(a, color.R, color.G, color.B);

    /// <summary>
    /// Sets the specified alpha value of this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="a">The alpha value to set.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SetA(this in Color color, float a) => new(a, color.R, color.G, color.B);

    #endregion

    #endregion

    #region Conversion

    /// <summary>
    /// Gets the current color as a RGB-HEX-string.
    /// </summary>
    /// <returns>The RGB-HEX-string.</returns>
    public static string AsRGBHexString(this in Color color, bool leadingHash = true) => (leadingHash ? "#" : "") + ConversionHelper.ToHex(color.GetR(), color.GetG(), color.GetB());

    /// <summary>
    /// Gets the current color as a ARGB-HEX-string.
    /// </summary>
    /// <returns>The ARGB-HEX-string.</returns>
    public static string AsARGBHexString(this in Color color, bool leadingHash = true) => (leadingHash ? "#" : "") + ConversionHelper.ToHex(color.GetA(), color.GetR(), color.GetG(), color.GetB());

    #endregion

    #region Factory

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using a HEX-string. 
    /// </summary>
    /// <param name="hexString">The HEX-representation of the color.</param>
    /// <returns>The color created from the HEX-string.</returns>
    public static Color FromHexString(string hexString)
    {
        if ((hexString == null) || (hexString.Length < 6))
            throw new ArgumentException("Invalid hex string", nameof(hexString));

        ReadOnlySpan<char> span = hexString.AsSpan();
        if (span[0] == '#')
            span = span[1..];

        byte[] data = ConversionHelper.HexToBytes(span);
        return data.Length switch
        {
            3 => new Color(data[0], data[1], data[2]),
            4 => new Color(data[0], data[1], data[2], data[3]),
            _ => throw new ArgumentException($"Invalid hex string '{hexString}'", nameof(hexString))
        };
    }

    #endregion
}