// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a corsair mousepad.
/// </summary>
public class CorsairMousepadRGBDevice : CorsairRGBDevice<CorsairMousepadRGBDeviceInfo>, IMousepad
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairMousepadRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the mousepad</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal CorsairMousepadRGBDevice(CorsairMousepadRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, LedMappings.Mousepad, updateQueue)
    { }

    #endregion
}