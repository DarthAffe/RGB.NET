#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
/// <summary>
/// CUE-SDK: contains information about separate LED-device connected to the channel controlled by the DIY-device.
/// </summary>
[StructLayout(LayoutKind.Sequential)]

internal class _CorsairChannelDeviceInfo
{
    /// <summary>
    /// CUE-SDK: type of the LED-device
    /// </summary>
    internal CorsairChannelDeviceType type;

    /// <summary>
    /// CUE-SDK: number of LEDs controlled by LED-device.
    /// </summary>
    internal int deviceLedCount;
}