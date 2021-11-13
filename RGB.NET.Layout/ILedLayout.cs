using RGB.NET.Core;

namespace RGB.NET.Layout;

/// <summary>
/// Represents a generic layour of a LED.
/// </summary>
public interface ILedLayout
{
    /// <summary>
    /// Gets or sets the Id of the <see cref="LedLayout"/>.
    /// </summary>
    string? Id { get; }

    /// <summary>
    /// Gets or sets the <see cref="RGB.NET.Core.Shape"/> of the <see cref="LedLayout"/>.
    /// </summary>
    Shape Shape { get; }

    /// <summary>
    /// Gets or sets the vecor-data representing a custom-shape of the <see cref="LedLayout"/>.
    /// </summary>
    string? ShapeData { get; }

    /// <summary>
    /// Gets the x-position of the <see cref="LedLayout"/>.
    /// </summary>
    float X { get; }

    /// <summary>
    /// Gets the y-position of the <see cref="LedLayout"/>.
    /// </summary>
    float Y { get; }

    /// <summary>
    /// Gets the width of the <see cref="LedLayout"/>.
    /// </summary>
    float Width { get; }

    /// <summary>
    /// Gets the height of the <see cref="LedLayout"/>.
    /// </summary>
    float Height { get; }

    /// <summary>
    /// Gets the the custom data associated with the LED.
    /// </summary>
    object? CustomData { get; }
}