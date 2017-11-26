using RGB.NET.Core;

namespace RGB.NET.Devices.Novation
{
    /// <summary>
    /// Represents a novation RGB-device.
    /// </summary>
    internal interface INovationRGBDevice : IRGBDevice
    {
        void Initialize();
    }
}
