using HidSharp;
using RGB.NET.Core;
using RGB.NET.HID;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGB.NET.Devices.Logitech.HID
{
    public class LightspeedHIDLoader<TLed, TData> : IEnumerable<HIDDeviceDefinition<TLed, TData>>
        where TLed : notnull
    {
        #region Properties & Fields

        private readonly Dictionary<int, HIDDeviceDefinition<TLed, TData>> _deviceDefinitions = new();

        /// <summary>
        /// Gets the vendor id used for this loader.
        /// </summary>
        public int VendorId => 0x046d;

        /// <summary>
        /// Gets or sets the filter used to determine which devices should be loaded.
        /// </summary>
        public RGBDeviceType LoadFilter { get; set; } = RGBDeviceType.All;

        private static List<int> ReceiverPids { get; } = new()
        {
            0xC539,
            0xC53A,
            0xC541,
            0xC545
        };

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new <see cref="HIDDeviceDefinition{TLed,TData}"/> to this loader.
        /// </summary>
        /// <param name="virtualPid">The virtual product id of the HID-device.</param>
        /// <param name="deviceType">The type of the device.</param>
        /// <param name="name">The name of the device.</param>
        /// <param name="ledMapping">The mapping of the leds of the device.</param>
        /// <param name="customData">Some custom data to attach to the device.</param>
        public void Add(int virtualPid, RGBDeviceType deviceType, string name, LedMapping<TLed> ledMapping, TData customData)
            => _deviceDefinitions.Add(virtualPid, new HIDDeviceDefinition<TLed, TData>(virtualPid, deviceType, name, ledMapping, customData));

        /// <summary>
        /// Gets a enumerable containing all devices from the definition-list that are connected and match the <see cref="LoadFilter"/>.
        /// </summary>
        /// <returns>The enumerable containing the connected devices.</returns>
        public IEnumerable<HIDDeviceDefinition<TLed, TData>> GetConnectedDevices()
        {
            foreach (var device in Detect())
            {
                if (_deviceDefinitions.TryGetValue(device, out HIDDeviceDefinition<TLed, TData>? definition))
                    if (LoadFilter.HasFlag(definition.DeviceType))
                        yield return definition;
            }
        }

        /// <summary>
        /// Gets a enumerable containing all the first device of each group of devices from the definition-list that are connected and match the <see cref="LoadFilter"/>.
        /// The grouping is done by the specified function.
        /// </summary>
        /// <typeparam name="TKey">The type of the key used to group the devices.</typeparam>
        /// <param name="groupBy">The function grouping the devices.</param>
        /// <returns>The enumerable containing the selected devices.</returns>
        public IEnumerable<HIDDeviceDefinition<TLed, TData>> GetConnectedDevices<TKey>(Func<HIDDeviceDefinition<TLed, TData>, TKey> groupBy)
            => GetConnectedDevices().GroupBy(x => groupBy(x))
                                    .Select(group => group.First());

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public IEnumerator<HIDDeviceDefinition<TLed, TData>> GetEnumerator() => _deviceDefinitions.Values.GetEnumerator();

        #endregion

        #region Private Methods

        private IEnumerable<int> Detect()
        {
            foreach (var receiverPid in ReceiverPids)
            {
                foreach (var wirelessPid in Detect(receiverPid))
                {
                    yield return wirelessPid;
                }
            }
        }

        private IEnumerable<int> Detect(int pid)
        {
            var receiverDevices = DeviceList.Local.GetHidDevices(VendorId, pid);
            var interfaceTwo = receiverDevices.Where(d => d.DevicePath.Contains("mi_02"));
            //this is terrible but i don't know how else to filter interfaces

            Dictionary<byte, HidDevice> deviceUsages = new();
            foreach (var item in interfaceTwo)
            {
                deviceUsages.Add((byte)item.GetUsage(), item);
            }

            foreach ((var wirelessPid, var deviceIndex) in GetWirelessDevices(deviceUsages))
            {
                yield return wirelessPid;
            }
        }

        private Dictionary<int, byte> GetWirelessDevices(Dictionary<byte, HidDevice> device_usages)
        {
            const byte LOGITECH_RECEIVER_ADDRESS = 0xFF;
            const byte LOGITECH_SET_REGISTER_REQUEST = 0x80;
            const byte LOGITECH_GET_REGISTER_REQUEST = 0x81;

            Dictionary<int, byte> map = new();

            if (device_usages.TryGetValue(1, out var device))
            {
                var stream = device.Open();

                var response = new FapResponse();

                var getConnectedDevices = new FapShortRequest();
                getConnectedDevices.Init(LOGITECH_RECEIVER_ADDRESS, LOGITECH_GET_REGISTER_REQUEST);

                stream.Write(getConnectedDevices.AsSpan());
                stream.Read(response.AsSpan());

                bool wireless_notifications = (response.Data01 & 1) == 1;
                if (!wireless_notifications)
                {
                    response = new FapResponse();

                    getConnectedDevices.Init(LOGITECH_RECEIVER_ADDRESS, LOGITECH_SET_REGISTER_REQUEST);
                    getConnectedDevices.Data1 = 1;

                    stream.Write(getConnectedDevices.AsSpan());
                    stream.Read(response.AsSpan());

                    if (getConnectedDevices.FeatureIndex == 0x8f)
                    {
                        //error??
                    }
                }

                response = new FapResponse();

                getConnectedDevices.Init(LOGITECH_RECEIVER_ADDRESS, LOGITECH_GET_REGISTER_REQUEST);
                getConnectedDevices.FeatureCommand = 0x02;

                stream.Write(getConnectedDevices.AsSpan());
                stream.Read(response.AsSpan());

                int deviceCount = response.Data01;
                if (deviceCount > 0)
                {
                    //log "Faking a reconnect to get device list"
                    deviceCount++;

                    response = new FapResponse();
                    getConnectedDevices.Init(LOGITECH_RECEIVER_ADDRESS, LOGITECH_SET_REGISTER_REQUEST);
                    getConnectedDevices.FeatureCommand = 0x02;
                    getConnectedDevices.Data0 = 0x02;
                    stream.Write(getConnectedDevices.AsSpan());

                    for (int i = 0; i < deviceCount; i++)
                    {
                        var devices = new FapResponse();
                        stream.Read(devices.AsSpan());
                        int wirelessPid = (devices.Data02 << 8) | devices.Data01;
                        if (devices.DeviceIndex != 0xff)
                        {
                            map.Add(wirelessPid, devices.DeviceIndex);
                        }
                    }
                }
            }

            return map;
        }

        #endregion
    }
}
