using RGB.NET.Core;

namespace RGB.NET.Devices.Razer
{
    /// <summary>
    /// Represents a razer RGB-device.
    /// </summary>
    internal interface IRazerRGBDevice : IRGBDevice
    {
        void Initialize();
        void Reset();
    }
}
