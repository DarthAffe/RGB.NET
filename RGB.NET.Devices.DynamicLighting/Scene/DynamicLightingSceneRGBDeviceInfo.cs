using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingSceneRGBDevice" />.
/// </summary>
public sealed class DynamicLightingSceneRGBDeviceInfo : DynamicLightingRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal DynamicLightingSceneRGBDeviceInfo(LampArrayInfo lampArrayInfo)
        : base(RGBDeviceType.Unknown, lampArrayInfo)
    { }

    #endregion
}