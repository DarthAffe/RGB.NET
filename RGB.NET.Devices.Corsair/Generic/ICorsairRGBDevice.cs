using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    /// <summary>
    /// Represents a corsair RGB-device.
    /// </summary>
    internal interface ICorsairRGBDevice : IRGBDevice
    {
        void Initialize();
    }
}
