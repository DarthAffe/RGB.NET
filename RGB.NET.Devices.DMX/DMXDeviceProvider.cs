// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Threading;
using RGB.NET.Core;
using RGB.NET.Devices.DMX.E131;

namespace RGB.NET.Devices.DMX;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for DMX devices.
/// </summary>
public sealed class DMXDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static Lazy<DMXDeviceProvider> _instance = new(LazyThreadSafetyMode.ExecutionAndPublication);
    /// <summary>
    /// Gets the singleton <see cref="DMXDeviceProvider"/> instance.
    /// </summary>
    public static DMXDeviceProvider Instance => _instance.Value;

    /// <summary>
    /// Gets a list of all defined device-definitions.
    /// </summary>
    public List<IDMXDeviceDefinition> DeviceDefinitions { get; } = new();

    #endregion

    #region Methods

    /// <summary>
    /// Adds the specified <see cref="IDMXDeviceDefinition" /> to this device-provider.
    /// </summary>
    /// <param name="deviceDefinition">The <see cref="IDMXDeviceDefinition"/> to add.</param>
    public void AddDeviceDefinition(IDMXDeviceDefinition deviceDefinition) => DeviceDefinitions.Add(deviceDefinition);

    /// <inheritdoc />
    protected override void InitializeSDK() { }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        for (int i = 0; i < DeviceDefinitions.Count; i++)
        {
            IDMXDeviceDefinition dmxDeviceDefinition = DeviceDefinitions[i];
            IRGBDevice? device = null;
            try
            {
                if (dmxDeviceDefinition is E131DMXDeviceDefinition e131DMXDeviceDefinition)
                    if (e131DMXDeviceDefinition.Leds.Count > 0)
                        device = new E131Device(new E131DeviceInfo(e131DMXDeviceDefinition), e131DMXDeviceDefinition.Leds, GetUpdateTrigger(i));
            }
            catch (Exception ex)
            {
                Throw(ex);
            }

            if (device != null)
                yield return device;
        }
    }

    protected override IDeviceUpdateTrigger CreateUpdateTrigger(int id, double updateRateHardLimit)
    {
        DeviceUpdateTrigger updateTrigger = new(updateRateHardLimit);
        if ((DeviceDefinitions[id] is E131DMXDeviceDefinition e131DMXDeviceDefinition))
            updateTrigger.HeartbeatTimer = e131DMXDeviceDefinition.HeartbeatTimer;

        return updateTrigger;
    }

    #endregion
}