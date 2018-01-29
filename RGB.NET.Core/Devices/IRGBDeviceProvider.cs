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

        /// <summary>
        /// Gets whether the application has exclusive access to devices or not.
        /// </summary>
        bool HasExclusiveAccess { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the <see cref="IRGBDeviceProvider"/> if not already happened or reloads it if it is already initialized.
        /// </summary>
        /// <param name="loadFilter">Specifies which types of devices to load.</param>
        /// <param name="exclusiveAccessIfPossible">Specifies whether the application should request exclusive access of possible or not.</param>
        /// <param name="throwExceptions">Specifies whether exception during the initialization sequence should be thrown or not.</param>
        /// <returns></returns>
        bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false);

        /// <summary>
        /// Resets all handled <see cref="IRGBDevice"/> back top default.
        /// </summary>
        void ResetDevices();

        #endregion
    }
}
