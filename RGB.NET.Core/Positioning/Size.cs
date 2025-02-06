﻿// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Diagnostics;

namespace RGB.NET.Core;

/// <summary>
/// Represents a size consisting of a width and a height.
/// </summary>
[DebuggerDisplay("[Width: {Width}, Height: {Height}]")]
public readonly struct Size : IEquatable<Size>
{
    #region Constants

    private static readonly Size INVALID = new(float.NaN, float.NaN);
    /// <summary>
    /// Gets a [NaN,NaN]-Size.
    /// </summary>
    public static ref readonly Size Invalid => ref INVALID;

    #endregion

    #region Properties & Fields

    /// <summary>
    /// Gets or sets the width component value of this <see cref="Size"/>.
    /// </summary>
    public float Width { get; }

    /// <summary>
    /// Gets or sets the height component value of this <see cref="Size"/>.
    /// </summary>
    public float Height { get; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Core.Size" /> using the provided size to define a square.
    /// </summary>
    /// <param name="size">The size used for the <see cref="P:RGB.NET.Core.Size.Width" /> component value and the <see cref="P:RGB.NET.Core.Size.Height" /> component value.</param>
    public Size(float size)
        : this(size, size)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Size"/> class using the provided values.
    /// </summary>
    /// <param name="width">The size used for the <see cref="Width"/> component value.</param>
    /// <param name="height">The size used for the <see cref="Height"/> component value.</param>
    public Size(float width, float height)
    {
        this.Width = width;
        this.Height = height;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Converts the <see cref="Width"/> and <see cref="Height"/> of this <see cref="Size"/> to a human-readable string.
    /// </summary>
    /// <returns>A string that contains the <see cref="Width"/> and <see cref="Height"/>  of this <see cref="Size"/>. For example "[Width: 100, Height: 20]".</returns>
    public override string ToString() => $"[Width: {Width}, Height: {Height}]";

    /// <summary>
    /// Tests whether the specified <see cref="Size" /> is equivalent to this <see cref="Size" />.
    /// </summary>
    /// <param name="other">The size to test.</param>
    /// <returns><c>true</c> if <paramref name="other" /> is equivalent to this <see cref="Size" />; otherwise, <c>false</c>.</returns>
    public bool Equals(Size other) => ((float.IsNaN(Width) && float.IsNaN(other.Width)) || Width.EqualsInTolerance(other.Width))
                                   && ((float.IsNaN(Height) && float.IsNaN(other.Height)) || Height.EqualsInTolerance(other.Height));

    /// <summary>
    /// Tests whether the specified object is a <see cref="Size" /> and is equivalent to this <see cref="Size" />.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Size" /> equivalent to this <see cref="Size" />; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => obj is Size other && Equals(other);

    /// <summary>
    /// Returns a hash code for this <see cref="Size" />.
    /// </summary>
    /// <returns>An integer value that specifies the hash code for this <see cref="Size" />.</returns>
    public override int GetHashCode() => HashCode.Combine(Width, Height);

    /// <summary>
    /// Deconstructs the size into the width and height value.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public void Deconstruct(out float width, out float height)
    {
        width = Width;
        height = Height;
    }

    #endregion

    #region Operators

    /// <summary>
    /// Returns a value that indicates whether two specified <see cref="Size" /> are equal.
    /// </summary>
    /// <param name="size1">The first <see cref="Size" /> to compare.</param>
    /// <param name="size2">The second <see cref="Size" /> to compare.</param>
    /// <returns><c>true</c> if <paramref name="size1" /> and <paramref name="size2" /> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Size size1, Size size2) => size1.Equals(size2);

    /// <summary>
    /// Returns a value that indicates whether two specified <see cref="Size" /> are equal.
    /// </summary>
    /// <param name="size1">The first <see cref="Size" /> to compare.</param>
    /// <param name="size2">The second <see cref="Size" /> to compare.</param>
    /// <returns><c>true</c> if <paramref name="size1" /> and <paramref name="size2" /> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Size size1, Size size2) => !(size1 == size2);

    /// <summary>
    /// Returns a new <see cref="Size"/> representing the addition of the two provided <see cref="Size"/>.
    /// </summary>
    /// <param name="size1">The first <see cref="Size"/>.</param>
    /// <param name="size2">The second <see cref="Size"/>.</param>
    /// <returns>A new <see cref="Size"/> representing the addition of the two provided <see cref="Size"/>.</returns>
    public static Size operator +(Size size1, Size size2) => new(size1.Width + size2.Width, size1.Height + size2.Height);

    /// <summary>
    /// Returns a new <see cref="Rectangle"/> created from the provided <see cref="Point"/> and <see cref="Size"/>.
    /// </summary>
    /// <param name="size">The <see cref="Size"/> of the rectangle.</param>
    /// <param name="point">The <see cref="Point"/> of the rectangle.</param>
    /// <returns>The rectangle created from the provided <see cref="Point"/> and <see cref="Size"/>.</returns>
    public static Rectangle operator +(Size size, Point point) => new(point, size);

    /// <summary>
    /// Returns a new <see cref="Size"/> representing the subtraction of the two provided <see cref="Size"/>.
    /// </summary>
    /// <param name="size1">The first <see cref="Size"/>.</param>
    /// <param name="size2">The second <see cref="Size"/>.</param>
    /// <returns>A new <see cref="Size"/> representing the subtraction of the two provided <see cref="Size"/>.</returns>
    public static Size operator -(Size size1, Size size2) => new(size1.Width - size2.Width, size1.Height - size2.Height);

    /// <summary>
    /// Returns a new <see cref="Size"/> representing the multiplication of the two provided <see cref="Size"/>.
    /// </summary>
    /// <param name="size1">The first <see cref="Size"/>.</param>
    /// <param name="size2">The second <see cref="Size"/>.</param>
    /// <returns>A new <see cref="Size"/> representing the multiplication of the two provided <see cref="Size"/>.</returns>
    public static Size operator *(Size size1, Size size2) => new(size1.Width * size2.Width, size1.Height * size2.Height);

    /// <summary>
    /// Returns a new <see cref="Size"/> representing the multiplication of the <see cref="Size"/> and the provided factor.
    /// </summary>
    /// <param name="size">The <see cref="Size"/>.</param>
    /// <param name="factor">The factor by which the <see cref="Size"/> should be multiplied.</param>
    /// <returns>A new <see cref="Size"/> representing the multiplication of the <see cref="Size"/> and the provided factor.</returns>
    public static Size operator *(Size size, float factor) => new(size.Width * factor, size.Height * factor);

    /// <summary>
    /// Returns a new <see cref="Size"/> representing the division of the two provided <see cref="Size"/>.
    /// </summary>
    /// <param name="size1">The first <see cref="Size"/>.</param>
    /// <param name="size2">The second <see cref="Size"/>.</param>
    /// <returns>A new <see cref="Size"/> representing the division of the two provided <see cref="Size"/>.</returns>
    public static Size operator /(Size size1, Size size2)
        => size2.Width.EqualsInTolerance(0) || size2.Height.EqualsInTolerance(0)
               ? Invalid : new Size(size1.Width / size2.Width, size1.Height / size2.Height);

    /// <summary>
    /// Returns a new <see cref="Size"/> representing the division of the <see cref="Size"/> and the provided factor.
    /// </summary>
    /// <param name="size">The <see cref="Size"/>.</param>
    /// <param name="factor">The factor by which the <see cref="Size"/> should be divided.</param>
    /// <returns>A new <see cref="Size"/> representing the division of the <see cref="Size"/> and the provided factor.</returns>
    public static Size operator /(Size size, float factor) => factor.EqualsInTolerance(0) ? Invalid : new Size(size.Width / factor, size.Height / factor);

    /// <summary>
    /// Returns a new <see cref="Size"/> representing the multiplication of the <see cref="Size"/> and the specified <see cref="Scale"/>.
    /// </summary>
    /// <param name="size">The <see cref="Size"/> to scale.</param>
    /// <param name="scale">The scaling factor.</param>
    /// <returns>A new <see cref="Size"/> representing the multiplication of the <see cref="Size"/> and the specified <see cref="Scale"/>.</returns>
    public static Size operator *(Size size, Scale scale) => new(size.Width * scale.Horizontal, size.Height * scale.Vertical);

    #endregion
}