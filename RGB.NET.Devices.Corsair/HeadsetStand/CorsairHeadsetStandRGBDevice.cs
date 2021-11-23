// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a corsair headset stand.
/// </summary>
public class CorsairHeadsetStandRGBDevice : CorsairRGBDevice<CorsairHeadsetStandRGBDeviceInfo>, IHeadsetStand
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairHeadsetStandRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the headset stand</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal CorsairHeadsetStandRGBDevice(CorsairHeadsetStandRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, LedMappings.HeadsetStand, updateQueue)
    { }

    #endregion
}