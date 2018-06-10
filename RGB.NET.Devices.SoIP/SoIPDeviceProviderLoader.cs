using RGB.NET.Core;

namespace RGB.NET.Devices.SoIP
{
    /// <summary>
    /// Represents a device provider loaded used to dynamically load SoIP (syncback over IP) devices into an application.
    /// </summary>
    public class SoIPDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => true;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => SoIPDeviceProvider.Instance;

        #endregion
    }
}
