using RGB.NET.Brushes.Gradients;
using RGB.NET.Core;

namespace RGB.NET.Brushes
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a basic gradient-brush.
    /// </summary>
    public interface IGradientBrush : IBrush
    {
        /// <summary>
        /// Gets the <see cref="IGradient"/> used by this <see cref="IGradientBrush"/>.
        /// </summary>
        IGradient? Gradient { get; }
    }
}
