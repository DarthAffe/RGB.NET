using RGB.NET.Core;

namespace RGB.NET.Devices.PlayStation;

/// <inheritdoc />
/// <summary>
/// Represents a generic device-info for a PlayStation controller.
/// </summary>
public sealed class PlayStationDeviceInfo : IRGBDeviceInfo
{
    #region Properties & Fields

    /// <summary>Gets the controller family (DualShock 4 / DualSense / DualSense Edge).</summary>
    public PlayStationControllerType ControllerType { get; }

    /// <summary>Gets the transport the controller is connected by.</summary>
    public PlayStationTransport Transport { get; }

    /// <summary>
    /// Gets a stable per-controller identifier derived from the OS device path. Used
    /// to disambiguate two same-model controllers connected at the same time.
    /// </summary>
    public string SerialNumber { get; }

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

    internal PlayStationDeviceInfo(PlayStationControllerType controllerType, PlayStationTransport transport, string serialNumber)
    {
        this.ControllerType = controllerType;
        this.Transport = transport;
        this.SerialNumber = serialNumber ?? string.Empty;

        this.DeviceType = RGBDeviceType.GameController;
        this.Manufacturer = "Sony";
        this.Model = controllerType switch
        {
            PlayStationControllerType.DualShock4 => "DualShock 4",
            PlayStationControllerType.DualSense => "DualSense",
            PlayStationControllerType.DualSenseEdge => "DualSense Edge",
            _ => "PlayStation Controller",
        };

        // Including transport + serial in DeviceName gives every physical controller
        // a stable identity that survives app restarts and disambiguates two same-
        // model controllers without depending on enumeration order. Trade-off:
        // switching the same controller from USB to BT produces a new name, so any
        // host-side state keyed by DeviceName won't auto-apply across transports.
        string transportTag = transport == PlayStationTransport.Bluetooth ? "BT" : "USB";
        string serialTag = !string.IsNullOrEmpty(SerialNumber) ? $" [{SerialNumber}]" : string.Empty;
        this.DeviceName = $"{Model} ({transportTag}){serialTag}";
    }

    #endregion
}
