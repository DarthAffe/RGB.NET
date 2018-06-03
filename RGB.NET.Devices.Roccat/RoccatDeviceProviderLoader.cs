using RGB.NET.Core;

namespace RGB.NET.Devices.Roccat
{
    /// <summary>
    /// Represents a device provider loaded used to dynamically load roccat devices into an application.
    /// </summary>
    public class RoccatDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => RoccatDeviceProvider.Instance;

        #endregion
    }
}
