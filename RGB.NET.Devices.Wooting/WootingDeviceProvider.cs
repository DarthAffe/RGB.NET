using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Enum;
using RGB.NET.Devices.Wooting.Generic;
using RGB.NET.Devices.Wooting.Keyboard;
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

        private static WootingDeviceProvider? _instance;
        /// <summary>
        /// Gets the singleton <see cref="WootingDeviceProvider"/> instance.
        /// </summary>
        public static WootingDeviceProvider Instance => _instance ?? new WootingDeviceProvider();

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX86NativePaths { get; } = new() { "x86/wooting-rgb-sdk.dll" };

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX64NativePaths { get; } = new() { "x64/wooting-rgb-sdk64.dll" };

        /// <inheritdoc />
        /// <summary>
        /// Indicates if the SDK is initialized and ready to use.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; } = Enumerable.Empty<IRGBDevice>();

        /// <summary>
        /// The <see cref="DeviceUpdateTrigger"/> used to trigger the updates for cooler master devices. 
        /// </summary>
        public DeviceUpdateTrigger UpdateTrigger { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WootingDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public WootingDeviceProvider()
        {
            if (_instance != null)
                throw new InvalidOperationException($"There can be only one instance of type {nameof(WootingDeviceProvider)}");
            _instance = this;

            UpdateTrigger = new DeviceUpdateTrigger();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <exception cref="RGBDeviceException">Thrown if the SDK failed to initialize</exception>
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                UpdateTrigger.Stop();

                _WootingSDK.Reload();

                IList<IRGBDevice> devices = new List<IRGBDevice>();
                if (_WootingSDK.KeyboardConnected())
                {
                    _WootingDeviceInfo nativeDeviceInfo = (_WootingDeviceInfo)Marshal.PtrToStructure(_WootingSDK.GetDeviceInfo(), typeof(_WootingDeviceInfo))!;
                    IWootingRGBDevice device = nativeDeviceInfo.Model switch
                    {
                        "Wooting two" => new WootingKeyboardRGBDevice(new WootingKeyboardRGBDeviceInfo(WootingDevicesIndexes.WootingTwo)),
                        "Wooting one" => new WootingKeyboardRGBDevice(new WootingKeyboardRGBDeviceInfo(WootingDevicesIndexes.WootingOne)),
                        _ => throw new RGBDeviceException("No supported Wooting keyboard connected")
                    };

                    device.Initialize(UpdateTrigger);
                    devices.Add(device);
                }

                UpdateTrigger.Start();

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
        public void Dispose()
        {
            try { UpdateTrigger.Dispose(); }
            catch { /* at least we tried */ }

            foreach (IRGBDevice device in Devices)
                try { device.Dispose(); }
                catch { /* at least we tried */ }
            Devices = Enumerable.Empty<IRGBDevice>();

            try { _WootingSDK.Close(); }
            catch { /* Unlucky.. */ }

            try { _WootingSDK.UnloadWootingSDK(); }
            catch { /* at least we tried */ }
        }

        #endregion
    }
}
