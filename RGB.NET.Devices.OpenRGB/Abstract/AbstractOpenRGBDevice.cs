using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a generic OpenRGB Device.
    /// </summary>
    public abstract class AbstractOpenRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IOpenRGBDevice
        where TDeviceInfo : AbstractOpenRGBDeviceInfo
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
    }
}
