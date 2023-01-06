// ReSharper disable VirtualMemberNeverOverriden.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable VirtualMemberNeverOverridden.Global

using System.Collections.Generic;

namespace RGB.NET.Core;

/// <inheritdoc cref="AbstractDecoratable{T}" />
/// <inheritdoc cref="IBrush" />
/// <summary>
/// Represents a basic brush.
/// </summary>
public abstract class AbstractBrush : AbstractDecoratable<IBrushDecorator>, IBrush
{
    #region Properties & Fields

    /// <inheritdoc />
    public bool IsEnabled { get; set; } = true;

    /// <inheritdoc />
    public RenderMode CalculationMode { get; set; } = RenderMode.Relative;

    /// <inheritdoc />
    public float Brightness { get; set; }

    /// <inheritdoc />
    public float Opacity { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractBrush"/> class.
    /// </summary>
    /// <param name="brightness">The overall percentage brightness of the brush. (default: 1.0)</param>
    /// <param name="opacity">The overall percentage opacity of the brush. (default: 1.0)</param>
    protected AbstractBrush(float brightness = 1, float opacity = 1)
    {
        this.Brightness = brightness;
        this.Opacity = opacity;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Renders the brush to the specified list of <see cref="RenderTarget"/>.
    /// </summary>
    /// <param name="rectangle">The bounding box the brush is rendered in.</param>
    /// <param name="renderTargets">The targets to render to.</param>
    /// <returns>A enumerable containing the rendered <see cref="Color"/> for each <see cref="RenderTarget"/>.</returns>
    public virtual IEnumerable<(RenderTarget renderTarget, Color color)> Render(Rectangle rectangle, IEnumerable<RenderTarget> renderTargets)
    {
        foreach (RenderTarget renderTarget in renderTargets)
        {
            Color color = GetColorAtPoint(rectangle, renderTarget);
            ApplyDecorators(rectangle, renderTarget, ref color);
            FinalizeColor(ref color);
            yield return (renderTarget, color);
        }
    }

    /// <summary>
    /// Applies all attached and enabled decorators to the brush.
    /// </summary>
    /// <param name="rectangle">The rectangle in which the brush should be drawn.</param>
    /// <param name="renderTarget">The target (key/point) from which the color should be taken.</param>
    /// <param name="color">The <see cref="Color"/> to be modified.</param>
    protected virtual void ApplyDecorators(in Rectangle rectangle, in RenderTarget renderTarget, ref Color color)
    {
        if (Decorators.Count == 0) return;

        lock (Decorators)
            foreach (IBrushDecorator decorator in Decorators)
                if (decorator.IsEnabled)
                    decorator.ManipulateColor(rectangle, renderTarget, ref color);
    }

    /// <summary>
    /// Gets the color at an specific point assuming the brush is drawn into the specified rectangle.
    /// </summary>
    /// <param name="rectangle">The rectangle in which the brush should be drawn.</param>
    /// <param name="renderTarget">The target (key/point) from which the color should be taken.</param>
    /// <returns>The color at the specified point.</returns>
    protected abstract Color GetColorAtPoint(in Rectangle rectangle, in RenderTarget renderTarget);

    /// <summary>
    /// Finalizes the color by appliing the overall brightness and opacity.<br/>
    /// </summary>
    /// <param name="color">The color to finalize.</param>
    /// <returns>The finalized color.</returns>
    protected virtual void FinalizeColor(ref Color color)
    {
        // Since we use HSV to calculate there is no way to make a color 'brighter' than 100%
        // Be carefull with the naming: Since we use HSV the correct term is 'value' but outside we call it 'brightness'
        // THIS IS NOT A HSB CALCULATION!!!
        if (Brightness < 1)
            color = color.MultiplyHSV(value: Brightness.Clamp(0, 1));

        if (Opacity < 1)
            color = color.MultiplyA(Opacity.Clamp(0, 1));
    }

    #endregion
}