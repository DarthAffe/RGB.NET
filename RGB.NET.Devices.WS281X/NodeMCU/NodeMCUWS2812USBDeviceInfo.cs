using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.NodeMCU;

// ReSharper disable once InconsistentNaming
/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.WS281X.NodeMCU.NodeMCUWS2812USBDevice" />.
/// </summary>
public class NodeMCUWS2812USBDeviceInfo : IRGBDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public string DeviceName { get; }

    /// <inheritdoc />
    public RGBDeviceType DeviceType => RGBDeviceType.LedStripe;

    /// <inheritdoc />
    public string Manufacturer => "NodeMCU";

    /// <inheritdoc />
    public string Model => "WS2812 WLAN";

    /// <inheritdoc />
    public object? LayoutMetadata { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="NodeMCUWS2812USBDeviceInfo"/> class.
    /// </summary>
    /// <param name="name">The name of this device.</param>
    public NodeMCUWS2812USBDeviceInfo(string name)
    {
        this.DeviceName = name;
    }

    #endregion
}