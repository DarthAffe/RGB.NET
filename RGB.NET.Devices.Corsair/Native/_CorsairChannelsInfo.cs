#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles

using System;
using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
/// <summary>
/// CUE-SDK: contains information about channels of the DIY-devices.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal class _CorsairChannelsInfo
{
    /// <summary>
    /// CUE-SDK: number of channels controlled by the device
    /// </summary>
    internal int channelsCount;

    /// <summary>
    /// CUE-SDK: array containing information about each separate channel of the DIY-device.
    /// Index of the channel in the array is same as index of the channel on the DIY-device.
    /// </summary>
    internal IntPtr channels;
}