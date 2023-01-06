using System;
using RGB.NET.Core;
using RGB.NET.Presets.Textures.Sampler;

namespace RGB.NET.Presets.Textures;

/// <inheritdoc  />
/// <summary>
/// Represents a texture of byte-data pixels.
/// </summary>
public sealed class BytePixelTexture : PixelTexture<byte>
{
    #region Properties & Fields

    private readonly byte[] _data;
    /// <inheritdoc />
    protected override ReadOnlySpan<byte> Data => _data;

    /// <summary>
    /// Gets the color format the data is in.
    /// </summary>
    public ColorFormat ColorFormat { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BytePixelTexture" /> class.
    /// A <see cref="AverageColorSampler"/> is used.
    /// </summary>
    /// <param name="with">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="data">The pixel-data of the texture.</param>
    /// <param name="colorFormat">The color format the data is in. (default: RGB)</param>
    public BytePixelTexture(int with, int height, byte[] data, ColorFormat colorFormat = ColorFormat.RGB)
        : this(with, height, data, new AverageByteSampler(), colorFormat)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BytePixelTexture" /> class.
    /// </summary>
    /// <param name="with">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="data">The pixel-data of the texture.</param>
    /// <param name="sampler">The sampler used to get the color of a region.</param>
    /// <param name="colorFormat">The color format the data is in. (default: RGB)</param>
    public BytePixelTexture(int with, int height, byte[] data, ISampler<byte> sampler, ColorFormat colorFormat = ColorFormat.RGB)
        : base(with, height, 3, sampler)
    {
        this._data = data;
        this.ColorFormat = colorFormat;

        if (Data.Length != ((with * height) * 3)) throw new ArgumentException($"Data-Length {Data.Length} differs from the specified size {with}x{height} * 3 bytes ({with * height * 3}).");
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override Color GetColor(in ReadOnlySpan<byte> pixel)
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