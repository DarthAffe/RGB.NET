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
    /// <inheritdoc />
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
        public Func<CultureInfo> GetCulture { get; set; } = CultureHelper.GetCurrentCulture;

        /// <summary>
        /// The <see cref="DeviceUpdateTrigger"/> used to trigger the updates for logitech devices. 
        /// </summary>
        public DeviceUpdateTrigger UpdateTrigger { get; private set; }

        // ReSharper disable once CollectionNeverQueried.Local - for now this is just to make sure they're never collected
        private readonly Dictionary<RGBDeviceType, LogitechZoneUpdateQueue> _zoneUpdateQueues = new Dictionary<RGBDeviceType, LogitechZoneUpdateQueue>();
        private LogitechPerDeviceUpdateQueue _perDeviceUpdateQueue;
        private LogitechPerKeyUpdateQueue _perKeyUpdateQueue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogitechDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public LogitechDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(LogitechDeviceProvider)}");
            _instance = this;

            UpdateTrigger = new DeviceUpdateTrigger();
            _perDeviceUpdateQueue = new LogitechPerDeviceUpdateQueue(UpdateTrigger);
            _perKeyUpdateQueue = new LogitechPerKeyUpdateQueue(UpdateTrigger);
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            try
            {
                if (IsInitialized)
                    _LogitechGSDK.LogiLedRestoreLighting();
            }
            catch { /* At least we tried ... */ }

            IsInitialized = false;

            try
            {
                UpdateTrigger?.Stop();

                _LogitechGSDK.Reload();
                if (!_LogitechGSDK.LogiLedInit()) return false;

                _LogitechGSDK.LogiLedSaveCurrentLighting();

                IList<IRGBDevice> devices = new List<IRGBDevice>();
                DeviceChecker.LoadDeviceList();

                try
                {
                    if (DeviceChecker.IsPerKeyDeviceConnected)
                    {
                        (string model, RGBDeviceType deviceType, int _, int _, string imageLayout, string layoutPath) = DeviceChecker.PerKeyDeviceData;
                        if (loadFilter.HasFlag(deviceType)) //TODO DarthAffe 07.12.2017: Check if it's worth to try another device if the one returned doesn't match the filter
                        {
                            ILogitechRGBDevice device = new LogitechPerKeyRGBDevice(new LogitechRGBDeviceInfo(deviceType, model, LogitechDeviceCaps.PerKeyRGB, 0, imageLayout, layoutPath));
                            device.Initialize(_perKeyUpdateQueue);
                            devices.Add(device);
                        }
                    }
                }
                catch { if (throwExceptions) throw; }

                try
                {
                    if (DeviceChecker.IsPerDeviceDeviceConnected)
                    {
                        (string model, RGBDeviceType deviceType, int _, int _, string imageLayout, string layoutPath) = DeviceChecker.PerDeviceDeviceData;
                        if (loadFilter.HasFlag(deviceType)) //TODO DarthAffe 07.12.2017: Check if it's worth to try another device if the one returned doesn't match the filter
                        {
                            ILogitechRGBDevice device = new LogitechPerDeviceRGBDevice(new LogitechRGBDeviceInfo(deviceType, model, LogitechDeviceCaps.DeviceRGB, 0, imageLayout, layoutPath));
                            device.Initialize(_perDeviceUpdateQueue);
                            devices.Add(device);
                        }
                    }
                }
                catch { if (throwExceptions) throw; }

                try
                {
                    if (DeviceChecker.IsZoneDeviceConnected)
                    {
                        foreach ((string model, RGBDeviceType deviceType, int _, int zones, string imageLayout, string layoutPath) in DeviceChecker.ZoneDeviceData)
                            try
                            {
                                if (loadFilter.HasFlag(deviceType))
                                {
                                    LogitechZoneUpdateQueue updateQueue = new LogitechZoneUpdateQueue(UpdateTrigger, deviceType);
                                    ILogitechRGBDevice device = new LogitechZoneRGBDevice(new LogitechRGBDeviceInfo(deviceType, model, LogitechDeviceCaps.DeviceRGB, zones, imageLayout, layoutPath));
                                    device.Initialize(updateQueue);
                                    devices.Add(device);
                                    _zoneUpdateQueues.Add(deviceType, updateQueue);
                                }
                            }
                            catch { if (throwExceptions) throw; }
                    }
                }
                catch { if (throwExceptions) throw; }

                UpdateTrigger?.Start();

                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
                IsInitialized = true;
            }
            catch
            {
                if (throwExceptions)
                    throw;
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public void ResetDevices() => _LogitechGSDK.LogiLedRestoreLighting();

        /// <inheritdoc />
        public void Dispose()
        {
            try { UpdateTrigger?.Dispose(); }
            catch { /* at least we tried */ }

            try { _LogitechGSDK.LogiLedRestoreLighting(); }
            catch { /* at least we tried */ }

            try { _LogitechGSDK.UnloadLogitechGSDK(); }
            catch { /* at least we tried */ }
        }

        #endregion
    }
}
