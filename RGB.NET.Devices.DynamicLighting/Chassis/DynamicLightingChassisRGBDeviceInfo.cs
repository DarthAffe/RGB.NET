using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingChassisRGBDevice" />.
/// </summary>
public sealed class DynamicLightingChassisRGBDeviceInfo : DynamicLightingRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal DynamicLightingChassisRGBDeviceInfo(LampArrayInfo lampArrayInfo)
        : base(RGBDeviceType.Chassis, lampArrayInfo)
    { }

    #endregion
}