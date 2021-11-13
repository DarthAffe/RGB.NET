using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.CoolerMaster;

/// <inheritdoc />
/// <summary>
/// Specifies the <see cref="T:RGB.NET.Core.RGBDeviceType" /> of a field.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class DeviceTypeAttribute : Attribute
{
    #region Properties & Fields

    /// <summary>
    /// Gets the <see cref="RGBDeviceType"/>.
    /// </summary>
    public RGBDeviceType DeviceType { get; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Internal constructor of the <see cref="T:RGB.NET.Devices.CoolerMaster.DeviceTypeAttribute" /> class.
    /// </summary>
    /// <param name="deviceType">The <see cref="T:RGB.NET.Core.RGBDeviceType" />.</param>
    public DeviceTypeAttribute(RGBDeviceType deviceType)
    {
        this.DeviceType = deviceType;
    }

    #endregion
}