using RGB.NET.Core;
using System.Collections.Generic;
using OpenRGBDevice = OpenRGB.NET.Models.Device;

namespace RGB.NET.Devices.OpenRGB
{
    /// <summary>
    /// Represents generic information for an OpenRGB Device
    /// </summary>
    public class OpenRGBGenericDeviceInfo : AbstractOpenRGBDeviceInfo
    {
        /// <summary>
        /// Initializes a new instance of <see cref="OpenRGBGenericDeviceInfo"/>.
        /// </summary>
        /// <param name="device">The OpenRGB device containing device-specific information.</param>
        public OpenRGBGenericDeviceInfo(OpenRGBDevice device) : base(device)
        { }
    }
}