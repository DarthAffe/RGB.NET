using System.Collections.Generic;
using System.Linq;
using HidSharp;
using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech.HID
{
    //TODO DarthAffe 04.02.2017: Rewrite this once the SDK supports per-device lighting to get all the devices connected.
    internal static class DeviceChecker
    {
        #region Constants

        private const int VENDOR_ID = 0x046D;

        //TODO DarthAffe 14.11.2017: Add devices
        private static readonly List<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)> PER_KEY_DEVICES
            = new List<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)>
              {
                  ("G910", RGBDeviceType.Keyboard, 0xC32B, 0, "DE", @"Keyboards\G910\UK"), //TODO DarthAffe 15.11.2017: Somehow detect the current layout
                  ("G910v2", RGBDeviceType.Keyboard, 0xC335, 0, "DE", @"Keyboards\G910\UK"),
                  ("G810", RGBDeviceType.Keyboard, 0xC337, 0, "DE", @"Keyboards\G810\UK"),
                  ("G610", RGBDeviceType.Keyboard, 0xC333, 0, "DE", @"Keyboards\G610\UK"),
                  ("G512", RGBDeviceType.Keyboard, 0xC33C, 0, "DE", @"Keyboards\G512\UK"),
                  ("G410", RGBDeviceType.Keyboard, 0xC330, 0, "DE", @"Keyboards\G410\UK"),
                  ("G213", RGBDeviceType.Keyboard, 0xC336, 0, "DE", @"Keyboards\G213\UK"),
                  ("Pro", RGBDeviceType.Keyboard, 0xC339, 0, "DE", @"Keyboards\Pro\UK"),
              };

        private static readonly List<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)> PER_DEVICE_DEVICES
            = new List<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)>
              {
                  ("G19", RGBDeviceType.Keyboard, 0xC228, 0, "DE", @"Keyboards\G19\UK"),
                  ("G19s", RGBDeviceType.Keyboard, 0xC229, 0, "DE", @"Keyboards\G19s\UK"),
                  ("G600", RGBDeviceType.Mouse, 0xC24A, 0, "default", @"Mice\G600"),
                  ("G300s", RGBDeviceType.Mouse, 0xC246, 0, "default", @"Mice\G300s"),
                  ("G510", RGBDeviceType.Keyboard, 0xC22D, 0, "DE", @"Keyboards\G510\UK"),
                  ("G510s", RGBDeviceType.Keyboard, 0xC22E, 0, "DE", @"Keyboards\G510s\UK"),
                  ("G13", RGBDeviceType.Keypad, 0xC21C, 0, "DE", @"Keypads\G13\UK"),
                  ("G110", RGBDeviceType.Keyboard, 0xC22B, 0, "DE", @"Keyboards\G110\UK"),
                  ("G710+", RGBDeviceType.Keyboard, 0xC24D, 0, "DE", @"Keyboards\G710+\UK"),
                  ("G105", RGBDeviceType.Keyboard, 0xC248, 0, "DE", @"Keyboards\G105\UK"),
                  ("G15", RGBDeviceType.Keyboard, 0xC222, 0, "DE", @"Keyboards\G15\UK"),
                  ("G11", RGBDeviceType.Keyboard, 0xC225, 0, "DE", @"Keyboards\G11\UK"),
              };

        private static readonly List<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)> ZONE_DEVICES
            = new List<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)>
              {
                  ("G213", RGBDeviceType.Keyboard, 0xC336, 5, "default", @"Keyboards\G213"),
                  ("G903", RGBDeviceType.Mouse, 0xC086, 2, "default", @"Mice\G903"),
                  ("G900", RGBDeviceType.Mouse, 0xC539, 2, "default", @"Mice\G900"),
                  ("G703", RGBDeviceType.Mouse, 0xC087, 2, "default", @"Mice\G703"),
                  ("G502 HERO", RGBDeviceType.Mouse, 0xC08B, 2, "default", @"Mice\G502"),
                  ("G502", RGBDeviceType.Mouse, 0xC332, 2, "default", @"Mice\G502"),
                  ("G403", RGBDeviceType.Mouse, 0xC083, 2, "default", @"Mice\G403"),
                  ("G303", RGBDeviceType.Mouse, 0xC080, 2, "default", @"Mice\G303"),
                  ("G203", RGBDeviceType.Mouse, 0xC084, 1, "default", @"Mice\G203"),
                  ("G Pro", RGBDeviceType.Mouse, 0xC085, 1, "default", @"Mice\GPro"),
                  ("G633", RGBDeviceType.Headset, 0x0A5C, 2, "default", @"Headsets\G633"),
                  ("G933", RGBDeviceType.Headset, 0x0A5B, 2, "default", @"Headsets\G933"),
                  ("G935", RGBDeviceType.Headset, 0x0A87, 2, "default", @"Headsets\G935"),
                  ("G560", RGBDeviceType.Speaker, 0x0A78, 4, "default", @"Speakers\G560"),
              };

        #endregion

        #region Properties & Fields

        public static bool IsPerKeyDeviceConnected { get; private set; }
        public static (string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath) PerKeyDeviceData { get; private set; }

        public static bool IsPerDeviceDeviceConnected { get; private set; }
        public static (string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath) PerDeviceDeviceData { get; private set; }

        public static bool IsZoneDeviceConnected { get; private set; }
        public static IEnumerable<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)> ZoneDeviceData { get; private set; }

        #endregion

        #region Methods

        internal static void LoadDeviceList()
        {
            List<int> ids = DeviceList.Local.GetHidDevices(VENDOR_ID).Select(x => x.ProductID).Distinct().ToList();

            foreach ((string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath) deviceData in PER_KEY_DEVICES)
                if (ids.Contains(deviceData.id))
                {
                    IsPerKeyDeviceConnected = true;
                    PerKeyDeviceData = deviceData;
                    break;
                }

            foreach ((string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath) deviceData in PER_DEVICE_DEVICES)
                if (ids.Contains(deviceData.id))
                {
                    IsPerDeviceDeviceConnected = true;
                    PerDeviceDeviceData = deviceData;
                    break;
                }

            Dictionary<RGBDeviceType, List<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)>> connectedZoneDevices
                = new Dictionary<RGBDeviceType, List<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)>>();
            foreach ((string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath) deviceData in ZONE_DEVICES)
            {
                if (ids.Contains(deviceData.id))
                {
                    IsZoneDeviceConnected = true;
                    if (!connectedZoneDevices.TryGetValue(deviceData.deviceType, out List<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)> deviceList))
                        connectedZoneDevices.Add(deviceData.deviceType, deviceList = new List<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)>());
                    deviceList.Add(deviceData);
                }
            }
            List<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)> zoneDeviceData
                = new List<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)>();
            foreach (KeyValuePair<RGBDeviceType, List<(string model, RGBDeviceType deviceType, int id, int zones, string imageLayout, string layoutPath)>> connectedZoneDevice in connectedZoneDevices)
            {
                int maxZones = connectedZoneDevice.Value.Max(x => x.zones);
                zoneDeviceData.Add(connectedZoneDevice.Value.First(x => x.zones == maxZones));
            }

            ZoneDeviceData = zoneDeviceData;
        }

        #endregion
    }
}
