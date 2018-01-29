using RGB.NET.Core;

namespace RGB.NET.Devices.Debug
{
    /// <summary>
    /// Represents a device provider loaded used to dynamically load debug devices into an application.
    /// </summary>
    public class DebugDeviceProviderLoader : IRGBDeviceProviderLoader<DebugDeviceProviderLoader>
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => DebugDeviceProvider.Instance;

        #endregion
    }
}
