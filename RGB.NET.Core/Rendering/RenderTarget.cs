// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace RGB.NET.Core;

/// <summary>
/// Represents a single target of a brush render.
/// </summary>
public readonly struct RenderTarget
{
    #region Properties & Fields

    /// <summary>
    /// Gets the target-<see cref="Core.Led"/>.
    /// </summary>
    public Led Led { get; }

    /// <summary>
    /// Gets the <see cref="Core.Rectangle"/> representing the area to render the target-<see cref="Core.Led"/>.
    /// </summary>
    public Rectangle Rectangle { get; }

    /// <summary>
    /// Gets the <see cref="Core.Point"/> representing the position to render the target-<see cref="Core.Led"/>.
    /// </summary>
    public Point Point => Rectangle.Center;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RenderTarget"/> class.
    /// </summary>
    /// <param name="led">The target-<see cref="Core.Led"/>.</param>
    /// <param name="rectangle">The <see cref="Core.Rectangle"/> representing the area to render the target-<see cref="Core.Led"/>.</param>
    public RenderTarget(Led led, Rectangle rectangle)
    {
        this.Led = led;
        this.Rectangle = rectangle;
    }

    #endregion
}