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
    internal CorsairMainboardRGBDeviceInfo(_CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset)
        : base(RGBDeviceType.Mainboard, nativeInfo, ledCount, ledOffset)
    { }

    #endregion
}