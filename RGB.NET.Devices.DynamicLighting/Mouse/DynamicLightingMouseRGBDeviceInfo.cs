using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingMouseRGBDevice" />.
/// </summary>
public sealed class DynamicLightingMouseRGBDeviceInfo : DynamicLightingRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal DynamicLightingMouseRGBDeviceInfo(LampArrayInfo lampArrayInfo)
        : base(RGBDeviceType.Mouse, lampArrayInfo)
    { }

    #endregion
}