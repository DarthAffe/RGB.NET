using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech
{
    /// <summary>
    /// Represents a generic information for a Logitech-<see cref="IRGBDevice"/>.
    /// </summary>
    public class LogitechRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public RGBDeviceType DeviceType { get; }

        /// <inheritdoc />
        public string Manufacturer => "Logitech";

        /// <inheritdoc />
        public string Model { get; }

        /// <inheritdoc />
        public Uri Image { get; protected set; }

        /// <inheritdoc />
        public RGBDeviceLighting Lighting
        {
            get
            {
                if (DeviceCaps.HasFlag(LogitechDeviceCaps.PerKeyRGB))
                    return RGBDeviceLighting.Key;

                if (DeviceCaps.HasFlag(LogitechDeviceCaps.DeviceRGB))
                    return RGBDeviceLighting.Keyboard;

                return RGBDeviceLighting.None;
            }
        }

        /// <summary>
        /// Gets a flag that describes device capabilities. (<see cref="LogitechDeviceCaps" />)
        /// </summary>
        public LogitechDeviceCaps DeviceCaps { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="LogitechRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="model">The represented device model.</param>
        /// <param name="deviceCaps">The lighting-capabilities of the device.</param>
        internal LogitechRGBDeviceInfo(RGBDeviceType deviceType, string model, LogitechDeviceCaps deviceCaps)
        {
            this.DeviceType = deviceType;
            this.Model = model;
            this.DeviceCaps = deviceCaps;
        }

        #endregion
    }
}
