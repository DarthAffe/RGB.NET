// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.CorsairLegacy;

/// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a corsair touchbar.
/// </summary>
public class CorsairTouchbarRGBDevice : CorsairRGBDevice<CorsairTouchbarRGBDeviceInfo>, IDRAM
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.CorsairLegacy.CorsairTouchbarRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the touchbar.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal CorsairTouchbarRGBDevice(CorsairTouchbarRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, LedMappings.Keyboard, updateQueue) //TODO DarthAffe 17.07.2022: Find someone with such a device and check which LedIds are actually used
    { }

    #endregion
}