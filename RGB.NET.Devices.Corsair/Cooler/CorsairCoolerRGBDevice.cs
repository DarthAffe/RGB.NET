// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using System.Collections.Generic;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a corsair cooler.
/// </summary>
public sealed class CorsairCoolerRGBDevice : CorsairRGBDevice<CorsairCoolerRGBDeviceInfo>, ICooler
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairCoolerRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the cooler.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal CorsairCoolerRGBDevice(CorsairCoolerRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, updateQueue)
    { }

    #endregion

    #region Methods

    protected override LedMapping<CorsairLedId> CreateMapping(IEnumerable<CorsairLedId> ids) => LedMappings.CreateCoolerMapping(ids);

    #endregion
}