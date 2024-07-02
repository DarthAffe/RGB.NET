namespace RGB.NET.Devices.WLED;

/// <inheritdoc />
public class WledDeviceDefinition(string address, string? manufacturer = null, string? model = null) : IWledDeviceDefinition
{
    #region Properties & Fields

    /// <inheritdoc />
    public string Address { get; } = address;

    /// <inheritdoc />
    public string? Manufacturer { get; } = manufacturer;

    /// <inheritdoc />
    public string? Model { get; } = model;

    #endregion
}