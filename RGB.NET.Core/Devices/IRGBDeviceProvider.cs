using System;
using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic device provider.
    /// </summary>
    public interface IRGBDeviceProvider : IDisposable
    {
        #region Properties & Fields

        /// <summary>
        /// Indicates if the used SDK is initialized and ready to use.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Gets a list of <see cref="IRGBDevice"/> loaded by this <see cref="IRGBDeviceProvider"/>.
        /// </summary>
        IEnumerable<IRGBDevice> Devices { get; }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when an exception is thrown in the device provider
        /// </summary>
        event EventHandler<Exception>? Exception;

        #endregion

        #region Methods

        bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool throwExceptions = false);

        #endregion
    }
}
