using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech
{
    /// <summary>
    /// Represents a device provider loaded used to dynamically load logitech devices into an application.
    /// </summary>
    public class LogitechDeviceProviderLoader : IRGBDeviceProviderLoader<LogitechDeviceProviderLoader>
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => LogitechDeviceProvider.Instance;

        #endregion
    }
}
