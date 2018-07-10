using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a Razer-<see cref="T:RGB.NET.Core.IRGBDevice" />.
    /// </summary>
    public class RazerRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the Id of the <see cref="RazerRGBDevice{TDeviceInfo}"/>.
        /// </summary>
        public Guid DeviceId { get; }

        /// <inheritdoc />
        public RGBDeviceType DeviceType { get; }

        /// <inheritdoc />
        public string DeviceName { get; }

        /// <inheritdoc />
        public string Manufacturer => "Razer";

        /// <inheritdoc />
        public string Model { get; }

        /// <inheritdoc />
        public Uri Image { get; set; }

        /// <inheritdoc />
        public bool SupportsSyncBack => false;

        /// <inheritdoc />
        public RGBDeviceLighting Lighting => RGBDeviceLighting.Key;

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="RazerRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceId">The Id of the <see cref="IRGBDevice"/>.</param>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="model">The model of the <see cref="IRGBDevice"/>.</param>
        internal RazerRGBDeviceInfo(Guid deviceId, RGBDeviceType deviceType, string model)
        {
            this.DeviceId = deviceId;
            this.DeviceType = deviceType;
            this.Model = model;

            DeviceName = $"{Manufacturer} {Model}";
        }

        #endregion
    }
}
