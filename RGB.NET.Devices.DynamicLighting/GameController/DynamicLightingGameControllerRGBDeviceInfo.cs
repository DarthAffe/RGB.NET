using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingGameControllerRGBDevice" />.
/// </summary>
public sealed class DynamicLightingGameControllerRGBDeviceInfo : DynamicLightingRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    internal DynamicLightingGameControllerRGBDeviceInfo(LampArrayInfo lampArrayInfo)
        : base(RGBDeviceType.GameController, lampArrayInfo)
    { }

    #endregion
}