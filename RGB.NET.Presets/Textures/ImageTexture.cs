﻿using System;
using HPPH;
using RGB.NET.Core;
using RGB.NET.Presets.Extensions;

namespace RGB.NET.Presets.Textures;

/// <inheritdoc />
/// <summary>
/// Represents a texture drawing  an <see cref="IImage"/>.
/// </summary>
public sealed class ImageTexture : ITexture
{
    #region Properties & Fields

    private readonly IImage _image;

    /// <inheritdoc />
    public Size Size { get; }

    /// <inheritdoc />
    public Color this[Point point]
    {
        get
        {
            int x = (int)MathF.Round((Size.Width - 1) * point.X.Clamp(0, 1));
            int y = (int)MathF.Round((Size.Height - 1) * point.Y.Clamp(0, 1));

            return _image[x, y].ToColor();
        }
    }

    /// <inheritdoc />
    public Color this[Rectangle rectangle]
    {
        get
        {
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
    public Color this[int x, int y, int width, int height] => _image[x, y, width, height].Average().ToColor();

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageTexture" /> class.
    /// </summary>
    /// <param name="image">The image represented by the texture.</param>
    public ImageTexture(IImage image)
    {
        this._image = image;

        Size = new Size(image.Width, image.Height);
    }

    #endregion
}