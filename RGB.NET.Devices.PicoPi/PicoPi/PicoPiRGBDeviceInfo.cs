using RGB.NET.Core;

namespace RGB.NET.Devices.PicoPi;

/// <summary>
/// Represents a generic information for a <see cref="PicoPiRGBDevice" />.
/// </summary>
public class PicoPiRGBDeviceInfo : IRGBDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public RGBDeviceType DeviceType { get; }

    /// <inheritdoc />
    public string DeviceName { get; }

    /// <inheritdoc />
    public string Manufacturer => "RGB.NET";

    /// <inheritdoc />
    public string Model { get; }

    /// <inheritdoc />
    public object? LayoutMetadata { get; set; }

    /// <summary>
    /// Gets the Id of the device.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the version of the device firmware.
    /// </summary>
    public int Version { get; }

    /// <summary>
    /// Gets the channel this device is using.
    /// </summary>
    public int Channel { get; }

    /// <summary>
    /// Gets the amount of LEDs on this device.
    /// </summary>
    public int LedCount { get; }

    #endregion

    #region Constructors

    internal PicoPiRGBDeviceInfo(RGBDeviceType deviceType, string model, string id, int version, int channel, int ledCount)
    {
        this.DeviceType = deviceType;
        this.Model = model;
        this.Id = id;
        this.Version = version;
        this.Channel = channel;
        this.LedCount = ledCount;

        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, $"{Model} {id} (Channel {channel})");
    }

    #endregion
}