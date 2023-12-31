#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable NotAccessedField.Global

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming    
/// <summary>
/// iCUE-SDK: contains led id and position of led
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal sealed class _CorsairLedPosition
{
    /// <summary>
    /// iCUE-SDK: unique identifier of led
    /// </summary>
    internal uint id;

    /// <summary>
    /// iCUE-SDK: for keyboards, mice, mousemats, headset stands and memory modules values are
    /// </summary>
    internal double cx;

    /// <summary>
    /// iCUE-SDK: in mm, for DIY-devices, headsets and coolers values are in logical units
    /// </summary>
    internal double cy;
};