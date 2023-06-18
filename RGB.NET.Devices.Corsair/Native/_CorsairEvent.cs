using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
/// <summary>
/// iCUE-SDK: contains information about event id and event data
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal sealed class _CorsairEvent
{
    /// <summary>
    /// iCUE-SDK: event identifier
    /// </summary>
    internal CorsairEventId id;

    /// <summary>
    /// Points to <see cref="_CorsairDeviceConnectionStatusChangedEvent"/> if _CorsairEvent's id is CEI_DeviceConnectionStatusChangedEvent,
    /// points to <see cref="_CorsairKeyEvent"/> if _CorsairEvent's id is CEI_KeyEvent
    /// </summary>
    internal nint eventPointer;
}
