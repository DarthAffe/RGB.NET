using RGB.NET.Core;

namespace RGB.NET.Devices.WLED;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a WLED-<see cref="T:RGB.NET.Core.IRGBDevice" />.
/// </summary>
public sealed class WledRGBDeviceInfo : IRGBDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public RGBDeviceType DeviceType => RGBDeviceType.LedStripe;

    /// <inheritdoc />
    public string DeviceName { get; }

    /// <inheritdoc />
    public string Manufacturer { get; }

    /// <inheritdoc />
    public string Model { get; }

    /// <inheritdoc />
    public object? LayoutMetadata { get; set; }

    /// <summary>
    /// Gets some info returned by the WLED-device.
    /// </summary>
    public WledInfo Info { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Internal constructor of managed <see cref="WledRGBDeviceInfo"/>.
    /// </summary>
    /// <param name="manufacturer">The manufacturer of the device.</param>
    /// <param name="model">The represented device model.</param>
    internal WledRGBDeviceInfo(WledInfo info, string? manufacturer, string? model)
    {
        this.Info = info;
        this.Manufacturer = manufacturer ?? info.Brand;
        this.Model = model ?? info.Name;

        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    #endregion
}