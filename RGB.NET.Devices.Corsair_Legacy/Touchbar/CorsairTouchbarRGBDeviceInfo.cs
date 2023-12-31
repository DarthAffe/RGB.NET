// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.CorsairLegacy.Native;

namespace RGB.NET.Devices.CorsairLegacy;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.CorsairLegacy.CorsairTouchbarRGBDevice" />.
/// </summary>
public class CorsairTouchbarRGBDeviceInfo : CorsairRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Internal constructor of managed <see cref="T:RGB.NET.Devices.CorsairLegacy.CorsairTouchbarRGBDeviceInfo" />.
    /// </summary>
    /// <param name="deviceIndex">The index of the <see cref="T:RGB.NET.Devices.CorsairLegacy.CorsairTouchbarRGBDevice" />.</param>
    /// <param name="nativeInfo">The native <see cref="T:RGB.NET.Devices.CorsairLegacy.Native._CorsairDeviceInfo" />-struct</param>
    internal CorsairTouchbarRGBDeviceInfo(int deviceIndex, _CorsairDeviceInfo nativeInfo)
        : base(deviceIndex, RGBDeviceType.Keypad, nativeInfo)
    { }

    #endregion
}