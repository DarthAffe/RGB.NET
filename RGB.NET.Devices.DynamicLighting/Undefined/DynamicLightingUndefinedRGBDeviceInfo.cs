using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingUndefinedRGBDevice" />.
/// </summary>
public sealed class DynamicLightingUndefinedRGBDeviceInfo : DynamicLightingRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal DynamicLightingUndefinedRGBDeviceInfo(LampArrayInfo lampArrayInfo)
        : base(RGBDeviceType.Unknown, lampArrayInfo)
    { }

    #endregion
}