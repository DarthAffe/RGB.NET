namespace RGB.NET.Devices.Corsair;

/// <summary>
/// iCUE-SDK: contains list of properties identifiers which can be read from device
/// </summary>
public enum CorsairDevicePropertyId
{
    /// <summary>
    /// iCUE-SDK: dummy value
    /// </summary>
    Invalid = 0,

    /// <summary>
    /// iCUE-SDK: array of CorsairDevicePropertyId members supported by device
    /// </summary>
    PropertyArray = 1,

    /// <summary>
    /// iCUE-SDK: indicates Mic state (On or Off); used for headset, headset stand
    /// </summary>
    MicEnabled = 2,

    /// <summary>
    /// iCUE-SDK: indicates Surround Sound state (On or Off); used for headset, headset stand
    /// </summary>
    SurroundSoundEnabled = 3,

    /// <summary>
    /// iCUE-SDK: indicates Sidetone state (On or Off); used for headset (where applicable)
    /// </summary>
    SidetoneEnabled = 4,

    /// <summary>
    /// iCUE-SDK: the number of active equalizer preset (integer, 1 - 5); used for headset, headset stand
    /// </summary>
    EqualizerPreset = 5,

    /// <summary>
    /// iCUE-SDK: keyboard physical layout (see CorsairPhysicalLayout for valid values); used for keyboard
    /// </summary>
    PhysicalLayout = 6,

    /// <summary>
    /// iCUE-SDK: keyboard logical layout (see CorsairLogicalLayout for valid values); used for keyboard
    /// </summary>
    LogicalLayout = 7,

    /// <summary>
    /// iCUE-SDK: array of programmable G, M or S keys on device
    /// </summary>
    MacroKeyArray = 8,

    /// <summary>
    /// iCUE-SDK: battery level (0 - 100); used for wireless devices
    /// </summary>
    BatteryLevel = 9,

    /// <summary>
    /// iCUE-SDK: total number of LEDs connected to the channel
    /// </summary>
    ChannelLedCount = 10,

    /// <summary>
    /// iCUE-SDK: number of LED-devices (fans, strips, etc.) connected to the channel which is controlled by the DIY device
    /// </summary>
    ChannelDeviceCount = 11,

    /// <summary>
    /// iCUE-SDK: array of integers, each element describes the number of LEDs controlled by the channel device
    /// </summary>
    ChannelDeviceLedCountArray = 12,

    /// <summary>
    /// iCUE-SDK: array of CorsairChannelDeviceType members, each element describes the type of the channel device
    /// </summary>
    ChannelDeviceTypeArray = 13
}