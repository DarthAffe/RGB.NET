// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using System.Collections.Generic;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a corsair ledStrip.
/// </summary>
public class CorsairLedStripRGBDevice : CorsairRGBDevice<CorsairLedStripRGBDeviceInfo>, ILedStripe
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairLedStripRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the ledStrip.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal CorsairLedStripRGBDevice(CorsairLedStripRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, updateQueue)
    { }

    #endregion

    #region Methods

    protected override LedMapping<CorsairLedId> CreateMapping(IEnumerable<CorsairLedId> ids) => LedMappings.CreateLedStripMapping(ids);

    #endregion
}