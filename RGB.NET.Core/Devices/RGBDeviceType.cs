using System;

namespace RGB.NET.Core;

/// <summary>
/// Contains a list of different types of device.
/// </summary>
[Flags]
public enum RGBDeviceType
{
    /// <summary>
    /// Represents nothing.
    /// </summary>
    None = 0,

    /// <summary>
    /// Represents a keyboard.
    /// </summary>
    Keyboard = 1 << 0,

    /// <summary>
    /// Represents a mouse.
    /// </summary>
    Mouse = 1 << 1,

    /// <summary>
    /// Represents a headset.
    /// </summary>
    Headset = 1 << 2,

    /// <summary>
    /// Represents a mousepad.
    /// </summary>
    Mousepad = 1 << 3,

    /// <summary>
    /// Represents a LED-stipe.
    /// </summary>
    LedStripe = 1 << 4,

    /// <summary>
    /// Represents a LED-matrix.
    /// </summary>
    LedMatrix = 1 << 5,

    /// <summary>
    /// Represents a Mainboard.
    /// </summary>
    Mainboard = 1 << 6,

    /// <summary>
    /// Represents a Graphics card.
    /// </summary>
    GraphicsCard = 1 << 7,

    /// <summary>
    /// Represents a DRAM-bank.
    /// </summary>
    DRAM = 1 << 8,

    /// <summary>
    /// Represents a headset stand.
    /// </summary>
    HeadsetStand = 1 << 9,

    /// <summary>
    /// Represents a keypad.
    /// </summary>
    Keypad = 1 << 10,

    /// <summary>
    /// Represents a fan.
    /// </summary>
    Fan = 1 << 11,

    /// <summary>
    /// Represents a speaker
    /// </summary>
    Speaker = 1 << 12,

    /// <summary>
    /// Represents a cooler.
    /// </summary>
    Cooler = 1 << 13,

    /// <summary>
    /// Represents a monitor.
    /// </summary>
    Monitor = 1 << 14,

    /// <summary>
    /// Represents a generic led-controller.
    /// </summary>
    LedController = 1 << 15,

    /// <summary>
    /// Represents a game controller.
    /// </summary>
    GameController = 1 << 16,

    /// <summary>
    /// Represents a chassis
    /// </summary>
    Chassis = 1 << 17,

    /// <summary>
    /// Represents a furniture
    /// </summary>
    Furniture = 1 << 18,

    /// <summary>
    /// Represents a wearable
    /// </summary>
    Wearable = 1 << 19,

    /// <summary>
    /// Represents a piece of art
    /// </summary>
    Art = 1 << 20,

    /// <summary>
    /// Represents a device where the type is not known or not present in the list.
    /// </summary>
    Unknown = 1 << 31,

    /// <summary>
    /// Represents all devices.
    /// </summary>
    All = ~None
}