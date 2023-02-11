#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
/// <summary>
/// iCUE-SDK: contains information about device
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal class _CorsairDeviceInfo
{
    /// <summary>
    /// iCUE-SDK: enum describing device type
    /// </summary>
    internal CorsairDeviceType type;

    /// <summary>
    /// iCUE-SDK: null terminated Unicode string that contains unique device identifier serial number. Can be empty, if serial number is not available for the device
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = _CUESDK.CORSAIR_STRING_SIZE_M)]
    internal string? id;

    /// <summary>
    /// iCUE-SDK: null terminated Unicode string that contains device serial number. Can be empty, if serial number is not available for the device
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = _CUESDK.CORSAIR_STRING_SIZE_M)]
    internal string? serial;

    /// <summary>
    /// iCUE-SDK: null terminated Unicode string that contains device model (like “K95RGB”)
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = _CUESDK.CORSAIR_STRING_SIZE_M)]
    internal string? model;

    /// <summary>
    /// iCUE-SDK: number of controllable LEDs on the device
    /// </summary>
    internal int ledCount;

    /// <summary>
    /// iCUE-SDK: number of channels controlled by the device
    /// </summary>
    internal int channelCount;
}