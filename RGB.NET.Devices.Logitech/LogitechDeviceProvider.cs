// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Linq;
using HidSharp;
using RGB.NET.Core;
using RGB.NET.Devices.Logitech.Native;
using RGB.NET.HID;

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

        private const int VENDOR_ID = 0x046D;

        public static HIDLoader<LogitechLedId, int> PerKeyDeviceDefinitions { get; } = new(VENDOR_ID)
        {
            { 0xC32B, RGBDeviceType.Keyboard, "G910", LedMappings.PerKey, 0 },
            { 0xC335, RGBDeviceType.Keyboard, "G910v2", LedMappings.PerKey, 0 },
            { 0xC541, RGBDeviceType.Keyboard, "G915", LedMappings.PerKey, 0 },
            { 0xC33F, RGBDeviceType.Keyboard, "G815", LedMappings.PerKey, 0 },
            { 0xC337, RGBDeviceType.Keyboard, "G810", LedMappings.PerKey, 0 },
            { 0xC331, RGBDeviceType.Keyboard, "G810", LedMappings.PerKey, 0 },
            { 0xC333, RGBDeviceType.Keyboard, "G610", LedMappings.PerKey, 0 },
            { 0xC33C, RGBDeviceType.Keyboard, "G512", LedMappings.PerKey, 0 },
            { 0xC342, RGBDeviceType.Keyboard, "G512 SE", LedMappings.PerKey, 0 },
            { 0xC232, RGBDeviceType.Keyboard, "G513 Carbon", LedMappings.PerKey, 0 },
            { 0xC330, RGBDeviceType.Keyboard, "G410", LedMappings.PerKey, 0 },
            { 0xC336, RGBDeviceType.Keyboard, "G213", LedMappings.PerKey, 0 },
            { 0xC339, RGBDeviceType.Keyboard, "Pro", LedMappings.PerKey, 0 },
            { 0xC343, RGBDeviceType.Keyboard, "G915 TKL", LedMappings.PerKey, 0 },
            { 0xC545, RGBDeviceType.Keyboard, "Lightspeed Keyboard Dongle", LedMappings.PerKey, 0 },
        };

        public static HIDLoader<int, (LogitechDeviceType deviceType, int zones)> PerZoneDeviceDefinitions { get; } = new(VENDOR_ID)
        {
            { 0xC336, RGBDeviceType.Keyboard, "G213", LedMappings.ZoneKeyboard, (LogitechDeviceType.Keyboard, 2) },
            { 0xC086, RGBDeviceType.Mouse, "G903", LedMappings.ZoneMouse, (LogitechDeviceType.Mouse, 2) },
            { 0xC081, RGBDeviceType.Mouse, "G900", LedMappings.ZoneMouse, (LogitechDeviceType.Mouse, 2) },
            { 0xC539, RGBDeviceType.Mouse, "Lightspeed Mouse Dongle", LedMappings.ZoneMouse, (LogitechDeviceType.Mouse, 2) },
            { 0xC087, RGBDeviceType.Mouse, "G703", LedMappings.ZoneMouse, (LogitechDeviceType.Mouse, 2) },
            { 0xC08B, RGBDeviceType.Mouse, "G502 HERO", LedMappings.ZoneMouse, (LogitechDeviceType.Mouse, 2) },
            { 0xC08D, RGBDeviceType.Mouse, "G502 Lightspeed", LedMappings.ZoneMouse, (LogitechDeviceType.Mouse, 2) },
            { 0xC332, RGBDeviceType.Mouse, "G502", LedMappings.ZoneMouse, (LogitechDeviceType.Mouse, 2) },
            { 0xC083, RGBDeviceType.Mouse, "G403", LedMappings.ZoneMouse, (LogitechDeviceType.Mouse, 2) },
            { 0xC080, RGBDeviceType.Mouse, "G303", LedMappings.ZoneMouse, (LogitechDeviceType.Mouse, 2) },
            { 0xC084, RGBDeviceType.Mouse, "G203", LedMappings.ZoneMouse, (LogitechDeviceType.Mouse, 1) },
            { 0xC085, RGBDeviceType.Mouse, "G Pro", LedMappings.ZoneMouse, (LogitechDeviceType.Mouse, 1) },
            { 0xC088, RGBDeviceType.Mouse, "G Pro Wireless", LedMappings.ZoneMouse, (LogitechDeviceType.Mouse, 2) },
            { 0xC08C, RGBDeviceType.Mouse, "G Pro Hero", LedMappings.ZoneMouse, (LogitechDeviceType.Mouse, 1) },
            { 0x0A5C, RGBDeviceType.Headset, "G633", LedMappings.ZoneHeadset, (LogitechDeviceType.Headset, 2) },
            { 0x0A5B, RGBDeviceType.Headset, "G933", LedMappings.ZoneHeadset, (LogitechDeviceType.Headset, 2) },
            { 0x0A87, RGBDeviceType.Headset, "G935", LedMappings.ZoneHeadset, (LogitechDeviceType.Headset, 2) },
            { 0x0A78, RGBDeviceType.Speaker, "G560", LedMappings.ZoneHeadset, (LogitechDeviceType.Speaker, 4) },
            { 0xAB5, RGBDeviceType.Speaker, "G733", LedMappings.ZoneSpeaker, (LogitechDeviceType.Speaker, 2) },
        };

        public static HIDLoader<int, int> PerDeviceDeviceDefinitions { get; } = new(VENDOR_ID)
        {
            { 0xC228, RGBDeviceType.Keyboard, "G19", LedMappings.Device, 0 },
            { 0xC229, RGBDeviceType.Keyboard, "G19s", LedMappings.Device, 0 },
            { 0xC24A, RGBDeviceType.Mouse, "G600", LedMappings.Device, 0 },
            { 0xC246, RGBDeviceType.Mouse, "G300s", LedMappings.Device, 0 },
            { 0xC22D, RGBDeviceType.Keyboard, "G510", LedMappings.Device, 0 },
            { 0xC22E, RGBDeviceType.Keyboard, "G510s", LedMappings.Device, 0 },
            { 0xC21C, RGBDeviceType.Keypad, "G13", LedMappings.Device, 0 },
            { 0xC22B, RGBDeviceType.Keyboard, "G110", LedMappings.Device, 0 },
            { 0xC24D, RGBDeviceType.Keyboard, "G710+", LedMappings.Device, 0 },
            { 0xC248, RGBDeviceType.Keyboard, "G105", LedMappings.Device, 0 },
            { 0xC222, RGBDeviceType.Keyboard, "G15", LedMappings.Device, 0 },
            { 0xC225, RGBDeviceType.Keyboard, "G11", LedMappings.Device, 0 },
        };

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
            if (!_LogitechGSDK.LogiLedInit()) Throw(new RGBDeviceException("Failed to initialize Logitech-SDK."), true);

            _LogitechGSDK.LogiLedSaveCurrentLighting();
        }

        protected override IEnumerable<IRGBDevice> GetLoadedDevices(RGBDeviceType loadFilter)
        {
            PerKeyDeviceDefinitions.LoadFilter = loadFilter;
            PerZoneDeviceDefinitions.LoadFilter = loadFilter;
            PerDeviceDeviceDefinitions.LoadFilter = loadFilter;

            return base.GetLoadedDevices(loadFilter);
        }

        //TODO DarthAffe 04.03.2021: Rework device selection and configuration for HID-based providers 
        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            IEnumerable<(HIDDeviceDefinition<LogitechLedId, int> definition, HidDevice device)> perKeyDevices = PerKeyDeviceDefinitions.GetConnectedDevices();
            if ((_perKeyUpdateQueue != null) && perKeyDevices.Any())
            {
                (HIDDeviceDefinition<LogitechLedId, int> definition, _) = perKeyDevices.First();
                yield return new LogitechPerKeyRGBDevice(new LogitechRGBDeviceInfo(definition.DeviceType, definition.Name, LogitechDeviceCaps.PerKeyRGB, 0), _perKeyUpdateQueue, definition.LedMapping);
            }

            IEnumerable<(HIDDeviceDefinition<int, (LogitechDeviceType deviceType, int zones)> definition, HidDevice device)> perZoneDevices = PerZoneDeviceDefinitions.GetConnectedDevices(x => x.CustomData.deviceType);
            foreach ((HIDDeviceDefinition<int, (LogitechDeviceType deviceType, int zones)> definition, _) in perZoneDevices)
            {
                LogitechZoneUpdateQueue updateQueue = new(GetUpdateTrigger(), definition.CustomData.deviceType);
                yield return new LogitechZoneRGBDevice(new LogitechRGBDeviceInfo(definition.DeviceType, definition.Name, LogitechDeviceCaps.DeviceRGB, definition.CustomData.zones), updateQueue, definition.LedMapping);
            }

            IEnumerable<(HIDDeviceDefinition<int, int> definition, HidDevice device)> perDeviceDevices = PerDeviceDeviceDefinitions.GetConnectedDevices();
            if ((_perDeviceUpdateQueue != null) && perDeviceDevices.Any())
            {
                (HIDDeviceDefinition<int, int> definition, _) = perDeviceDevices.First();
                yield return new LogitechPerDeviceRGBDevice(new LogitechRGBDeviceInfo(definition.DeviceType, definition.Name, LogitechDeviceCaps.DeviceRGB, 0), _perDeviceUpdateQueue, definition.LedMapping);
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
