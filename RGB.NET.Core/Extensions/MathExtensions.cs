using System;
using System.Runtime.CompilerServices;

namespace RGB.NET.Core;

/// <summary>
/// Offers some extensions and helper-methods for the work with floats.
/// </summary>
public static class FloatExtensions
{
    #region Constants

    /// <summary>
    /// Defines the precision RGB.NET processes floating point comparisons in.
    /// </summary>
    public const float TOLERANCE = 1E-7f;

    #endregion

    #region Methods

    /// <summary>
    /// Checks if two values are equal respecting the <see cref="TOLERANCE"/>.
    /// </summary>
    /// <param name="value1">The first value to compare.</param>
    /// <param name="value2">The first value to compare.</param>
    /// <returns><c>true</c> if the difference is smaller than the <see cref="TOLERANCE"/>; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsInTolerance(this float value1, float value2) => Math.Abs(value1 - value2) < TOLERANCE;

    /// <summary>
    /// Clamps the provided value to be bigger or equal min and smaller or equal max.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The lower value of the range the value is clamped to.</param>
    /// <param name="max">The higher value of the range the value is clamped to.</param>
    /// <returns>The clamped value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp(this float value, float min, float max)
    {
        // ReSharper disable ConvertIfStatementToReturnStatement - I'm not sure why, but inlining this statement reduces performance by ~10%
        if (value < min) return min;
        if (value > max) return max;
        return value;
        // ReSharper restore ConvertIfStatementToReturnStatement
    }

    /// <summary>
    /// Clamps the provided value to be bigger or equal min and smaller or equal max.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The lower value of the range the value is clamped to.</param>
    /// <param name="max">The higher value of the range the value is clamped to.</param>
    /// <returns>The clamped value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(this int value, int min, int max)
    {
        // ReSharper disable ConvertIfStatementToReturnStatement - I'm not sure why, but inlining this statement reduces performance by ~10%
        if (value < min) return min;
        if (value > max) return max;
        return value;
        // ReSharper restore ConvertIfStatementToReturnStatement
    }

    /// <summary>
    /// Enforces the provided value to be in the specified range by wrapping it around the edges if it exceeds them.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <param name="min">The lower value of the range the value is wrapped into.</param>
    /// <param name="max">The higher value of the range the value is wrapped into.</param>
    /// <returns>The wrapped value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Wrap(this float value, float min, float max)
    {
        float range = max - min;

        while (value >= max)
            value -= range;

        while (value < min)
            value += range;

        return value;
    }

    /// <summary>
    /// Converts a normalized float value in the range [0..1] to a byte [0..255].
    /// </summary>
    /// <param name="percentage">The normalized float value to convert.</param>
    /// <returns>The byte value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte GetByteValueFromPercentage(this float percentage)
    {
        if (float.IsNaN(percentage)) return 0;

        percentage = percentage.Clamp(0, 1.0f);
        return (byte)(percentage >= 1.0f ? 255 : percentage * 256.0f);
    }

    /// <summary>
    /// Converts a byte value [0..255] to a normalized float value in the range [0..1].
    /// </summary>
    /// <param name="value">The byte value to convert.</param>
    /// <returns>The normalized float value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetPercentageFromByteValue(this byte value)
        => value == 255 ? 1.0f : (value / 256.0f);

    #endregion
}