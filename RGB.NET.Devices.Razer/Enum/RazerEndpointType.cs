using System;

namespace RGB.NET.Devices.Razer;

/// <summary>
/// Represents a type of Razer SDK endpoint
/// </summary>
[Flags]
public enum RazerEndpointType
{
    /// <summary>
    /// No endpoint
    /// </summary>
    None = 0,

    /// <summary>
    /// The keyboard endpoint
    /// </summary>
    Keyboard = 1 << 0,

    /// <summary>
    /// The laptop keyboard endpoint, shares the <see cref="Keyboard"/> endpoint but has a different LED layout
    /// </summary>
    LaptopKeyboard = 1 << 1,

    /// <summary>
    /// The mouse endpoint
    /// </summary>
    Mouse = 1 << 2,

    /// <summary>
    /// The headset endpoint
    /// </summary>
    Headset = 1 << 3,

    /// <summary>
    /// The mousepad endpoint
    /// </summary>
    Mousepad = 1 << 4,

    /// <summary>
    /// The keypad endpoint
    /// </summary>
    Keypad = 1 << 5,

    /// <summary>
    /// The Chroma Link endpoint
    /// </summary>
    ChromaLink = 1 << 6,

    /// <summary>
    /// All endpoints
    /// </summary>
    All = ~None
}
