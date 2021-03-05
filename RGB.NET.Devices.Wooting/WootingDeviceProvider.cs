using System;
using System.Collections.Generic;
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
    public class WootingDeviceProvider : AbstractRGBDeviceProvider
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

        protected override void InitializeSDK()
        {
            _WootingSDK.Reload();
        }

        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            if (_WootingSDK.KeyboardConnected())
            {
                _WootingDeviceInfo nativeDeviceInfo = (_WootingDeviceInfo)Marshal.PtrToStructure(_WootingSDK.GetDeviceInfo(), typeof(_WootingDeviceInfo))!;
                IWootingRGBDevice? device = nativeDeviceInfo.Model switch
                {
                    "Wooting two" => new WootingKeyboardRGBDevice(new WootingKeyboardRGBDeviceInfo(WootingDevicesIndexes.WootingTwo), GetUpdateTrigger()),
                    "Wooting one" => new WootingKeyboardRGBDevice(new WootingKeyboardRGBDeviceInfo(WootingDevicesIndexes.WootingOne), GetUpdateTrigger()),
                    _ => null
                };

                if (device == null)
                    Throw(new RGBDeviceException("No supported Wooting keyboard connected"));
                else
                    yield return device;
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            try { _WootingSDK.Close(); }
            catch { /* Unlucky.. */ }

            try { _WootingSDK.UnloadWootingSDK(); }
            catch { /* at least we tried */ }
        }

        #endregion
    }
}
