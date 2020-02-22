using RGB.NET.Core;

namespace RGB.NET.Devices.Msi
{
    /// <summary>
    /// Represents a device provider loaded used to dynamically load MSI devices into an application.
    /// </summary>
    public class MsiDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => MsiDeviceProvider.Instance;

        #endregion
    }
}
