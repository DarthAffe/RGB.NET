// ReSharper disable EventNeverSubscribedTo.Global

using System;
using RGB.NET.Core;
using RGB.NET.Presets.Decorators;

namespace RGB.NET.Presets.Textures.Gradients;

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
    Color GetColor(float offset);

    /// <summary>
    /// Moves the <see cref="IGradient"/> by the provided offset.
    /// </summary>
    /// <param name="offset">The offset the <see cref="IGradient"/> should be moved.</param>
    void Move(float offset);
}