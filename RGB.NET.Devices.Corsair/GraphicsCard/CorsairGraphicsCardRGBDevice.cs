// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a corsair graphics card.
/// </summary>
public class CorsairGraphicsCardRGBDevice : CorsairRGBDevice<CorsairGraphicsCardRGBDeviceInfo>, IGraphicsCard
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairGraphicsCardRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the graphics card.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal CorsairGraphicsCardRGBDevice(CorsairGraphicsCardRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, LedMappings.GraphicsCard, updateQueue)
    { }

    #endregion
}