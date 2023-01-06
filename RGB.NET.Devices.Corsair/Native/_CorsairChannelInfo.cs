#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles

using System;
using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
/// <summary>
/// CUE-SDK: contains information about separate channel of the DIY-device.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal class _CorsairChannelInfo
{
    /// <summary>
    /// CUE-SDK: total number of LEDs connected to the channel;
    /// </summary>
    internal int totalLedsCount;

    /// <summary>
    /// CUE-SDK: number of LED-devices (fans, strips, etc.) connected to the channel which is controlled by the DIY device
    /// </summary>
    internal int devicesCount;

    /// <summary>
    /// CUE-SDK: array containing information about each separate LED-device connected to the channel controlled by the DIY device.
    /// Index of the LED-device in array is same as the index of the LED-device connected to the DIY-device.
    /// </summary>
    internal IntPtr devices;
}