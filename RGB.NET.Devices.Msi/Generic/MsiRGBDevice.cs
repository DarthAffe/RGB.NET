using RGB.NET.Core;

namespace RGB.NET.Devices.Msi;

/// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a generic MSI-device. (keyboard, mouse, headset, mousepad).
/// </summary>
public abstract class MsiRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IMsiRGBDevice
    where TDeviceInfo : MsiRGBDeviceInfo
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MsiRGBDevice{TDeviceInfo}"/> class.
    /// </summary>
    /// <param name="info">The generic information provided by MSI for the device.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    protected MsiRGBDevice(TDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
        : base(info, new MsiDeviceUpdateQueue(updateTrigger, info.MsiDeviceType))
    { }

    #endregion
}