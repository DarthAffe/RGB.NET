using System;
using System.Runtime.InteropServices;
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

    /// <summary>
    /// Gets the index of the <see cref="CorsairRGBDevice{TDeviceInfo}"/>.
    /// </summary>
    public int CorsairDeviceIndex { get; }

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
    /// Gets a flag that describes device capabilities. (<see cref="CorsairDeviceCaps" />)
    /// </summary>
    public CorsairDeviceCaps CapsMask { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Internal constructor of managed <see cref="CorsairRGBDeviceInfo"/>.
    /// </summary>
    /// <param name="deviceIndex">The index of the <see cref="CorsairRGBDevice{TDeviceInfo}"/>.</param>
    /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
    /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
    internal CorsairRGBDeviceInfo(int deviceIndex, RGBDeviceType deviceType, _CorsairDeviceInfo nativeInfo)
    {
        this.CorsairDeviceIndex = deviceIndex;
        this.DeviceType = deviceType;
        this.CorsairDeviceType = nativeInfo.type;
        this.Model = nativeInfo.model == IntPtr.Zero ? string.Empty : Regex.Replace(Marshal.PtrToStringAnsi(nativeInfo.model) ?? string.Empty, " ?DEMO", string.Empty, RegexOptions.IgnoreCase);
        this.DeviceId = nativeInfo.deviceId ?? string.Empty;
        this.CapsMask = (CorsairDeviceCaps)nativeInfo.capsMask;

        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    /// <summary>
    /// Internal constructor of managed <see cref="CorsairRGBDeviceInfo"/>.
    /// </summary>
    /// <param name="deviceIndex">The index of the <see cref="CorsairRGBDevice{TDeviceInfo}"/>.</param>
    /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
    /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
    /// <param name="modelName">The name of the device-model (overwrites the one provided with the device info).</param>
    internal CorsairRGBDeviceInfo(int deviceIndex, RGBDeviceType deviceType, _CorsairDeviceInfo nativeInfo, string modelName)
    {
        this.CorsairDeviceIndex = deviceIndex;
        this.DeviceType = deviceType;
        this.CorsairDeviceType = nativeInfo.type;
        this.Model = modelName;
        this.DeviceId = nativeInfo.deviceId ?? string.Empty;
        this.CapsMask = (CorsairDeviceCaps)nativeInfo.capsMask;

        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    #endregion
}