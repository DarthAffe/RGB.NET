// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global


#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace RGB.NET.Devices.Corsair;

/// <summary>
/// Contains a list of available corsair device types.
/// </summary>
public enum CorsairDeviceType
{
    Unknown = 0,
    Mouse = 1,
    Keyboard = 2,
    Headset = 3,
    Mousepad = 4,
    HeadsetStand = 5,
    CommanderPro = 6,
    LightningNodePro = 7,
    MemoryModule = 8,
    Cooler = 9,
    Mainboard = 10,
    GraphicsCard = 11
};