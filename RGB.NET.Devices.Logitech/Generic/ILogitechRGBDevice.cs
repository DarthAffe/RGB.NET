using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech
{
    /// <summary>
    /// Represents a logitech RGB-device.
    /// </summary>
    internal interface ILogitechRGBDevice : IRGBDevice
    {
        void Initialize(UpdateQueue updateQueue);
    }
}
