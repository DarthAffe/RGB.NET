#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles

using System;
using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
/// <summary>
/// CUE-SDK: contains information about device
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal class _CorsairDeviceInfo
{
    /// <summary>
    /// CUE-SDK: enum describing device type
    /// </summary>
    internal CorsairDeviceType type;

    /// <summary>
    /// CUE-SDK: null - terminated device model(like “K95RGB”)
    /// </summary>
    internal IntPtr model;

    /// <summary>
    /// CUE-SDK: enum describing physical layout of the keyboard or mouse
    /// </summary>
    internal int physicalLayout;

    /// <summary>
    /// CUE-SDK: enum describing logical layout of the keyboard as set in CUE settings
    /// </summary>
    internal int logicalLayout;

    /// <summary>
    /// CUE-SDK: mask that describes device capabilities, formed as logical “or” of CorsairDeviceCaps enum values
    /// </summary>
    internal int capsMask;

    /// <summary>
    /// CUE-SDK: number of controllable LEDs on the device
    /// </summary>
    internal int ledsCount;

    /// <summary>
    /// CUE-SDK: structure that describes channels of the DIY-devices
    /// </summary>
    internal _CorsairChannelsInfo? channels;

    /// <summary>
    /// CUE-SDK: null-terminated string that contains unique device identifier that uniquely identifies device at least within session
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    internal string? deviceId;
}