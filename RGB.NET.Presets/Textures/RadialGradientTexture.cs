// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Presets.Helper;
using RGB.NET.Presets.Textures.Gradients;

namespace RGB.NET.Presets.Textures;

/// <inheritdoc />
/// <summary>
/// Represents a texture drawing a radial gradient around a center point.
/// </summary>
public sealed class RadialGradientTexture : AbstractGradientTexture
{
    #region Properties & Fields

    private float _referenceDistance = GradientHelper.CalculateDistance(new Point(0.5f, 0.5f), new Point(0, 0));

    private Point _center = new(0.5f, 0.5f);
    /// <summary>
    /// Gets or sets the center <see cref="Point"/> (as percentage in the range [0..1]) around which the <see cref="RadialGradientTexture"/> should be drawn. (default: 0.5, 0.5)
    /// </summary>
    public Point Center
    {
        get => _center;
        set
        {
            if (SetProperty(ref _center, value))
                CalculateReferenceDistance();
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Presets.Textures.RadialGradientTexture" /> class.
    /// </summary>
    /// <param name="size">The size of the texture.</param>
    /// <param name="gradient">The gradient drawn by the brush.</param>
    public RadialGradientTexture(Size size, IGradient gradient)
        : base(size, gradient)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Presets.Textures.RadialGradientTexture" /> class.
    /// </summary>
    /// <param name="size">The size of the texture.</param>
    /// <param name="gradient">The gradient drawn by the brush.</param>
    /// <param name="center">The center point (as percentage in the range [0..1]).</param>
    public RadialGradientTexture(Size size, IGradient gradient, Point center)
        : base(size, gradient)
    {
        this.Center = center;
    }

    #endregion

    #region Methods

    private void CalculateReferenceDistance()
    {
        float referenceX = Center.X < 0.5f ? 1 : 0;
        float referenceY = Center.Y < 0.5f ? 1 : 0;
        _referenceDistance = GradientHelper.CalculateDistance(new Point(referenceX, referenceY), Center);
    }

    /// <inheritdoc />
    protected override Color GetColor(in Point point)
    {
        float distance = GradientHelper.CalculateDistance(point, Center);
        float offset = distance / _referenceDistance;
        return Gradient.GetColor(offset);
    }

    #endregion
}