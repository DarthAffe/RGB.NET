// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a corsair mouse.
/// </summary>
public class CorsairMouseRGBDevice : CorsairRGBDevice<CorsairMouseRGBDeviceInfo>, IMouse
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairMouseRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the mouse</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal CorsairMouseRGBDevice(CorsairMouseRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, LedMappings.Mouse, updateQueue)
    { }

    #endregion
}