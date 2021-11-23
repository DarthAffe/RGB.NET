using RGB.NET.Core;

namespace RGB.NET.Devices.Razer;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a Razer-<see cref="T:RGB.NET.Core.IRGBDevice" />.
/// </summary>
public class RazerRGBDeviceInfo : IRGBDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public RGBDeviceType DeviceType { get; }

    /// <inheritdoc />
    public string DeviceName { get; }

    /// <inheritdoc />
    public string Manufacturer => "Razer";

    /// <inheritdoc />
    public string Model { get; }

    /// <inheritdoc />
    public object? LayoutMetadata { get; set; }

    /// <summary>
    /// Gets the Razer SDK endpoint type the <see cref="IRGBDevice"/> is addressed through.
    /// </summary>
    public RazerEndpointType EndpointType { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Internal constructor of managed <see cref="RazerRGBDeviceInfo"/>.
    /// </summary>
    /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
    /// <param name="endpointType">The Razer SDK endpoint type the <see cref="IRGBDevice"/> is addressed through.</param>
    /// <param name="model">The model of the <see cref="IRGBDevice"/>.</param>
    internal RazerRGBDeviceInfo(RGBDeviceType deviceType, RazerEndpointType endpointType, string model)
    {
        this.DeviceType = deviceType;
        this.EndpointType = endpointType;
        this.Model = model;

        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    #endregion
}