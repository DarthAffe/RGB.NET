using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.SoIP.Client
{
    /// <inheritdoc />
    /// <summary>
    /// Represents device information for a <see cref="SoIPClientRGBDevice"/> />.
    /// </summary>
    public class SoIPClientRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public RGBDeviceType DeviceType => RGBDeviceType.Unknown;

        /// <inheritdoc />
        public string Manufacturer { get; }

        /// <inheritdoc />
        public string Model { get; }

        /// <inheritdoc />
        public RGBDeviceLighting Lighting => RGBDeviceLighting.None;

        /// <inheritdoc />
        public bool SupportsSyncBack => true;

        /// <inheritdoc />
        public Uri Image { get; set; }

        /// <summary>
        /// The hostname of the device.
        /// </summary>
        public string Hostname { get; }

        /// <summary>
        /// The port of the device. 
        /// </summary>
        public int Port { get; }

        #endregion

        #region Constructors

        internal SoIPClientRGBDeviceInfo(SoIPClientDeviceDefinition deviceDefinition)
        {
            this.Manufacturer = deviceDefinition.Manufacturer;
            this.Model = deviceDefinition.Model;
            this.Hostname = deviceDefinition.Hostname;
            this.Port = deviceDefinition.Port;
        }

        #endregion
    }
}
