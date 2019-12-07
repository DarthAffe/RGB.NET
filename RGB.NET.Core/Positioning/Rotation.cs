﻿// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Diagnostics;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents an angular rotation.
    /// </summary>
    [DebuggerDisplay("[{Degrees}°]")]
    public struct Rotation
    {
        #region Constants

        private const double TWO_PI = Math.PI * 2.0;
        private const double RADIANS_DEGREES_CONVERSION = 180.0 / Math.PI;
        private const double DEGREES_RADIANS_CONVERSION = Math.PI / 180.0;

        #endregion

        #region Properties & Fields

        /// <summary>
        /// Gets the angle in degrees.
        /// </summary>
        public double Degrees { get; }

        /// <summary>
        /// Gets the angle in radians.
        /// </summary>
        public double Radians { get; }

        /// <summary>
        /// Gets a bool indicating if the rotation is > 0.
        /// </summary>
        public bool IsRotated => !Degrees.EqualsInTolerance(0);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Rotation"/> class using the provided values.
        /// </summary>
        /// <param name="scale">The rotation in degrees.</param>
        public Rotation(double degrees)
            : this(degrees, degrees * DEGREES_RADIANS_CONVERSION)
        { }

        private Rotation(double degrees, double radians)
        {
            this.Degrees = degrees % 360.0;
            this.Radians = radians % TWO_PI;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new Rotation out of the given degree-angle.
        /// </summary>
        /// <param name="degrees">The angle in degrees.</param>
        /// <returns>The new rotation.</returns>
        public static Rotation FromDegrees(double degrees) => new Rotation(degrees);

        /// <summary>
        /// Creates a new Rotation out of the given radian-angle.
        /// </summary>
        /// <param name="degrees">The angle in radians.</param>
        /// <returns>The new rotation.</returns>
        public static Rotation FromRadians(double radians) => new Rotation(radians * RADIANS_DEGREES_CONVERSION, radians);

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
        public override bool Equals(object obj) => obj is Rotation other && Equals(other);

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
        public static bool operator ==(Rotation rotation1, Rotation rotation2) => rotation1.Equals(rotation2);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Rotation" /> are equal.
        /// </summary>
        /// <param name="rotation1">The first <see cref="Rotation" /> to compare.</param>
        /// <param name="rotation2">The second <see cref="Rotation" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="rotation1" /> and <paramref name="rotation2" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Rotation rotation1, Rotation rotation2) => !(rotation1 == rotation2);

        /// <summary>
        /// Returns a new <see cref="Rotation"/> representing the addition of the <see cref="Rotation"/> and the provided value.
        /// </summary>
        /// <param name="rotation">The <see cref="Rotation"/>.</param>
        /// <param name="value">The value to add.</param>
        /// <returns>A new <see cref="Rotation"/> representing the addition of the <see cref="Rotation"/> and the provided value.</returns>
        public static Rotation operator +(Rotation rotation, double value) => new Rotation(rotation.Degrees + value);

        /// <summary>
        /// Returns a new <see cref="Rotation"/> representing the subtraction of the <see cref="Rotation"/> and the provided value.
        /// </summary>
        /// <param name="rotation">The <see cref="Rotation"/>.</param>
        /// <param name="value">The value to substract.</param>
        /// <returns>A new <see cref="Rotation"/> representing the subtraction of the <see cref="Rotation"/> and the provided value.</returns>
        public static Rotation operator -(Rotation rotation, double value) => new Rotation(rotation.Degrees - value);

        /// <summary>
        /// Returns a new <see cref="Rotation"/> representing the multiplication of the <see cref="Rotation"/> and the provided value.
        /// </summary>
        /// <param name="rotation">The <see cref="Rotation"/>.</param>
        /// <param name="value">The value to multiply with.</param>
        /// <returns>A new <see cref="Rotation"/> representing the multiplication of the <see cref="Rotation"/> and the provided value.</returns>
        public static Rotation operator *(Rotation rotation, double value) => new Rotation(rotation.Degrees * value);

        /// <summary>
        /// Returns a new <see cref="Rotation"/> representing the division of the <see cref="Rotation"/> and the provided value.
        /// </summary>
        /// <param name="rotation">The <see cref="Rotation"/>.</param>
        /// <param name="value">The value to device with.</param>
        /// <returns>A new <see cref="Rotation"/> representing the division of the <see cref="Rotation"/> and the provided value.</returns>
        public static Rotation operator /(Rotation rotation, double value) => value.EqualsInTolerance(0) ? new Rotation(0) : new Rotation(rotation.Degrees / value);

        /// <summary>
        /// Converts a double to a <see cref="Rotation" />.
        /// </summary>
        /// <param name="rotation">The rotation in degrees to convert.</param>
        public static implicit operator Rotation(double rotation) => new Rotation(rotation);

        /// <summary>
        /// Converts <see cref="Rotation" /> to a double representing the rotation in degrees.
        /// </summary>
        /// <param name="rotation">The rotatio to convert.</param>
        public static implicit operator double(Rotation rotation) => rotation.Degrees;

        #endregion
    }
}
