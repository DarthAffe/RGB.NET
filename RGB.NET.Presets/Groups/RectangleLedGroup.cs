// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Presets.Groups;

/// <inheritdoc />
/// <summary>
/// Represents a <see cref="T:RGB.NET.Presets.Groups.RectangleLedGroup" /> containing <see cref="T:RGB.NET.Core.Led" /> which physically lay inside a <see cref="T:RGB.NET.Core.Rectangle" />.
/// </summary>
public class RectangleLedGroup : AbstractLedGroup
{
    #region Properties & Fields

    private IList<Led>? _ledCache;

    private Rectangle _rectangle;
    /// <summary>
    /// Gets or sets the <see cref="Core.Rectangle"/> the <see cref="Led"/>  should be taken from.
    /// </summary>
    public Rectangle Rectangle
    {
        get => _rectangle;
        set
        {
            if (SetProperty(ref _rectangle, value))
                InvalidateCache();
        }
    }

    private double _minOverlayPercentage;
    /// <summary>
    /// Gets or sets the minimal percentage overlay a <see cref="Led"/>  must have with the <see cref="Core.Rectangle" /> to be taken into the <see cref="RectangleLedGroup"/>.
    /// </summary>
    public double MinOverlayPercentage
    {
        get => _minOverlayPercentage;
        set
        {
            if (SetProperty(ref _minOverlayPercentage, value))
                InvalidateCache();
        }
    }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Groups.RectangleLedGroup" /> class.
    /// </summary>
    /// <param name="surface">Specifies the surface to attach this group to or <c>null</c> if the group should not be attached on creation.</param>
    /// <param name="fromLed">They first <see cref="T:RGB.NET.Core.Led" />  to calculate the <see cref="T:RGB.NET.Core.Rectangle" /> of this <see cref="T:RGB.NET.Groups.RectangleLedGroup" /> from.</param>
    /// <param name="toLed">They second <see cref="T:RGB.NET.Core.Led" />  to calculate the <see cref="T:RGB.NET.Core.Rectangle" /> of this <see cref="T:RGB.NET.Groups.RectangleLedGroup" /> from.</param>
    /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a <see cref="T:RGB.NET.Core.Led" />  must have with the <see cref="P:RGB.NET.Groups.RectangleLedGroup.Rectangle" /> to be taken into the <see cref="T:RGB.NET.Groups.RectangleLedGroup" />. (default: 0.5)</param>
    public RectangleLedGroup(RGBSurface? surface, Led fromLed, Led toLed, double minOverlayPercentage = 0.5)
        : this(surface, new Rectangle(fromLed.Boundary, toLed.Boundary), minOverlayPercentage)
    { }

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Groups.RectangleLedGroup" /> class.
    /// </summary>
    /// <param name="surface">Specifies the surface to attach this group to or <c>null</c> if the group should not be attached on creation.</param>
    /// <param name="fromPoint">They first point to calculate the <see cref="T:RGB.NET.Core.Rectangle" /> of this <see cref="T:RGB.NET.Groups.RectangleLedGroup" /> from.</param>
    /// <param name="toPoint">They second point to calculate the <see cref="T:RGB.NET.Core.Rectangle" /> of this <see cref="T:RGB.NET.Groups.RectangleLedGroup" /> from.</param>
    /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a <see cref="T:RGB.NET.Core.Led" />  must have with the <see cref="P:RGB.NET.Groups.RectangleLedGroup.Rectangle" /> to be taken into the <see cref="T:RGB.NET.Groups.RectangleLedGroup" />. (default: 0.5)</param>
    public RectangleLedGroup(RGBSurface? surface, Point fromPoint, Point toPoint, double minOverlayPercentage = 0.5)
        : this(surface, new Rectangle(fromPoint, toPoint), minOverlayPercentage)
    { }

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Groups.RectangleLedGroup" /> class.
    /// </summary>
    /// <param name="surface">Specifies the surface to attach this group to or <c>null</c> if the group should not be attached on creation.</param>
    /// <param name="rectangle">The <see cref="T:RGB.NET.Core.Rectangle" /> of this <see cref="T:RGB.NET.Groups.RectangleLedGroup" />.</param>
    /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a <see cref="T:RGB.NET.Core.Led" />  must have with the <see cref="P:RGB.NET.Groups.RectangleLedGroup.Rectangle" /> to be taken into the <see cref="T:RGB.NET.Groups.RectangleLedGroup" />. (default: 0.5)</param>
    public RectangleLedGroup(RGBSurface? surface, Rectangle rectangle, double minOverlayPercentage = 0.5)
        : base(surface)
    {
        this.Rectangle = rectangle;
        this.MinOverlayPercentage = minOverlayPercentage;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void OnAttach()
    {
        base.OnAttach();
        Surface!.SurfaceLayoutChanged += RGBSurfaceOnSurfaceLayoutChanged;
    }

    /// <inheritdoc />
    public override void OnDetach()
    {
        Surface!.SurfaceLayoutChanged -= RGBSurfaceOnSurfaceLayoutChanged;

        base.OnDetach();
    }

    private void RGBSurfaceOnSurfaceLayoutChanged(SurfaceLayoutChangedEventArgs args) => InvalidateCache();

    /// <inheritdoc />
    /// <summary>
    /// Gets a list containing all <see cref="T:RGB.NET.Core.Led" /> of this <see cref="T:RGB.NET.Presets.Groups.RectangleLedGroup" />.
    /// </summary>
    /// <returns>The list containing all <see cref="T:RGB.NET.Core.Led" /> of this <see cref="T:RGB.NET.Presets.Groups.RectangleLedGroup" />.</returns>
    protected override IEnumerable<Led> GetLeds() => _ledCache ??= (Surface?.Leds.Where(led => led.AbsoluteBoundary.CalculateIntersectPercentage(Rectangle) >= MinOverlayPercentage).ToList() ?? new List<Led>());

    private void InvalidateCache() => _ledCache = null;

    #endregion
}