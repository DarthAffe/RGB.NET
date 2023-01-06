using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Layout;

/// <summary>
/// Represents a generic layout of a device.
/// </summary>
public interface IDeviceLayout
{
    /// <summary>
    /// Gets or sets the name of the <see cref="IDeviceLayout"/>.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Gets or sets the description of the <see cref="IDeviceLayout"/>.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Gets or sets the author of the <see cref="IDeviceLayout"/>.
    /// </summary>
    string? Author { get; }

    /// <summary>
    /// Gets or sets the <see cref="RGBDeviceType"/> of the <see cref="IDeviceLayout"/>.
    /// </summary>
    RGBDeviceType Type { get; }

    /// <summary>
    /// Gets or sets the vendor of the <see cref="IDeviceLayout"/>.
    /// </summary>
    string? Vendor { get; }

    /// <summary>
    /// Gets or sets the model of the <see cref="IDeviceLayout"/>.
    /// </summary>
    string? Model { get; }

    /// <summary>
    /// Gets or sets the <see cref="Core.Shape"/> of the <see cref="IDeviceLayout"/>.
    /// </summary>
    Shape Shape { get; }

    /// <summary>
    /// Gets or sets the width of the <see cref="IDeviceLayout"/>.
    /// </summary>
    float Width { get; }

    /// <summary>
    /// Gets or sets the height of the <see cref="IDeviceLayout"/>.
    /// </summary>
    float Height { get; }

    /// <summary>
    /// Gets or sets a list of <see cref="ILedLayout"/> representing all the <see cref="Led"/> of the <see cref="IDeviceLayout"/>.
    /// </summary>
    IEnumerable<ILedLayout> Leds { get; }

    /// <summary>
    /// Gets the the custom data associated with the device.
    /// </summary>
    object? CustomData { get; }
}