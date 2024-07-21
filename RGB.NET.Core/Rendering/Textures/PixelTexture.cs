﻿// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Runtime.CompilerServices;

namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents a texture made of pixels (like a common image).
/// </summary>
/// <typeparam name="T">The type of the pixels.</typeparam>
public abstract class PixelTexture<T> : ITexture
    where T : unmanaged
{
    #region Properties & Fields

    /// <summary>
    /// Gets the underlying pixel data.
    /// </summary>
    protected abstract ReadOnlySpan<T> Data { get; }

    /// <summary>
    /// Gets the amount of data-entries per pixel.
    /// </summary>
    protected int DataPerPixel { get; }

    /// <summary>
    /// Gets the stride of the data.
    /// </summary>
    protected int Stride { get; }

    /// <summary>
    /// Gets or sets the sampler used to get the color of a region.
    /// </summary>
    public ISampler<T> Sampler { get; set; }

    /// <inheritdoc />
    public Size Size { get; }

    /// <inheritdoc />
    public virtual Color this[Point point]
    {
        get
        {
            if (Data.Length == 0) return Color.Transparent;

            int x = (int)MathF.Round((Size.Width - 1) * point.X.Clamp(0, 1));
            int y = (int)MathF.Round((Size.Height - 1) * point.Y.Clamp(0, 1));
            return GetColor(GetPixelData(x, y));
        }
    }

    /// <inheritdoc />
    public virtual Color this[Rectangle rectangle]
    {
        get
        {
            if (Data.Length == 0) return Color.Transparent;

            int x = (int)MathF.Round((Size.Width - 1) * rectangle.Location.X.Clamp(0, 1));
            int y = (int)MathF.Round((Size.Height - 1) * rectangle.Location.Y.Clamp(0, 1));
            int width = (int)MathF.Round(Size.Width * rectangle.Size.Width.Clamp(0, 1));
            int height = (int)MathF.Round(Size.Height * rectangle.Size.Height.Clamp(0, 1));

            if ((width == 0) && (rectangle.Size.Width > 0)) width = 1;
            if ((height == 0) && (rectangle.Size.Height > 0)) height = 1;

            return this[x, y, width, height];
        }
    }

    /// <summary>
    /// Gets the sampled color inside the specified region.
    /// </summary>
    /// <param name="x">The x-location of the region.</param>
    /// <param name="y">The y-location of the region.</param>
    /// <param name="width">The with of the region.</param>
    /// <param name="height">The height of the region.</param>
    /// <returns>The sampled color.</returns>
    public virtual Color this[int x, int y, int width, int height]
    {
        get
        {
            if (Data.Length == 0) return Color.Transparent;

            if ((width == 0) || (height == 0)) return Color.Transparent;
            if ((width == 1) && (height == 1)) return GetColor(GetPixelData(x, y));

            SamplerInfo<T> samplerInfo = new(x, y, width, height, Stride, DataPerPixel, Data);

            Span<T> pixelData = stackalloc T[DataPerPixel];
            Sampler.Sample(samplerInfo, pixelData);

            return GetColor(pixelData);
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PixelTexture{T}" /> class.
    /// </summary>
    /// <param name="with">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="dataPerPixel">The amount of data-entries per pixel.</param>
    /// <param name="sampler">The sampler used to get the color of a region.</param>
    /// <param name="stride">The stride of the data or -1 if the width should be used.</param>
    public PixelTexture(int with, int height, int dataPerPixel, ISampler<T> sampler, int stride = -1)
    {
        this.Stride = stride == -1 ? with : stride;
        this.DataPerPixel = dataPerPixel;
        this.Sampler = sampler;

        Size = new Size(with, height);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Converts the pixel-data to a color.
    /// </summary>
    /// <param name="pixel">The pixel-data to convert.</param>
    /// <returns>The color represented by the specified pixel-data.</returns>
    protected abstract Color GetColor(ReadOnlySpan<T> pixel);

    /// <summary>
    /// Gets the pixel-data at the specified location.
    /// </summary>
    /// <param name="x">The x-location.</param>
    /// <param name="y">The y-location.</param>
    /// <returns>The pixel-data on the specified location.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ReadOnlySpan<T> GetPixelData(int x, int y) => Data.Slice((y * Stride) + x, DataPerPixel);

    #endregion
}

/// <inheritdoc />
/// <summary>
/// Represents a texture made of color-pixels.
/// </summary>
public sealed class PixelTexture : PixelTexture<Color>
{
    #region Properties & Fields

    private readonly Color[] _data;

    /// <inheritdoc />
    protected override ReadOnlySpan<Color> Data => _data;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PixelTexture" /> class.
    /// A <see cref="AverageColorSampler"/> is used.
    /// </summary>
    /// <param name="with">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="data">The pixel-data of the texture.</param>
    public PixelTexture(int with, int height, Color[] data)
        : this(with, height, data, new AverageColorSampler())
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PixelTexture" /> class.
    /// </summary>
    /// <param name="with">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="data">The pixel-data of the texture.</param>
    /// <param name="sampler">The sampler used to get the color of a region.</param>
    public PixelTexture(int with, int height, Color[] data, ISampler<Color> sampler)
        : base(with, height, 1, sampler)
    {
        this._data = data;

        if (Data.Length != (with * height)) throw new ArgumentException($"Data-Length {Data.Length} differs from the specified size {with}x{height} ({with * height}).");
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Color GetColor(ReadOnlySpan<Color> pixel) => pixel[0];

    #endregion
}