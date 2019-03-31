using RGB.NET.Core;

namespace RGB.NET.Devices.SteelSeries
{
    /// <summary>
    /// Represents a device provider loaded used to dynamically load steelseries devices into an application.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class SteelSeriesDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => SteelSeriesDeviceProvider.Instance;

        #endregion
    }
}
