namespace RGB.NET.Devices.Asus;

/// <summary>
/// Represents a type of ASUS LED as known by the ASUS SDK
/// </summary>
public enum AsusLedType
{
    /// <summary>
    /// An ASUS LED that is present on a device's IAuraSyncKeyboard.Keys enumerable
    /// </summary>
    Key,

    /// <summary>
    /// An ASUS LED that is present on a device's IAuraSyncDevice.Lights enumerable
    /// </summary>
    Light
}