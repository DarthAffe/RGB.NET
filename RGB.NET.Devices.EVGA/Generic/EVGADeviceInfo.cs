using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RGB.NET.Devices.EVGA.Generic
{
    public class EVGADeviceInfo : IRGBDeviceInfo
    {
        public RGBDeviceType DeviceType => RGBDeviceType.GraphicsCard;

        public string DeviceName => "GPU";

        public string Manufacturer => "EVGA";

        public string Model => "GPU";

        public RGBDeviceLighting Lighting => RGBDeviceLighting.Key;

        public bool SupportsSyncBack => false;

        public Uri Image { get; set; }
    }
}
