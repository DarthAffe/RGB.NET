using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.Bitwizard;

// ReSharper disable once InconsistentNaming
/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.WS281X.Bitwizard.BitwizardWS2812USBDeviceInfo" />.
/// </summary>
public class BitwizardWS2812USBDeviceInfo : IRGBDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public string DeviceName { get; }

    /// <inheritdoc />
    public RGBDeviceType DeviceType => RGBDeviceType.LedStripe;

    /// <inheritdoc />
    public string Manufacturer => "Bitwizard";

    /// <inheritdoc />
    public string Model => "WS2812 USB";

    /// <inheritdoc />
    public object? LayoutMetadata { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BitwizardWS2812USBDeviceInfo"/> class.
    /// </summary>
    /// <param name="name">The name of this device.</param>
    public BitwizardWS2812USBDeviceInfo(string name)
    {
        this.DeviceName = name;
    }

    #endregion
}