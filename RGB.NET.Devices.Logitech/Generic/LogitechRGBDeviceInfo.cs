using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a Logitech-<see cref="T:RGB.NET.Core.IRGBDevice" />.
    /// </summary>
    public class LogitechRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public RGBDeviceType DeviceType { get; }

        /// <inheritdoc />
        public string DeviceName { get; }

        /// <inheritdoc />
        public string Manufacturer => "Logitech";

        /// <inheritdoc />
        public string Model { get; }

        /// <inheritdoc />
        public Uri Image { get; set; }

        /// <inheritdoc />
        public RGBDeviceLighting Lighting
        {
            get
            {
                if (DeviceCaps.HasFlag(LogitechDeviceCaps.PerKeyRGB))
                    return RGBDeviceLighting.Key;

                if (DeviceCaps.HasFlag(LogitechDeviceCaps.DeviceRGB))
                    return RGBDeviceLighting.Device;

                return RGBDeviceLighting.None;
            }
        }

        /// <inheritdoc />
        public bool SupportsSyncBack => false;

        /// <summary>
        /// Gets a flag that describes device capabilities. (<see cref="LogitechDeviceCaps" />)
        /// </summary>
        public LogitechDeviceCaps DeviceCaps { get; }

        /// <summary>
        /// Gets the amount of zones the <see cref="LogitechRGBDevice{TDeviceInfo}"/> is able to control (0 for single-color and per-key devices)
        /// </summary>
        public int Zones { get; }

        /// <summary>
        /// Gets the layout used to decide which images to load.
        /// </summary>
        internal string ImageLayout { get; }

        /// <summary>
        /// Gets the path/name of the layout-file.
        /// </summary>
        internal string LayoutPath { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="LogitechRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="model">The represented device model.</param>
        /// <param name="deviceCaps">The lighting-capabilities of the device.</param>
        /// <param name="zones">The amount of zones the device is able to control.</param>
        /// <param name="imageLayout">The layout used to decide which images to load.</param>
        /// <param name="layoutPath">The path/name of the layout-file.</param>
        internal LogitechRGBDeviceInfo(RGBDeviceType deviceType, string model, LogitechDeviceCaps deviceCaps,
                                       int zones, string imageLayout, string layoutPath)
        {
            this.DeviceType = deviceType;
            this.Model = model;
            this.DeviceCaps = deviceCaps;
            this.Zones = zones;
            this.ImageLayout = imageLayout;
            this.LayoutPath = layoutPath;

            DeviceName = $"{Manufacturer} {Model}";
        }

        #endregion
    }
}
