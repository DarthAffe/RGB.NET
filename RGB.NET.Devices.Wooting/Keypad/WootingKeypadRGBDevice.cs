using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Enum;
using RGB.NET.Devices.Wooting.Generic;

namespace RGB.NET.Devices.Wooting.Keypad;

/// <inheritdoc cref="WootingRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a Wooting keyboard.
/// </summary>
public sealed class WootingKeypadRGBDevice : WootingRGBDevice<WootingKeypadRGBDeviceInfo>, IKeypad
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Wooting.Keyboard.WootingKeypadRGBDevice" /> class.
    /// </summary>
    /// <param name="deviceType">The type of the Wooting device.</param>
    /// <param name="info">The specific information provided by Wooting for the keyboard</param>
    /// <param name="updateQueue">The update queue used to update this device.</param>
    internal WootingKeypadRGBDevice(WootingDeviceType deviceType, WootingKeypadRGBDeviceInfo info, IUpdateQueue updateQueue)
        : base(deviceType, info, updateQueue)
    { }

    #endregion
}
