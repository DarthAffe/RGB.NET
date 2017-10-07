using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Aura
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a Corsair-<see cref="T:RGB.NET.Core.IRGBDevice" />.
    /// </summary>
    public class AuraRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public RGBDeviceType DeviceType { get; }

        /// <inheritdoc />
        public string Manufacturer { get; }

        /// <inheritdoc />
        public string Model { get; }

        /// <inheritdoc />
        public Uri Image { get; protected set; }

        /// <inheritdoc />
        public RGBDeviceLighting Lighting => RGBDeviceLighting.Key;

        /// <summary>
        /// Gets the index of the <see cref="AuraRGBDevice"/>.
        /// </summary>
        internal IntPtr Handle { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="AuraRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="handle">The handle of the <see cref="IRGBDevice"/>.</param>
        /// <param name="manufacturer">The manufacturer-name of the <see cref="IRGBDevice"/>.</param>
        /// <param name="model">The model-name of the <see cref="IRGBDevice"/>.</param>
        internal AuraRGBDeviceInfo(RGBDeviceType deviceType, IntPtr handle, string manufacturer = "Unknown", string model = "Generic Aura-Device")
        {
            this.DeviceType = deviceType;
            this.Handle = handle;
            this.Manufacturer = manufacturer;
            this.Model = model;
        }

        #endregion
    }
}
