using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingFurnitureRGBDevice" />.
/// </summary>
public sealed class DynamicLightingFurnitureRGBDeviceInfo : DynamicLightingRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal DynamicLightingFurnitureRGBDeviceInfo(LampArrayInfo lampArrayInfo)
        : base(RGBDeviceType.Furniture, lampArrayInfo)
    { }

    #endregion
}