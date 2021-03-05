// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.Logitech.HID;
using RGB.NET.Devices.Logitech.Native;

namespace RGB.NET.Devices.Logitech
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for logitech devices.
    /// </summary>
    public class LogitechDeviceProvider : AbstractRGBDeviceProvider
    {
        #region Properties & Fields

        private static LogitechDeviceProvider? _instance;
        /// <summary>
        /// Gets the singleton <see cref="LogitechDeviceProvider"/> instance.
        /// </summary>
        public static LogitechDeviceProvider Instance => _instance ?? new LogitechDeviceProvider();

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX86NativePaths { get; } = new() { "x86/LogitechLedEnginesWrapper.dll" };

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX64NativePaths { get; } = new() { "x64/LogitechLedEnginesWrapper.dll" };

        private LogitechPerDeviceUpdateQueue? _perDeviceUpdateQueue;
        private LogitechPerKeyUpdateQueue? _perKeyUpdateQueue;

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
        }

        #endregion

        #region Methods

        protected override void InitializeSDK()
        {
            _perDeviceUpdateQueue = new LogitechPerDeviceUpdateQueue(GetUpdateTrigger());
            _perKeyUpdateQueue = new LogitechPerKeyUpdateQueue(GetUpdateTrigger());

            _LogitechGSDK.Reload();
            if (!_LogitechGSDK.LogiLedInit()) Throw(new RGBDeviceException("Failed to initialize Logitech-SDK."));

            _LogitechGSDK.LogiLedSaveCurrentLighting();
        }

        //TODO DarthAffe 04.03.2021: Rework device selection and configuration for HID-based providers 
        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            DeviceChecker.LoadDeviceList();

            if (DeviceChecker.IsPerKeyDeviceConnected && (_perKeyUpdateQueue != null))
            {
                (string model, RGBDeviceType deviceType, int _, int _) = DeviceChecker.PerKeyDeviceData;
                yield return new LogitechPerKeyRGBDevice(new LogitechRGBDeviceInfo(deviceType, model, LogitechDeviceCaps.PerKeyRGB, 0), _perKeyUpdateQueue);
            }

            if (DeviceChecker.IsPerDeviceDeviceConnected && (_perDeviceUpdateQueue != null))
            {
                (string model, RGBDeviceType deviceType, int _, int _) = DeviceChecker.PerDeviceDeviceData;
                yield return new LogitechPerDeviceRGBDevice(new LogitechRGBDeviceInfo(deviceType, model, LogitechDeviceCaps.DeviceRGB, 0), _perDeviceUpdateQueue);
            }

            if (DeviceChecker.IsZoneDeviceConnected)
            {
                foreach ((string model, RGBDeviceType deviceType, int _, int zones) in DeviceChecker.ZoneDeviceData)
                {
                    LogitechZoneUpdateQueue updateQueue = new(GetUpdateTrigger(), deviceType);
                    yield return new LogitechZoneRGBDevice(new LogitechRGBDeviceInfo(deviceType, model, LogitechDeviceCaps.DeviceRGB, zones), updateQueue);
                }
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            try { _LogitechGSDK.LogiLedRestoreLighting(); }
            catch { /* at least we tried */ }

            try { _LogitechGSDK.UnloadLogitechGSDK(); }
            catch { /* at least we tried */ }
        }

        #endregion
    }
}
