using OpenRGB.NET;
using OpenRGB.NET.Models;
using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Devices.OpenRGB;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for OpenRGB devices.
/// </summary>
public class OpenRGBDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private readonly List<OpenRGBClient> _clients = new();

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
                OpenRGBClient openRgb = new(ip: deviceDefinition.Ip, port: deviceDefinition.Port, name: deviceDefinition.ClientName, autoconnect: true);
                _clients.Add(openRgb);
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

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        foreach (OpenRGBClient? openRgb in _clients)
        {
            int deviceCount = openRgb.GetControllerCount();

            for (int i = 0; i < deviceCount; i++)
            {
                Device? device = openRgb.GetControllerData(i);

                int directModeIndex = Array.FindIndex(device.Modes, d => d.Name == "Direct");
                if (directModeIndex != -1)
                {
                    //set the device to direct mode if it has it
                    openRgb.SetMode(i, directModeIndex);
                }
                else if (!ForceAddAllDevices)
                {
                    //if direct mode does not exist
                    //and if we're not forcing, continue to the next device.
                    continue;
                }

                OpenRGBUpdateQueue? updateQueue = new(GetUpdateTrigger(), i, openRgb, device);

                if (PerZoneDeviceFlag.HasFlag(Helper.GetRgbNetDeviceType(device.Type)))
                {
                    int totalLedCount = 0;

                    foreach (Zone zone in device.Zones)
                        if (zone.LedCount > 0)
                        {
                            yield return new OpenRGBZoneDevice(new OpenRGBDeviceInfo(device), totalLedCount, zone, updateQueue);
                            totalLedCount += (int)zone.LedCount;
                        }
                }
                else
                    yield return new OpenRGBGenericDevice(new OpenRGBDeviceInfo(device), updateQueue);
            }
        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        foreach (OpenRGBClient client in _clients)
        {
            try { client.Dispose(); }
            catch { /* at least we tried */ }
        }

        _clients.Clear();
        DeviceDefinitions.Clear();
        Devices = Enumerable.Empty<IRGBDevice>();

        GC.SuppressFinalize(this);
    }
    #endregion
}
