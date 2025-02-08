﻿namespace RGB.NET.Core;

/// <summary>
/// Represents a behavior of a color for base operations.
/// </summary>
public interface IColorBehavior
{
    /// <summary>
    /// Converts the specified <see cref="Color"/> to a string representation.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>The string representation of the specified color.</returns>
    string ToString(Color color);

    /// <summary>
    /// Tests whether the specified object is a <see cref="Color" /> and is equivalent to this <see cref="Color" />.
    /// </summary>
    /// <param name="color">The color to test.</param>
    /// <param name="obj">The object to test.</param>
    /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Color" /> equivalent to this <see cref="Color" />; otherwise, <c>false</c>.</returns>
    bool Equals(Color color, object? obj);

    /// <summary>
    /// Tests whether the specified object is a <see cref="Color" /> and is equivalent to this <see cref="Color" />.
    /// </summary>
    /// <param name="color">The first color to test.</param>
    /// <param name="color2">The second color to test.</param>
    /// <returns><c>true</c> if <paramref name="color2" /> equivalent to this <see cref="Color" />; otherwise, <c>false</c>.</returns>
    bool Equals(Color color, Color color2);

    /// <summary>
    /// Returns a hash code for this <see cref="Color" />.
    /// </summary>
    /// <returns>An integer value that specifies the hash code for this <see cref="Color" />.</returns>
    int GetHashCode(Color color);

    /// <summary>
    /// Blends a <see cref="Color"/> over this color.
    /// </summary>
    /// <param name="baseColor">The <see cref="Color"/> to to blend over.</param>
    /// <param name="blendColor">The <see cref="Color"/> to blend.</param>
    Color Blend(Color baseColor, Color blendColor);
}