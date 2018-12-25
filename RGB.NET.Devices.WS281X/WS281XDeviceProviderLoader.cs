using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider loaded used to dynamically load WS281X devices into an application.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    public class WS281XDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => true;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => WS281XDeviceProvider.Instance;

        #endregion
    }
}
