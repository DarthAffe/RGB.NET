#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable NotAccessedField.Global

using System.Runtime.InteropServices;
using RGB.NET.Devices.Corsair.Events;

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
    /// Points to <see cref="CorsairDeviceConnectionStatusChangedEvent"/> if _CorsairEvent's id is CEI_DeviceConnectionStatusChangedEvent,
    /// points to <see cref="CorsairKeyEvent"/> if _CorsairEvent's id is CEI_KeyEvent
    /// </summary>
    internal nint eventPointer;
}