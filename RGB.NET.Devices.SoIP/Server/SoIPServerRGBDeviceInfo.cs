using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.SoIP.Server
{
    /// <inheritdoc />
    /// <summary>
    /// Represents device information for a <see cref="SoIPServerRGBDevice"/> />.
    /// </summary>
    public class SoIPServerRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public object DeviceId => Port;

        /// <inheritdoc />
        public RGBDeviceType DeviceType => RGBDeviceType.Unknown;

        /// <inheritdoc />
        public string Manufacturer { get; }

        /// <inheritdoc />
        public string Model { get; }

        /// <inheritdoc />
        public RGBDeviceLighting Lighting => RGBDeviceLighting.Key;

        /// <inheritdoc />
        public bool SupportsSyncBack => false;

        /// <inheritdoc />
        public Uri Image { get; set; }

        /// <summary>
        /// The port of the device. 
        /// </summary>
        public int Port { get; }

        #endregion

        #region Constructors

        internal SoIPServerRGBDeviceInfo(SoIPServerDeviceDefinition deviceDefinition)
        {
            this.Manufacturer = deviceDefinition.Manufacturer;
            this.Model = deviceDefinition.Model;
            this.Port = deviceDefinition.Port;
        }

        #endregion
    }
}
