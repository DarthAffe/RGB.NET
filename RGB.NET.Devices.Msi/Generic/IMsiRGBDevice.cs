using RGB.NET.Core;

namespace RGB.NET.Devices.Msi
{
    /// <summary>
    /// Represents a MSI RGB-device.
    /// </summary>
    internal interface IMsiRGBDevice : IRGBDevice
    {
        void Initialize(MsiDeviceUpdateQueue updateQueue, int ledCount);
    }
}
