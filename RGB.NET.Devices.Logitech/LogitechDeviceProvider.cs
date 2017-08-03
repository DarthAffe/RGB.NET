// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using RGB.NET.Core;
using RGB.NET.Devices.Logitech.HID;
using RGB.NET.Devices.Logitech.Native;

namespace RGB.NET.Devices.Logitech
{
    /// <summary>
    /// Represents a device provider responsible for logitech devices.
    /// </summary>
    public class LogitechDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static LogitechDeviceProvider _instance;
        /// <summary>
        /// Gets the singleton <see cref="LogitechDeviceProvider"/> instance.
        /// </summary>
        public static LogitechDeviceProvider Instance => _instance ?? new LogitechDeviceProvider();

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX86NativePaths { get; } = new List<string> { "x86/LogitechLedEnginesWrapper.dll" };

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX64NativePaths { get; } = new List<string> { "x64/LogitechLedEnginesWrapper.dll" };

        /// <inheritdoc />
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets the loaded architecture (x64/x86).
        /// </summary>
        public string LoadedArchitecture => _LogitechGSDK.LoadedArchitecture;

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; }

        /// <inheritdoc />
        public bool HasExclusiveAccess => false; // Exclusive access isn't possible for logitech devices.

        /// <summary>
        /// Gets or sets a function to get the culture for a specific device.
        /// </summary>
        public Func<CultureInfo> GetCulture { get; set; } = () => CultureHelper.GetCurrentCulture();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogitechDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public LogitechDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instanc of type {nameof(LogitechDeviceProvider)}");
            _instance = this;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool Initialize(bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            try
            {
                if (IsInitialized)
                    _LogitechGSDK.LogiLedRestoreLighting();
            }
            catch
            { /* At least we tried ... */ }

            IsInitialized = false;

            try
            {
                _LogitechGSDK.Reload();
                if (!_LogitechGSDK.LogiLedInit()) return false;

                _LogitechGSDK.LogiLedSaveCurrentLighting();

                IList<IRGBDevice> devices = new List<IRGBDevice>();

                DeviceChecker.LoadDeviceList();
                if (DeviceChecker.IsDeviceConnected)
                {
                    LogitechRGBDevice device = new LogitechKeyboardRGBDevice(new LogitechKeyboardRGBDeviceInfo(
                        DeviceChecker.ConnectedDeviceModel, LogitechDeviceCaps.PerKeyRGB, GetCulture()));
                    devices.Add(device);

                    try
                    {
                        device.Initialize();
                    }
                    catch
                    {
                        if (throwExceptions)
                            throw;
                        return false;
                    }
                }
                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
            }
            catch
            {
                if (throwExceptions)
                    throw;
                else
                    return false;
            }

            IsInitialized = true;

            return true;
        }

        /// <inheritdoc />
        public void ResetDevices()
        {
            _LogitechGSDK.LogiLedRestoreLighting();
        }

        #endregion
    }
}
