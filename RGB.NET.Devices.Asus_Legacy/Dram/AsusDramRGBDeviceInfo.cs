using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Asus.AsusDramRGBDevice" />.
    /// </summary>
    public class AsusDramRGBDeviceInfo : AsusRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public override bool SupportsSyncBack => true;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Asus.AsusDramRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="handle">The handle of the <see cref="IRGBDevice"/>.</param>
        internal AsusDramRGBDeviceInfo(RGBDeviceType deviceType, IntPtr handle)
            : base(deviceType, handle)
        { }

        #endregion
    }
}
