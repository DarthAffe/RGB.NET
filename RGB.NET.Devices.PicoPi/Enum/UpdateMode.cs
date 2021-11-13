// ReSharper disable InconsistentNaming
#pragma warning disable 1591

namespace RGB.NET.Devices.PicoPi.Enum;

/// <summary>
/// Contains a list of possible ways of communication with the device.
/// </summary>
public enum UpdateMode
{
    Auto = 0x00,
    HID = 0x01,
    BULK = 0x02,
}