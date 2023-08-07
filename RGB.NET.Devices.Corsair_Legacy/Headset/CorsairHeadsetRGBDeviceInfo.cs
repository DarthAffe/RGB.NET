using RGB.NET.Core;
using RGB.NET.Devices.CorsairLegacy.Native;

namespace RGB.NET.Devices.CorsairLegacy;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.CorsairLegacy.CorsairHeadsetRGBDevice" />.
/// </summary>
public class CorsairHeadsetRGBDeviceInfo : CorsairRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Internal constructor of managed <see cref="T:RGB.NET.Devices.CorsairLegacy.CorsairHeadsetRGBDeviceInfo" />.
    /// </summary>
    /// <param name="deviceIndex">The index of the <see cref="T:RGB.NET.Devices.CorsairLegacy.CorsairHeadsetRGBDevice" />.</param>
    /// <param name="nativeInfo">The native <see cref="T:RGB.NET.Devices.CorsairLegacy.Native._CorsairDeviceInfo" />-struct</param>
    internal CorsairHeadsetRGBDeviceInfo(int deviceIndex, _CorsairDeviceInfo nativeInfo)
        : base(deviceIndex, RGBDeviceType.Headset, nativeInfo)
    { }

    #endregion
}