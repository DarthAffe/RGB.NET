using RGB.NET.Core;

namespace RGB.NET.Devices.Novation
{
    /// <summary>
    /// Represents a device provider loaded used to dynamically load novation devices into an application.
    /// </summary>
    public class NovationDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => NovationDeviceProvider.Instance;

        #endregion
    }
}
