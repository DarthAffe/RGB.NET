using Windows.Devices.Lights;
using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a DynamicLighting-<see cref="T:RGB.NET.Core.IRGBDevice" />.
/// </summary>
public class DynamicLightingRGBDeviceInfo : IRGBDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public RGBDeviceType DeviceType { get; }

    /// <inheritdoc />
    public string DeviceName { get; }

    /// <inheritdoc />
    public string Manufacturer => "Dynamic Lighting";

    /// <inheritdoc />
    public string Model { get; }

    /// <summary>
    /// Returns the unique ID of the device
    /// </summary>
    public string DeviceId { get; }

    /// <inheritdoc />
    public object? LayoutMetadata { get; set; }

    /// <summary>
    /// Gets the amount of LEDs this device contains.
    /// </summary>
    public int LedCount { get; }

    internal LampArray LampArray { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Internal constructor of managed <see cref="DynamicLightingRGBDeviceInfo"/>.
    /// </summary>
    /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
    /// <param name="lampArrayInfo">The low level information for this device.</param>
    internal DynamicLightingRGBDeviceInfo(RGBDeviceType deviceType, LampArrayInfo lampArrayInfo)
    {
        this.DeviceType = deviceType;

        LampArray = lampArrayInfo.LampArray;
        DeviceId = lampArrayInfo.Id;
        Model = lampArrayInfo.DisplayName;
        LedCount = lampArrayInfo.LampArray.LampCount;

        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    #endregion
}