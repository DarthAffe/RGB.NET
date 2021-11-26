// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global


#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace RGB.NET.Devices.Corsair;

/// <summary>
/// Contains a list of available corsair channel device types.
/// </summary>
public enum CorsairChannelDeviceType
{
    Invalid = 0,
    FanHD = 1,
    FanSP = 2,
    FanLL = 3,
    FanML = 4,
    Strip = 5,
    DAP = 6,
    Pump = 7,
    FanQL = 8,
    WaterBlock = 9,
    FanSPPRO = 10
};