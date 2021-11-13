using RGB.NET.Core;

namespace RGB.NET.Devices.Novation;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a Corsair-<see cref="T:RGB.NET.Core.IRGBDevice" />.
/// </summary>
public class NovationRGBDeviceInfo : IRGBDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public RGBDeviceType DeviceType { get; }

    /// <inheritdoc />
    public string DeviceName { get; }

    /// <inheritdoc />
    public string Manufacturer => "Novation";

    /// <inheritdoc />
    public string Model { get; }

    /// <inheritdoc />
    public object? LayoutMetadata { get; set; }

    /// <summary>
    /// Gets the <see cref="NovationColorCapabilities"/> of the <see cref="IRGBDevice"/>.
    /// </summary>
    public NovationColorCapabilities ColorCapabilities { get; }

    /// <summary>
    /// Gets the (midi)-id of the <see cref="IRGBDevice"/>..
    /// </summary>
    public int DeviceId { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Internal constructor of managed <see cref="NovationRGBDeviceInfo"/>.
    /// </summary>
    /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
    /// <param name="model">The represented device model.</param>
    /// <param name="deviceId">The (midi)-id of the <see cref="IRGBDevice"/>.</param>
    /// <param name="colorCapabilities">The <see cref="NovationColorCapabilities"/> of the <see cref="IRGBDevice"/>.</param>
    internal NovationRGBDeviceInfo(RGBDeviceType deviceType, string model, int deviceId, NovationColorCapabilities colorCapabilities)
    {
        this.DeviceType = deviceType;
        this.Model = model;
        this.DeviceId = deviceId;
        this.ColorCapabilities = colorCapabilities;

        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    #endregion
}