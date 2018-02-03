using System;
using RGB.NET.Core;

namespace RGB.NET.Brushes.Gradients
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a basic gradient.
    /// </summary>
    public interface IGradient : IDecoratable<IGradientDecorator>
    {
        /// <summary>
        /// Occurs if the <see cref="IGradient"/> is changed.
        /// </summary>
        event EventHandler GradientChanged;

        /// <summary>
        /// Gets the <see cref="Color"/> of the <see cref="IGradient"/> on the specified offset.
        /// </summary>
        /// <param name="offset">The percentage offset to take the <see cref="Color"/> from.</param>
        /// <returns>The <see cref="Color"/> at the specific offset.</returns>
        Color GetColor(double offset);

        /// <summary>
        /// Moves the <see cref="IGradient"/> by the provided offset.
        /// </summary>
        /// <param name="offset">The offset the <see cref="IGradient"/> should be moved.</param>
        void Move(double offset);
    }
}
