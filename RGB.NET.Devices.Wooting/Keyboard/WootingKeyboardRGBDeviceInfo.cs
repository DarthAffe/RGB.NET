using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Enum;
using RGB.NET.Devices.Wooting.Generic;
using RGB.NET.Devices.Wooting.Native;

namespace RGB.NET.Devices.Wooting.Keyboard;

/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.Wooting.Keyboard.WootingKeyboardRGBDevice" />.
/// </summary>
public sealed class WootingKeyboardRGBDeviceInfo : WootingRGBDeviceInfo, IKeyboardDeviceInfo
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
    /// <param name="layout">The layout of the keyboard.</param>
    /// <param name="model">The model of the keyboard.</param>
    /// <param name="name">The name of the keyboard.</param>
    internal WootingKeyboardRGBDeviceInfo(KeyboardLayoutType layout, string model, string name)
        : base(RGBDeviceType.Keyboard, model, name)
    {
        Layout = layout;
    }

    #endregion
}
