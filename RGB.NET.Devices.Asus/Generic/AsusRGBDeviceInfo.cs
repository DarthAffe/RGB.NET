using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a Corsair-<see cref="T:RGB.NET.Core.IRGBDevice" />.
/// </summary>
public class AsusRGBDeviceInfo : IRGBDeviceInfo
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

    /// <summary>
    /// Gets the SDK-aura-device this device represents.
    /// </summary>
    public IAuraSyncDevice Device { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Internal constructor of managed <see cref="AsusRGBDeviceInfo"/>.
    /// </summary>
    /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
    /// <param name="device">The <see cref="IAuraSyncDevice"/> backing this RGB.NET device.</param>
    /// <param name="manufacturer">The manufacturer-name of the <see cref="IRGBDevice"/>.</param>
    /// <param name="model">The model-name of the <see cref="IRGBDevice"/>.</param>
    internal AsusRGBDeviceInfo(RGBDeviceType deviceType, IAuraSyncDevice device, string? model = null, string manufacturer = "Asus")
    {
        this.DeviceType = deviceType;
        this.Device = device;
        this.Model = model ?? device.Name;
        this.Manufacturer = manufacturer;

        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    #endregion
}