// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using System.Collections.Generic;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a corsair gamecontroller.
/// </summary>
public sealed class CorsairGameControllerRGBDevice : CorsairRGBDevice<CorsairGameControllerRGBDeviceInfo>, IGameController
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairGameControllerRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the gamecontroller</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal CorsairGameControllerRGBDevice(CorsairGameControllerRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, updateQueue)
    { }

    #endregion

    #region Methods

    protected override LedMapping<CorsairLedId> CreateMapping(IEnumerable<CorsairLedId> ids) => LedMappings.CreateGameControllerMapping(ids);

    #endregion
}