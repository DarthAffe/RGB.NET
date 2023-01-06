using System;
using RGB.NET.Core;
using RGB.NET.Presets.Textures.Sampler;

namespace RGB.NET.Presets.Textures;

/// <inheritdoc  />
/// <summary>
/// Represents a texture of float-data pixels.
/// </summary>
public sealed class FloatPixelTexture : PixelTexture<float>
{
    #region Properties & Fields

    private readonly float[] _data;
    /// <inheritdoc />
    protected override ReadOnlySpan<float> Data => _data;

    /// <summary>
    /// Gets the color format the data is in.
    /// </summary>
    public ColorFormat ColorFormat { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="FloatPixelTexture" /> class.
    /// A <see cref="AverageColorSampler"/> is used.
    /// </summary>
    /// <param name="with">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="data">The pixel-data of the texture.</param>
    /// <param name="colorFormat">The color format the data is in. (default: RGB)</param>
    public FloatPixelTexture(int with, int height, float[] data, ColorFormat colorFormat = ColorFormat.RGB)
        : this(with, height, data, new AverageFloatSampler(), colorFormat)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="FloatPixelTexture" /> class.
    /// </summary>
    /// <param name="with">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="data">The pixel-data of the texture.</param>
    /// <param name="sampler">The sampler used to get the color of a region.</param>
    /// <param name="colorFormat">The color format the data is in. (default: RGB)</param>
    public FloatPixelTexture(int with, int height, float[] data, ISampler<float> sampler, ColorFormat colorFormat = ColorFormat.RGB)
        : base(with, height, 3, sampler)
    {
        this._data = data;
        this.ColorFormat = colorFormat;

        if (Data.Length != ((with * height) * 3)) throw new ArgumentException($"Data-Length {Data.Length} differs from the specified size {with}x{height} * 3 bytes ({with * height * 3}).");
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override Color GetColor(in ReadOnlySpan<float> pixel)
    {
        return ColorFormat switch
        {
            ColorFormat.RGB => new Color(pixel[0], pixel[1], pixel[2]),
            ColorFormat.BGR => new Color(pixel[2], pixel[1], pixel[0]),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    #endregion
}