using System;

namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Contains the removed device from <see cref="IRGBDeviceProvider" />
/// </summary>
public class RGBDeviceRemovedEventArgs : EventArgs
{
    /// <summary>
    /// Removed device.
    /// </summary>
    public IRGBDevice Device { get; }

    /// <inheritdoc />
    public RGBDeviceRemovedEventArgs(IRGBDevice device)
    {
        Device = device;
    }
}
