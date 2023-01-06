using RGB.NET.Core;
using RGB.NET.Presets.Textures.Gradients;

namespace RGB.NET.Presets.Textures;

/// <inheritdoc cref="AbstractBindable" />
/// <inheritdoc cref="ITexture" />
/// <summary>
/// Represents a abstract texture containing a color-gradient.
/// </summary>
public abstract class AbstractGradientTexture : AbstractBindable, ITexture
{
    #region Properties & Fields

    /// <summary>
    /// Gets the gradient used to generate this texture.
    /// </summary>
    public IGradient Gradient { get; }

    /// <inheritdoc />
    public Size Size { get; }

    /// <inheritdoc />
    public Color this[in Point point] => GetColor(point);

    /// <inheritdoc />
    public Color this[in Rectangle rectangle] => GetColor(rectangle.Center);

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractGradientTexture" /> class.
    /// </summary>
    /// <param name="size">The size of the texture.</param>
    /// <param name="gradient">The gradient used to generate this texture.</param>
    protected AbstractGradientTexture(Size size, IGradient gradient)
    {
        this.Size = size;
        this.Gradient = gradient;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets the color at the specified location of the texture.
    /// </summary>
    /// <param name="point">The location to get the color from.</param>
    /// <returns>The color at the specified location.</returns>
    protected abstract Color GetColor(in Point point);

    #endregion
}