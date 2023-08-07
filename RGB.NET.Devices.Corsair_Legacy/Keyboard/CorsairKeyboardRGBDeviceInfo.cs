// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.CorsairLegacy.Native;

namespace RGB.NET.Devices.CorsairLegacy;

/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.CorsairLegacy.CorsairKeyboardRGBDevice" />.
/// </summary>
public class CorsairKeyboardRGBDeviceInfo : CorsairRGBDeviceInfo, IKeyboardDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public KeyboardLayoutType Layout { get; }

    /// <summary>
    /// Gets the physical layout of the keyboard.
    /// </summary>
    public CorsairPhysicalKeyboardLayout PhysicalLayout { get; }

    /// <summary>
    /// Gets the logical layout of the keyboard as set in CUE settings.
    /// </summary>
    public CorsairLogicalKeyboardLayout LogicalLayout { get; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Internal constructor of managed <see cref="T:RGB.NET.Devices.CorsairLegacy.CorsairKeyboardRGBDeviceInfo" />.
    /// </summary>
    /// <param name="deviceIndex">The index of the <see cref="T:RGB.NET.Devices.CorsairLegacy.CorsairKeyboardRGBDevice" />.</param>
    /// <param name="nativeInfo">The native <see cref="T:RGB.NET.Devices.CorsairLegacy.Native._CorsairDeviceInfo" />-struct</param>
    internal CorsairKeyboardRGBDeviceInfo(int deviceIndex, _CorsairDeviceInfo nativeInfo)
        : base(deviceIndex, RGBDeviceType.Keyboard, nativeInfo)
    {
        this.PhysicalLayout = (CorsairPhysicalKeyboardLayout)nativeInfo.physicalLayout;
        this.LogicalLayout = (CorsairLogicalKeyboardLayout)nativeInfo.logicalLayout;
        this.Layout = PhysicalLayout switch
        {
            CorsairPhysicalKeyboardLayout.US => KeyboardLayoutType.ANSI,
            CorsairPhysicalKeyboardLayout.UK => KeyboardLayoutType.ISO,
            CorsairPhysicalKeyboardLayout.BR => KeyboardLayoutType.ABNT,
            CorsairPhysicalKeyboardLayout.JP => KeyboardLayoutType.JIS,
            CorsairPhysicalKeyboardLayout.KR => KeyboardLayoutType.KS,
            _ => KeyboardLayoutType.Unknown
        };
    }

    #endregion
}