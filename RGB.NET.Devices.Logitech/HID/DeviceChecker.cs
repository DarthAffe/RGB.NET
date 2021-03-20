using System.Collections.Generic;
using System.Linq;
using HidSharp;
using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech.HID
{
    internal static class DeviceChecker
    {
        #region Constants

        private const int VENDOR_ID = 0x046D;

        private static readonly List<(string model, RGBDeviceType deviceType, int id, int zones)> PER_KEY_DEVICES
            = new()
            {
                ("G910", RGBDeviceType.Keyboard, 0xC32B, 0),
                ("G910v2", RGBDeviceType.Keyboard, 0xC335, 0),
                ("G915", RGBDeviceType.Keyboard, 0xC541, 0),
                ("G815", RGBDeviceType.Keyboard, 0xC33F, 0),
                ("G810", RGBDeviceType.Keyboard, 0xC337, 0),
                ("G810", RGBDeviceType.Keyboard, 0xC331, 0),
                ("G610", RGBDeviceType.Keyboard, 0xC333, 0),
                ("G512", RGBDeviceType.Keyboard, 0xC33C, 0),
                ("G512 SE", RGBDeviceType.Keyboard, 0xC342, 0),
                ("G410", RGBDeviceType.Keyboard, 0xC330, 0),
                ("G213", RGBDeviceType.Keyboard, 0xC336, 0),
                ("Pro", RGBDeviceType.Keyboard, 0xC339, 0),
                ("G915 TKL", RGBDeviceType.Keyboard, 0xC343, 0),
                ("Lightspeed Keyboard Dongle", RGBDeviceType.Keyboard, 0xC545, 0),
            };

        private static readonly List<(string model, RGBDeviceType deviceType, int id, int zones)> PER_DEVICE_DEVICES
            = new()
            {
                ("G19", RGBDeviceType.Keyboard, 0xC228, 0),
                ("G19s", RGBDeviceType.Keyboard, 0xC229, 0),
                ("G600", RGBDeviceType.Mouse, 0xC24A, 0),
                ("G300s", RGBDeviceType.Mouse, 0xC246, 0),
                ("G510", RGBDeviceType.Keyboard, 0xC22D, 0),
                ("G510s", RGBDeviceType.Keyboard, 0xC22E, 0),
                ("G13", RGBDeviceType.Keypad, 0xC21C, 0),
                ("G110", RGBDeviceType.Keyboard, 0xC22B, 0),
                ("G710+", RGBDeviceType.Keyboard, 0xC24D, 0),
                ("G105", RGBDeviceType.Keyboard, 0xC248, 0),
                ("G15", RGBDeviceType.Keyboard, 0xC222, 0),
                ("G11", RGBDeviceType.Keyboard, 0xC225, 0),
            };

        private static readonly List<(string model, RGBDeviceType deviceType, int id, int zones)> ZONE_DEVICES
            = new()
            {
                ("G213", RGBDeviceType.Keyboard, 0xC336, 5),
                ("G903", RGBDeviceType.Mouse, 0xC086, 2),
                ("Lightspeed Mouse Dongle", RGBDeviceType.Mouse, 0xC539, 2),
                ("G703", RGBDeviceType.Mouse, 0xC087, 2),
                ("G502 HERO", RGBDeviceType.Mouse, 0xC08B, 2),
                ("G502 Lightspeed", RGBDeviceType.Mouse, 0xC08D, 2),
                ("G502", RGBDeviceType.Mouse, 0xC332, 2),
                ("G403", RGBDeviceType.Mouse, 0xC083, 2),
                ("G303", RGBDeviceType.Mouse, 0xC080, 2),
                ("G203", RGBDeviceType.Mouse, 0xC084, 1),
                ("G Pro", RGBDeviceType.Mouse, 0xC085, 1),
                ("G Pro Wireless", RGBDeviceType.Mouse, 0xC088, 2),
                ("G Pro Hero", RGBDeviceType.Mouse, 0xC08C, 1),
                ("G633", RGBDeviceType.Headset, 0x0A5C, 2),
                ("G933", RGBDeviceType.Headset, 0x0A5B, 2),
                ("G935", RGBDeviceType.Headset, 0x0A87, 2),
                ("G560", RGBDeviceType.Speaker, 0x0A78, 4),
                ("G733", RGBDeviceType.Speaker, 0xAB5, 2),
            };

        #endregion

        #region Properties & Fields

        public static bool IsPerKeyDeviceConnected { get; private set; }
        public static (string model, RGBDeviceType deviceType, int id, int zones) PerKeyDeviceData { get; private set; }

        public static bool IsPerDeviceDeviceConnected { get; private set; }
        public static (string model, RGBDeviceType deviceType, int id, int zones) PerDeviceDeviceData { get; private set; }

        public static bool IsZoneDeviceConnected { get; private set; }
        public static IEnumerable<(string model, RGBDeviceType deviceType, int id, int zones)> ZoneDeviceData { get; private set; } = Enumerable.Empty<(string model, RGBDeviceType deviceType, int id, int zones)>();

        #endregion

        #region Methods

        internal static void LoadDeviceList()
        {
            List<int> ids = DeviceList.Local.GetHidDevices(VENDOR_ID).Select(x => x.ProductID).Distinct().ToList();

            foreach ((string model, RGBDeviceType deviceType, int id, int zones) deviceData in PER_KEY_DEVICES)
                if (ids.Contains(deviceData.id))
                {
                    IsPerKeyDeviceConnected = true;
                    PerKeyDeviceData = deviceData;
                    break;
                }

            foreach ((string model, RGBDeviceType deviceType, int id, int zones) deviceData in PER_DEVICE_DEVICES)
                if (ids.Contains(deviceData.id))
                {
                    IsPerDeviceDeviceConnected = true;
                    PerDeviceDeviceData = deviceData;
                    break;
                }

            Dictionary<RGBDeviceType, List<(string model, RGBDeviceType deviceType, int id, int zones)>> connectedZoneDevices = new();
            foreach ((string model, RGBDeviceType deviceType, int id, int zones) deviceData in ZONE_DEVICES)
            {
                if (ids.Contains(deviceData.id))
                {
                    IsZoneDeviceConnected = true;
                    if (!connectedZoneDevices.TryGetValue(deviceData.deviceType, out List<(string model, RGBDeviceType deviceType, int id, int zones)>? deviceList))
                        connectedZoneDevices.Add(deviceData.deviceType, deviceList = new List<(string model, RGBDeviceType deviceType, int id, int zones)>());
                    deviceList.Add(deviceData);
                }
            }
            List<(string model, RGBDeviceType deviceType, int id, int zones)> zoneDeviceData = new();
            foreach (KeyValuePair<RGBDeviceType, List<(string model, RGBDeviceType deviceType, int id, int zones)>> connectedZoneDevice in connectedZoneDevices)
            {
                int maxZones = connectedZoneDevice.Value.Max(x => x.zones);
                zoneDeviceData.Add(connectedZoneDevice.Value.First(x => x.zones == maxZones));
            }

            ZoneDeviceData = zoneDeviceData;
        }

        #endregion
    }
}
