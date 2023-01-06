using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech;

/// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a generic Logitech-device. (keyboard, mouse, headset, mousepad).
/// </summary>
public abstract class LogitechRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, ILogitechRGBDevice
    where TDeviceInfo : LogitechRGBDeviceInfo
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="LogitechRGBDevice{TDeviceInfo}"/> class.
    /// </summary>
    /// <param name="info">The generic information provided by Logitech for the device.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    protected LogitechRGBDevice(TDeviceInfo info, IUpdateQueue updateQueue)
        : base(info, updateQueue)
    { }

    #endregion
}