using RGB.NET.Core;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <summary>
    /// Represents a device provider loaded used to dynamically load cooler-master devices into an application.
    /// </summary>
    public class CoolerMasterDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => CoolerMasterDeviceProvider.Instance;

        #endregion
    }
}
