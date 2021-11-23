using RGB.NET.Core;

namespace RGB.NET.Devices.Asus;

/// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a generic Asus-device. (keyboard, mouse, headset, mousepad).
/// </summary>
public abstract class AsusRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IAsusRGBDevice
    where TDeviceInfo : AsusRGBDeviceInfo
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AsusRGBDevice{TDeviceInfo}"/> class.
    /// </summary>
    /// <param name="info">The generic information provided by Asus for the device.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    protected AsusRGBDevice(TDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
        : base(info, new AsusUpdateQueue(updateTrigger, info.Device))
    { }

    #endregion
}