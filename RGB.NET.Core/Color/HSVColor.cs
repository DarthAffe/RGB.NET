// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
using System;

namespace RGB.NET.Core
{
    public static class HSVColor
    {
        #region Getter

        /// <summary>
        /// Gets the hue component value (HSV-color space) of this <see cref="Color"/> as degree in the range [0..360].
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static double GetHue(this Color color) => color.GetHSV().hue;

        /// <summary>
        /// Gets the saturation component value (HSV-color space) of this <see cref="Color"/> in the range [0..1].
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static double GetSaturation(this Color color) => color.GetHSV().saturation;

        /// <summary>
        /// Gets the value component value (HSV-color space) of this <see cref="Color"/> in the range [0..1].
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static double GetValue(this Color color) => color.GetHSV().value;

        /// <summary>
        /// Gets the hue, saturation and value component values (HSV-color space) of this <see cref="Color"/>.
        /// Hue as degree in the range [0..1].
        /// Saturation in the range [0..1].
        /// Value in the range [0..1].
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static (double hue, double saturation, double value) GetHSV(this Color color)
            => CaclulateHSVFromRGB(color.R, color.G, color.B);

        #endregion

        #region Manipulation

        /// <summary>
        /// Adds the given HSV values to this color.
        /// </summary>
        /// <param name="hue">The hue value to add.</param>
        /// <param name="saturation">The saturation value to add.</param>
        /// <param name="value">The value value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public static Color AddHSV(this Color color, double hue = 0, double saturation = 0, double value = 0)
        {
            (double cHue, double cSaturation, double cValue) = color.GetHSV();
            return Create(color.A, cHue + hue, cSaturation + saturation, cValue + value);
        }

        /// <summary>
        /// Subtracts the given HSV values to this color.
        /// </summary>
        /// <param name="hue">The hue value to subtract.</param>
        /// <param name="saturation">The saturation value to subtract.</param>
        /// <param name="value">The value value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public static Color SubtractHSV(this Color color, double hue = 0, double saturation = 0, double value = 0)
        {
            (double cHue, double cSaturation, double cValue) = color.GetHSV();
            return Create(color.A, cHue - hue, cSaturation - saturation, cValue - value);
        }

        /// <summary>
        /// Multiplies the given HSV values to this color.
        /// </summary>
        /// <param name="hue">The hue value to multiply.</param>
        /// <param name="saturation">The saturation value to multiply.</param>
        /// <param name="value">The value value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public static Color MultiplyHSV(this Color color, double hue = 1, double saturation = 1, double value = 1)
        {
            (double cHue, double cSaturation, double cValue) = color.GetHSV();
            return Create(color.A, cHue * hue, cSaturation * saturation, cValue * value);
        }
        
        /// <summary>
        /// Divides the given HSV values to this color.
        /// </summary>
        /// <param name="hue">The hue value to divide.</param>
        /// <param name="saturation">The saturation value to divide.</param>
        /// <param name="value">The value value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public static Color DivideHSV(this Color color, double hue = 1, double saturation = 1, double value = 1)
        {
            (double cHue, double cSaturation, double cValue) = color.GetHSV();
            return Create(color.A, cHue / hue, cSaturation / saturation, cValue / value);
        }

        /// <summary>
        /// Sets the given hue value of this color.
        /// </summary>
        /// <param name="hue">The hue value to set.</param>
        /// <param name="saturation">The saturation value to set.</param>
        /// <param name="value">The value value to set.</param>
        /// <returns>The new color after the modification.</returns>
        public static Color SetHSV(this Color color, double? hue = null, double? saturation = null, double? value = null)
        {
            (double cHue, double cSaturation, double cValue) = color.GetHSV();
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
        public static Color Create(double hue, double saturation, double value)
            => Create(1.0, hue, saturation, value);

        /// <summary>
        /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using AHSV-Values. 
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="hue">The hue component value of this <see cref="Color"/>.</param>
        /// <param name="saturation">The saturation component value of this <see cref="Color"/>.</param>
        /// <param name="value">The value component value of this <see cref="Color"/>.</param>
        /// <returns>The color created from the values.</returns>
        public static Color Create(byte a, double hue, double saturation, double value)
            => Create((double)a / byte.MaxValue, hue, saturation, value);

        /// <summary>
        /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using AHSV-Values. 
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="hue">The hue component value of this <see cref="Color"/>.</param>
        /// <param name="saturation">The saturation component value of this <see cref="Color"/>.</param>
        /// <param name="value">The value component value of this <see cref="Color"/>.</param>
        /// <returns>The color created from the values.</returns>
        public static Color Create(int a, double hue, double saturation, double value)
            => Create((double)a / byte.MaxValue, hue, saturation, value);

        /// <summary>
        /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using AHSV-Values. 
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="hue">The hue component value of this <see cref="Color"/>.</param>
        /// <param name="saturation">The saturation component value of this <see cref="Color"/>.</param>
        /// <param name="value">The value component value of this <see cref="Color"/>.</param>
        /// <returns>The color created from the values.</returns>
        public static Color Create(double a, double hue, double saturation, double value)
        {
            (double r, double g, double b) = CalculateRGBFromHSV(hue, saturation, value);
            return new Color(a, r, g, b);
        }

        #endregion

        #region Helper

        private static (double h, double s, double v) CaclulateHSVFromRGB(double r, double g, double b)
        {
            if (r.EqualsInTolerance(g) && g.EqualsInTolerance(b)) return (0, 0, r);

            double min = Math.Min(Math.Min(r, g), b);
            double max = Math.Max(Math.Max(r, g), b);

            double hue;
            if (max.EqualsInTolerance(min))
                hue = 0;
            else if (max.EqualsInTolerance(r)) // r is max
                hue = (g - b) / (max - min);
            else if (max.EqualsInTolerance(g)) // g is max
                hue = 2.0 + ((b - r) / (max - min));
            else // b is max
                hue = 4.0 + ((r - g) / (max - min));

            hue = hue * 60.0;
            hue = hue.Wrap(0, 360);

            double saturation = max.EqualsInTolerance(0) ? 0 : 1.0 - (min / max);
            double value = Math.Max(r, Math.Max(g, b));

            return (hue, saturation, value);
        }

        private static (double r, double g, double b) CalculateRGBFromHSV(double h, double s, double v)
        {
            h = h.Wrap(0, 360);
            s = s.Clamp(0, 1);
            v = v.Clamp(0, 1);

            if (s <= 0.0)
                return (v, v, v);

            double hh = h / 60.0;
            int i = (int)hh;
            double ff = hh - i;
            double p = v * (1.0 - s);
            double q = v * (1.0 - (s * ff));
            double t = v * (1.0 - (s * (1.0 - ff)));

            switch (i)
            {
                case 0:
                    return (v, t, p);
                case 1:
                    return (q, v, p);
                case 2:
                    return (p, v, t);
                case 3:
                    return (p, q, v);
                case 4:
                    return (t, p, v);
                default:
                    return (v, p, q);
            }
        }

        #endregion
    }
}
