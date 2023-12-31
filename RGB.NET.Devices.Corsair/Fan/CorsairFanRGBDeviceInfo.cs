// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.Corsair.CorsairFanRGBDevice" />.
/// </summary>
public sealed class CorsairFanRGBDeviceInfo : CorsairRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal CorsairFanRGBDeviceInfo(_CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset)
        : base(RGBDeviceType.Fan, nativeInfo, ledCount, ledOffset)
    { }

    /// <inheritdoc />
    internal CorsairFanRGBDeviceInfo(_CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset, string modelName)
        : base(RGBDeviceType.Fan, nativeInfo, ledCount, ledOffset, modelName)
    { }

    #endregion
}