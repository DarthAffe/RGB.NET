// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

using System;
using System.Diagnostics;

namespace RGB.NET.Core;

/// <summary>
/// Represents an ARGB (alpha, red, green, blue) color.
/// </summary>
[DebuggerDisplay("[A: {A}, R: {R}, G: {G}, B: {B}]")]
public readonly struct Color
{
    #region Constants

    private static readonly Color TRANSPARENT = new(0, 0, 0, 0);
    /// <summary>
    /// Gets an transparent color [A: 0, R: 0, G: 0, B: 0]
    /// </summary>
    public static ref readonly Color Transparent => ref TRANSPARENT;

    #endregion

    #region Properties & Fields

    /// <summary>
    /// Gets or sets the <see cref="IColorBehavior"/> used to perform operations on colors.
    /// </summary>
    public static IColorBehavior Behavior { get; set; } = new DefaultColorBehavior();

    /// <summary>
    /// Gets the alpha component value of this <see cref="Color"/> as percentage in the range [0..1].
    /// </summary>
    public float A { get; }

    /// <summary>
    /// Gets the red component value of this <see cref="Color"/> as percentage in the range [0..1].
    /// </summary>
    public float R { get; }

    /// <summary>
    /// Gets the green component value of this <see cref="Color"/> as percentage in the range [0..1].
    /// </summary>
    public float G { get; }

    /// <summary>
    /// Gets the blue component value of this <see cref="Color"/> as percentage in the range [0..1].
    /// </summary>
    public float B { get; }

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
        : this(a.GetPercentageFromByteValue(), r.GetPercentageFromByteValue(), g.GetPercentageFromByteValue(), b.GetPercentageFromByteValue())
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
    public Color(float r, float g, float b)
        : this(1.0f, r, g, b)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Color"/> struct using ARGB-percent values.
    /// </summary>
    /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="r">The red component value of this <see cref="Color"/>.</param>
    /// <param name="g">The green component value of this <see cref="Color"/>.</param>
    /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
    public Color(float a, byte r, byte g, byte b)
        : this(a, r.GetPercentageFromByteValue(), g.GetPercentageFromByteValue(), b.GetPercentageFromByteValue())
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Color"/> struct using ARGB-percent values.
    /// </summary>
    /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="r">The red component value of this <see cref="Color"/>.</param>
    /// <param name="g">The green component value of this <see cref="Color"/>.</param>
    /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
    public Color(float a, int r, int g, int b)
        : this(a, (byte)r.Clamp(0, byte.MaxValue), (byte)g.Clamp(0, byte.MaxValue), (byte)b.Clamp(0, byte.MaxValue))
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Color"/> struct using ARGB-percent values.
    /// </summary>
    /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="r">The red component value of this <see cref="Color"/>.</param>
    /// <param name="g">The green component value of this <see cref="Color"/>.</param>
    /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
    public Color(int a, float r, float g, float b)
        : this((byte)a.Clamp(0, byte.MaxValue), r, g, b)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Color"/> struct using ARGB-percent values.
    /// </summary>
    /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="r">The red component value of this <see cref="Color"/>.</param>
    /// <param name="g">The green component value of this <see cref="Color"/>.</param>
    /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
    public Color(byte a, float r, float g, float b)
        : this(a.GetPercentageFromByteValue(), r, g, b)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Color"/> struct using ARGB-percent values.
    /// </summary>
    /// <param name="a">The alpha component value of this <see cref="Color"/>.</param>
    /// <param name="r">The red component value of this <see cref="Color"/>.</param>
    /// <param name="g">The green component value of this <see cref="Color"/>.</param>
    /// <param name="b">The blue component value of this <see cref="Color"/>.</param>
    public Color(float a, float r, float g, float b)
    {
        A = a.Clamp(0, 1);
        R = r.Clamp(0, 1);
        G = g.Clamp(0, 1);
        B = b.Clamp(0, 1);
    }

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Core.Color" /> struct by cloning a existing <see cref="T:RGB.NET.Core.Color" />.
    /// </summary>
    /// <param name="color">The <see cref="T:RGB.NET.Core.Color" /> the values are copied from.</param>
    public Color(in Color color)
        : this(color.A, color.R, color.G, color.B)
    { }

    #endregion

    #region Methods

    /// <summary>
    /// Gets a human-readable string, as defined by the current <see cref="Behavior"/>.
    /// </summary>
    /// <returns>A string that contains the individual byte values of this <see cref="Color"/>. Default format: "[A: 255, R: 255, G: 0, B: 0]".</returns>
    public override string ToString() => Behavior.ToString(this);

    /// <summary>
    /// Tests whether the specified object is a <see cref="Color" /> and is equivalent to this <see cref="Color" />, as defined by the current <see cref="Behavior"/>.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Color" /> equivalent to this <see cref="Color" />; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => Behavior.Equals(this, obj);

    /// <summary>
    /// Returns a hash code for this <see cref="Color" />, as defined by the current <see cref="Behavior"/>.
    /// </summary>
    /// <returns>An integer value that specifies the hash code for this <see cref="Color" />.</returns>
    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Behavior.GetHashCode(this);

    /// <summary>
    /// Blends a <see cref="Color"/> over this color, as defined by the current <see cref="Behavior"/>.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to blend.</param>
    public Color Blend(in Color color) => Behavior.Blend(this, color);

    #endregion

    #region Operators

    /// <summary>
    /// Blends the provided colors as if <see cref="Blend"/> would've been called on <paramref name="color1" />.
    /// </summary>
    /// <param name="color1">The base color.</param>
    /// <param name="color2">The color to blend.</param>
    /// <returns>The blended color.</returns>
    public static Color operator +(in Color color1, in Color color2) => color1.Blend(color2);

    /// <summary>
    /// Returns a value that indicates whether two specified <see cref="Color" /> are equal.
    /// </summary>
    /// <param name="color1">The first <see cref="Color" /> to compare.</param>
    /// <param name="color2">The second <see cref="Color" /> to compare.</param>
    /// <returns><c>true</c> if <paramref name="color1" /> and <paramref name="color2" /> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(in Color color1, in Color color2) => color1.Equals(color2);

    /// <summary>
    /// Returns a value that indicates whether two specified <see cref="Color" /> are equal.
    /// </summary>
    /// <param name="color1">The first <see cref="Color" /> to compare.</param>
    /// <param name="color2">The second <see cref="Color" /> to compare.</param>
    /// <returns><c>true</c> if <paramref name="color1" /> and <paramref name="color2" /> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(in Color color1, in Color color2) => !(color1 == color2);

    /// <summary>
    /// Converts a <see cref="ValueTuple"/> of ARGB-components to a <see cref="Color"/>.
    /// </summary>
    /// <param name="components">The <see cref="ValueTuple"/> containing the components.</param>
    /// <returns>The color.</returns>
    public static implicit operator Color((byte r, byte g, byte b) components) => new(components.r, components.g, components.b);

    /// <summary>
    /// Converts a <see cref="ValueTuple"/> of ARGB-components to a <see cref="Color"/>.
    /// </summary>
    /// <param name="components">The <see cref="ValueTuple"/> containing the components.</param>
    /// <returns>The color.</returns>
    public static implicit operator Color((byte a, byte r, byte g, byte b) components) => new(components.a, components.r, components.g, components.b);

    /// <summary>
    /// Converts a <see cref="ValueTuple"/> of ARGB-components to a <see cref="Color"/>.
    /// </summary>
    /// <param name="components">The <see cref="ValueTuple"/> containing the components.</param>
    /// <returns>The color.</returns>
    public static implicit operator Color((int r, int g, int b) components) => new(components.r, components.g, components.b);

    /// <summary>
    /// Converts a <see cref="ValueTuple"/> of ARGB-components to a <see cref="Color"/>.
    /// </summary>
    /// <param name="components">The <see cref="ValueTuple"/> containing the components.</param>
    /// <returns>The color.</returns>
    public static implicit operator Color((int a, int r, int g, int b) components) => new(components.a, components.r, components.g, components.b);

    /// <summary>
    /// Converts a <see cref="ValueTuple"/> of ARGB-components to a <see cref="Color"/>.
    /// </summary>
    /// <param name="components">The <see cref="ValueTuple"/> containing the components.</param>
    /// <returns>The color.</returns>
    public static implicit operator Color((float r, float g, float b) components) => new(components.r, components.g, components.b);

    /// <summary>
    /// Converts a <see cref="ValueTuple"/> of ARGB-components to a <see cref="Color"/>.
    /// </summary>
    /// <param name="components">The <see cref="ValueTuple"/> containing the components.</param>
    /// <returns>The color.</returns>
    public static implicit operator Color((float a, float r, float g, float b) components) => new(components.a, components.r, components.g, components.b);

    #endregion
}