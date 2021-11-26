// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ReturnTypeCanBeEnumerable.Global

using System.Collections.Generic;

namespace RGB.NET.Core;

/// <summary>
/// Represents a basic brush.
/// </summary>
public interface IBrush : IDecoratable<IBrushDecorator>
{
    /// <summary>
    /// Gets or sets if the <see cref="IBrush"/> is enabled and will be drawn on an update.
    /// </summary>
    bool IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the calculation mode used for the rectangle/points used for color-selection in brushes.
    /// </summary>
    RenderMode CalculationMode { get; set; }

    /// <summary>
    /// Gets or sets the overall percentage brightness of the <see cref="IBrush"/>.
    /// </summary>
    float Brightness { get; set; }

    /// <summary>
    /// Gets or sets the overall percentage opacity of the <see cref="IBrush"/>.
    /// </summary>
    float Opacity { get; set; }

    /// <summary>
    /// Performs the render pass of the <see cref="IBrush"/> and calculates the raw <see cref="Color"/> for all requested <see cref="RenderTarget"/>.
    /// </summary>
    /// <param name="rectangle">The <see cref="Rectangle"/> in which the brush should be drawn.</param>
    /// <param name="renderTargets">The <see cref="RenderTarget"/> (keys/points) of which the color should be calculated.</param>
    IEnumerable<(RenderTarget renderTarget, Color color)> Render(Rectangle rectangle, IEnumerable<RenderTarget> renderTargets);
}