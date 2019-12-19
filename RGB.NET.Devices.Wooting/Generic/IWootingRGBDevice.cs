using RGB.NET.Core;

namespace RGB.NET.Devices.Wooting.Generic
{
    /// <summary>
    /// Represents a Wooting RGB-device.
    /// </summary>
    internal interface IWootingRGBDevice : IRGBDevice
    {
        void Initialize(IDeviceUpdateTrigger updateTrigger);
    }
}
