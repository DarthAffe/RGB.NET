// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.Corsair.CorsairUnknownRGBDevice" />.
/// </summary>
public class CorsairUnknownRGBDeviceInfo : CorsairRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal CorsairUnknownRGBDeviceInfo(_CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset)
        : base(RGBDeviceType.Unknown, nativeInfo, ledCount, ledOffset)
    { }

    #endregion
}