using RGB.NET.Core;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <summary>
    /// Represents a CoolerMaster RGB-device.
    /// </summary>
    internal interface ICoolerMasterRGBDevice : IRGBDevice
    {
        void Initialize();
    }
}
