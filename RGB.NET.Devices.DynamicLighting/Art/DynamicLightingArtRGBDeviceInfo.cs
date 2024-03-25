using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingArtRGBDevice" />.
/// </summary>
public sealed class DynamicLightingArtRGBDeviceInfo : DynamicLightingRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal DynamicLightingArtRGBDeviceInfo(LampArrayInfo lampArrayInfo)
        : base(RGBDeviceType.Art, lampArrayInfo)
    { }

    #endregion
}