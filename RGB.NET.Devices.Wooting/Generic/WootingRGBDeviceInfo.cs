using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Enum;
using RGB.NET.Devices.Wooting.Native;

namespace RGB.NET.Devices.Wooting.Generic;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a Wooting-<see cref="T:RGB.NET.Core.IRGBDevice" />.
/// </summary>
public class WootingRGBDeviceInfo : IRGBDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public RGBDeviceType DeviceType { get; }

    /// <inheritdoc />
    public string DeviceName { get; }

    /// <inheritdoc />
    public string Manufacturer => "Wooting";

    /// <inheritdoc />
    public string Model { get; }

    /// <inheritdoc />
    public object? LayoutMetadata { get; set; }

    /// <summary>
    /// Gets the <see cref="Enum.WootingDeviceType"/> of the <see cref="WootingRGBDevice{TDeviceInfo}"/>.
    /// </summary>
    public WootingDeviceType WootingDeviceType { get; }

    /// <summary>
    /// Gets the <see cref="Enum.WootingLayoutType"/> of the <see cref="WootingRGBDevice{TDeviceInfo}"/>.
    /// </summary>
    public WootingLayoutType WootingLayoutType { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Internal constructor of managed <see cref="WootingRGBDeviceInfo"/>.
    /// </summary>
    /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
    /// <param name="deviceInfo">The <see cref="_WootingDeviceInfo"/> of the <see cref="IRGBDevice"/>.</param>
    internal WootingRGBDeviceInfo(RGBDeviceType deviceType, _WootingDeviceInfo deviceInfo)
    {
        this.DeviceType = deviceType;
        this.WootingDeviceType = deviceInfo.DeviceType;
        this.WootingLayoutType = deviceInfo.LayoutType;

        Model = deviceInfo.Model;
        DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
    }

    #endregion
}