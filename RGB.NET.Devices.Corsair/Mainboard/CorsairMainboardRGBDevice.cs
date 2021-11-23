// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a corsair memory.
/// </summary>
public class CorsairMainboardRGBDevice : CorsairRGBDevice<CorsairMainboardRGBDeviceInfo>, IMainboard
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairMainboardRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the memory.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal CorsairMainboardRGBDevice(CorsairMainboardRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, LedMappings.Mainboard, updateQueue)
    { }

    #endregion
}