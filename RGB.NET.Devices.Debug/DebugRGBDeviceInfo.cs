using RGB.NET.Core;

namespace RGB.NET.Devices.Debug;

/// <inheritdoc />
/// <summary>
/// Represents device information for a <see cref="DebugRGBDevice"/> />.
/// </summary>
public class DebugRGBDeviceInfo : IRGBDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public RGBDeviceType DeviceType { get; }

    /// <inheritdoc />
    public string DeviceName { get; }

    /// <inheritdoc />
    public string Manufacturer { get; }

    /// <inheritdoc />
    public string Model { get; }

    /// <inheritdoc />
    public object? LayoutMetadata { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Internal constructor of <see cref="DebugRGBDeviceInfo"/>.
    /// </summary>
    /// <param name="deviceType">The <see cref="RGBDeviceType"/> of the device.</param>
    /// <param name="manufacturer">The manufacturer of the device.</param>
    /// <param name="model">The model of the device.</param>
    /// <param name="customData">Some custom data for this device provided by the layout.</param>
    internal DebugRGBDeviceInfo(RGBDeviceType deviceType, string manufacturer, string model, object? customData)
    {
        this.DeviceType = deviceType;
        this.Manufacturer = manufacturer;
        this.Model = model;
        this.LayoutMetadata = customData;

        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    #endregion
}