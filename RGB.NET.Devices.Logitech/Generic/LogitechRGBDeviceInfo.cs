using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a Logitech-<see cref="T:RGB.NET.Core.IRGBDevice" />.
/// </summary>
public class LogitechRGBDeviceInfo : IRGBDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public RGBDeviceType DeviceType { get; }

    /// <inheritdoc />
    public string DeviceName { get; }

    /// <inheritdoc />
    public string Manufacturer => "Logitech";

    /// <inheritdoc />
    public string Model { get; }

    /// <inheritdoc />
    public object? LayoutMetadata { get; set; }

    /// <summary>
    /// Gets a flag that describes device capabilities. (<see cref="LogitechDeviceCaps" />)
    /// </summary>
    public LogitechDeviceCaps DeviceCaps { get; }

    /// <summary>
    /// Gets the amount of zones the <see cref="LogitechRGBDevice{TDeviceInfo}"/> is able to control (0 for single-color and per-key devices)
    /// </summary>
    public int Zones { get; }

    /// <summary>
    /// Gets the zone at which LEDs start being mapped
    /// </summary>
    public int ZoneOffset { get; }
    
    #endregion

    #region Constructors

    /// <summary>
    /// Internal constructor of managed <see cref="LogitechRGBDeviceInfo"/>.
    /// </summary>
    /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
    /// <param name="model">The represented device model.</param>
    /// <param name="deviceCaps">The lighting-capabilities of the device.</param>
    /// <param name="zones">The amount of zones the device is able to control.</param>
    /// <param name="zoneOffset">The zone at which to start mapping LEDs.</param>
    internal LogitechRGBDeviceInfo(RGBDeviceType deviceType, string model, LogitechDeviceCaps deviceCaps, int zones, int zoneOffset = 0)
    {
        this.DeviceType = deviceType;
        this.Model = model;
        this.DeviceCaps = deviceCaps;
        this.Zones = zones;
        this.ZoneOffset = zoneOffset;

        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    #endregion
}