using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Enum;
using RGB.NET.Devices.Wooting.Generic;

namespace RGB.NET.Devices.Wooting.Keyboard;

/// <inheritdoc cref="WootingRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a Wooting keyboard.
/// </summary>
public sealed class WootingKeyboardRGBDevice : WootingRGBDevice<WootingKeyboardRGBDeviceInfo>, IKeyboard
{
    #region Properties & Fields

    IKeyboardDeviceInfo IKeyboard.DeviceInfo => DeviceInfo;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Wooting.Keyboard.WootingKeyboardRGBDevice" /> class.
    /// </summary>
    /// <param name="deviceType">The type of the Wooting device.</param>
    /// <param name="info">The specific information provided by Wooting for the keyboard</param>
    /// <param name="updateQueue">The update queue used to update this device.</param>
    internal WootingKeyboardRGBDevice(WootingDeviceType deviceType, WootingKeyboardRGBDeviceInfo info, IUpdateQueue updateQueue)
        : base(deviceType, info, updateQueue)
    { }

    #endregion
}
