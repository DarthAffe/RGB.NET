// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.Corsair.CorsairCoolerRGBDevice" />.
/// </summary>
public class CorsairCoolerRGBDeviceInfo : CorsairRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal CorsairCoolerRGBDeviceInfo(_CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset)
        : base(RGBDeviceType.Cooler, nativeInfo, ledCount, ledOffset)
    { }

    /// <inheritdoc />
    internal CorsairCoolerRGBDeviceInfo(_CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset, string modelName)
        : base(RGBDeviceType.Cooler, nativeInfo, ledCount, ledOffset, modelName)
    { }

    #endregion
}