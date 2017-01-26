using RGB.NET.Core;

namespace RGB.NET.Brushes.Gradients
{
    /// <summary>
    /// Represents a basic gradient.
    /// </summary>
    public interface IGradient
    {
        /// <summary>
        /// Gets the <see cref="Color"/> of the <see cref="IGradient"/> on the specified offset.
        /// </summary>
        /// <param name="offset">The percentage offset to take the <see cref="Color"/> from.</param>
        /// <returns>The <see cref="Color"/> at the specific offset.</returns>
        Color GetColor(double offset);
    }
}
