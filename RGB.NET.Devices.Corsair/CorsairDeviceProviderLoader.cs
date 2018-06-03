using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    /// <summary>
    /// Represents a device provider loaded used to dynamically load corsair devices into an application.
    /// </summary>
    public class CorsairDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => CorsairDeviceProvider.Instance;

        #endregion
    }
}
