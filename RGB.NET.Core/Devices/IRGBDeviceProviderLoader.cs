namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic device provider loaded used to dynamically load devices into an application.
    /// This class should always provide an empty public constructor!
    /// </summary>
    public interface IRGBDeviceProviderLoader
    {
        /// <summary>
        /// Indicates if the returned device-provider needs some specific initialization before use.
        /// </summary>
        bool RequiresInitialization { get; }

        /// <summary>
        /// Gets the device-provider.
        /// </summary>
        /// <returns>The device-provider.</returns>
        IRGBDeviceProvider GetDeviceProvider();
    }
}
