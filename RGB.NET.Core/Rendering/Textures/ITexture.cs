namespace RGB.NET.Core;

/// <summary>
/// Represents a generic texture.
/// </summary>
public interface ITexture
{
    /// <summary>
    /// Gets a empty texture.
    /// </summary>
    static ITexture Empty => new EmptyTexture();
        
    /// <summary>
    /// Gets the size of the texture
    /// </summary>
    Size Size { get; }

    /// <summary>
    /// Gets the color at the specified location.
    /// </summary>
    /// <param name="point">The location to get the color from.</param>
    /// <returns>The color at the specified location.</returns>
    Color this[in Point point] { get; }

    /// <summary>
    /// Gets the sampled color inside the specified rectangle.
    /// </summary>
    /// <param name="rectangle">The rectangle to get the color from.</param>
    /// <returns>The sampled color.</returns>
    Color this[in Rectangle rectangle] { get; }
}