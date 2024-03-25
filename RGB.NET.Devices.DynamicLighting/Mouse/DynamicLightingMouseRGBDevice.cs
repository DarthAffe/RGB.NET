// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc cref="DynamicLightingRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a Dynamic Lighting Mouse-device.
/// </summary>
public sealed class DynamicLightingMouseRGBDevice : DynamicLightingRGBDevice<DynamicLightingMouseRGBDeviceInfo>, IMouse
{
    #region Properties & Fields

    /// <inheritdoc/>
    protected override LedId ReferenceLedId => LedId.Mouse1;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingMouseRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The meta data for this device.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal DynamicLightingMouseRGBDevice(DynamicLightingMouseRGBDeviceInfo info, DynamicLightingDeviceUpdateQueue updateQueue)
        : base(info, updateQueue)
    { }

    #endregion
}