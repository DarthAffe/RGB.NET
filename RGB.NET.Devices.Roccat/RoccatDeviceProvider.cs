using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RGB.NET.Core;
using RGB.NET.Devices.Roccat.Native;

namespace RGB.NET.Devices.Roccat
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for roccat (TalkFX)devices.
    /// </summary>
    public class RoccatDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static RoccatDeviceProvider _instance;
        /// <summary>
        /// Gets the singleton <see cref="RoccatDeviceProvider"/> instance.
        /// </summary>
        public static RoccatDeviceProvider Instance => _instance ?? new RoccatDeviceProvider();

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX86NativePaths { get; } = new List<string> { "x86/RoccatTalkSDKWrapper.dll" };

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX64NativePaths { get; } = new List<string> { "x64/RoccatTalkSDKWrapper.dll" };

        /// <inheritdoc />
        /// <summary>
        /// Indicates if the SDK is initialized and ready to use.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets the loaded architecture (x64/x86).
        /// </summary>
        public string LoadedArchitecture => _RoccatSDK.LoadedArchitecture;

        /// <inheritdoc />
        /// <summary>
        /// Gets whether the application has exclusive access to the SDK or not.
        /// </summary>
        public bool HasExclusiveAccess { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; }

        private IntPtr _sdkHandle;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoccatDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public RoccatDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(RoccatDeviceProvider)}");
            _instance = this;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <exception cref="RGBDeviceException">Thrown if the SDK is already initialized or if the SDK is not compatible to CUE.</exception>
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                _sdkHandle = _RoccatSDK.InitSDK();

                IList<IRGBDevice> devices = new List<IRGBDevice>();

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
            if (_sdkHandle != IntPtr.Zero)
            {
                try { _RoccatSDK.RestoreLedRGB(_sdkHandle); }
                catch { /* We tried our best */}

                try { _RoccatSDK.SetRyosKbSDKMode(_sdkHandle, false); }
                catch { /* We tried our best */}

                try { _RoccatSDK.UnloadSDK(_sdkHandle); }
                catch { /* We tried our best */}
            }
        }

        #endregion
    }
}
