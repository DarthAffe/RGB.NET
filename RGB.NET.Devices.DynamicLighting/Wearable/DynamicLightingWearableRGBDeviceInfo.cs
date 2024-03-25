using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingWearableRGBDevice" />.
/// </summary>
public sealed class DynamicLightingWearableRGBDeviceInfo : DynamicLightingRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal DynamicLightingWearableRGBDeviceInfo(LampArrayInfo lampArrayInfo)
        : base(RGBDeviceType.Wearable, lampArrayInfo)
    { }

    #endregion
}