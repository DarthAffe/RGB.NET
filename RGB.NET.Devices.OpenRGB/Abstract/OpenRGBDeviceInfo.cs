using RGB.NET.Core;
using OpenRGBDevice = OpenRGB.NET.Device;

namespace RGB.NET.Devices.OpenRGB;

/// <summary>
/// Represents generic information for an OpenRGB Device
/// </summary>
public class OpenRGBDeviceInfo : IRGBDeviceInfo
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
    /// Gets the OpenRGB device.
    /// </summary>
    public OpenRGBDevice OpenRGBDevice { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of <see cref="OpenRGBDeviceInfo"/>.
    /// </summary>
    /// <param name="openRGBDevice">The OpenRGB device to extract information from.</param>
    internal OpenRGBDeviceInfo(OpenRGBDevice openRGBDevice)
    {
        this.OpenRGBDevice = openRGBDevice;

        DeviceType = Helper.GetRgbNetDeviceType(openRGBDevice.Type);
        Manufacturer = Helper.GetVendorName(openRGBDevice);
        Model = Helper.GetModelName(openRGBDevice);
        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    #endregion
}
