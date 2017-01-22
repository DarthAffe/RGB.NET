namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic information for a <see cref="IRGBDevice"/>
    /// </summary>
    public interface IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the <see cref="Core.DeviceType"/> of the <see cref="IRGBDevice"/>.
        /// </summary>
        DeviceType DeviceType { get; }

        /// <summary>
        /// Gets the manufacturer-name of the <see cref="IRGBDevice"/>.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        /// Gets the model-name of the <see cref="IRGBDevice"/>.
        /// </summary>
        string Model { get; }

        #endregion

        #region Methods

        #endregion
    }
}
