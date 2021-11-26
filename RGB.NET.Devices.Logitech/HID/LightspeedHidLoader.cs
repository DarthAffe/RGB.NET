using HidSharp;
using RGB.NET.Core;
using RGB.NET.HID;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Devices.Logitech.HID;

/// <summary>
/// Represents a loaded for logitech HID-devices.
/// </summary>
/// <typeparam name="TLed">The type of the identifier leds are mapped to.</typeparam>
/// <typeparam name="TData">The type of the custom data added to the HID-device.</typeparam>
public class LightspeedHIDLoader<TLed, TData> : IEnumerable<HIDDeviceDefinition<TLed, TData>>
    where TLed : notnull
{
    #region Constants

    private const int LOGITECH_PROTOCOL_TIMEOUT = 300;
    private const int VENDOR_ID = 0x046D;

    // ReSharper disable once StaticMemberInGenericType - This is used like a const
    private static readonly List<int> RECEIVER_PIDS = new()
                                                      {
                                                          0xC539,
                                                          0xC53A,
                                                          0xC541,
                                                          0xC545
                                                      };

    #endregion

    #region Properties & Fields

    private readonly Dictionary<int, HIDDeviceDefinition<TLed, TData>> _deviceDefinitions = new();

    /// <summary>
    /// Gets the vendor id used for this loader.
    /// </summary>
    public int VendorId => VENDOR_ID;

    /// <summary>
    /// Gets or sets the filter used to determine which devices should be loaded.
    /// </summary>
    public RGBDeviceType LoadFilter { get; set; } = RGBDeviceType.All;

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
        foreach (int device in Detect())
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
        => GetConnectedDevices().GroupBy(groupBy)
                                .Select(group => group.First());

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public IEnumerator<HIDDeviceDefinition<TLed, TData>> GetEnumerator() => _deviceDefinitions.Values.GetEnumerator();

    #endregion

    #region Private Methods

    private IEnumerable<int> Detect() => RECEIVER_PIDS.SelectMany(Detect);

    private IEnumerable<int> Detect(int pid)
    {
        Dictionary<byte, HidDevice> deviceUsages = DeviceList.Local
                                                             .GetHidDevices(VendorId, pid)
                                                             .Where(d => d.DevicePath.Contains("mi_02"))
                                                             .ToDictionary(x => (byte)x.GetUsage(), x => x);

        foreach ((int wirelessPid, byte _) in GetWirelessDevices(deviceUsages))
            yield return wirelessPid;
    }

    private Dictionary<int, byte> GetWirelessDevices(IReadOnlyDictionary<byte, HidDevice> deviceUsages)
    {
        const byte LOGITECH_RECEIVER_ADDRESS = 0xFF;
        const byte LOGITECH_SET_REGISTER_REQUEST = 0x80;
        const byte LOGITECH_GET_REGISTER_REQUEST = 0x81;

        Dictionary<int, byte> map = new();

        if (!deviceUsages.TryGetValue(1, out HidDevice? device) || !device.TryOpen(out HidStream stream))
            return map;

        int tries = 0;
        const int maxTries = 5;
        while (tries < maxTries)
        {
            try
            {
                stream.ReadTimeout = LOGITECH_PROTOCOL_TIMEOUT;
                stream.WriteTimeout = LOGITECH_PROTOCOL_TIMEOUT;

                FapResponse response = new();

                FapShortRequest getConnectedDevices = new();
                getConnectedDevices.Init(LOGITECH_RECEIVER_ADDRESS, LOGITECH_GET_REGISTER_REQUEST);

                stream.Write(getConnectedDevices.AsSpan());
                stream.Read(response.AsSpan());

                bool wirelessNotifications = (response.Data01 & 1) == 1;
                if (!wirelessNotifications)
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
                if (deviceCount <= 0)
                    return map;

                //Add 1 to the device_count to include the receiver
                deviceCount++;

                getConnectedDevices.Init(LOGITECH_RECEIVER_ADDRESS, LOGITECH_SET_REGISTER_REQUEST);
                getConnectedDevices.FeatureCommand = 0x02;
                getConnectedDevices.Data0 = 0x02;
                stream.Write(getConnectedDevices.AsSpan());

                for (int i = 0; i < deviceCount; i++)
                {
                    FapResponse devices = new();
                    stream.Read(devices.AsSpan());
                    int wirelessPid = (devices.Data02 << 8) | devices.Data01;
                    if (devices.DeviceIndex != 0xff)
                        map.Add(wirelessPid, devices.DeviceIndex);
                }

                break;
            }
            catch
            {
                tries++;
                //This might timeout if LGS or GHUB interfere.
                //Retry.
            }
        }

        return map;
    }

    #endregion
}