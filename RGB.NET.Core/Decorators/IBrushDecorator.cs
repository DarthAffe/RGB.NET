namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents a <see cref="T:RGB.NET.Core.IDecorator" /> decorating a <see cref="T:RGB.NET.Core.IBrush" />.
/// </summary>
public interface IBrushDecorator : IDecorator
{
    /// <summary>
    /// Decorator-Method called by the <see cref="IBrush"/>.
    /// </summary>
    /// <param name="rectangle">The rectangle in which the <see cref="IBrush"/> should be drawn.</param>
    /// <param name="renderTarget">The target (key/point) from which the <see cref="Color"/> should be taken.</param>
    /// <param name="color">The <see cref="Color"/> to be modified.</param>
    void ManipulateColor(in Rectangle rectangle, in RenderTarget renderTarget, ref Color color);
}