using RGB.NET.Core;
using System.Collections.Generic;
using OpenRGBDevice = OpenRGB.NET.Models.Device;

namespace RGB.NET.Devices.OpenRGB
{
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Wooting.Keyboard.WootingKeyboardRGBDevice" />.
    /// </summary>
    public class OpenRGBZoneDeviceInfo : AbstractOpenRGBDeviceInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        public OpenRGBZoneDeviceInfo(OpenRGBDevice device) : base(device)
        { }
    }
}