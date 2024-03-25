using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc cref="DynamicLightingRGBDeviceInfo" />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingKeyboardRGBDevice" />.
/// </summary>
public sealed class DynamicLightingKeyboardRGBDeviceInfo : DynamicLightingRGBDeviceInfo, IKeyboardDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc/>
    public KeyboardLayoutType Layout => KeyboardLayoutType.Unknown;

    #endregion

    #region Constructors

    /// <inheritdoc />
    internal DynamicLightingKeyboardRGBDeviceInfo(LampArrayInfo lampArrayInfo)
        : base(RGBDeviceType.Keyboard, lampArrayInfo)
    { }

    #endregion
}