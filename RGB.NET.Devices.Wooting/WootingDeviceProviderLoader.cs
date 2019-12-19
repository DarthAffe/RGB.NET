using RGB.NET.Core;

namespace RGB.NET.Devices.Wooting
{
    /// <summary>
    /// Represents a device provider loaded used to dynamically load Wooting devices into an application.
    /// </summary>
    public class WootingDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => WootingDeviceProvider.Instance;

        #endregion
    }
}
