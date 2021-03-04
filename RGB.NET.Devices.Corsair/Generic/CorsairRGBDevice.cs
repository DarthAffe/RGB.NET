using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a generic CUE-device. (keyboard, mouse, headset, mousepad).
    /// </summary>
    public abstract class CorsairRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, ICorsairRGBDevice
        where TDeviceInfo : CorsairRGBDeviceInfo
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairRGBDevice{TDeviceInfo}"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by CUE for the device.</param>
        protected CorsairRGBDevice(TDeviceInfo info, CorsairDeviceUpdateQueue updateQueue)
            : base(info, updateQueue)
        { }

        #endregion
    }
}
