// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

using System.ComponentModel;
using RGB.NET.Core;
using RGB.NET.Devices.CoolerMaster.Attributes;

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace RGB.NET.Devices.CoolerMaster
{
    /// <summary>
    /// Contains a list of available device-indexes.
    /// </summary>
    public enum CoolerMasterDevicesIndexes
    {
        [Description("MasterKeys Pro L")]
        [DeviceType(RGBDeviceType.Keyboard)]
        MasterKeys_L = 0,

        [Description("MasterKeys ProS")]
        [DeviceType(RGBDeviceType.Keyboard)]
        MasterKeys_S = 1,

        [Description("MasterKeys Pro L White")]
        [DeviceType(RGBDeviceType.Keyboard)]
        MasterKeys_L_White = 2,

        [Description("MasterKeys Pro M White")]
        [DeviceType(RGBDeviceType.Keyboard)]
        MasterKeys_M_White = 3,

        [Description("MasterMouse Pro L")]
        [DeviceType(RGBDeviceType.Mouse)]
        MasterMouse_L = 4,

        [Description("MasterMouse S")]
        [DeviceType(RGBDeviceType.Mouse)]
        MasterMouse_S = 5,

        [Description("MasterKeys Pro M")]
        [DeviceType(RGBDeviceType.Keyboard)]
        MasterKeys_M = 6,

        [Description("MasterKeys Pro S White")]
        [DeviceType(RGBDeviceType.Keyboard)]
        MasterKeys_S_White = 7
    }
}
