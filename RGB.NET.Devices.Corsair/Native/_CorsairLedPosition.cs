#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
/// <summary>
/// CUE-SDK: contains led id and position of led rectangle.Most of the keys are rectangular.
/// In case if key is not rectangular(like Enter in ISO / UK layout) it returns the smallest rectangle that fully contains the key
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal class _CorsairLedPosition
{
    /// <summary>
    /// CUE-SDK: identifier of led
    /// </summary>
    internal CorsairLedId LedId;

    /// <summary>
    /// CUE-SDK: values in mm
    /// </summary>
    internal double top;

    /// <summary>
    /// CUE-SDK: values in mm
    /// </summary>
    internal double left;

    /// <summary>
    /// CUE-SDK: values in mm
    /// </summary>
    internal double height;

    /// <summary>
    /// CUE-SDK: values in mm
    /// </summary>
    internal double width;
}