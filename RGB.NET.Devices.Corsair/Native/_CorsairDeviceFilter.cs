#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
/// <summary>
/// iCUE-SDK: contains device search filter
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal class _CorsairDeviceFilter
{
    #region Properties & Fields

    /// <summary>
    /// iCUE-SDK: mask that describes device types, formed as logical “or” of CorsairDeviceType enum values
    /// </summary>
    internal CorsairDeviceType deviceTypeMask;

    #endregion

    #region Constructors

    public _CorsairDeviceFilter() { }

    public _CorsairDeviceFilter(CorsairDeviceType filter)
    {
        this.deviceTypeMask = filter;
    }

    #endregion
}
