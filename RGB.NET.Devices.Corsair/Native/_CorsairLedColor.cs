#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable NotAccessedField.Global

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming    
/// <summary>
/// iCUE-SDK: contains information about led and its color
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct _CorsairLedColor
{
    #region Properties & Fields

    /// <summary>
    /// iCUE-SDK: identifier of LED to set
    /// </summary>
    internal CorsairLedId ledId;

    /// <summary>
    /// iCUE-SDK: red   brightness[0..255]
    /// </summary>
    internal byte r;

    /// <summary>
    /// iCUE-SDK: green brightness[0..255]
    /// </summary>
    internal byte g;

    /// <summary>
    /// iCUE-SDK: blue  brightness[0..255]
    /// </summary>
    internal byte b;

    /// <summary>
    /// iCUE-SDK: alpha channel [0..255]. The opacity of the color from 0 for completely translucent to 255 for opaque
    /// </summary>
    internal byte a;

    #endregion

    #region Constructors

    public _CorsairLedColor() { }

    public _CorsairLedColor(CorsairLedId ledId, byte r, byte g, byte b, byte a)
    {
        this.ledId = ledId;
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    #endregion
};