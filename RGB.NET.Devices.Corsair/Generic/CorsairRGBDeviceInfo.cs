using System.Text.RegularExpressions;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a Corsair-<see cref="T:RGB.NET.Core.IRGBDevice" />.
/// </summary>
public class CorsairRGBDeviceInfo : IRGBDeviceInfo
{
    #region Properties & Fields

    /// <summary>
    /// Gets the corsair specific device type.
    /// </summary>
    public CorsairDeviceType CorsairDeviceType { get; }

    /// <inheritdoc />
    public RGBDeviceType DeviceType { get; }

    /// <inheritdoc />
    public string DeviceName { get; }

    /// <inheritdoc />
    public string Manufacturer => "Corsair";

    /// <inheritdoc />
    public string Model { get; }

    /// <summary>
    /// Returns the unique ID provided by the Corsair-SDK.
    /// Returns string.Empty for Custom devices.
    /// </summary>
    public string DeviceId { get; }

    /// <inheritdoc />
    public object? LayoutMetadata { get; set; }
    
    /// <summary>
    /// Gets the amount of LEDs this device contains.
    /// </summary>
    public int LedCount { get; }

    /// <summary>
    /// Gets the offset used to access the LEDs of this device.
    /// </summary>
    internal int LedOffset { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Internal constructor of managed <see cref="CorsairRGBDeviceInfo"/>.
    /// </summary>
    /// <param name="deviceIndex">The index of the <see cref="CorsairRGBDevice{TDeviceInfo}"/>.</param>
    /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
    /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
    internal CorsairRGBDeviceInfo(RGBDeviceType deviceType, _CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset)
    {
        this.DeviceType = deviceType;
        this.CorsairDeviceType = nativeInfo.type;
        this.Model = nativeInfo.model == null ? string.Empty : Regex.Replace(nativeInfo.model ?? string.Empty, " ?DEMO", string.Empty, RegexOptions.IgnoreCase);
        this.DeviceId = nativeInfo.id ?? string.Empty;
        this.LedCount = ledCount;
        this.LedOffset = ledOffset;

        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    /// <summary>
    /// Internal constructor of managed <see cref="CorsairRGBDeviceInfo"/>.
    /// </summary>
    /// <param name="deviceIndex">The index of the <see cref="CorsairRGBDevice{TDeviceInfo}"/>.</param>
    /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
    /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
    /// <param name="modelName">The name of the device-model (overwrites the one provided with the device info).</param>
    internal CorsairRGBDeviceInfo(RGBDeviceType deviceType, _CorsairDeviceInfo nativeInfo, int ledCount, int ledOffset, string modelName)
    {
        this.DeviceType = deviceType;
        this.CorsairDeviceType = nativeInfo.type;
        this.Model = modelName;
        this.DeviceId = nativeInfo.id ?? string.Empty;
        this.LedCount = ledCount;
        this.LedOffset = ledOffset;

        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    #endregion
}