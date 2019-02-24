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
    [DebuggerDisplay("[A: {A}, R: {R}, G: {G}, B: {B}]")]
    public struct Color
    {
        #region Constants

        /// <summary>
        /// Gets an transparent color [A: 0, R: 0, G: 0, B: 0]
        /// </summary>
        public static Color Transparent => new Color(0, 0, 0, 0);

        #endregion

        #region Properties & Fields

        /// <summary>
        /// Gets the alpha component value of this <see cref="Color"/> as byte in the range [0..255].
        /// </summary>
        public byte A { get; }

        /// <summary>
        /// Gets the alpha component value of this <see cref="Color"/> as percentage in the range [0..1].
        /// </summary>
        public double APercent { get; }

        /// <summary>
        /// Gets the red component value of this <see cref="Color"/> as byte in the range [0..255].
        /// </summary>
        public byte R { get; }

        /// <summary>
        /// Gets the red component value of this <see cref="Color"/> as percentage in the range [0..1].
        /// </summary>
        public double RPercent { get; }

        /// <summary>
        /// Gets the green component value of this <see cref="Color"/> as byte in the range [0..255].
        /// </summary>
        public byte G { get; }

        /// <summary>
        /// Gets the green component value of this <see cref="Color"/> as percentage in the range [0..1].
        /// </summary>
        public double GPercent { get; }

        /// <summary>
        /// Gets the blue component value of this <see cref="Color"/> as byte in the range [0..255].
        /// </summary>
        public byte B { get; }

        /// <summary>
        /// Gets the blue component value of this <see cref="Color"/> as percentage in the range [0..1].
        /// </summary>
        public double BPercent { get; }

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
            : this(a, r, g, b, a.GetPercentageFromByteValue(), r.GetPercentageFromByteValue(), g.GetPercentageFromByteValue(), b.GetPercentageFromByteValue())
        { }

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
        /// Alpha defaults to 1.0.
        /// </summary>
        /// <param name="r">The red component value of this <see cref="Color"/>.</param>
        /// <param name="g">The green component value of this <see cref="Color"/>.</param>
        /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
        public Color(double r, double g, double b)
            : this(1.0, r, g, b)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct using ARGB-percent values.
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="r">The red component value of this <see cref="Color"/>.</param>
        /// <param name="g">The green component value of this <see cref="Color"/>.</param>
        /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
        public Color(double a, byte r, byte g, byte b)
            : this(a.GetByteValueFromPercentage(), r, g, b,
                   a.Clamp(0, 1), r.GetPercentageFromByteValue(), g.GetPercentageFromByteValue(), b.GetPercentageFromByteValue())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct using ARGB-percent values.
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="r">The red component value of this <see cref="Color"/>.</param>
        /// <param name="g">The green component value of this <see cref="Color"/>.</param>
        /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
        public Color(double a, int r, int g, int b)
            : this(a, (byte)r.Clamp(0, byte.MaxValue), (byte)g.Clamp(0, byte.MaxValue), (byte)b.Clamp(0, byte.MaxValue))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct using ARGB-percent values.
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="r">The red component value of this <see cref="Color"/>.</param>
        /// <param name="g">The green component value of this <see cref="Color"/>.</param>
        /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
        public Color(int a, double r, double g, double b)
            : this((byte)a.Clamp(0, byte.MaxValue), r, g, b)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct using ARGB-percent values.
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="r">The red component value of this <see cref="Color"/>.</param>
        /// <param name="g">The green component value of this <see cref="Color"/>.</param>
        /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
        public Color(byte a, double r, double g, double b)
            : this(a, r.GetByteValueFromPercentage(), g.GetByteValueFromPercentage(), b.GetByteValueFromPercentage(),
                   a.GetPercentageFromByteValue(), r.Clamp(0, 1), g.Clamp(0, 1), b.Clamp(0, 1))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct using ARGB-percent values.
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
        /// <param name="r">The red component value of this <see cref="Color"/>.</param>
        /// <param name="g">The green component value of this <see cref="Color"/>.</param>
        /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
        public Color(double a, double r, double g, double b)
            : this(a.GetByteValueFromPercentage(), r.GetByteValueFromPercentage(), g.GetByteValueFromPercentage(), b.GetByteValueFromPercentage(),
                  a.Clamp(0, 1), r.Clamp(0, 1), g.Clamp(0, 1), b.Clamp(0, 1))
        { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct by cloning a existing <see cref="T:RGB.NET.Core.Color" />.
        /// </summary>
        /// <param name="color">The <see cref="T:RGB.NET.Core.Color" /> the values are copied from.</param>
        public Color(Color color)
            : this(color.APercent, color.RPercent, color.GPercent, color.BPercent)
        { }

        private Color(byte a, byte r, byte g, byte b, double aP, double rP, double gP, double bP)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;

            this.APercent = aP;
            this.RPercent = rP;
            this.GPercent = gP;
            this.BPercent = bP;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the individual byte values of this <see cref="Color"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the individual byte values of this <see cref="Color"/>. For example "[A: 255, R: 255, G: 0, B: 0]".</returns>
        public override string ToString() => $"[A: {A}, R: {R}, G: {G}, B: {B}]";

        /// <summary>
        /// Tests whether the specified object is a <see cref="Color" /> and is equivalent to this <see cref="Color" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Color" /> equivalent to this <see cref="Color" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Color)) return false;

            (byte a, byte r, byte g, byte b) = (Color)obj;
            return (a == A) && (r == R) && (g == G) && (b == B);
        }

        /// <summary>
        /// Returns a hash code for this <see cref="Color" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="Color" />.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = APercent.GetHashCode();
                hashCode = (hashCode * 397) ^ RPercent.GetHashCode();
                hashCode = (hashCode * 397) ^ GPercent.GetHashCode();
                hashCode = (hashCode * 397) ^ BPercent.GetHashCode();
                return hashCode;
            }
        }

        #region Deconstruction

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
