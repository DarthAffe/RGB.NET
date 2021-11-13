using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Enum;
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
    public KeyboardLayoutType Layout { get; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Wooting.WootingKeyboardRGBDeviceInfo" />.
    /// </summary>
    /// <param name="deviceInfo">The native <see cref="T:RGB.NET.Devices.Wooting.Native._WootingDeviceInfo" />.</param>
    internal WootingKeyboardRGBDeviceInfo(_WootingDeviceInfo deviceInfo)
        : base(RGBDeviceType.Keyboard, deviceInfo)
    {
        Layout = WootingLayoutType switch
        {
            WootingLayoutType.ANSI => KeyboardLayoutType.ANSI,
            WootingLayoutType.ISO => KeyboardLayoutType.ISO,
            _ => KeyboardLayoutType.Unknown
        };
    }

    #endregion
}