namespace RGB.NET.Core;

/// <summary>
/// Represents a generic keyboard-device.
/// </summary>
public interface IKeyboard : IRGBDevice
{
    /// <summary>
    /// Gets the device information assiciated with this device.
    /// </summary>
    new IKeyboardDeviceInfo DeviceInfo { get; }
}

/// <summary>
/// Represents a generic keyboard device information.
/// </summary>
public interface IKeyboardDeviceInfo : IRGBDeviceInfo
{
    /// <summary>
    /// Gets the <see cref="KeyboardLayoutType"/> of the keyboard.
    /// </summary>
    KeyboardLayoutType Layout { get; }
}