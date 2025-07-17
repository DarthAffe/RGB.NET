using RGB.NET.Core;

namespace RGB.NET.Devices.Wooting.Generic;

public abstract class WootingRGBDeviceInfo : IRGBDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public RGBDeviceType DeviceType { get; }

    /// <inheritdoc />
    public string DeviceName { get; }

    /// <inheritdoc />
    public string Manufacturer => "Wooting";

    /// <inheritdoc />
    public string Model { get; }

    /// <inheritdoc />
    public object? LayoutMetadata { get; set; }

    #endregion

    #region Constructors

    protected WootingRGBDeviceInfo(RGBDeviceType deviceType, string model, string name)
    {
        this.DeviceType = deviceType;
        this.Model = model;
        this.DeviceName = name;
    }

    #endregion
}
