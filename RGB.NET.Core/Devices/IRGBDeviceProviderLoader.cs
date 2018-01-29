namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic device provider loaded used to dynamically load devices into an application.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRGBDeviceProviderLoader<T>
        where T : class, IRGBDeviceProviderLoader<T>, new()
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
