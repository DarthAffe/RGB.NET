using System;
using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Enum;
using RGB.NET.Devices.Wooting.Helper;

namespace RGB.NET.Devices.Wooting.Generic
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a Wooting-<see cref="T:RGB.NET.Core.IRGBDevice" />.
    /// </summary>
    public class WootingRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public RGBDeviceType DeviceType { get; }

        /// <inheritdoc />
        public string DeviceName { get; }

        /// <inheritdoc />
        public string Manufacturer => "Wooting";

        /// <inheritdoc />
        public string Model { get; }

        /// <inheritdoc />
        public Uri Image { get; set; }

        /// <inheritdoc />
        public RGBDeviceLighting Lighting => RGBDeviceLighting.Key;

        /// <inheritdoc />
        public bool SupportsSyncBack => false;

        /// <summary>
        /// Gets the <see cref="WootingDevicesIndexes"/> of the <see cref="WootingRGBDevice{TDeviceInfo}"/>.
        /// </summary>
        public WootingDevicesIndexes DeviceIndex { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="WootingRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="deviceIndex">The <see cref="WootingDevicesIndexes"/> of the <see cref="IRGBDevice"/>.</param>
        internal WootingRGBDeviceInfo(RGBDeviceType deviceType, WootingDevicesIndexes deviceIndex)
        {
            this.DeviceType = deviceType;
            this.DeviceIndex = deviceIndex;

            Model = deviceIndex.GetDescription();
            DeviceName = $"{Manufacturer} {Model}";
        }

        #endregion
    }
}
