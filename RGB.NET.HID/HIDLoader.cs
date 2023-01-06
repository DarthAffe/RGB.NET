using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HidSharp;
using RGB.NET.Core;

namespace RGB.NET.HID;

/// <summary>
/// Represents the data used to define a HID-device.
/// </summary>
/// <typeparam name="TLed">The type of the identifier leds are mapped to.</typeparam>
/// <typeparam name="TData">The type of the custom data added to the HID-device.</typeparam>
public record HIDDeviceDefinition<TLed, TData>(int ProductId, RGBDeviceType DeviceType, string Name, LedMapping<TLed> LedMapping, TData CustomData) where TLed : notnull;

/// <summary>
/// Represents a loaded for HID-devices based on the specified definitions.
/// </summary>
/// <typeparam name="TLed">The type of the identifier leds are mapped to.</typeparam>
/// <typeparam name="TData">The type of the custom data added to the HID-device.</typeparam>
public class HIDLoader<TLed, TData> : IEnumerable<HIDDeviceDefinition<TLed, TData>>
    where TLed : notnull
{
    #region Properties & Fields

    private readonly Dictionary<int, HIDDeviceDefinition<TLed, TData>> _deviceDefinitions = new();

    /// <summary>
    /// Gets the vendor id used for this loader.
    /// </summary>
    public int VendorId { get; }

    /// <summary>
    /// Gets or sets the filter used to determine which devices should be loaded.
    /// </summary>
    public RGBDeviceType LoadFilter { get; set; } = RGBDeviceType.All;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="HIDLoader{TLed,TData}"/> class.
    /// </summary>
    /// <param name="vendorId">The vendor id used for this loader.</param>
    public HIDLoader(int vendorId)
    {
        this.VendorId = vendorId;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds a new <see cref="HIDDeviceDefinition{TLed,TData}"/> to this loader.
    /// </summary>
    /// <param name="productId">The product id of the HID-device.</param>
    /// <param name="deviceType">The type of the device.</param>
    /// <param name="name">The name of the device.</param>
    /// <param name="ledMapping">The mapping of the leds of the device.</param>
    /// <param name="customData">Some custom data to attach to the device.</param>
    public void Add(int productId, RGBDeviceType deviceType, string name, LedMapping<TLed> ledMapping, TData customData)
        => _deviceDefinitions.Add(productId, new HIDDeviceDefinition<TLed, TData>(productId, deviceType, name, ledMapping, customData));

    /// <summary>
    /// Gets a enumerable containing all devices from the definition-list that are connected and match the <see cref="LoadFilter"/>.
    /// </summary>
    /// <returns>The enumerable containing the connected devices.</returns>
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

    /// <summary>
    /// Gets a enumerable containing all the first device of each group of devices from the definition-list that are connected and match the <see cref="LoadFilter"/>.
    /// The grouping is done by the specified function.
    /// </summary>
    /// <typeparam name="TKey">The type of the key used to group the devices.</typeparam>
    /// <param name="groupBy">The function grouping the devices.</param>
    /// <returns>The enumerable containing the selected devices.</returns>
    public IEnumerable<(HIDDeviceDefinition<TLed, TData> definition, HidDevice device)> GetConnectedDevices<TKey>(Func<HIDDeviceDefinition<TLed, TData>, TKey> groupBy)
        => GetConnectedDevices().GroupBy(x => groupBy(x.definition))
                                .Select(group => group.First());

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public IEnumerator<HIDDeviceDefinition<TLed, TData>> GetEnumerator() => _deviceDefinitions.Values.GetEnumerator();

    #endregion
}