using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.Corsair.CorsairHeadsetStandRGBDevice" />.
/// </summary>
public class CorsairHeadsetStandRGBDeviceInfo : CorsairRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal CorsairHeadsetStandRGBDeviceInfo(_CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset)
        : base(RGBDeviceType.HeadsetStand, nativeInfo, ledCount, ledOffset)
    { }

    #endregion
}