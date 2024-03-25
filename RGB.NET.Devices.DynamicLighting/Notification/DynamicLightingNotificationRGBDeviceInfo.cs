using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingNotificationRGBDevice" />.
/// </summary>
public sealed class DynamicLightingNotificationRGBDeviceInfo : DynamicLightingRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal DynamicLightingNotificationRGBDeviceInfo(LampArrayInfo lampArrayInfo)
        : base(RGBDeviceType.Unknown, lampArrayInfo)
    { }

    #endregion
}