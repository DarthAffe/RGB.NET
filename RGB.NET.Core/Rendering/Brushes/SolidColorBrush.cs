// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents a brush drawing only a single color.
/// </summary>
public class SolidColorBrush : AbstractBrush
{
    #region Properties & Fields

    private Color _color;
    /// <summary>
    /// Gets or sets the <see cref="Color"/> drawn by this <see cref="SolidColorBrush"/>.
    /// </summary>
    public Color Color
    {
        get => _color;
        set => SetProperty(ref _color, value);
    }

    #endregion

    #region Constructors
        
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Core.SolidColorBrush" /> class.
    /// </summary>
    /// <param name="color">The <see cref="P:RGB.NET.Core.SolidColorBrush.Color" /> drawn by this <see cref="T:RGB.NET.Core.SolidColorBrush" />.</param>
    public SolidColorBrush(Color color)
    {
        this.Color = color;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override Color GetColorAtPoint(in Rectangle rectangle, in RenderTarget renderTarget) => Color;

    #endregion

    #region Operators

    /// <summary>
    /// Converts a <see cref="Color" /> to a <see cref="SolidColorBrush" />.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert.</param>
    public static explicit operator SolidColorBrush(Color color) => new(color);

    /// <summary>
    /// Converts a <see cref="SolidColorBrush" /> to a <see cref="Color" />.
    /// </summary>
    /// <param name="brush">The <see cref="Color"/> to convert.</param>
    public static implicit operator Color(SolidColorBrush brush) => brush.Color;

    #endregion
}