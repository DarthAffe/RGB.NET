namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a keyboard-device
    /// </summary>
    public interface IKeyboard : IRGBDevice
    {
        new IKeyboardDeviceInfo DeviceInfo { get; }
    }

    public interface IKeyboardDeviceInfo : IRGBDeviceInfo
    {
        KeyboardLayoutType Layout { get; }
    }
}
