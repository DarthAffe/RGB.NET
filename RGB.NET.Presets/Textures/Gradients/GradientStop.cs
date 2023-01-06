// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

using RGB.NET.Core;

namespace RGB.NET.Presets.Textures.Gradients;

/// <summary>
/// Represents a stop on a gradient.
/// </summary>
public class GradientStop : AbstractBindable
{
    #region Properties & Fields

    private float _offset;
    /// <summary>
    /// Gets or sets the percentage offset to place this <see cref="GradientStop"/>. This should be inside the range of [0..1] but it's not necessary.
    /// </summary>
    public float Offset
    {
        get => _offset;
        set => SetProperty(ref _offset, value);
    }

    private Color _color;
    /// <summary>
    /// Gets or sets the <see cref="Color"/> of this <see cref="GradientStop"/>.
    /// </summary>
    public Color Color
    {
        get => _color;
        set => SetProperty(ref _color, value);
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="GradientStop"/> class.
    /// </summary>
    /// <param name="offset">The percentage offset to place this <see cref="GradientStop"/>.</param>
    /// <param name="color">The <see cref="Color"/> of the <see cref="GradientStop"/>.</param>
    public GradientStop(float offset, Color color)
    {
        this.Offset = offset;
        this.Color = color;
    }

    #endregion
}