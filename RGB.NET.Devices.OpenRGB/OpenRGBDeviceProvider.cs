using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenRGB.NET;
using OpenRgbDevice = OpenRGB.NET.Device;

namespace RGB.NET.Devices.OpenRGB;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for OpenRGB devices.
/// </summary>
public sealed class OpenRGBDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private readonly List<OpenRGBClientWrapper> _clients = new();

    private static OpenRGBDeviceProvider? _instance;

    /// <summary>
    /// Gets the singleton <see cref="OpenRGBDeviceProvider"/> instance.
    /// </summary>
    public static OpenRGBDeviceProvider Instance => _instance ?? new OpenRGBDeviceProvider();

    /// <summary>
    /// Gets a list of all defined device-definitions.
    /// </summary>
    public List<OpenRGBServerDefinition> DeviceDefinitions { get; } = new();

    /// <summary>
    /// Indicates whether all devices will be added, or just the ones with a 'Direct' mode. Defaults to false.
    /// </summary>
    public bool ForceAddAllDevices { get; set; } = false;

    /// <summary>
    /// Defines which device types will be separated by zones. Defaults to <see cref="RGBDeviceType.LedStripe" /> | <see cref="RGBDeviceType.Mainboard"/> | <see cref="RGBDeviceType.Speaker" />.
    /// </summary>
    public RGBDeviceType PerZoneDeviceFlag { get; } = RGBDeviceType.LedStripe | RGBDeviceType.Mainboard | RGBDeviceType.Speaker;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenRGBDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public OpenRGBDeviceProvider()
    {
        if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(OpenRGBDeviceProvider)}");
        _instance = this;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds the specified <see cref="OpenRGBServerDefinition" /> to this device-provider.
    /// </summary>
    /// <param name="deviceDefinition">The <see cref="OpenRGBServerDefinition"/> to add.</param>
    // ReSharper disable once UnusedMember.Global
    public void AddDeviceDefinition(OpenRGBServerDefinition deviceDefinition) => DeviceDefinitions.Add(deviceDefinition);

    /// <inheritdoc />
    protected override void InitializeSDK()
    {
        foreach (OpenRGBServerDefinition? deviceDefinition in DeviceDefinitions)
        {
            try
            {
                OpenRgbClient openRgb = new(ip: deviceDefinition.Ip, port: deviceDefinition.Port, name: deviceDefinition.ClientName, autoConnect: true);
                OpenRGBClientWrapper wrapper = new(openRgb);
                openRgb.DeviceListUpdated += (_, _) => RefreshClient(wrapper);
                _clients.Add(wrapper);
                deviceDefinition.Connected = true;
            }
            catch (Exception e)
            {
                deviceDefinition.Connected = false;
                deviceDefinition.LastError = e.Message;
                Throw(e);
            }
        }
    }
    
    private void RefreshClient(OpenRGBClientWrapper openRgbClient)
    {
        List<IRGBDevice> currentDevices = new(openRgbClient.Devices);
        List<IRGBDevice> newDevices = GetOrgbClientDevices(openRgbClient.OpenRgbClient)
                                      .SelectMany((device, i) => SplitDevice(device, i, openRgbClient.OpenRgbClient)).ToList();
        
        foreach (IRGBDevice rgbDevice in currentDevices.Except(newDevices))
        {
            RemoveDevice(rgbDevice); 
            openRgbClient.Devices.Remove(rgbDevice);
        }
        foreach (IRGBDevice rgbDevice in newDevices)
        {
            openRgbClient.Devices.Add(rgbDevice);
            AddDevice(rgbDevice);
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        return _clients.SelectMany(LoadClientDevices);
    }

    private IEnumerable<IRGBDevice> LoadClientDevices(OpenRGBClientWrapper client)
    {
        List<IRGBDevice> devices = GetOrgbClientDevices(client.OpenRgbClient)
                                   .SelectMany((device, i) => SplitDevice(device, i, client.OpenRgbClient))
                                   .ToList();
        client.Devices.AddRange(devices);
        return devices;
    }

    private IEnumerable<OpenRgbDevice> GetOrgbClientDevices(IOpenRgbClient openRgb)
    {
        int deviceCount = openRgb.GetControllerCount();

        for (int i = 0; i < deviceCount; i++)
        {
            OpenRgbDevice device = openRgb.GetControllerData(i);

            int directModeIndex = Array.FindIndex(device.Modes, d => d.Name == "Direct");
            if (directModeIndex != -1)
            {
                //set the device to direct mode if it has it
                openRgb.UpdateMode(i, directModeIndex);
            }
            else if (!ForceAddAllDevices)
            {
                //if direct mode does not exist
                //and if we're not forcing, continue to the next device.
                continue;
            }

            if (device.Zones.Length == 0)
                continue;
            if (device.Zones.All(z => z.LedCount == 0))
                continue;

            yield return device;
        }
    }

    private IEnumerable<IRGBDevice> SplitDevice(OpenRgbDevice device, int i, IOpenRgbClient openRgb)
    {
        IDeviceUpdateTrigger clientUpdateTrigger = GetUpdateTrigger();
        OpenRGBUpdateQueue updateQueue = new(clientUpdateTrigger, i, openRgb, device);

        bool anyZoneHasSegments = device.Zones.Any(z => z.Segments.Length > 0);
        bool splitDeviceByZones = anyZoneHasSegments || PerZoneDeviceFlag.HasFlag(Helper.GetRgbNetDeviceType(device.Type));

        if (!splitDeviceByZones)
        {
            yield return new OpenRGBGenericDevice(new OpenRGBDeviceInfo(device), updateQueue);
            yield break;
        }

        int totalLedCount = 0;

        foreach (Zone zone in device.Zones)
        {
            if (zone.LedCount <= 0)
                continue;

            if (zone.Segments.Length <= 0)
            {
                string zoneId = zone.Name;
                yield return new OpenRGBZoneDevice(new OpenRGBDeviceInfo(device, zoneId), totalLedCount, zone, updateQueue);
                totalLedCount += (int)zone.LedCount;
            }
            else
            {
                foreach (Segment segment in zone.Segments)
                {
                    string zoneId = segment.Name;
                    yield return new OpenRGBSegmentDevice(new OpenRGBDeviceInfo(device, zoneId), totalLedCount, segment, updateQueue);
                    totalLedCount += (int)segment.LedCount;
                }
            }
        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        foreach (OpenRGBClientWrapper wrapper in _clients)
        {
            try { wrapper.Dispose(); }
            catch { /* at least we tried */ }
        }

        _clients.Clear();
        DeviceDefinitions.Clear();

        IdGenerator.ResetCounter(typeof(OpenRGBDeviceProvider));
    }

    #endregion
}
