#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned

using System;
using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
/// <summary>
/// CUE-SDK: contains number of leds and arrays with their positions
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal class _CorsairLedPositions
{
    /// <summary>
    /// CUE-SDK: integer value.Number of elements in following array
    /// </summary>
    internal int numberOfLed;

    /// <summary>
    /// CUE-SDK: array of led positions
    /// </summary>
    internal IntPtr pLedPosition;
}