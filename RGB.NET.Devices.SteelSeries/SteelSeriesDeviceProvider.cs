using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.SteelSeries.API;
using RGB.NET.HID;

namespace RGB.NET.Devices.SteelSeries;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for SteelSeries-devices.
/// </summary>
public class SteelSeriesDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static SteelSeriesDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="SteelSeriesDeviceProvider"/> instance.
    /// </summary>
    public static SteelSeriesDeviceProvider Instance => _instance ?? new SteelSeriesDeviceProvider();

    private const int VENDOR_ID = 0x1038;

    /// <summary>
    /// Gets the HID-definitions for SteelSeries-devices.
    /// </summary>
    public static HIDLoader<SteelSeriesLedId, SteelSeriesDeviceType> DeviceDefinitions { get; } = new(VENDOR_ID)
    {
        //Mice
        { 0x1836, RGBDeviceType.Mouse, "Aerox 3", LedMappings.MouseThreeZone, SteelSeriesDeviceType.ThreeZone },
        { 0x183A, RGBDeviceType.Mouse, "Aerox 3 Wireless", LedMappings.MouseThreeZone, SteelSeriesDeviceType.ThreeZone },
        { 0x1702, RGBDeviceType.Mouse, "Rival 100", LedMappings.MouseOneZone, SteelSeriesDeviceType.OneZone },
        { 0x1814, RGBDeviceType.Mouse, "Rival 105", LedMappings.MouseOneZone, SteelSeriesDeviceType.OneZone },
        { 0x1816, RGBDeviceType.Mouse, "Rival 106", LedMappings.MouseOneZone, SteelSeriesDeviceType.OneZone },
        { 0x1729, RGBDeviceType.Mouse, "Rival 110", LedMappings.MouseOneZone, SteelSeriesDeviceType.OneZone },
        { 0x0472, RGBDeviceType.Mouse, "Rival 150", LedMappings.MouseOneZone, SteelSeriesDeviceType.OneZone },
        { 0x1710, RGBDeviceType.Mouse, "Rival 300", LedMappings.MouseTwoZone, SteelSeriesDeviceType.TwoZone },
        { 0x1720, RGBDeviceType.Mouse, "Rival 310", LedMappings.MouseTwoZone, SteelSeriesDeviceType.TwoZone },
        { 0x1722, RGBDeviceType.Mouse, "Sensei 310", LedMappings.MouseTwoZone, SteelSeriesDeviceType.TwoZone },
        { 0x170E, RGBDeviceType.Mouse, "Rival 500", LedMappings.MouseTwoZone, SteelSeriesDeviceType.TwoZone },
        { 0x1724, RGBDeviceType.Mouse, "Rival 600", LedMappings.MouseEightZone, SteelSeriesDeviceType.EightZone },
        { 0x1726, RGBDeviceType.Mouse, "Rival 650", LedMappings.MouseEightZone, SteelSeriesDeviceType.EightZone },
        { 0x172B, RGBDeviceType.Mouse, "Rival 650", LedMappings.MouseEightZone, SteelSeriesDeviceType.EightZone },
        { 0x1700, RGBDeviceType.Mouse, "Rival 700", LedMappings.MouseTwoZone, SteelSeriesDeviceType.TwoZone },
        { 0x1824, RGBDeviceType.Mouse, "Rival 3 (Old Firmware)", LedMappings.MouseThreeZone, SteelSeriesDeviceType.ThreeZone },
        { 0x184C, RGBDeviceType.Mouse, "Rival 3", LedMappings.MouseThreeZone, SteelSeriesDeviceType.ThreeZone },
        { 0x1830, RGBDeviceType.Mouse, "Rival 3 Wireless", LedMappings.MouseThreeZone, SteelSeriesDeviceType.ThreeZone },
        { 0x1832, RGBDeviceType.Mouse, "Sensei Ten", LedMappings.MouseTwoZone, SteelSeriesDeviceType.TwoZone },
        { 0x1838, RGBDeviceType.Mouse, "Aerox 3 Wireless", LedMappings.MouseThreeZone, SteelSeriesDeviceType.ThreeZone },

        //Keyboards
        { 0x161C, RGBDeviceType.Keyboard, "Apex 5", LedMappings.KeyboardMappingUk, SteelSeriesDeviceType.PerKey },
        { 0x1612, RGBDeviceType.Keyboard, "Apex 7", LedMappings.KeyboardMappingUk, SteelSeriesDeviceType.PerKey },
        { 0x1618, RGBDeviceType.Keyboard, "Apex 7 TKL", LedMappings.KeyboardTklMappingUk, SteelSeriesDeviceType.PerKey },
        { 0x0616, RGBDeviceType.Keyboard, "Apex M750", LedMappings.KeyboardMappingUk, SteelSeriesDeviceType.PerKey },
        { 0x1600, RGBDeviceType.Keyboard, "Apex M800", LedMappings.KeyboardMappingUk, SteelSeriesDeviceType.PerKey },
        { 0x1610, RGBDeviceType.Keyboard, "Apex Pro", LedMappings.KeyboardMappingUk, SteelSeriesDeviceType.PerKey },
        { 0x1614, RGBDeviceType.Keyboard, "Apex Pro TKL", LedMappings.KeyboardTklMappingUk, SteelSeriesDeviceType.PerKey },

        //Headsets
        { 0x12AA, RGBDeviceType.Headset, "Arctis 5", LedMappings.HeadsetTwoZone, SteelSeriesDeviceType.TwoZone },
        { 0x1250, RGBDeviceType.Headset, "Arctis 5 Game", LedMappings.HeadsetTwoZone, SteelSeriesDeviceType.TwoZone },
        { 0x1251, RGBDeviceType.Headset, "Arctis 5 Game - Dota 2 edition", LedMappings.HeadsetTwoZone, SteelSeriesDeviceType.TwoZone },
        { 0x12A8, RGBDeviceType.Headset, "Arctis 5 Game - PUBG edition", LedMappings.HeadsetTwoZone, SteelSeriesDeviceType.TwoZone },
        { 0x1252, RGBDeviceType.Headset, "Arctis Pro Game", LedMappings.HeadsetTwoZone, SteelSeriesDeviceType.TwoZone },

        //Mousepads
        { 0x1507, RGBDeviceType.Mousepad, "QCK Prism", LedMappings.MousepadTwelveZone, SteelSeriesDeviceType.TwelveZone },
        { 0x150D, RGBDeviceType.Mousepad, "QCK Prism Cloth", LedMappings.MousepadTwoZone, SteelSeriesDeviceType.TwoZone },

        //Monitors
        { 0x1126, RGBDeviceType.Monitor, "MGP27C", LedMappings.MonitorOnehundredandthreeZone, SteelSeriesDeviceType.OneHundredAndThreeZone },
    };

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SteelSeriesDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public SteelSeriesDeviceProvider()
    {
        if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(SteelSeriesDeviceProvider)}");
        _instance = this;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void InitializeSDK()
    {
        if (!SteelSeriesSDK.IsInitialized)
            SteelSeriesSDK.Initialize();
    }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> GetLoadedDevices(RGBDeviceType loadFilter)
    {
        DeviceDefinitions.LoadFilter = loadFilter;

        return base.GetLoadedDevices(loadFilter);
    }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        foreach ((HIDDeviceDefinition<SteelSeriesLedId, SteelSeriesDeviceType> definition, _) in DeviceDefinitions.GetConnectedDevices(x => x.CustomData))
        {
            string? apiName = definition.CustomData.GetAPIName();
            if (apiName == null)
                Throw(new RGBDeviceException($"Missing API-name for device {definition.Name}"));
            else
                yield return new SteelSeriesRGBDevice(new SteelSeriesRGBDeviceInfo(definition.DeviceType, definition.Name, definition.CustomData), apiName, definition.LedMapping, GetUpdateTrigger());
        }
    }

    /// <inheritdoc />
    protected override IDeviceUpdateTrigger CreateUpdateTrigger(int id, double updateRateHardLimit) => new SteelSeriesDeviceUpdateTrigger(updateRateHardLimit);

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        try { SteelSeriesSDK.Dispose(); }
        catch { /* shit happens */ }
    }

    #endregion
}