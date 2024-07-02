namespace RGB.NET.Devices.WLED;

/// <summary>
/// Represents the data used to create a WLED-device.
/// </summary>
public interface IWledDeviceDefinition
{
    string Address { get; }
    string? Manufacturer { get; }
    string? Model { get; }
}