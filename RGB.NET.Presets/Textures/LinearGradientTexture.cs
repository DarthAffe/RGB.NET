// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Presets.Helper;
using RGB.NET.Presets.Textures.Gradients;

namespace RGB.NET.Presets.Textures;

/// <inheritdoc />
/// <summary>
/// Represents a texture drawing a linear gradient.
/// </summary>
public sealed class LinearGradientTexture : AbstractGradientTexture
{
    #region Properties & Fields

    private Point _startPoint = new(0, 0.5f);
    /// <summary>
    /// Gets or sets the start <see cref="Point"/> (as percentage in the range [0..1]) of the <see cref="IGradient"/> drawn by this <see cref="LinearGradientTexture"/>. (default: 0.0, 0.5)
    /// </summary>
    public Point StartPoint
    {
        get => _startPoint;
        set => SetProperty(ref _startPoint, value);
    }

    private Point _endPoint = new(1, 0.5f);
    /// <summary>
    /// Gets or sets the end <see cref="Point"/>  (as percentage in the range [0..1]) of the <see cref="IGradient"/> drawn by this <see cref="LinearGradientTexture"/>. (default: 1.0, 0.5)
    /// </summary>
    public Point EndPoint
    {
        get => _endPoint;
        set => SetProperty(ref _endPoint, value);
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Presets.Textures.LinearGradientTexture" /> class.
    /// </summary>
    /// <param name="size">The size of the texture.</param>
    /// <param name="gradient">The <see cref="T:RGB.NET.Presets.Gradients.IGradient" /> drawn by this <see cref="T:RGB.NET.Presets.Textures.LinearGradientTexture" />.</param>
    public LinearGradientTexture(Size size, IGradient gradient)
        : base(size, gradient)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Presets.Textures.LinearGradientTexture" /> class.
    /// </summary>
    /// <param name="size">The size of the texture.</param>
    /// <param name="gradient">The <see cref="T:RGB.NET.Presets.Gradients.IGradient" /> drawn by this <see cref="T:RGB.NET.Presets.Textures.LinearGradientTexture" />.</param>
    /// <param name="startPoint">The start <see cref="T:RGB.NET.Core.Point" /> (as percentage in the range [0..1]).</param>
    /// <param name="endPoint">The end <see cref="T:RGB.NET.Core.Point" /> (as percentage in the range [0..1]).</param>
    public LinearGradientTexture(Size size, IGradient gradient, Point startPoint, Point endPoint)
        : base(size, gradient)
    {
        this.StartPoint = startPoint;
        this.EndPoint = endPoint;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override Color GetColor(in Point point)
    {
        float offset = GradientHelper.CalculateLinearGradientOffset(StartPoint, EndPoint, point);
        return Gradient.GetColor(offset);
    }

    #endregion
}