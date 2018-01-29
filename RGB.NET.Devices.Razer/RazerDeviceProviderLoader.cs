using RGB.NET.Core;

namespace RGB.NET.Devices.Razer
{
    /// <summary>
    /// Represents a device provider loaded used to dynamically load razer devices into an application.
    /// </summary>
    public class RazerDeviceProviderLoader : IRGBDeviceProviderLoader<RazerDeviceProviderLoader>
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => RazerDeviceProvider.Instance;

        #endregion
    }
}
