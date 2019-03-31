using System.Collections.Generic;
using System.Linq;
using HidSharp;
using RGB.NET.Core;
using DeviceDataList = System.Collections.Generic.List<(string model, RGB.NET.Core.RGBDeviceType deviceType, int id, RGB.NET.Devices.SteelSeries.SteelSeriesDeviceType steelSeriesDeviceType, string imageLayout, string layoutPath, System.Collections.Generic.Dictionary<RGB.NET.Core.LedId, RGB.NET.Devices.SteelSeries.SteelSeriesLedId> ledMapping)>;
using LedMapping = System.Collections.Generic.Dictionary<RGB.NET.Core.LedId, RGB.NET.Devices.SteelSeries.SteelSeriesLedId>;

namespace RGB.NET.Devices.SteelSeries.HID
{
    internal static class DeviceChecker
    {
        #region Constants

        private const int VENDOR_ID = 0x1038;

        //TODO DarthAffe 16.02.2019: Add devices
        private static readonly DeviceDataList DEVICES = new DeviceDataList
        {
            ("Rival 500", RGBDeviceType.Mouse, 0x170E, SteelSeriesDeviceType.TwoZone, "default", @"Mice\Rival500", new LedMapping { { LedId.Mouse1, SteelSeriesLedId.ZoneOne},
                                                                                                                                    { LedId.Mouse2, SteelSeriesLedId.ZoneTwo} }),
        };

        #endregion

        #region Properties & Fields

        public static DeviceDataList ConnectedDevices { get; } = new DeviceDataList();

        #endregion

        #region Methods

        internal static void LoadDeviceList(RGBDeviceType loadFilter)
        {
            ConnectedDevices.Clear();

            HashSet<int> ids = new HashSet<int>(DeviceList.Local.GetHidDevices(VENDOR_ID).Select(x => x.ProductID).Distinct());
            DeviceDataList connectedDevices = DEVICES.Where(d => ids.Contains(d.id) && loadFilter.HasFlag(d.deviceType)).ToList();

            List<SteelSeriesDeviceType> connectedDeviceTypes = connectedDevices.Select(d => d.steelSeriesDeviceType).ToList();
            foreach (SteelSeriesDeviceType deviceType in connectedDeviceTypes)
                ConnectedDevices.Add(connectedDevices.Where(d => d.steelSeriesDeviceType == deviceType).OrderByDescending(d => d.ledMapping.Count).First());
        }

        #endregion
    }
}
