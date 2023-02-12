// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for Cooler Master devices.
/// </summary>
public class AsusDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static AsusDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="AsusDeviceProvider"/> instance.
    /// </summary>
    public static AsusDeviceProvider Instance => _instance ?? new AsusDeviceProvider();

    private IAuraSdk2? _sdk;
    private IAuraSyncDeviceCollection? _devices; //HACK DarthAffe 05.04.2021: Due to some researches this might fix the access violation in the asus-sdk

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AsusDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public AsusDeviceProvider()
    {
        if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(AsusDeviceProvider)}");
        _instance = this;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void InitializeSDK()
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        _sdk = (IAuraSdk2)new AuraSdk();
        _sdk.SwitchMode();
    }

    /// <inheritdoc />
    
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        if (_sdk == null) yield break;

        _devices = _sdk.Enumerate(0);
        for (int i = 0; i < _devices.Count; i++)
        {
            IAuraSyncDevice device = _devices[i];
            yield return (AsusDeviceType)device.Type switch
            {
                AsusDeviceType.MB_RGB => new AsusMainboardRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Mainboard, device, WMIHelper.GetMainboardInfo()?.model ?? device.Name), GetUpdateTrigger()),
                AsusDeviceType.MB_ADDRESABLE => new AsusUnspecifiedRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.LedStripe, device), LedId.LedStripe1, GetUpdateTrigger()),
                AsusDeviceType.VGA_RGB => new AsusGraphicsCardRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.GraphicsCard, device), GetUpdateTrigger()),
                AsusDeviceType.HEADSET_RGB => new AsusHeadsetRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Headset, device), GetUpdateTrigger()),
                AsusDeviceType.DRAM_RGB => new AsusDramRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.DRAM, device), GetUpdateTrigger()),
                AsusDeviceType.KEYBOARD_RGB => new AsusKeyboardRGBDevice(new AsusKeyboardRGBDeviceInfo(device), LedMappings.KeyboardMapping, GetUpdateTrigger()),
                AsusDeviceType.NB_KB_RGB => new AsusKeyboardRGBDevice(new AsusKeyboardRGBDeviceInfo(device), LedMappings.KeyboardMapping, GetUpdateTrigger()),
                AsusDeviceType.NB_KB_4ZONE_RGB => new AsusKeyboardRGBDevice(new AsusKeyboardRGBDeviceInfo(device), null, GetUpdateTrigger()),
                AsusDeviceType.MOUSE_RGB => new AsusMouseRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Mouse, device), GetUpdateTrigger()),
                _ => new AsusUnspecifiedRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Unknown, device), LedId.Custom1, GetUpdateTrigger())
            };
        }
    }
    /*
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        if (_sdk == null) { 
            throw new InvalidOperationException($"Asus Library not yet loaded.");
            yield break;
        }

        var enviroment = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
        var filePath = $"{enviroment}/ASUSDEBUG.txt";
        var content = "";

        _devices = _sdk.Enumerate(0);
        for (int i = 0; i < _devices.Count; i++)
        {
            IAuraSyncDevice device = _devices[i];

            content += $"Name: {device.Name} Type: {device.Type} Type Hex: {device.Type:X} LEDs: {device.Lights.Count} \n\n";

            yield return (AsusDeviceType)device.Type switch
            {
                AsusDeviceType.MB_RGB => new AsusMainboardRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Mainboard, device, WMIHelper.GetMainboardInfo()?.model ?? device.Name), GetUpdateTrigger()),
                AsusDeviceType.MB_ADDRESABLE => new AsusUnspecifiedRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.LedStripe, device), LedId.LedStripe1, GetUpdateTrigger()),
                AsusDeviceType.VGA_RGB => new AsusGraphicsCardRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.GraphicsCard, device), GetUpdateTrigger()),
                AsusDeviceType.HEADSET_RGB => new AsusHeadsetRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Headset, device), GetUpdateTrigger()),
                AsusDeviceType.DRAM_RGB => new AsusDramRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.DRAM, device), GetUpdateTrigger()),
                AsusDeviceType.KEYBOARD_RGB => new AsusKeyboardRGBDevice(new AsusKeyboardRGBDeviceInfo(device), LedMappings.KeyboardMapping, GetUpdateTrigger()),
                AsusDeviceType.NB_KB_RGB => new AsusKeyboardRGBDevice(new AsusKeyboardRGBDeviceInfo(device), LedMappings.KeyboardMapping, GetUpdateTrigger()),
                AsusDeviceType.NB_KB_4ZONE_RGB => new AsusKeyboardRGBDevice(new AsusKeyboardRGBDeviceInfo(device), null, GetUpdateTrigger()),
                AsusDeviceType.MOUSE_RGB => new AsusMouseRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Mouse, device), GetUpdateTrigger()),
                AsusDeviceType.TERMINAL_RGB => new AsusUnspecifiedRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.LedController, device), LedId.Custom1, GetUpdateTrigger()),
                _ => new AsusUnspecifiedRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Unknown, device), LedId.Custom1, GetUpdateTrigger())
            };
        }

        System.IO.File.WriteAllText(filePath, content);
    }
    */

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        try { _sdk?.ReleaseControl(0); }
        catch { /* at least we tried */ }

        _devices = null;
        _sdk = null;

        GC.SuppressFinalize(this);
    }

    #endregion
}