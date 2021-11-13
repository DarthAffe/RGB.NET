// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.Corsair.CorsairMainboardRGBDevice" />.
/// </summary>
public class CorsairMainboardRGBDeviceInfo : CorsairRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Corsair.CorsairMainboardRGBDeviceInfo" />.
    /// </summary>
    /// <param name="deviceIndex">The index of the <see cref="T:RGB.NET.Devices.Corsair.CorsairMainboardRGBDevice" />.</param>
    /// <param name="nativeInfo">The native <see cref="T:RGB.NET.Devices.Corsair.Native._CorsairDeviceInfo" />-struct</param>
    internal CorsairMainboardRGBDeviceInfo(int deviceIndex, _CorsairDeviceInfo nativeInfo)
        : base(deviceIndex, RGBDeviceType.Mainboard, nativeInfo)
    { }

    #endregion
}