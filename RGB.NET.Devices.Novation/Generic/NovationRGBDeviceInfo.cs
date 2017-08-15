using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Novation
{
    /// <summary>
    /// Represents a generic information for a Corsair-<see cref="IRGBDevice"/>.
    /// </summary>
    public class NovationRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public RGBDeviceType DeviceType { get; }

        /// <inheritdoc />
        public string Manufacturer => "Novation";

        /// <inheritdoc />
        public string Model { get; }

        /// <inheritdoc />
        public Uri Image { get; protected set; }

        /// <inheritdoc />
        public RGBDeviceLighting Lighting => RGBDeviceLighting.Key;

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="NovationRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="model">The represented device model.</param>
        internal NovationRGBDeviceInfo(RGBDeviceType deviceType, string model)
        {
            this.DeviceType = deviceType;
            this.Model = model;
        }

        #endregion
    }
}
