using System;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic information for a <see cref="IRGBDevice"/>
    /// </summary>
    public interface IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the <see cref="RGBDeviceType"/> of the <see cref="IRGBDevice"/>.
        /// </summary>
        RGBDeviceType DeviceType { get; }

        /// <summary>
        /// Unique name of the <see cref="IRGBDevice"/>.
        /// </summary>
        string DeviceName { get; }

        /// <summary>
        /// Gets the manufacturer-name of the <see cref="IRGBDevice"/>.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        /// Gets the model-name of the <see cref="IRGBDevice"/>.
        /// </summary>
        string Model { get; }

        /// <summary>
        /// Gets the lighting capability of the <see cref="IRGBDevice"/>
        /// </summary>
        RGBDeviceLighting Lighting { get; }

        /// <summary>
        /// Gets a bool indicating, if the <see cref="IRGBDevice"/> supports SynBacks or not.
        /// </summary>
        bool SupportsSyncBack { get; }

        /// <summary>
        /// Gets the URI of an image of the <see cref="IRGBDevice"/> or null if there is no image.
        /// </summary>
        Uri Image { get; set; }

        #endregion
    }
}
