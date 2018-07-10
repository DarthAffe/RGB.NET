using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Debug
{
    /// <inheritdoc />
    /// <summary>
    /// Represents device information for a <see cref="DebugRGBDevice"/> />.
    /// </summary>
    public class DebugRGBDeviceInfo : IRGBDeviceInfo
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
        public RGBDeviceLighting Lighting { get; }

        /// <inheritdoc />
        public bool SupportsSyncBack { get; }

        /// <inheritdoc />
        public Uri Image { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of <see cref="DebugRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceType">The <see cref="RGBDeviceType"/> of the device.</param>
        /// <param name="manufacturer">The manufacturer of the device.</param>
        /// <param name="model">The model of the device.</param>
        /// <param name="lighting">The <see cref="RGBDeviceLighting"/> of the device.</param>
        /// <param name="supportsSyncBack">True if the device supports syncback; false if not.</param>
        internal DebugRGBDeviceInfo(RGBDeviceType deviceType, string manufacturer, string model, RGBDeviceLighting lighting, bool supportsSyncBack, string deviceName = null)
        {
            this.DeviceType = deviceType;
            this.Manufacturer = manufacturer;
            this.Model = model;
            this.Lighting = lighting;
            this.SupportsSyncBack = supportsSyncBack;

            DeviceName = deviceName ?? $"{Manufacturer} {Model}";
        }

        #endregion
    }
}
