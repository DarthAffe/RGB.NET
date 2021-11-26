// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Diagnostics;

namespace RGB.NET.Core;

/// <summary>
/// Represents a scaling.
/// </summary>
[DebuggerDisplay("[Horizontal: {Horizontal}, Vertical: {Vertical}]")]
public readonly struct Scale
{
    #region Properties & Fields

    /// <summary>
    /// Gets the horizontal scaling value.
    /// </summary>
    public float Horizontal { get; }

    /// <summary>
    /// Gets the vertical scaling value.
    /// </summary>
    public float Vertical { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Scale"/> class using the provided values.
    /// </summary>
    /// <param name="scale">The value used for horizontal and vertical scaling. 0 if not set.</param>
    public Scale(float scale = 1.0f) : this(scale, scale)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Scale"/> class using the provided values.
    /// </summary>
    /// <param name="horizontal">The value used for horizontal scaling.</param>
    /// <param name="vertical">The value used for vertical scaling.</param>
    public Scale(float horizontal, float vertical)
    {
        this.Horizontal = horizontal;
        this.Vertical = vertical;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Tests whether the specified <see cref="Scale"/> is equivalent to this <see cref="Scale" />.
    /// </summary>
    /// <param name="other">The scale to test.</param>
    /// <returns><c>true</c> if <paramref name="other" /> is equivalent to this <see cref="Scale" />; otherwise, <c>false</c>.</returns>
    public bool Equals(Scale other) => Horizontal.EqualsInTolerance(other.Horizontal) && Vertical.EqualsInTolerance(other.Vertical);

    /// <summary>
    /// Tests whether the specified object is a <see cref="Scale" /> and is equivalent to this <see cref="Scale" />.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Scale" /> equivalent to this <see cref="Scale" />; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => obj is Scale other && Equals(other);

    /// <summary>
    /// Returns a hash code for this <see cref="Scale" />.
    /// </summary>
    /// <returns>An integer value that specifies the hash code for this <see cref="Scale" />.</returns>
    public override int GetHashCode() { unchecked { return (Horizontal.GetHashCode() * 397) ^ Vertical.GetHashCode(); } }

    /// <summary>
    /// Deconstructs the scale into the horizontal and vertical value.
    /// </summary>
    /// <param name="horizontalScale">The horizontal scaling value.</param>
    /// <param name="verticalScale">The vertical scaling value.</param>
    public void Deconstruct(out float horizontalScale, out float verticalScale)
    {
        horizontalScale = Horizontal;
        verticalScale = Vertical;
    }

    #endregion

    #region Operators

    /// <summary>
    /// Returns a value that indicates whether two specified <see cref="Scale" /> are equal.
    /// </summary>
    /// <param name="scale1">The first <see cref="Scale" /> to compare.</param>
    /// <param name="scale2">The second <see cref="Scale" /> to compare.</param>
    /// <returns><c>true</c> if <paramref name="scale1" /> and <paramref name="scale2" /> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Scale scale1, Scale scale2) => scale1.Equals(scale2);

    /// <summary>
    /// Returns a value that indicates whether two specified <see cref="Scale" /> are equal.
    /// </summary>
    /// <param name="scale1">The first <see cref="Scale" /> to compare.</param>
    /// <param name="scale2">The second <see cref="Scale" /> to compare.</param>
    /// <returns><c>true</c> if <paramref name="scale1" /> and <paramref name="scale2" /> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Scale scale1, Scale scale2) => !(scale1 == scale2);

    /// <summary>
    /// Returns a new <see cref="Scale"/> representing the addition of the <see cref="Scale"/> and the provided value.
    /// </summary>
    /// <param name="scale">The <see cref="Scale"/>.</param>
    /// <param name="value">The value to add.</param>
    /// <returns>A new <see cref="Scale"/> representing the addition of the <see cref="Scale"/> and the provided value.</returns>
    public static Scale operator +(Scale scale, float value) => new(scale.Horizontal + value, scale.Vertical + value);

    /// <summary>
    /// Returns a new <see cref="Scale"/> representing the subtraction of the <see cref="Scale"/> and the provided value.
    /// </summary>
    /// <param name="scale">The <see cref="Scale"/>.</param>
    /// <param name="value">The value to substract.</param>
    /// <returns>A new <see cref="Scale"/> representing the subtraction of the <see cref="Scale"/> and the provided value.</returns>
    public static Scale operator -(Scale scale, float value) => new(scale.Horizontal - value, scale.Vertical - value);

    /// <summary>
    /// Returns a new <see cref="Scale"/> representing the multiplication of the <see cref="Scale"/> and the provided value.
    /// </summary>
    /// <param name="scale">The <see cref="Scale"/>.</param>
    /// <param name="value">The value to multiply with.</param>
    /// <returns>A new <see cref="Scale"/> representing the multiplication of the <see cref="Scale"/> and the provided value.</returns>
    public static Scale operator *(Scale scale, float value) => new(scale.Horizontal * value, scale.Vertical * value);

    /// <summary>
    /// Returns a new <see cref="Scale"/> representing the division of the <see cref="Scale"/> and the provided value.
    /// </summary>
    /// <param name="scale">The <see cref="Scale"/>.</param>
    /// <param name="value">The value to device with.</param>
    /// <returns>A new <see cref="Scale"/> representing the division of the <see cref="Scale"/> and the provided value.</returns>
    public static Scale operator /(Scale scale, float value) => value.EqualsInTolerance(0) ? new Scale(0) : new Scale(scale.Horizontal / value, scale.Vertical / value);


    /// <summary>
    /// Converts a float to a <see cref="Scale" />.
    /// </summary>
    /// <param name="scale">The scale value to convert.</param>
    public static implicit operator Scale(float scale) => new(scale);

    #endregion
}