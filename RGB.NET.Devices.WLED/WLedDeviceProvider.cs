// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.WLED;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for WS2812B- and WS2811-Led-devices.
/// </summary>
// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public sealed class WledDeviceProvider : AbstractRGBDeviceProvider
{
    #region Constants

    private const int HEARTBEAT_TIMER = 250;

    #endregion

    #region Properties & Fields

    // ReSharper disable once InconsistentNaming
    private static readonly object _lock = new();

    private static WledDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="WledDeviceProvider"/> instance.
    /// </summary>
    public static WledDeviceProvider Instance
    {
        get
        {
            lock (_lock)
                return _instance ?? new WledDeviceProvider();
        }
    }

    /// <summary>
    /// Gets a list of all defined device-definitions.
    /// </summary>
    public List<IWledDeviceDefinition> DeviceDefinitions { get; } = [];

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WledDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public WledDeviceProvider()
    {
        lock (_lock)
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(WledDeviceProvider)}");
            _instance = this;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds the specified <see cref="IWledDeviceDefinition" /> to this device-provider.
    /// </summary>
    /// <param name="deviceDefinition">The <see cref="IWledDeviceDefinition"/> to add.</param>
    public void AddDeviceDefinition(IWledDeviceDefinition deviceDefinition) => DeviceDefinitions.Add(deviceDefinition);

    /// <inheritdoc />
    protected override void InitializeSDK() { }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        int i = 0;
        foreach (IWledDeviceDefinition deviceDefinition in DeviceDefinitions)
        {
            IDeviceUpdateTrigger updateTrigger = GetUpdateTrigger(i++);
            WledRGBDevice? device = CreateWledDevice(deviceDefinition, updateTrigger);
            if (device != null)
                yield return device;
        }
    }

    private static WledRGBDevice? CreateWledDevice(IWledDeviceDefinition deviceDefinition, IDeviceUpdateTrigger updateTrigger)
    {
        WledInfo? wledInfo = WledAPI.Info(deviceDefinition.Address);
        if (wledInfo == null) return null;

        return new WledRGBDevice(new WledRGBDeviceInfo(wledInfo, deviceDefinition.Manufacturer, deviceDefinition.Model), deviceDefinition.Address, updateTrigger);
    }

    protected override IDeviceUpdateTrigger CreateUpdateTrigger(int id, double updateRateHardLimit) => new DeviceUpdateTrigger(updateRateHardLimit) { HeartbeatTimer = HEARTBEAT_TIMER };

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        lock (_lock)
        {
            base.Dispose(disposing);

            DeviceDefinitions.Clear();

            _instance = null;
        }
    }

    #endregion
}