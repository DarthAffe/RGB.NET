// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a corsair headset.
/// </summary>
public class CorsairHeadsetRGBDevice : CorsairRGBDevice<CorsairHeadsetRGBDeviceInfo>, IHeadset
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairHeadsetRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the headset</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal CorsairHeadsetRGBDevice(CorsairHeadsetRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, LedMappings.Headset, updateQueue)
    { }

    #endregion
}