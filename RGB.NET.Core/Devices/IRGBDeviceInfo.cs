namespace RGB.NET.Core;

/// <summary>
/// Represents a generic information for a <see cref="IRGBDevice"/>
/// </summary>
public interface IRGBDeviceInfo
{
    #region Properties & Fields

    /// <summary>
    /// Gets the <see cref="RGBDeviceType"/> of the <see cref="IRGBDevice"/>.
    /// </summary>
    RGBDeviceType DeviceType { get; }

    /// <summary>
    /// Unique name of the <see cref="IRGBDevice"/>.
    /// </summary>
    string DeviceName { get; }

    /// <summary>
    /// Gets the manufacturer-name of the <see cref="IRGBDevice"/>.
    /// </summary>
    string Manufacturer { get; }

    /// <summary>
    /// Gets the model-name of the <see cref="IRGBDevice"/>.
    /// </summary>
    string Model { get; }

    /// <summary>
    /// Gets custom metadata added to the layout.
    /// </summary>
    object? LayoutMetadata { get; set; }

    #endregion
}