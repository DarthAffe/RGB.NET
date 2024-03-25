using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingPeripheralRGBDevice" />.
/// </summary>
public sealed class DynamicLightingPeripheralRGBDeviceInfo : DynamicLightingRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal DynamicLightingPeripheralRGBDeviceInfo(LampArrayInfo lampArrayInfo)
        : base(RGBDeviceType.Unknown, lampArrayInfo)
    { }

    #endregion
}