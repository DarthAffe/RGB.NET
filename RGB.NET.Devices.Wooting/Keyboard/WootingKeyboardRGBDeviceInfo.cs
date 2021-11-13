using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Generic;
using RGB.NET.Devices.Wooting.Native;

namespace RGB.NET.Devices.Wooting.Keyboard;

/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.Wooting.Keyboard.WootingKeyboardRGBDevice" />.
/// </summary>
public class WootingKeyboardRGBDeviceInfo : WootingRGBDeviceInfo, IKeyboardDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public KeyboardLayoutType Layout => KeyboardLayoutType.Unknown;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Wooting.WootingKeyboardRGBDeviceInfo" />.
    /// </summary>
    /// <param name="deviceInfo">The native <see cref="T:RGB.NET.Devices.Wooting.Native._WootingDeviceInfo" />.</param>
    internal WootingKeyboardRGBDeviceInfo(_WootingDeviceInfo deviceInfo)
        : base(RGBDeviceType.Keyboard, deviceInfo)
    { }

    #endregion
}