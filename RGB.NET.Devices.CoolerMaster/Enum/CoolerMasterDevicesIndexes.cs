// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

using System.ComponentModel;
using RGB.NET.Core;

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace RGB.NET.Devices.CoolerMaster;

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
    MasterKeys_S_White = 7,

    [Description("MM520")]
    [DeviceType(RGBDeviceType.Mouse)]
    MM520 = 8,

    [Description("MM530")]
    [DeviceType(RGBDeviceType.Mouse)]
    MM530 = 9,

    [Description("MasterKeys MK750")]
    [DeviceType(RGBDeviceType.Keyboard)]
    MasterKeys_MK750 = 10,

    [Description("CK372")]
    [DeviceType(RGBDeviceType.Keyboard)]
    CK372 = 11,

    [Description("CK550")]
    [DeviceType(RGBDeviceType.Keyboard)]
    CK550 = 12,

    [Description("CK551")]
    [DeviceType(RGBDeviceType.Keyboard)]
    CK551 = 13,

    [Description("MM830")]
    [DeviceType(RGBDeviceType.Mouse)]
    MM830 = 14,

    [Description("CK530")]
    [DeviceType(RGBDeviceType.Keyboard)]
    CK530 = 15,

    [Description("MK850")]
    [DeviceType(RGBDeviceType.Keyboard)]
    MK850 = 16,

    [Description("SK630")]
    [DeviceType(RGBDeviceType.Keyboard)]
    SK630 = 17,

    [Description("SK650")]
    [DeviceType(RGBDeviceType.Keyboard)]
    SK650 = 18,

    [Description("SK621")]
    [DeviceType(RGBDeviceType.Keyboard)]
    SK621 = 19,

    [Description("MK730")]
    [DeviceType(RGBDeviceType.Keyboard)]
    MK730 = 20,

    [DeviceType(RGBDeviceType.None)]
    Default = 0xFFFF
}