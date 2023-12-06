using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Generic;
using RGB.NET.Devices.Wooting.Native;

namespace RGB.NET.Devices.Wooting.Keypad;

/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.Wooting.Keypad.WootingKeypadRGBDevice" />.
/// </summary>
public sealed class WootingKeypadRGBDeviceInfo : WootingRGBDeviceInfo
{
    internal WootingKeypadRGBDeviceInfo(_WootingDeviceInfo deviceInfo, byte deviceIndex)
        : base(RGBDeviceType.Keypad, deviceInfo, deviceIndex)
    {
        
    }
}
