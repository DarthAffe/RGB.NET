using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.Corsair.CorsairHeadsetRGBDevice" />.
/// </summary>
public class CorsairHeadsetRGBDeviceInfo : CorsairRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal CorsairHeadsetRGBDeviceInfo(_CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset)
        : base(RGBDeviceType.Headset, nativeInfo, ledCount, ledOffset)
    { }

    #endregion
}