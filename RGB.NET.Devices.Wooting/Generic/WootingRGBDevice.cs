using RGB.NET.Core;

namespace RGB.NET.Devices.Wooting.Generic;

/// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
/// <inheritdoc cref="IWootingRGBDevice" />
/// <summary>
/// Represents a Wooting-device
/// </summary>
public abstract class WootingRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IWootingRGBDevice
    where TDeviceInfo : WootingRGBDeviceInfo
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WootingRGBDevice{TDeviceInfo}"/> class.
    /// </summary>
    /// <param name="info">The generic information provided by Wooting for the device.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    protected WootingRGBDevice(TDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
        : base(info, new WootingUpdateQueue(updateTrigger))
    { }

    #endregion
}