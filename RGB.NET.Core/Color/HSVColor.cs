// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
using System;

namespace RGB.NET.Core;

/// <summary>
/// Contains helper-methods and extension for the <see cref="Color"/>-type to work in the HSV color space.
/// </summary>
public static class HSVColor
{
    #region Getter

    /// <summary>
    /// Gets the hue component value (HSV-color space) of this <see cref="Color"/> as degree in the range [0..360].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The hue component value of the color.</returns>
    public static float GetHue(this in Color color) => color.GetHSV().hue;

    /// <summary>
    /// Gets the saturation component value (HSV-color space) of this <see cref="Color"/> in the range [0..1].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The saturation component value of the color.</returns>
    public static float GetSaturation(this in Color color) => color.GetHSV().saturation;

    /// <summary>
    /// Gets the value component value (HSV-color space) of this <see cref="Color"/> in the range [0..1].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>The value component value of the color.</returns>
    public static float GetValue(this in Color color) => color.GetHSV().value;

    /// <summary>
    /// Gets the hue, saturation and value component values (HSV-color space) of this <see cref="Color"/>.
    /// Hue as degree in the range [0..1].
    /// Saturation in the range [0..1].
    /// Value in the range [0..1].
    /// </summary>
    /// <param name="color">The color to get the value from.</param>
    /// <returns>A tuple containing the hue, saturation and value component value of the color.</returns>
    public static (float hue, float saturation, float value) GetHSV(this in Color color)
        => CaclulateHSVFromRGB(color.R, color.G, color.B);

    #endregion

    #region Manipulation

    /// <summary>
    /// Adds the specified HSV values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="hue">The hue value to add.</param>
    /// <param name="saturation">The saturation value to add.</param>
    /// <param name="value">The value value to add.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color AddHSV(this in Color color, float hue = 0, float saturation = 0, float value = 0)
    {
        (float cHue, float cSaturation, float cValue) = color.GetHSV();
        return Create(color.A, cHue + hue, cSaturation + saturation, cValue + value);
    }

    /// <summary>
    /// Subtracts the specified HSV values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="hue">The hue value to subtract.</param>
    /// <param name="saturation">The saturation value to subtract.</param>
    /// <param name="value">The value value to subtract.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SubtractHSV(this in Color color, float hue = 0, float saturation = 0, float value = 0)
    {
        (float cHue, float cSaturation, float cValue) = color.GetHSV();
        return Create(color.A, cHue - hue, cSaturation - saturation, cValue - value);
    }

    /// <summary>
    /// Multiplies the specified HSV values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="hue">The hue value to multiply.</param>
    /// <param name="saturation">The saturation value to multiply.</param>
    /// <param name="value">The value value to multiply.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color MultiplyHSV(this in Color color, float hue = 1, float saturation = 1, float value = 1)
    {
        (float cHue, float cSaturation, float cValue) = color.GetHSV();
        return Create(color.A, cHue * hue, cSaturation * saturation, cValue * value);
    }

    /// <summary>
    /// Divides the specified HSV values to this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="hue">The hue value to divide.</param>
    /// <param name="saturation">The saturation value to divide.</param>
    /// <param name="value">The value value to divide.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color DivideHSV(this in Color color, float hue = 1, float saturation = 1, float value = 1)
    {
        (float cHue, float cSaturation, float cValue) = color.GetHSV();
        return Create(color.A, cHue / hue, cSaturation / saturation, cValue / value);
    }

    /// <summary>
    /// Sets the specified hue value of this color.
    /// </summary>
    /// <param name="color">The color to modify.</param>
    /// <param name="hue">The hue value to set.</param>
    /// <param name="saturation">The saturation value to set.</param>
    /// <param name="value">The value value to set.</param>
    /// <returns>The new color after the modification.</returns>
    public static Color SetHSV(this in Color color, float? hue = null, float? saturation = null, float? value = null)
    {
        (float cHue, float cSaturation, float cValue) = color.GetHSV();
        return Create(color.A, hue ?? cHue, saturation ?? cSaturation, value ?? cValue);
    }

    #endregion

    #region Factory

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using HSV-Values. 
    /// </summary>
    /// <param name="hue">The hue component value of this <see cref="Color"/>.</param>
    /// <param name="saturation">The saturation component value of this <see cref="Color"/>.</param>
    /// <param name="value">The value component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(float hue, float saturation, float value)
        => Create(1.0f, hue, saturation, value);

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using AHSV-Values. 
    /// </summary>
    /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="hue">The hue component value of this <see cref="Color"/>.</param>
    /// <param name="saturation">The saturation component value of this <see cref="Color"/>.</param>
    /// <param name="value">The value component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(byte a, float hue, float saturation, float value)
        => Create((float)a / byte.MaxValue, hue, saturation, value);

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using AHSV-Values. 
    /// </summary>
    /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="hue">The hue component value of this <see cref="Color"/>.</param>
    /// <param name="saturation">The saturation component value of this <see cref="Color"/>.</param>
    /// <param name="value">The value component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(int a, float hue, float saturation, float value)
        => Create((float)a / byte.MaxValue, hue, saturation, value);

    /// <summary>
    /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using AHSV-Values. 
    /// </summary>
    /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="hue">The hue component value of this <see cref="Color"/>.</param>
    /// <param name="saturation">The saturation component value of this <see cref="Color"/>.</param>
    /// <param name="value">The value component value of this <see cref="Color"/>.</param>
    /// <returns>The color created from the values.</returns>
    public static Color Create(float a, float hue, float saturation, float value)
    {
        (float r, float g, float b) = CalculateRGBFromHSV(hue, saturation, value);
        return new Color(a, r, g, b);
    }

    #endregion

    #region Helper

    private static (float h, float s, float v) CaclulateHSVFromRGB(float r, float g, float b)
    {
        if (r.EqualsInTolerance(g) && g.EqualsInTolerance(b)) return (0, 0, r);

        float min = Math.Min(Math.Min(r, g), b);
        float max = Math.Max(Math.Max(r, g), b);

        float hue;
        if (max.EqualsInTolerance(min))
            hue = 0;
        else if (max.EqualsInTolerance(r)) // r is max
            hue = (g - b) / (max - min);
        else if (max.EqualsInTolerance(g)) // g is max
            hue = 2.0f + ((b - r) / (max - min));
        else // b is max
            hue = 4.0f + ((r - g) / (max - min));

        hue *= 60.0f;
        hue = hue.Wrap(0, 360);

        float saturation = max.EqualsInTolerance(0) ? 0 : 1.0f - (min / max);
        float value = Math.Max(r, Math.Max(g, b));

        return (hue, saturation, value);
    }

    private static (float r, float g, float b) CalculateRGBFromHSV(float h, float s, float v)
    {
        h = h.Wrap(0, 360);
        s = s.Clamp(0, 1);
        v = v.Clamp(0, 1);

        if (s <= 0.0)
            return (v, v, v);

        float hh = h / 60.0f;
        int i = (int)hh;
        float ff = hh - i;
        float p = v * (1.0f - s);
        float q = v * (1.0f - (s * ff));
        float t = v * (1.0f - (s * (1.0f - ff)));

        return i switch
        {
            0 => (v, t, p),
            1 => (q, v, p),
            2 => (p, v, t),
            3 => (p, q, v),
            4 => (t, p, v),
            _ => (v, p, q)
        };
    }

    #endregion
}