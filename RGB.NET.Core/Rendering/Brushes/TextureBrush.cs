namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents a brush drawing a texture.
/// </summary>
public class TextureBrush : AbstractBrush
{
    #region Properties & Fields

    private ITexture _texture = ITexture.Empty;
    /// <summary>
    /// Gets or sets the texture drawn by this brush.
    /// </summary>
    public ITexture Texture
    {
        get => _texture;
        set => SetProperty(ref _texture, value);
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TextureBrush" /> class.
    /// </summary>
    /// <param name="texture">The texture drawn by this brush.</param>
    public TextureBrush(ITexture texture)
    {
        this.Texture = texture;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override Color GetColorAtPoint(in Rectangle rectangle, in RenderTarget renderTarget)
    {
        Rectangle normalizedRect = renderTarget.Rectangle / rectangle;
        return Texture[normalizedRect];
    }

    #endregion
}