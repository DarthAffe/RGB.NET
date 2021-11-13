namespace RGB.NET.Devices.Razer;

/// <summary>
/// Represents a type of Razer SDK endpoint
/// </summary>
public enum RazerEndpointType
{
    /// <summary>
    /// The keyboard endpoint
    /// </summary>
    Keyboard,

    /// <summary>
    /// The laptop keyboard endpoint, shares the <see cref="Keyboard"/> endpoint but has a different LED layout
    /// </summary>
    LaptopKeyboard,

    /// <summary>
    /// The mouse endpoint
    /// </summary>
    Mouse,

    /// <summary>
    /// The headset endpoint
    /// </summary>
    Headset,

    /// <summary>
    /// The mousepad endpoint
    /// </summary>
    Mousepad,

    /// <summary>
    /// The keypad endpoint
    /// </summary>
    Keypad,

    /// <summary>
    /// The Chroma Link endpoint
    /// </summary>
    ChromaLink,
}