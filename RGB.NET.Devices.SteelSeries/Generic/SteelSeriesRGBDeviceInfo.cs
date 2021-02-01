using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.SteelSeries
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a SteelSeries-<see cref="T:RGB.NET.Core.IRGBDevice" />.
    /// </summary>
    public class SteelSeriesRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public RGBDeviceType DeviceType { get; }

        /// <inheritdoc />
        public string DeviceName { get; }

        /// <inheritdoc />
        public string Manufacturer => "SteelSeries";

        /// <inheritdoc />
        public string Model { get; }

        /// <inheritdoc />
        public object? LayoutMetadata { get; set; }

        public SteelSeriesDeviceType SteelSeriesDeviceType { get; }

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
        /// Internal constructor of managed <see cref="SteelSeriesRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="model">The represented device model.</param>
        /// <param name="deviceCaps">The lighting-capabilities of the device.</param>
        /// <param name="imageLayout">The layout used to decide which images to load.</param>
        /// <param name="layoutPath">The path/name of the layout-file.</param>
        internal SteelSeriesRGBDeviceInfo(RGBDeviceType deviceType, string model, SteelSeriesDeviceType steelSeriesDeviceType, string imageLayout, string layoutPath)
        {
            this.DeviceType = deviceType;
            this.Model = model;
            this.SteelSeriesDeviceType = steelSeriesDeviceType;
            this.ImageLayout = imageLayout;
            this.LayoutPath = layoutPath;

            DeviceName = $"{Manufacturer} {Model}";
        }

        #endregion
    }
}
