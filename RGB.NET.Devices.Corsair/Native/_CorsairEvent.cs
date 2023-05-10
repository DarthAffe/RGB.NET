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
    /// iCUE-SDK: Anonymous union with fields:
    /// const CorsairDeviceConnectionStatusChangedEvent *deviceConnectionStatusChangedEvent - when id == CEI_DeviceConnectionStatusChangedEvent
    ///     contains valid pointer to structure with information about connected or disconnected device
    /// const CorsairKeyEvent *keyEvent - when id == CEI_KeyEvent
    ///     contains valid pointer to structure with information about pressed or released G, M or S button and device where this event happened
    /// </summary>
    internal CorsairEventUnion corsairEventUnion;
}

[StructLayout(LayoutKind.Sequential)]
internal sealed class CorsairEventUnion
{
    /// <summary>
    /// Points to <see cref="CorsairDeviceConnectionStatusChangedEvent"/> if _CorsairEvent's id is CEI_DeviceConnectionStatusChangedEvent,
    /// points to <see cref="CorsairKeyEvent"/> if _CorsairEvent's id is CEI_KeyEvent
    /// </summary>
    internal nint eventPointer;
}

