using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <summary>
    /// Represents a device provider loaded used to dynamically load asus devices into an application.
    /// </summary>
    public class AsusDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => AsusDeviceProvider.Instance;

        #endregion
    }
}
