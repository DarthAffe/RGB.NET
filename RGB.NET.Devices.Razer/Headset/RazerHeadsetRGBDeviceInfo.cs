using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Razer.RazerHeadsetRGBDevice" />.
    /// </summary>
    public class RazerHeadsetRGBDeviceInfo : RazerRGBDeviceInfo
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Razer.RazerHeadsetRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceId">The Id of the <see cref="IRGBDevice"/>.</param>
        /// <param name="model">The model of the <see cref="IRGBDevice"/>.</param>
        internal RazerHeadsetRGBDeviceInfo(Guid deviceId, string model)
            : base(deviceId, RGBDeviceType.Headset, model)
        { }

        #endregion
    }
}
