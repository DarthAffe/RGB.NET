#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable NotAccessedField.Global

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming    
/// <summary>
/// CUE-SDK: contains information about led and its color
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal class _CorsairLedColor
{
    /// <summary>
    /// CUE-SDK: identifier of LED to set
    /// </summary>
    internal int ledId;

    /// <summary>
    /// CUE-SDK: red   brightness[0..255]
    /// </summary>
    internal int r;

    /// <summary>
    /// CUE-SDK: green brightness[0..255]
    /// </summary>
    internal int g;

    /// <summary>
    /// CUE-SDK: blue  brightness[0..255]
    /// </summary>
    internal int b;
};