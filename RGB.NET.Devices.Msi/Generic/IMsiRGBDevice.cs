using RGB.NET.Core;

namespace RGB.NET.Devices.Msi
{
    /// <summary>
    /// Represents a msi RGB-device.
    /// </summary>
    internal interface IMsiRGBDevice : IRGBDevice
    {
        void Initialize();
    }
}
