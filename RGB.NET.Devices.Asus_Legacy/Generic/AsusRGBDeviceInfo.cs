using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a Corsair-<see cref="T:RGB.NET.Core.IRGBDevice" />.
    /// </summary>
    public abstract class AsusRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public RGBDeviceType DeviceType { get; }

        /// <inheritdoc />
        public string DeviceName { get; }

        /// <inheritdoc />
        public string Manufacturer { get; }

        /// <inheritdoc />
        public string Model { get; }

        /// <inheritdoc />
        public Uri Image { get; set; }

        /// <inheritdoc />
        public RGBDeviceLighting Lighting => RGBDeviceLighting.Key;

        /// <inheritdoc />
        public abstract bool SupportsSyncBack { get; }

        /// <summary>
        /// Gets the index of the <see cref="AsusRGBDevice{TDeviceInfo}"/>.
        /// </summary>
        internal IntPtr Handle { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="AsusRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="handle">The handle of the <see cref="IRGBDevice"/>.</param>
        /// <param name="manufacturer">The manufacturer-name of the <see cref="IRGBDevice"/>.</param>
        /// <param name="model">The model-name of the <see cref="IRGBDevice"/>.</param>
        internal AsusRGBDeviceInfo(RGBDeviceType deviceType, IntPtr handle, string model = "Generic Asus-Device", string manufacturer = "Asus")
        {
            this.DeviceType = deviceType;
            this.Handle = handle;
            this.Model = model;
            this.Manufacturer = manufacturer;

            DeviceName = $"{Manufacturer} {Model}";
        }

        #endregion
    }
}
