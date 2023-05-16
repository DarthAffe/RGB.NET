#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable NotAccessedField.Global

using System.Runtime.InteropServices;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair.Events;

/// <summary>
/// iCUE-SDK: contains information about device that was connected or disconnected
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal sealed class CorsairKeyEvent
{
    /// <summary>
    /// iCUE-SDK: null terminated Unicode string that contains unique device identifier
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = _CUESDK.CORSAIR_STRING_SIZE_M)]
    internal string? deviceId;
    
    /// <summary>
    /// iCUE-SDK: G, M or S key that was pressed/released
    /// </summary>
    internal uint keyId;
    
    /// <summary>
    /// iCUE-SDK: true if pressed, false if released
    /// </summary>
    [MarshalAs(UnmanagedType.U1)]
    internal bool isPressed;
}