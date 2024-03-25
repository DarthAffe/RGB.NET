﻿// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc cref="DynamicLightingRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a Dynamic Lighting Art-device.
/// </summary>
public sealed class DynamicLightingArtRGBDevice : DynamicLightingRGBDevice<DynamicLightingArtRGBDeviceInfo>, IArt
{
    #region Properties & Fields

    /// <inheritdoc/>
    protected override LedId ReferenceLedId => LedId.Art1;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.DynamicLighting.DynamicLightingArtRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The meta data for this device.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal DynamicLightingArtRGBDevice(DynamicLightingArtRGBDeviceInfo info, DynamicLightingDeviceUpdateQueue updateQueue)
        : base(info, updateQueue)
    { }

    #endregion
}