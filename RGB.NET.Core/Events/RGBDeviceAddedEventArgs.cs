using System;

namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Contains the detected device from <see cref="IRGBDeviceProvider" />
/// </summary>
public class RGBDeviceAddedEventArgs : EventArgs
{
    /// <summary>
    /// Newly detected device.
    /// </summary>
    public IRGBDevice Device { get; }

    /// <inheritdoc />
    public RGBDeviceAddedEventArgs(IRGBDevice device)
    {
        Device = device;
    }
}
