using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HidSharp;
using RGB.NET.Core;

namespace RGB.NET.HID
{
    public record HIDDeviceDefinition<TLed, TData>(int ProductId, RGBDeviceType DeviceType, string Name, LedMapping<TLed> LedMapping, TData CustomData) where TLed : notnull;

    public class HIDLoader<TLed, TData> : IEnumerable<HIDDeviceDefinition<TLed, TData>>
        where TLed : notnull
    {
        #region Properties & Fields

        private readonly Dictionary<int, HIDDeviceDefinition<TLed, TData>> _deviceDefinitions = new();

        public int VendorId { get; }

        public RGBDeviceType LoadFilter { get; set; } = RGBDeviceType.All;

        #endregion

        #region Constructors

        public HIDLoader(int vendorId)
        {
            this.VendorId = vendorId;
        }

        #endregion

        #region Methods

        public void Add(int productId, RGBDeviceType deviceType, string name, LedMapping<TLed> ledMapping, TData customData)
            => _deviceDefinitions.Add(productId, new HIDDeviceDefinition<TLed, TData>(productId, deviceType, name, ledMapping, customData));

        public IEnumerable<(HIDDeviceDefinition<TLed, TData> definition, HidDevice device)> GetConnectedDevices()
        {
            IEnumerable<HidDevice> devices = DeviceList.Local.GetHidDevices(VendorId);
            foreach (HidDevice device in devices)
            {
                if (_deviceDefinitions.TryGetValue(device.ProductID, out HIDDeviceDefinition<TLed, TData>? definition))
                    if (LoadFilter.HasFlag(definition.DeviceType))
                        yield return (definition, device);
            }
        }

        public IEnumerable<(HIDDeviceDefinition<TLed, TData> definition, HidDevice device)> GetConnectedDevices<TKey>(Func<HIDDeviceDefinition<TLed, TData>, TKey> groupBy)
            => GetConnectedDevices().GroupBy(x => groupBy(x.definition))
                                    .Select(group => group.First());

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<HIDDeviceDefinition<TLed, TData>> GetEnumerator() => _deviceDefinitions.Values.GetEnumerator();

        #endregion
    }
}
