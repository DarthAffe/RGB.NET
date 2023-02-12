namespace RGB.NET.Devices.Corsair;

/// <summary>
/// iCUE-SDK: contains a list of led groups. Led group is used as a part of led identifier
/// </summary>
public enum CorsairLedGroup
{
    /// <summary>
    /// iCUE-SDK: for keyboard leds
    /// </summary>
    Keyboard = 0,

    /// <summary>
    /// iCUE-SDK: for keyboard leds on G keys
    /// </summary>
    KeyboardGKeys = 1,

    /// <summary>
    /// iCUE-SDK: for keyboard lighting pipe leds
    /// </summary>
    KeyboardEdge = 2,

    /// <summary>
    /// iCUE-SDK: for vendor specific keyboard leds (ProfileSwitch, DialRing, etc.)
    /// </summary>
    KeyboardOem = 3,

    /// <summary>
    /// iCUE-SDK: for mouse leds
    /// </summary>
    Mouse = 4,

    /// <summary>
    /// iCUE-SDK: for mousemat leds
    /// </summary>
    Mousemat = 5,

    /// <summary>
    /// iCUE-SDK: for headset leds
    /// </summary>
    Headset = 6,

    /// <summary>
    /// iCUE-SDK: for headset stand leds
    /// </summary>
    HeadsetStand = 7,

    /// <summary>
    /// iCUE-SDK: for memory module leds
    /// </summary>
    MemoryModule = 8,

    /// <summary>
    /// iCUE-SDK: for motherboard leds
    /// </summary>
    Motherboard = 9,

    /// <summary>
    /// iCUE-SDK: for graphics card leds
    /// </summary>
    GraphicsCard = 10,

    /// <summary>
    /// iCUE-SDK: for leds on the first channel of DIY devices and coolers
    /// </summary>
    DIY_Channel1 = 11,

    /// <summary>
    /// iCUE-SDK: for leds on the second channel of DIY devices and coolers
    /// </summary>
    DIY_Channel2 = 12,

    /// <summary>
    /// iCUE-SDK: for leds on the third channel of DIY devices and coolers
    /// </summary>
    DIY_Channel3 = 13,

    /// <summary>
    /// iCUE-SDK: for touchbar leds
    /// </summary>
    Touchbar = 14
}