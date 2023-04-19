// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using System.Collections.Generic;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a corsair mouse.
/// </summary>
public sealed class CorsairMouseRGBDevice : CorsairRGBDevice<CorsairMouseRGBDeviceInfo>, IMouse
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairMouseRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the mouse</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal CorsairMouseRGBDevice(CorsairMouseRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, updateQueue)
    { }

    #endregion

    #region Methods

    protected override LedMapping<CorsairLedId> CreateMapping(IEnumerable<CorsairLedId> ids) => LedMappings.CreateMouseMapping(ids);

    #endregion
}