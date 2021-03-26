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

        /// <inheritdoc />
        public RGBDeviceType DeviceType { get; }

        /// <inheritdoc />
        public string DeviceName { get; }

        /// <inheritdoc />
        public string Manufacturer => "Razer";

        /// <inheritdoc />
        public string Model { get; }

        /// <inheritdoc />
        public object? LayoutMetadata { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="RazerRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="model">The model of the <see cref="IRGBDevice"/>.</param>
        internal RazerRGBDeviceInfo(RGBDeviceType deviceType, string model)
        {
            this.DeviceType = deviceType;
            this.Model = model;

            DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
        }

        #endregion
    }
}
