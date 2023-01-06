#pragma warning disable 1591

namespace RGB.NET.Devices.Logitech;

/// <summary>
/// Contains list of available logitech device types.
/// </summary>
public enum LogitechDeviceType
{
    Keyboard = 0x0,
    Mouse = 0x3,
    Mousemat = 0x4,
    Headset = 0x8,
    Speaker = 0xE
}