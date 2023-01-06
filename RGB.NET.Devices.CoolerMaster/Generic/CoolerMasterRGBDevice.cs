using System;
using RGB.NET.Core;
using RGB.NET.Devices.CoolerMaster.Native;

namespace RGB.NET.Devices.CoolerMaster;

/// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a generic CoolerMaster-device. (keyboard, mouse, headset, mousepad).
/// </summary>
public abstract class CoolerMasterRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, ICoolerMasterRGBDevice
    where TDeviceInfo : CoolerMasterRGBDeviceInfo
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CoolerMasterRGBDevice{TDeviceInfo}"/> class.
    /// </summary>
    /// <param name="info">The generic information provided by CoolerMaster for the device.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    protected CoolerMasterRGBDevice(TDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
        : base(info, new CoolerMasterUpdateQueue(updateTrigger, info.DeviceIndex))
    { }

    #endregion

    #region Methods

    /// <inheritdoc cref="IDisposable.Dispose" />
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}.Dispose" />
    public override void Dispose()
    {
        _CoolerMasterSDK.EnableLedControl(false, DeviceInfo.DeviceIndex);

        base.Dispose();
    }

    #endregion
}