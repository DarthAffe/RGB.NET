// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Diagnostics;

namespace RGB.NET.Core;

/// <summary>
/// Represents an angular rotation.
/// </summary>
[DebuggerDisplay("[{" + nameof(Degrees) + "}°]")]
public readonly struct Rotation
{
    #region Constants

    private const float TWO_PI = MathF.PI * 2.0f;
    private const float RADIANS_DEGREES_CONVERSION = 180.0f / MathF.PI;
    private const float DEGREES_RADIANS_CONVERSION = MathF.PI / 180.0f;

    #endregion

    #region Properties & Fields

    /// <summary>
    /// Gets the angle in degrees.
    /// </summary>
    public float Degrees { get; }

    /// <summary>
    /// Gets the angle in radians.
    /// </summary>
    public float Radians { get; }

    /// <summary>
    /// Gets a bool indicating if the rotation is > 0.
    /// </summary>
    public bool IsRotated => !Degrees.EqualsInTolerance(0);

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Rotation"/> class using the provided values.
    /// </summary>
    /// <param name="degrees">The rotation in degrees.</param>
    public Rotation(float degrees)
        : this(degrees, degrees * DEGREES_RADIANS_CONVERSION)
    { }

    private Rotation(float degrees, float radians)
    {
        this.Degrees = degrees % 360.0f;
        this.Radians = radians % TWO_PI;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Creates a new Rotation out of the specified degree-angle.
    /// </summary>
    /// <param name="degrees">The angle in degrees.</param>
    /// <returns>The new rotation.</returns>
    public static Rotation FromDegrees(float degrees) => new(degrees);

    /// <summary>
    /// Creates a new Rotation out of the specified radian-angle.
    /// </summary>
    /// <param name="radians">The angle in radians.</param>
    /// <returns>The new rotation.</returns>
    public static Rotation FromRadians(float radians) => new(radians * RADIANS_DEGREES_CONVERSION, radians);

    /// <summary>
    /// Tests whether the specified <see cref="Rotation" /> is equivalent to this <see cref="Rotation" />.
    /// </summary>
    /// <param name="other">The rotation to test.</param>
    /// <returns><c>true</c> if <paramref name="other" /> is equivalent to this <see cref="Rotation" />; otherwise, <c>false</c>.</returns>
    public bool Equals(Rotation other) => Degrees.EqualsInTolerance(other.Degrees);

    /// <summary>
    /// Tests whether the specified object is a <see cref="Rotation" /> and is equivalent to this <see cref="Rotation" />.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Rotation" /> equivalent to this <see cref="Rotation" />; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => obj is Rotation other && Equals(other);

    /// <summary>
    /// Returns a hash code for this <see cref="Rotation" />.
    /// </summary>
    /// <returns>An integer value that specifies the hash code for this <see cref="Rotation" />.</returns>
    public override int GetHashCode() => Degrees.GetHashCode();

    #endregion

    #region Operators

    /// <summary>
    /// Returns a value that indicates whether two specified <see cref="Rotation" /> are equal.
    /// </summary>
    /// <param name="rotation1">The first <see cref="Rotation" /> to compare.</param>
    /// <param name="rotation2">The second <see cref="Rotation" /> to compare.</param>
    /// <returns><c>true</c> if <paramref name="rotation1" /> and <paramref name="rotation2" /> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(in Rotation rotation1, in Rotation rotation2) => rotation1.Equals(rotation2);

    /// <summary>
    /// Returns a value that indicates whether two specified <see cref="Rotation" /> are equal.
    /// </summary>
    /// <param name="rotation1">The first <see cref="Rotation" /> to compare.</param>
    /// <param name="rotation2">The second <see cref="Rotation" /> to compare.</param>
    /// <returns><c>true</c> if <paramref name="rotation1" /> and <paramref name="rotation2" /> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(in Rotation rotation1, in Rotation rotation2) => !(rotation1 == rotation2);

    /// <summary>
    /// Returns a new <see cref="Rotation"/> representing the addition of the <see cref="Rotation"/> and the provided value.
    /// </summary>
    /// <param name="rotation">The <see cref="Rotation"/>.</param>
    /// <param name="value">The value to add.</param>
    /// <returns>A new <see cref="Rotation"/> representing the addition of the <see cref="Rotation"/> and the provided value.</returns>
    public static Rotation operator +(in Rotation rotation, float value) => new(rotation.Degrees + value);

    /// <summary>
    /// Returns a new <see cref="Rotation"/> representing the subtraction of the <see cref="Rotation"/> and the provided value.
    /// </summary>
    /// <param name="rotation">The <see cref="Rotation"/>.</param>
    /// <param name="value">The value to substract.</param>
    /// <returns>A new <see cref="Rotation"/> representing the subtraction of the <see cref="Rotation"/> and the provided value.</returns>
    public static Rotation operator -(in Rotation rotation, float value) => new(rotation.Degrees - value);

    /// <summary>
    /// Returns a new <see cref="Rotation"/> representing the multiplication of the <see cref="Rotation"/> and the provided value.
    /// </summary>
    /// <param name="rotation">The <see cref="Rotation"/>.</param>
    /// <param name="value">The value to multiply with.</param>
    /// <returns>A new <see cref="Rotation"/> representing the multiplication of the <see cref="Rotation"/> and the provided value.</returns>
    public static Rotation operator *(in Rotation rotation, float value) => new(rotation.Degrees * value);

    /// <summary>
    /// Returns a new <see cref="Rotation"/> representing the division of the <see cref="Rotation"/> and the provided value.
    /// </summary>
    /// <param name="rotation">The <see cref="Rotation"/>.</param>
    /// <param name="value">The value to device with.</param>
    /// <returns>A new <see cref="Rotation"/> representing the division of the <see cref="Rotation"/> and the provided value.</returns>
    public static Rotation operator /(in Rotation rotation, float value) => value.EqualsInTolerance(0) ? new Rotation(0) : new Rotation(rotation.Degrees / value);

    /// <summary>
    /// Converts a float to a <see cref="Rotation" />.
    /// </summary>
    /// <param name="rotation">The rotation in degrees to convert.</param>
    public static implicit operator Rotation(float rotation) => new(rotation);

    /// <summary>
    /// Converts <see cref="Rotation" /> to a float representing the rotation in degrees.
    /// </summary>
    /// <param name="rotation">The rotatio to convert.</param>
    public static implicit operator float(in Rotation rotation) => rotation.Degrees;

    #endregion
}