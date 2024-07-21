using HPPH;
using RGB.NET.Core;

namespace RGB.NET.Presets.Extensions;

/// <summary>
/// Offers some extensions related to HPPH.
/// </summary>
public static class HPPHExtensions
{
    /// <summary>
    /// Converts the given HPPH <see cref="IColor"/> to a RGB.NET <see cref="Color"/>.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>The converted color.</returns>
    public static Color ToColor(this IColor color) => new(color.A, color.R, color.G, color.B);

    /// <summary>
    /// Converts the given HPPH <see cref="IColor"/> to a RGB.NET <see cref="Color"/>.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <typeparam name="T">The color-type of the HPPH color.</typeparam>
    /// <returns>The converted color.</returns>
    public static Color ToColor<T>(this T color)
        where T : struct, IColor
        => new(color.A, color.R, color.G, color.B);
}