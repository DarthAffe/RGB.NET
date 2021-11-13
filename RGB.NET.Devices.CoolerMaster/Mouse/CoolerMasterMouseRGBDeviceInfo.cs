using RGB.NET.Core;

namespace RGB.NET.Devices.CoolerMaster;

/// <inheritdoc />
/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.CoolerMaster.CoolerMasterMouseRGBDevice" />.
/// </summary>
public class CoolerMasterMouseRGBDeviceInfo : CoolerMasterRGBDeviceInfo
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Internal constructor of managed <see cref="T:RGB.NET.Devices.CoolerMaster.CoolerMasterMouseRGBDeviceInfo" />.
    /// </summary>
    /// <param name="deviceIndex">The index of the <see cref="T:RGB.NET.Devices.CoolerMaster.CoolerMasterMouseRGBDevice" />.</param>
    internal CoolerMasterMouseRGBDeviceInfo(CoolerMasterDevicesIndexes deviceIndex)
        : base(RGBDeviceType.Mouse, deviceIndex)
    { }

    #endregion
}