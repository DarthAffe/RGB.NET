using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <summary>
    /// Represents a asus RGB-device.
    /// </summary>
    internal interface IAsusRGBDevice : IRGBDevice
    {
        void Initialize(IDeviceUpdateTrigger updateTrigger);
    }
}
