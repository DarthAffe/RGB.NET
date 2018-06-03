using RGB.NET.Core;

namespace RGB.NET.Devices.DMX
{
    /// <summary>
    /// Represents a device provider loaded used to dynamically load DMX devices into an application.
    /// </summary>
    public class DMXDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => true;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => DMXDeviceProvider.Instance;

        #endregion
    }
}
