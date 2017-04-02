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

        /// <summary>
        /// Gets the singleton <see cref="LogitechDeviceProvider"/> instance.
        /// </summary>
        public static LogitechDeviceProvider Instance { get; } = new LogitechDeviceProvider();

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

        private LogitechDeviceProvider()
        { }

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
