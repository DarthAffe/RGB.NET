using System;

namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents the default-behavior for the work with colors.
/// </summary>
public class DefaultColorBehavior : IColorBehavior
{
    #region Methods

    /// <summary>
    /// Converts the individual byte values of this <see cref="Color"/> to a human-readable string.
    /// </summary>
    /// <returns>A string that contains the individual byte values of this <see cref="Color"/>. For example "[A: 255, R: 255, G: 0, B: 0]".</returns>
    public virtual string ToString(in Color color) => $"[A: {color.GetA()}, R: {color.GetR()}, G: {color.GetG()}, B: {color.GetB()}]";

    /// <summary>
    /// Tests whether the specified object is a <see cref="Color" /> and is equivalent to this <see cref="Color" />.
    /// </summary>
    /// <param name="color">The color to test.</param>
    /// <param name="obj">The object to test.</param>
    /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="Color" /> equivalent to this <see cref="Color" />; otherwise, <c>false</c>.</returns>
    public virtual bool Equals(in Color color, object? obj)
    {
        if (obj is not Color color2) return false;

        (float a, float r, float g, float b) = color2.GetRGB();
        return color.A.EqualsInTolerance(a) && color.R.EqualsInTolerance(r) && color.G.EqualsInTolerance(g) && color.B.EqualsInTolerance(b);
    }

    /// <summary>
    /// Returns a hash code for this <see cref="Color" />.
    /// </summary>
    /// <returns>An integer value that specifies the hash code for this <see cref="Color" />.</returns>
    public virtual int GetHashCode(in Color color) => HashCode.Combine(color.A, color.R, color.G, color.B);

    /// <summary>
    /// Blends a <see cref="Color"/> over this color.
    /// </summary>
    /// <param name="baseColor">The <see cref="Color"/> to to blend over.</param>
    /// <param name="blendColor">The <see cref="Color"/> to blend.</param>
    public virtual Color Blend(in Color baseColor, in Color blendColor)
    {
        if (blendColor.A.EqualsInTolerance(0)) return baseColor;

        if (blendColor.A.EqualsInTolerance(1))
            return blendColor;

        float resultA = (1.0f - ((1.0f - blendColor.A) * (1.0f - baseColor.A)));
        float resultR = (((blendColor.R * blendColor.A) / resultA) + ((baseColor.R * baseColor.A * (1.0f - blendColor.A)) / resultA));
        float resultG = (((blendColor.G * blendColor.A) / resultA) + ((baseColor.G * baseColor.A * (1.0f - blendColor.A)) / resultA));
        float resultB = (((blendColor.B * blendColor.A) / resultA) + ((baseColor.B * baseColor.A * (1.0f - blendColor.A)) / resultA));

        return new Color(resultA, resultR, resultG, resultB);
    }

    #endregion
}