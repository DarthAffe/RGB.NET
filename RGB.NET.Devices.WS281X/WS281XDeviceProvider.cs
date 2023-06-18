// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.Threading;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for WS2812B- and WS2811-Led-devices.
/// </summary>
// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public sealed class WS281XDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static Lazy<WS281XDeviceProvider> _instance = new(LazyThreadSafetyMode.ExecutionAndPublication);
    /// <summary>
    /// Gets the singleton <see cref="WS281XDeviceProvider"/> instance.
    /// </summary>
    public static WS281XDeviceProvider Instance => _instance.Value;

    /// <summary>
    /// Gets a list of all defined device-definitions.
    /// </summary>
    // ReSharper disable once CollectionNeverUpdated.Global
    // ReSharper disable once ReturnTypeCanBeEnumerable.Global
    public List<IWS281XDeviceDefinition> DeviceDefinitions { get; } = new();

    #endregion

    #region Methods

    /// <summary>
    /// Adds the specified <see cref="IWS281XDeviceDefinition" /> to this device-provider.
    /// </summary>
    /// <param name="deviceDefinition">The <see cref="IWS281XDeviceDefinition"/> to add.</param>
    // ReSharper disable once UnusedMember.Global
    public void AddDeviceDefinition(IWS281XDeviceDefinition deviceDefinition) => DeviceDefinitions.Add(deviceDefinition);

    /// <inheritdoc />
    protected override void InitializeSDK() { }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        int i = 0;
        foreach (IWS281XDeviceDefinition deviceDefinition in DeviceDefinitions)
        {
            IDeviceUpdateTrigger updateTrigger = GetUpdateTrigger(i++);
            foreach (IRGBDevice device in deviceDefinition.CreateDevices(updateTrigger))
                yield return device;
        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        DeviceDefinitions.Clear();
    }

    #endregion
}