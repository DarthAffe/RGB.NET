using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB;

/// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a generic OpenRGB Device.
/// </summary>
public abstract class AbstractOpenRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IOpenRGBDevice
    where TDeviceInfo : OpenRGBDeviceInfo
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractOpenRGBDevice{TDeviceInfo}"/> class.
    /// </summary>
    /// <param name="info">The generic information provided by OpenRGB for this device.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    protected AbstractOpenRGBDevice(TDeviceInfo info, IUpdateQueue updateQueue)
        : base(info, updateQueue)
    { }

    #endregion

    #region Methods

    private bool Equals(AbstractOpenRGBDevice<TDeviceInfo> other)
    {
        return DeviceInfo.DeviceName == other.DeviceInfo.DeviceName;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is AbstractOpenRGBDevice<TDeviceInfo> other && Equals(other));
    }

    public override int GetHashCode()
    {
        return DeviceInfo.DeviceName.GetHashCode();
    }

    #endregion
}
