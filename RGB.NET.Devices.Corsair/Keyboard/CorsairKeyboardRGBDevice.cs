// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a corsair keyboard.
/// </summary>
public class CorsairKeyboardRGBDevice : CorsairRGBDevice<CorsairKeyboardRGBDeviceInfo>, IKeyboard
{
    #region Properties & Fields

    IKeyboardDeviceInfo IKeyboard.DeviceInfo => DeviceInfo;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairKeyboardRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the keyboard.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    internal CorsairKeyboardRGBDevice(CorsairKeyboardRGBDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
        : base(info, LedMappings.Keyboard, updateQueue)
    { }

    #endregion
}