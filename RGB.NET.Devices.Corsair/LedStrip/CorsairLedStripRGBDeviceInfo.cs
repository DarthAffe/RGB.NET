// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.Corsair.CorsairLedStripRGBDevice" />.
/// </summary>
public class CorsairLedStripRGBDeviceInfo : CorsairRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal CorsairLedStripRGBDeviceInfo(_CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset)
        : base(RGBDeviceType.LedStripe, nativeInfo, ledCount, ledOffset)
    { }

    /// <inheritdoc />
    internal CorsairLedStripRGBDeviceInfo(_CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset, string modelName)
        : base(RGBDeviceType.LedStripe, nativeInfo, ledCount, ledOffset, modelName)
    { }

    #endregion
}