// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

using System;
using System.Diagnostics;

namespace RGB.NET.Core
{
    /// <inheritdoc />
    /// <summary>
    /// Represents an ARGB (alpha, red, green, blue) color.
    /// </summary>
    [DebuggerDisplay("[A: {A}, R: {R}, G: {G}, B: {B}, H: {Hue}, S: {Saturation}, V: {Value}]")]
    public struct Color
    {
        #region Constants

        /// <summary>
        /// Gets an transparent color [A: 0, R: 0, G: 0, B: 0]
        /// </summary>
        public static Color Transparent => new Color(0, 0, 0, 0);

        #endregion

        #region Properties & Fields

        #region RGB

        /// <summary>
        /// Gets the alpha component value of this <see cref="Color"/> as byte in the range [0..255].
        /// </summary>
        public byte A { get; }

        /// <summary>
        /// Gets the alpha component value of this <see cref="Color"/> as percentage in the range [0..1].
        /// </summary>
        public double APercent => A / (double)byte.MaxValue;

        /// <summary>
        /// Gets the red component value of this <see cref="Color"/> as byte in the range [0..255].
        /// </summary>
        public byte R { get; }

        /// <summary>
        /// Gets the red component value of this <see cref="Color"/> as percentage in the range [0..1].
        /// </summary>
        public double RPercent => R / (double)byte.MaxValue;

        /// <summary>
        /// Gets the green component value of this <see cref="Color"/> as byte in the range [0..255].
        /// </summary>
        public byte G { get; }

        /// <summary>
        /// Gets the green component value of this <see cref="Color"/> as percentage in the range [0..1].
        /// </summary>
        public double GPercent => G / (double)byte.MaxValue;

        /// <summary>
        /// Gets the blue component value of this <see cref="Color"/> as byte in the range [0..255].
        /// </summary>
        public byte B { get; }

        /// <summary>
        /// Gets the blue component value of this <see cref="Color"/> as percentage in the range [0..1].
        /// </summary>
        public double BPercent => B / (double)byte.MaxValue;

        #endregion

        #region HSV

        /// <summary>
        /// Gets the hue component value (HSV-color space) of this <see cref="Color"/> as degree in the range [0..360].
        /// </summary>
        public double Hue { get; }

        /// <summary>
        /// Gets the saturation component value (HSV-color space) of this <see cref="Color"/> as degree in the range [0..1].
        /// </summary>
        public double Saturation { get; }

        /// <summary>
        /// Gets the value component value (HSV-color space) of this <see cref="Color"/> as degree in the range [0..1].
        /// </summary>
        public double Value { get; }

        #endregion

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using RGB-Values. 
        /// Alpha defaults to 255.
        /// </summary>
        /// <param name="r">The red component value of this <see cref="T:RGB.NET.Core.Color" />.</param>
        /// <param name="g">The green component value of this <see cref="T:RGB.NET.Core.Color" />.</param>
        /// <param name="b">The blue component value of this <see cref="T:RGB.NET.Core.Color" />.</param>
        public Color(byte r, byte g, byte b)
            : this(byte.MaxValue, r, g, b)
        { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using RGB-Values. 
        /// Alpha defaults to 255.
        /// </summary>
        /// <param name="r">The red component value of this <see cref="T:RGB.NET.Core.Color" />.</param>
        /// <param name="g">The green component value of this <see cref="T:RGB.NET.Core.Color" />.</param>
        /// <param name="b">The blue component value of this <see cref="T:RGB.NET.Core.Color" />.</param>
        public Color(int r, int g, int b)
            : this((byte)r.Clamp(0, byte.MaxValue), (byte)g.Clamp(0, byte.MaxValue), (byte)b.Clamp(0, byte.MaxValue))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct using ARGB values.
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="r">The red component value of this <see cref="Color"/>.</param>
        /// <param name="g">The green component value of this <see cref="Color"/>.</param>
        /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
        public Color(byte a, byte r, byte g, byte b)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;

            (double h, double s, double v) = CaclulateHSVFromRGB(r, g, b);
            Hue = h;
            Saturation = s;
            Value = v;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct using ARGB values.
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="r">The red component value of this <see cref="Color"/>.</param>
        /// <param name="g">The green component value of this <see cref="Color"/>.</param>
        /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
        public Color(int a, int r, int g, int b)
            : this((byte)a.Clamp(0, byte.MaxValue), (byte)r.Clamp(0, byte.MaxValue), (byte)g.Clamp(0, byte.MaxValue), (byte)b.Clamp(0, byte.MaxValue))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct using RGB-percent values.
        /// </summary>
        /// <param name="r">The red component value of this <see cref="Color"/>.</param>
        /// <param name="g">The green component value of this <see cref="Color"/>.</param>
        /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
        public Color(double r, double g, double b)
            : this(byte.MaxValue,
                   r.GetByteValueFromPercentage(),
                   g.GetByteValueFromPercentage(),
                   b.GetByteValueFromPercentage())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct using ARGB-percent values.
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="r">The red component value of this <see cref="Color"/>.</param>
        /// <param name="g">The green component value of this <see cref="Color"/>.</param>
        /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
        public Color(double a, double r, double g, double b)
            : this(a.GetByteValueFromPercentage(),
                   r.GetByteValueFromPercentage(),
                   g.GetByteValueFromPercentage(),
                   b.GetByteValueFromPercentage())
        { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct by cloning a existing <see cref="T:RGB.NET.Core.Color" />.
        /// </summary>
        /// <param name="color">The <see cref="T:RGB.NET.Core.Color" /> the values are copied from.</param>
        public Color(Color color)
            : this(color.A, color.R, color.G, color.B) { }

        #endregion

        #region Methods

        #region Factory

        /// <summary>
        /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using HSV-Values. 
        /// </summary>
        /// <param name="hue">The hue component value of this <see cref="Color"/>.</param>
        /// <param name="saturation">The saturation component value of this <see cref="Color"/>.</param>
        /// <param name="value">The value component value of this <see cref="Color"/>.</param>
        /// <returns>The color created from the values.</returns>
        public static Color FromHSV(double hue, double saturation, double value)
        {
            (byte r, byte g, byte b) = CalculateRGBFromHSV(hue, saturation, value);
            return new Color(r, g, b);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct using AHSV-Values. 
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="hue">The hue component value of this <see cref="Color"/>.</param>
        /// <param name="saturation">The saturation component value of this <see cref="Color"/>.</param>
        /// <param name="value">The value component value of this <see cref="Color"/>.</param>
        /// <returns>The color created from the values.</returns>
        public static Color FromHSV(int a, double hue, double saturation, double value)
        {
            (byte r, byte g, byte b) = CalculateRGBFromHSV(hue, saturation, value);
            return new Color(a, r, g, b);
        }

        #endregion

        private static (double h, double s, double v) CaclulateHSVFromRGB(byte r, byte g, byte b)
        {
            if ((r == g) && (g == b)) return (0, 0, r / 255.0);

            int min = Math.Min(Math.Min(r, g), b);
            int max = Math.Max(Math.Max(r, g), b);

            double hue;
            if (max == min)
                hue = 0;
            else if (max == r) // r is max
                hue = (g - b) / (double)(max - min);
            else if (max == g) // g is max
                hue = 2.0 + ((b - r) / (double)(max - min));
            else // b is max
                hue = 4.0 + ((r - g) / (double)(max - min));

            hue = hue * 60.0;
            if (hue < 0.0)
                hue += 360.0;

            double saturation = (max == 0) ? 0 : 1.0 - (min / (double)max);
            double value = Math.Max(r, Math.Max(g, b)) / 255.0;

            return (hue, saturation, value);
        }

        private static (byte r, byte g, byte b) CalculateRGBFromHSV(double h, double s, double v)
        {
            h = h.Wrap(0, 360);
            s = s.Clamp(0, 1);
            v = v.Clamp(0, 1);

            if (s <= 0.0)
            {
                byte val = v.GetByteValueFromPercentage();
                return (val, val, val);
            }

            double hh = h / 60.0;
            int i = (int)hh;
            double ff = hh - i;
            double p = v * (1.0 - s);
            double q = v * (1.0 - (s * ff));
            double t = v * (1.0 - (s * (1.0 - ff)));

            switch (i)
            {
                case 0:
                    return (v.GetByteValueFromPercentage(),
                            t.GetByteValueFromPercentage(),
                            p.GetByteValueFromPercentage());
                case 1:
                    return (q.GetByteValueFromPercentage(),
                            v.GetByteValueFromPercentage(),
                            p.GetByteValueFromPercentage());
                case 2:
                    return (p.GetByteValueFromPercentage(),
                            v.GetByteValueFromPercentage(),
                            t.GetByteValueFromPercentage());
                case 3:
                    return (p.GetByteValueFromPercentage(),
                            q.GetByteValueFromPercentage(),
                            v.GetByteValueFromPercentage());
                case 4:
                    return (t.GetByteValueFromPercentage(),
                            p.GetByteValueFromPercentage(),
                            v.GetByteValueFromPercentage());
                default:
                    return (v.GetByteValueFromPercentage(),
                            p.GetByteValueFromPercentage(),
                            q.GetByteValueFromPercentage());
            }
        }

        /// <summary>
        /// Converts the individual byte values of this <see cref="Color"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the individual byte values of this <see cref="Color"/>. For example "[A: 255, R: 255, G: 0, B: 0]".</returns>
        public override string ToString() => $"[A: {A}, R: {R}, G: {G}, B: {B}, H: {Hue}, S: {Saturation}, V: {Value}]";

        /// <summary>
        /// Tests whether the specified object is a <see cref="Color" /> and is equivalent to this <see cref="Color" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Color" /> equivalent to this <see cref="Color" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Color)) return false;

            Color compareColor = (Color)obj;
            return (compareColor.A == A) && (compareColor.R == R) && (compareColor.G == G) && (compareColor.B == B);
        }

        /// <summary>
        /// Returns a hash code for this <see cref="Color" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="Color" />.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = A.GetHashCode();
                hashCode = (hashCode * 397) ^ R.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Gets the current color as a RGB-HEX-string.
        /// </summary>
        /// <returns>The RGB-HEX-string.</returns>
        public string AsRGBHexString() => ConversionHelper.ToHex(R, G, B);

        /// <summary>
        /// Gets the current color as a ARGB-HEX-string.
        /// </summary>
        /// <returns>The ARGB-HEX-string.</returns>
        public string AsARGBHexString() => ConversionHelper.ToHex(A, R, G, B);

        #region Deconstruction

        /// <summary>
        /// Deconstructs the Color into it's components.
        /// </summary>
        /// <param name="a">The alpha component of this color.</param>
        /// <param name="r">The red component of this color.</param>
        /// <param name="g">The green component of this color.</param>
        /// <param name="b">The blue component of this color.</param>
        /// <param name="hue">The hue component of this color.</param>
        /// <param name="saturation">The saturation component of this color.</param>
        /// <param name="value">The value component of this color.</param>
        public void Deconstruct(out byte a, out byte r, out byte g, out byte b, out double hue, out double saturation, out double value)
        {
            Deconstruct(out a, out r, out g, out b);
            Deconstruct(out hue, out saturation, out value);
        }

        /// <summary>
        /// Deconstructs the Color into it's ARGB-components.
        /// </summary>
        /// <param name="a">The alpha component of this color.</param>
        /// <param name="r">The red component of this color.</param>
        /// <param name="g">The green component of this color.</param>
        /// <param name="b">The blue component of this color.</param>
        public void Deconstruct(out byte a, out byte r, out byte g, out byte b)
        {
            a = A;
            r = R;
            g = G;
            b = B;
        }

        /// <summary>
        /// Deconstructs the Color into it's HSV-components.
        /// </summary>
        /// <param name="hue">The hue component of this color.</param>
        /// <param name="saturation">The saturation component of this color.</param>
        /// <param name="value">The value component of this color.</param>
        public void Deconstruct(out double hue, out double saturation, out double value)
        {
            hue = Hue;
            saturation = Saturation;
            value = Value;
        }

        #endregion

        #region Manipulation

        /// <summary>
        /// Blends a <see cref="Color"/> over this color.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to blend.</param>
        public Color Blend(Color color)
        {
            if (color.A == 0) return this;

            if (color.A == 255)
                return color;

            double resultA = (1.0 - ((1.0 - color.APercent) * (1.0 - APercent)));
            double resultR = (((color.RPercent * color.APercent) / resultA) + ((RPercent * APercent * (1.0 - color.APercent)) / resultA));
            double resultG = (((color.GPercent * color.APercent) / resultA) + ((GPercent * APercent * (1.0 - color.APercent)) / resultA));
            double resultB = (((color.BPercent * color.APercent) / resultA) + ((BPercent * APercent * (1.0 - color.APercent)) / resultA));

            return new Color(resultA, resultR, resultG, resultB);
        }

        #region Add

        /// <summary>
        /// Adds the given RGB values to this color.
        /// </summary>
        /// <param name="r">The red value to add.</param>
        /// <param name="g">The green value to add.</param>
        /// <param name="b">The blue value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddRGB(int r, int g, int b) => AddRGB(0, r, g, b);

        /// <summary>
        /// Adds the given RGB values to this color.
        /// </summary>
        /// <param name="a">The alpha value to add.</param>
        /// <param name="r">The red value to add.</param>
        /// <param name="g">The green value to add.</param>
        /// <param name="b">The blue value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddRGB(int a, int r, int g, int b)
            => new Color(A + a, R + r, G + g, B + b);

        /// <summary>
        /// Adds the given RGB-percent values to this color.
        /// </summary>
        /// <param name="rPercent">The red value to add.</param>
        /// <param name="gPercent">The green value to add.</param>
        /// <param name="bPercent">The blue value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddPercent(double rPercent, double gPercent, double bPercent) => AddPercent(0, rPercent, gPercent, bPercent);

        /// <summary>
        /// Adds the given RGB-percent values to this color.
        /// </summary>
        /// <param name="aPercent">The alpha value to add.</param>
        /// <param name="rPercent">The red value to add.</param>
        /// <param name="gPercent">The green value to add.</param>
        /// <param name="bPercent">The blue value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddPercent(double aPercent, double rPercent, double gPercent, double bPercent)
            => new Color(APercent + aPercent, RPercent + rPercent, GPercent + gPercent, BPercent + bPercent);

        /// <summary>
        /// Adds the given HSV values to this color.
        /// </summary>
        /// <param name="hue">The hue value to add.</param>
        /// <param name="saturation">The saturation value to add.</param>
        /// <param name="value">The value value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddHSV(double hue, double saturation, double value) => AddHSV(0, hue, saturation, value);

        /// <summary>
        /// Adds the given HSV values to this color.
        /// </summary>
        /// <param name="a">The alpha value to add.</param>
        /// <param name="hue">The hue value to add.</param>
        /// <param name="saturation">The saturation value to add.</param>
        /// <param name="value">The value value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddHSV(int a, double hue, double saturation, double value)
            => FromHSV(A + a, Hue + hue, Saturation + saturation, Value + value);

        /// <summary>
        /// Adds the given alpha value to this color.
        /// </summary>
        /// <param name="a">The alpha value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddA(int a) => new Color(A + a, R, G, B);

        /// <summary>
        /// Adds the given alpha-percent value to this color.
        /// </summary>
        /// <param name="aPercent">The alpha value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddAPercent(double aPercent) => new Color(APercent + aPercent, RPercent, GPercent, BPercent);

        /// <summary>
        /// Adds the given red value to this color.
        /// </summary>
        /// <param name="r">The red value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddR(int r) => new Color(A, R + r, G, B);

        /// <summary>
        /// Adds the given red-percent value to this color.
        /// </summary>
        /// <param name="rPercent">The red value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddRPercent(double rPercent) => new Color(APercent, RPercent + rPercent, GPercent, BPercent);

        /// <summary>
        /// Adds the given green value to this color.
        /// </summary>
        /// <param name="g">The green value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddG(int g) => new Color(A, R, G + g, B);

        /// <summary>
        /// Adds the given green-percent value to this color.
        /// </summary>
        /// <param name="gPercent">The green value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddGPercent(double gPercent) => new Color(APercent, RPercent, GPercent + gPercent, BPercent);

        /// <summary>
        /// Adds the given blue value to this color.
        /// </summary>
        /// <param name="b">The blue value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddB(int b) => new Color(A, R, G, B + b);

        /// <summary>
        /// Adds the given blue-percent value to this color.
        /// </summary>
        /// <param name="bPercent">The blue value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddBPercent(double bPercent) => new Color(APercent, RPercent, GPercent, BPercent + bPercent);

        /// <summary>
        /// Adds the given hue value to this color.
        /// </summary>
        /// <param name="hue">The hue value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddHue(double hue) => FromHSV(A, Hue + hue, Saturation, Value);

        /// <summary>
        /// Adds the given saturation value to this color.
        /// </summary>
        /// <param name="saturation">The saturation value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddSaturation(double saturation) => FromHSV(A, Hue, Saturation + saturation, Value);

        /// <summary>
        /// Adds the given value value to this color.
        /// </summary>
        /// <param name="value">The value value to add.</param>
        /// <returns>The new color after the modification.</returns>
        public Color AddValue(double value) => FromHSV(A, Hue, Saturation, Value + value);

        #endregion

        #region Subtract

        /// <summary>
        /// Subtracts the given RGB values to this color.
        /// </summary>
        /// <param name="r">The red value to subtract.</param>
        /// <param name="g">The green value to subtract.</param>
        /// <param name="b">The blue value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractRGB(int r, int g, int b) => SubtractRGB(0, r, g, b);

        /// <summary>
        /// Subtracts the given RGB values to this color.
        /// </summary>
        /// <param name="a">The alpha value to subtract.</param>
        /// <param name="r">The red value to subtract.</param>
        /// <param name="g">The green value to subtract.</param>
        /// <param name="b">The blue value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractRGB(int a, int r, int g, int b)
            => new Color(A - a, R - r, G - g, B - b);

        /// <summary>
        /// Subtracts the given RGB-percent values to this color.
        /// </summary>
        /// <param name="rPercent">The red value to subtract.</param>
        /// <param name="gPercent">The green value to subtract.</param>
        /// <param name="bPercent">The blue value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractPercent(double rPercent, double gPercent, double bPercent) => SubtractPercent(0, rPercent, gPercent, bPercent);

        /// <summary>
        /// Subtracts the given RGB-percent values to this color.
        /// </summary>
        /// <param name="aPercent">The alpha value to subtract.</param>
        /// <param name="rPercent">The red value to subtract.</param>
        /// <param name="gPercent">The green value to subtract.</param>
        /// <param name="bPercent">The blue value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractPercent(double aPercent, double rPercent, double gPercent, double bPercent)
            => new Color(APercent - aPercent, RPercent - rPercent, GPercent - gPercent, BPercent - bPercent);

        /// <summary>
        /// Subtracts the given HSV values to this color.
        /// </summary>
        /// <param name="hue">The hue value to subtract.</param>
        /// <param name="saturation">The saturation value to subtract.</param>
        /// <param name="value">The value value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractHSV(double hue, double saturation, double value) => SubtractHSV(0, hue, saturation, value);

        /// <summary>
        /// Subtracts the given HSV values to this color.
        /// </summary>
        /// <param name="a">The alpha value to subtract.</param>
        /// <param name="hue">The hue value to subtract.</param>
        /// <param name="saturation">The saturation value to subtract.</param>
        /// <param name="value">The value value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractHSV(int a, double hue, double saturation, double value)
            => FromHSV(A - a, Hue - hue, Saturation - saturation, Value - value);

        /// <summary>
        /// Subtracts the given alpha value to this color.
        /// </summary>
        /// <param name="a">The alpha value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractA(int a) => new Color(A - a, R, G, B);

        /// <summary>
        /// Subtracts the given alpha-percent value to this color.
        /// </summary>
        /// <param name="aPercent">The alpha value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractAPercent(double aPercent) => new Color(APercent - aPercent, RPercent, GPercent, BPercent);

        /// <summary>
        /// Subtracts the given red value to this color.
        /// </summary>
        /// <param name="r">The red value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractR(int r) => new Color(A, R - r, G, B);

        /// <summary>
        /// Subtracts the given red-percent value to this color.
        /// </summary>
        /// <param name="rPercent">The red value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractRPercent(double rPercent) => new Color(APercent, RPercent - rPercent, GPercent, BPercent);

        /// <summary>
        /// Subtracts the given green value to this color.
        /// </summary>
        /// <param name="g">The green value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractG(int g) => new Color(A, R, G - g, B);

        /// <summary>
        /// Subtracts the given green-percent value to this color.
        /// </summary>
        /// <param name="gPercent">The green value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractGPercent(double gPercent) => new Color(APercent, RPercent, GPercent - gPercent, BPercent);

        /// <summary>
        /// Subtracts the given blue value to this color.
        /// </summary>
        /// <param name="b">The blue value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractB(int b) => new Color(A, R, G, B - b);

        /// <summary>
        /// Subtracts the given blue-percent value to this color.
        /// </summary>
        /// <param name="bPercent">The blue value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractBPercent(double bPercent) => new Color(APercent, RPercent, GPercent, BPercent - bPercent);

        /// <summary>
        /// Subtracts the given hue value to this color.
        /// </summary>
        /// <param name="hue">The hue value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractHue(double hue) => FromHSV(A, Hue - hue, Saturation, Value);

        /// <summary>
        /// Subtracts the given saturation value to this color.
        /// </summary>
        /// <param name="saturation">The saturation value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractSaturation(double saturation) => FromHSV(A, Hue, Saturation - saturation, Value);

        /// <summary>
        /// Subtracts the given value value to this color.
        /// </summary>
        /// <param name="value">The value value to subtract.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SubtractValue(double value) => FromHSV(A, Hue, Saturation, Value - value);

        #endregion

        #region Multiply

        /// <summary>
        /// Multiplies the given RGB values to this color.
        /// </summary>
        /// <param name="r">The red value to multiply.</param>
        /// <param name="g">The green value to multiply.</param>
        /// <param name="b">The blue value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyRGB(double r, double g, double b) => MultiplyRGB(1, r, g, b);

        /// <summary>
        /// Multiplies the given RGB values to this color.
        /// </summary>
        /// <param name="a">The alpha value to multiply.</param>
        /// <param name="r">The red value to multiply.</param>
        /// <param name="g">The green value to multiply.</param>
        /// <param name="b">The blue value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyRGB(double a, double r, double g, double b)
            => new Color((int)Math.Round(A * a), (int)Math.Round(R * r), (int)Math.Round(G * g), (int)Math.Round(B * b));

        /// <summary>
        /// Multiplies the given RGB-percent values to this color.
        /// </summary>
        /// <param name="rPercent">The red value to multiply.</param>
        /// <param name="gPercent">The green value to multiply.</param>
        /// <param name="bPercent">The blue value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyPercent(double rPercent, double gPercent, double bPercent) => MultiplyPercent(1, rPercent, gPercent, bPercent);

        /// <summary>
        /// Multiplies the given RGB-percent values to this color.
        /// </summary>
        /// <param name="aPercent">The alpha value to multiply.</param>
        /// <param name="rPercent">The red value to multiply.</param>
        /// <param name="gPercent">The green value to multiply.</param>
        /// <param name="bPercent">The blue value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyPercent(double aPercent, double rPercent, double gPercent, double bPercent)
            => new Color(APercent * aPercent, RPercent * rPercent, GPercent * gPercent, BPercent * bPercent);

        /// <summary>
        /// Multiplies the given HSV values to this color.
        /// </summary>
        /// <param name="hue">The hue value to multiply.</param>
        /// <param name="saturation">The saturation value to multiply.</param>
        /// <param name="value">The value value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyHSV(double hue, double saturation, double value) => MultiplyHSV(1, hue, saturation, value);

        /// <summary>
        /// Multiplies the given HSV values to this color.
        /// </summary>
        /// <param name="a">The alpha value to multiply.</param>
        /// <param name="hue">The hue value to multiply.</param>
        /// <param name="saturation">The saturation value to multiply.</param>
        /// <param name="value">The value value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyHSV(double a, double hue, double saturation, double value)
            => FromHSV((int)Math.Round(A * a), Hue * hue, Saturation * saturation, Value * value);

        /// <summary>
        /// Multiplies the given alpha value to this color.
        /// </summary>
        /// <param name="a">The alpha value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyA(double a) => new Color((int)Math.Round(A * a), R, G, B);

        /// <summary>
        /// Multiplies the given alpha-percent value to this color.
        /// </summary>
        /// <param name="aPercent">The alpha value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyAPercent(double aPercent) => new Color(APercent * aPercent, RPercent, GPercent, BPercent);

        /// <summary>
        /// Multiplies the given red value to this color.
        /// </summary>
        /// <param name="r">The red value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyR(double r) => new Color(A, (int)Math.Round(R * r), G, B);

        /// <summary>
        /// Multiplies the given red-percent value to this color.
        /// </summary>
        /// <param name="rPercent">The red value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyRPercent(double rPercent) => new Color(APercent, RPercent * rPercent, GPercent, BPercent);

        /// <summary>
        /// Multiplies the given green value to this color.
        /// </summary>
        /// <param name="g">The green value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyG(double g) => new Color(A, R, (int)Math.Round(G * g), B);

        /// <summary>
        /// Multiplies the given green-percent value to this color.
        /// </summary>
        /// <param name="gPercent">The green value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyGPercent(double gPercent) => new Color(APercent, RPercent, GPercent * gPercent, BPercent);

        /// <summary>
        /// Multiplies the given blue value to this color.
        /// </summary>
        /// <param name="b">The blue value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyB(double b) => new Color(A, R, G, (int)Math.Round(B * b));

        /// <summary>
        /// Multiplies the given blue-percent value to this color.
        /// </summary>
        /// <param name="bPercent">The blue value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyBPercent(double bPercent) => new Color(APercent, RPercent, GPercent, BPercent * bPercent);

        /// <summary>
        /// Multiplies the given hue value to this color.
        /// </summary>
        /// <param name="hue">The hue value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyHue(double hue) => FromHSV(A, Hue * hue, Saturation, Value);

        /// <summary>
        /// Multiplies the given saturation value to this color.
        /// </summary>
        /// <param name="saturation">The saturation value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplySaturation(double saturation) => FromHSV(A, Hue, Saturation * saturation, Value);

        /// <summary>
        /// Multiplies the given value value to this color.
        /// </summary>
        /// <param name="value">The value value to multiply.</param>
        /// <returns>The new color after the modification.</returns>
        public Color MultiplyValue(double value) => FromHSV(A, Hue, Saturation, Value * value);

        #endregion

        #region Divide

        /// <summary>
        /// Divides the given RGB values to this color.
        /// </summary>
        /// <param name="r">The red value to divide.</param>
        /// <param name="g">The green value to divide.</param>
        /// <param name="b">The blue value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideRGB(double r, double g, double b) => DivideRGB(1, r, g, b);

        /// <summary>
        /// Divides the given RGB values to this color.
        /// </summary>
        /// <param name="a">The alpha value to divide.</param>
        /// <param name="r">The red value to divide.</param>
        /// <param name="g">The green value to divide.</param>
        /// <param name="b">The blue value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideRGB(double a, double r, double g, double b)
            => new Color((int)Math.Round(A / a), (int)Math.Round(R / r), (int)Math.Round(G / g), (int)Math.Round(B / b));

        /// <summary>
        /// Divides the given RGB-percent values to this color.
        /// </summary>
        /// <param name="rPercent">The red value to divide.</param>
        /// <param name="gPercent">The green value to divide.</param>
        /// <param name="bPercent">The blue value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DividePercent(double rPercent, double gPercent, double bPercent) => DividePercent(1, rPercent, gPercent, bPercent);

        /// <summary>
        /// Divides the given RGB-percent values to this color.
        /// </summary>
        /// <param name="aPercent">The alpha value to divide.</param>
        /// <param name="rPercent">The red value to divide.</param>
        /// <param name="gPercent">The green value to divide.</param>
        /// <param name="bPercent">The blue value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DividePercent(double aPercent, double rPercent, double gPercent, double bPercent)
            => new Color(APercent / aPercent, RPercent / rPercent, GPercent / gPercent, BPercent / bPercent);

        /// <summary>
        /// Divides the given HSV values to this color.
        /// </summary>
        /// <param name="hue">The hue value to divide.</param>
        /// <param name="saturation">The saturation value to divide.</param>
        /// <param name="value">The value value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideHSV(double hue, double saturation, double value) => DivideHSV(1, hue, saturation, value);

        /// <summary>
        /// Divides the given HSV values to this color.
        /// </summary>
        /// <param name="a">The alpha value to divide.</param>
        /// <param name="hue">The hue value to divide.</param>
        /// <param name="saturation">The saturation value to divide.</param>
        /// <param name="value">The value value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideHSV(double a, double hue, double saturation, double value)
            => FromHSV((int)Math.Round(A / a), Hue / hue, Saturation / saturation, Value / value);

        /// <summary>
        /// Divides the given alpha value to this color.
        /// </summary>
        /// <param name="a">The alpha value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideA(double a) => new Color((int)Math.Round(A / a), R, G, B);

        /// <summary>
        /// Divides the given alpha-percent value to this color.
        /// </summary>
        /// <param name="aPercent">The alpha value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideAPercent(double aPercent) => new Color(APercent / aPercent, RPercent, GPercent, BPercent);

        /// <summary>
        /// Divides the given red value to this color.
        /// </summary>
        /// <param name="r">The red value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideRValue(double r) => new Color(A, (int)Math.Round(R / r), G, B);

        /// <summary>
        /// Divides the given red-percent value to this color.
        /// </summary>
        /// <param name="rPercent">The red value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideRPercent(double rPercent) => new Color(APercent, RPercent / rPercent, GPercent, BPercent);

        /// <summary>
        /// Divides the given green value to this color.
        /// </summary>
        /// <param name="g">The green value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideGValue(double g) => new Color(A, R, (int)Math.Round(G / g), B);

        /// <summary>
        /// Divides the given green-percent value to this color.
        /// </summary>
        /// <param name="gPercent">The green value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideGPercent(double gPercent) => new Color(APercent, RPercent, GPercent / gPercent, BPercent);

        /// <summary>
        /// Divides the given blue value to this color.
        /// </summary>
        /// <param name="b">The blue value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideBValue(double b) => new Color(A, R, G, (int)Math.Round(B / b));

        /// <summary>
        /// Divides the given blue-percent value to this color.
        /// </summary>
        /// <param name="bPercent">The blue value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideBPercent(double bPercent) => new Color(APercent, RPercent, GPercent, BPercent / bPercent);

        /// <summary>
        /// Divides the given hue value to this color.
        /// </summary>
        /// <param name="hue">The hue value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideHue(double hue) => FromHSV(A, Hue / hue, Saturation, Value);

        /// <summary>
        /// Divides the given saturation value to this color.
        /// </summary>
        /// <param name="saturation">The saturation value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideSaturation(double saturation) => FromHSV(A, Hue, Saturation / saturation, Value);

        /// <summary>
        /// Divides the given value value to this color.
        /// </summary>
        /// <param name="value">The value value to divide.</param>
        /// <returns>The new color after the modification.</returns>
        public Color DivideValue(double value) => FromHSV(A, Hue, Saturation, Value / value);

        #endregion

        #region Set

        /// <summary>
        /// Sets the given alpha value of this color.
        /// </summary>
        /// <param name="a">The alpha value to set.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SetA(int a) => new Color(a, R, G, B);

        /// <summary>
        /// Sets the given alpha-percent value of this color.
        /// </summary>
        /// <param name="aPercent">The alpha value to set.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SetAPercent(double aPercent) => new Color(aPercent, RPercent, GPercent, BPercent);

        /// <summary>
        /// Sets the given red value of this color.
        /// </summary>
        /// <param name="r">The red value to set.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SetR(int r) => new Color(A, r, G, B);

        /// <summary>
        /// Sets the given red-percent value of this color.
        /// </summary>
        /// <param name="rPercent">The red value to set.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SetRPercent(double rPercent) => new Color(APercent, rPercent, GPercent, BPercent);

        /// <summary>
        /// Sets the given green value of this color.
        /// </summary>
        /// <param name="g">The green value to set.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SetG(int g) => new Color(A, R, g, B);

        /// <summary>
        /// Sets the given green-percent value of this color.
        /// </summary>
        /// <param name="gPercent">The green value to set.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SetGPercent(double gPercent) => new Color(APercent, RPercent, gPercent, BPercent);

        /// <summary>
        /// Sets the given blue value of this color.
        /// </summary>
        /// <param name="b">The blue value to set.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SetB(int b) => new Color(A, R, G, b);

        /// <summary>
        /// Sets the given blue-percent value of this color.
        /// </summary>
        /// <param name="bPercent">The blue value to set.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SetBPercent(double bPercent) => new Color(APercent, RPercent, GPercent, bPercent);

        /// <summary>
        /// Sets the given hue value of this color.
        /// </summary>
        /// <param name="hue">The hue value to set.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SetHue(double hue) => FromHSV(A, hue, Saturation, Value);

        /// <summary>
        /// Sets the given saturation value of this color.
        /// </summary>
        /// <param name="saturation">The saturation value to set.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SetSaturation(double saturation) => FromHSV(A, Hue, saturation, Value);

        /// <summary>
        /// Sets the given value value of this color.
        /// </summary>
        /// <param name="value">The value value to set.</param>
        /// <returns>The new color after the modification.</returns>
        public Color SetValue(double value) => FromHSV(A, Hue, Saturation, value);

        #endregion

        #endregion

        #endregion

        #region Operators

        /// <summary>
        /// Blends the provided colors as if <see cref="Blend"/> would've been called on <paramref name="color1" />.
        /// </summary>
        /// <param name="color1">The base color.</param>
        /// <param name="color2">The color to blend.</param>
        /// <returns>The blended color.</returns>
        public static Color operator +(Color color1, Color color2) => color1.Blend(color2);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Color" /> are equal.
        /// </summary>
        /// <param name="color1">The first <see cref="Color" /> to compare.</param>
        /// <param name="color2">The second <see cref="Color" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="color1" /> and <paramref name="color2" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Color color1, Color color2) => color1.Equals(color2);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Color" /> are equal.
        /// </summary>
        /// <param name="color1">The first <see cref="Color" /> to compare.</param>
        /// <param name="color2">The second <see cref="Color" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="color1" /> and <paramref name="color2" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Color color1, Color color2) => !(color1 == color2);

        /// <summary>
        /// Converts a <see cref="ValueTuple"/> of ARGB-components to a <see cref="Color"/>.
        /// </summary>
        /// <param name="components">The <see cref="ValueTuple"/> containing the components.</param>
        /// <returns>The color.</returns>
        public static implicit operator Color((byte a, byte r, byte g, byte b) components) => new Color(components.a, components.r, components.g, components.b);

        /// <summary>
        /// Converts a <see cref="ValueTuple"/> of HSV-components to a <see cref="Color"/>.
        /// </summary>
        /// <param name="components">The <see cref="ValueTuple"/> containing the components.</param>
        /// <returns>The color.</returns>
        public static implicit operator Color((double hue, double saturation, double value) components) => new Color(components.hue, components.saturation, components.value);

        #endregion
    }
}
