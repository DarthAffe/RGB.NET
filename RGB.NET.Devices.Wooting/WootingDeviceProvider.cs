using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Native;

namespace RGB.NET.Devices.Wooting
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for Wooting devices.
    /// </summary>
    public class WootingDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static WootingDeviceProvider _instance;
        /// <summary>
        /// Gets the singleton <see cref="WootingDeviceProvider"/> instance.
        /// </summary>
        public static WootingDeviceProvider Instance => _instance ?? new WootingDeviceProvider();

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX86NativePaths { get; } = new List<string> { "x86/wooting-rgb-sdk.dll" };

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX64NativePaths { get; } = new List<string> { "x64/wooting-rgb-sdk64.dll" };

        /// <inheritdoc />
        /// <summary>
        /// Indicates if the SDK is initialized and ready to use.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets the loaded architecture (x64/x86).
        /// </summary>
        public string LoadedArchitecture => _WootingSDK.LoadedArchitecture;

        /// <inheritdoc />
        /// <summary>
        /// Gets whether the application has exclusive access to the SDK or not.
        /// </summary>
        public bool HasExclusiveAccess => false;

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WootingDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public WootingDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(WootingDeviceProvider)}");
            _instance = this;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <exception cref="RGBDeviceException">Thrown if the SDK failed to initialize</exception>
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                _WootingSDK.Reload();

                IList<IRGBDevice> devices = new List<IRGBDevice>();
                if (_WootingSDK.KeyboardConnected())
                {
                    if (_WootingSDK.IsWootingOne())
                    {
                        
                    } 
                    else if (_WootingSDK.IsWootingTwo())
                    {

                    }
                }
                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
                IsInitialized = true;
            }
            catch
            {
                if (throwExceptions) throw;
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public void ResetDevices()
        { }

        /// <inheritdoc />
        public void Dispose()
        {
            try { _WootingSDK.Reset(); }
            catch { /* Unlucky.. */}
        }

        #endregion
    }
}
