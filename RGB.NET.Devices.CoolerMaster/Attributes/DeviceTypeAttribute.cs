using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.CoolerMaster.Attributes
{
    /// <summary>
    /// Specifies the <see cref="RGBDeviceType"/> of a field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DeviceTypeAttribute : Attribute
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the <see cref="RGBDeviceType"/>.
        /// </summary>
        public RGBDeviceType DeviceType { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of the <see cref="DeviceTypeAttribute"/> class.
        /// </summary>
        /// <param name="deviceType">The <see cref="RGBDeviceType"/>.</param>
        public DeviceTypeAttribute(RGBDeviceType deviceType)
        {
            this.DeviceType = deviceType;
        }

        #endregion
    }
}
