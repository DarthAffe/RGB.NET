// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

using System;

namespace RGB.NET.Devices.Corsair;

/// <summary>
/// iCUE-SDK: Contains a list of available corsair device types.
/// </summary>
[Flags]
public enum CorsairDeviceType : uint
{
    /// <summary>
    /// iCUE-SDK: for unknown/invalid devices
    /// </summary>
    Unknown = 0x0000,

    /// <summary>
    /// iCUE-SDK: for keyboards
    /// </summary>
    Keyboard = 0x0001,

    /// <summary>
    /// iCUE-SDK: for mice
    /// </summary>
    Mouse = 0x0002,

    /// <summary>
    /// iCUE-SDK: for mousemats
    /// </summary>
    Mousemat = 0x0004,

    /// <summary>
    /// iCUE-SDK: for headsets
    /// </summary>
    Headset = 0x0008,

    /// <summary>
    /// iCUE-SDK: for headset stands
    /// </summary>
    HeadsetStand = 0x0010,

    /// <summary>
    /// iCUE-SDK: for DIY-devices like Commander PRO
    /// </summary>
    FanLedController = 0x0020,

    /// <summary>
    /// iCUE-SDK: for DIY-devices like Lighting Node PRO
    /// </summary>
    LedController = 0x0040,

    /// <summary>
    /// iCUE-SDK: for memory modules
    /// </summary>
    MemoryModule = 0x0080,

    /// <summary>
    /// iCUE-SDK: for coolers
    /// </summary>
    Cooler = 0x0100,

    /// <summary>
    /// iCUE-SDK: for motherboards
    /// </summary>
    Motherboard = 0x0200,

    /// <summary>
    /// iCUE-SDK: for graphics cards
    /// </summary>
    GraphicsCard = 0x0400,

    /// <summary>
    /// iCUE-SDK: for touchbars
    /// </summary>
    Touchbar = 0x0800,

    /// <summary>
    /// iCUE-SDK: for all devices
    /// </summary>
    All = 0xFFFFFFFF
};