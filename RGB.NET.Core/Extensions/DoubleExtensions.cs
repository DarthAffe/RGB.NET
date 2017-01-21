using System;
using System.Runtime.CompilerServices;

namespace RGB.NET.Core.Extensions
{
    /// <summary>
    /// Offers some extensions and helper-methods for the work with doubles
    /// </summary>
    public static class DoubleExtensions
    {
        #region Constants

        /// <summary>
        /// Defines the precision RGB.NET processes floating point comparisons in.
        /// </summary>
        public const double TOLERANCE = 1E-10;

        #endregion

        #region Methods

        /// <summary>
        /// Checks if two values are equal respecting the <see cref="TOLERANCE"/>.
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The first value to compare.</param>
        /// <returns><c>true</c> if the difference is smaller than the <see cref="TOLERANCE"/>; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsInTolerance(this double value1, double value2)
        {
            return Math.Abs(value1 - value2) < TOLERANCE;
        }

        /// <summary>
        /// Camps the provided value to be bigger or equal min and smaller or equal max.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The lower value of the range the value is clamped to.</param>
        /// <param name="max">The higher value of the range the value is clamped to.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(this double value, double min, double max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        #endregion
    }
}
