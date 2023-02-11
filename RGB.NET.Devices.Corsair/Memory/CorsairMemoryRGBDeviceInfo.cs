// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.Corsair.CorsairMemoryRGBDevice" />.
/// </summary>
public class CorsairMemoryRGBDeviceInfo : CorsairRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal CorsairMemoryRGBDeviceInfo(_CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset)
        : base(RGBDeviceType.DRAM, nativeInfo, ledCount, ledOffset)
    { }

    /// <inheritdoc />
    internal CorsairMemoryRGBDeviceInfo(_CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset, string modelName)
        : base(RGBDeviceType.DRAM, nativeInfo, ledCount, ledOffset, modelName)
    { }

    #endregion
}