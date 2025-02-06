// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;
using RGB.NET.HID;

namespace RGB.NET.Devices.Razer;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for razer devices.
/// </summary>
public sealed class RazerDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    // ReSharper disable once InconsistentNaming
    private static readonly object _lock = new();

    private static RazerDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="RazerDeviceProvider"/> instance.
    /// </summary>
    public static RazerDeviceProvider Instance
    {
        get
        {
            lock (_lock)
                return _instance ?? new RazerDeviceProvider();
        }
    }

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX86NativePaths { get; } = [@"%systemroot%\SysWOW64\RzChromaSDK.dll"];

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX64NativePaths { get; } = [@"%systemroot%\System32\RzChromaSDK.dll", @"%systemroot%\System32\RzChromaSDK64.dll"];

    /// <summary>
    /// Forces to load the devices represented by the emulator even if they aren't reported to exist.
    /// </summary>
    public RazerEndpointType LoadEmulatorDevices { get; set; } = RazerEndpointType.None;

    private const int VENDOR_ID = 0x1532;

    /// <summary>
    /// Gets the HID-definitions for Razer-devices.
    /// </summary>
    public static HIDLoader<int, RazerEndpointType> DeviceDefinitions { get; } = new(VENDOR_ID)
    {
        // Keyboards
        { 0x010D, RGBDeviceType.Keyboard, "BlackWidow Ultimate 2012", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x010E, RGBDeviceType.Keyboard, "BlackWidow Classic (Alternate)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x010F, RGBDeviceType.Keyboard, "Anansi", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x011A, RGBDeviceType.Keyboard, "BlackWidow Ultimate 2013", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x011B, RGBDeviceType.Keyboard, "BlackWidow Stealth", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x011C, RGBDeviceType.Keyboard, "BlackWidow Tournament Edition 2014", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0202, RGBDeviceType.Keyboard, "DeathStalker Expert", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0203, RGBDeviceType.Keyboard, "BlackWidow Chroma", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0204, RGBDeviceType.Keyboard, "DeathStalker Chroma", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0205, RGBDeviceType.Keyboard, "Blade Stealth", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0209, RGBDeviceType.Keyboard, "BlackWidow Tournament Edition Chroma", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x020F, RGBDeviceType.Keyboard, "Blade QHD", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0210, RGBDeviceType.Keyboard, "Blade Pro (Late 2016)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0211, RGBDeviceType.Keyboard, "BlackWidow Chroma (Overwatch)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0214, RGBDeviceType.Keyboard, "BlackWidow Ultimate 2016", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0216, RGBDeviceType.Keyboard, "BlackWidow X Chroma", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0217, RGBDeviceType.Keyboard, "BlackWidow X Ultimate", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x021A, RGBDeviceType.Keyboard, "BlackWidow X Tournament Edition Chroma", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x021E, RGBDeviceType.Keyboard, "Ornata Chroma", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x021F, RGBDeviceType.Keyboard, "Ornata", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0220, RGBDeviceType.Keyboard, "Blade Stealth (Late 2016)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0221, RGBDeviceType.Keyboard, "BlackWidow Chroma V2", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0224, RGBDeviceType.Keyboard, "Blade (Late 2016)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0225, RGBDeviceType.Keyboard, "Blade Pro (2017)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0226, RGBDeviceType.Keyboard, "Huntsman Elite", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0227, RGBDeviceType.Keyboard, "Huntsman", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0228, RGBDeviceType.Keyboard, "BlackWidow Elite", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x022A, RGBDeviceType.Keyboard, "Cynosa Chroma", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x022C, RGBDeviceType.Keyboard, "Cynosa Chroma Pro", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x022D, RGBDeviceType.Keyboard, "Blade Stealth (Mid 2017)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x022F, RGBDeviceType.Keyboard, "Blade Pro FullHD (2017)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0232, RGBDeviceType.Keyboard, "Blade Stealth (Late 2017)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0233, RGBDeviceType.Keyboard, "Blade 15 (2018)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0234, RGBDeviceType.Keyboard, "Blade Pro 17 (2019)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0235, RGBDeviceType.Keyboard, "BlackWidow Lite (2018)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0237, RGBDeviceType.Keyboard, "BlackWidow Essential", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0239, RGBDeviceType.Keyboard, "Blade Stealth (2019)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x023A, RGBDeviceType.Keyboard, "Blade 15 (2019) Advanced", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x023B, RGBDeviceType.Keyboard, "Blade 15 (2018) Base Model", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x023F, RGBDeviceType.Keyboard, "Cynosa Lite", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0240, RGBDeviceType.Keyboard, "Blade 15 (2018) Mercury", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0241, RGBDeviceType.Keyboard, "BlackWidow (2019)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0243, RGBDeviceType.Keyboard, "Huntsman Tournament Edition", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0245, RGBDeviceType.Keyboard, "Blade 15 (Mid 2019) Mercury", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0246, RGBDeviceType.Keyboard, "Blade 15 (Mid 2019) Base", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x024A, RGBDeviceType.Keyboard, "Blade Stealth (Late 2019)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x024C, RGBDeviceType.Keyboard, "Blade Pro (Late 2019)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x024D, RGBDeviceType.Keyboard, "Blade 15 Studio Edition (2019)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x024E, RGBDeviceType.Keyboard, "BlackWidow V3", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0252, RGBDeviceType.Keyboard, "Blade Stealth (Early 2020)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0253, RGBDeviceType.Keyboard, "Blade 15 Advanced (2020)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0255, RGBDeviceType.Keyboard, "Blade 15 (Early 2020) Base", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0256, RGBDeviceType.Keyboard, "Blade Blade Pro (Early 2020)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x0257, RGBDeviceType.Keyboard, "Huntsman Mini", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0258, RGBDeviceType.Keyboard, "BlackWidow V3 Mini", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0259, RGBDeviceType.Keyboard, "Blade Stealth (Late 2020)", LedMappings.Keyboard, RazerEndpointType.LaptopKeyboard },
        { 0x025A, RGBDeviceType.Keyboard, "BlackWidow V3 Pro", LedMappings.Keyboard, RazerEndpointType.Keyboard }, // The keyboard, only present when connected with cable
        { 0x025C, RGBDeviceType.Keyboard, "BlackWidow V3 Pro", LedMappings.Keyboard, RazerEndpointType.Keyboard }, // The dongle, may not be present when connected with cable
        { 0x025D, RGBDeviceType.Keyboard, "Ornata Chroma V2", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x025E, RGBDeviceType.Keyboard, "Cynosa V2", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0266, RGBDeviceType.Keyboard, "Huntsman V2", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0269, RGBDeviceType.Keyboard, "Huntsman Mini (JP)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x026A, RGBDeviceType.Keyboard, "Book 13 (2020)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x026B, RGBDeviceType.Keyboard, "Huntsman V2 TKL", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x026C, RGBDeviceType.Keyboard, "Huntsman V2", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x026D, RGBDeviceType.Keyboard, "Blade 15 Advanced (Early 2021)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x026E, RGBDeviceType.Keyboard, "Blade 17 Pro (Early 2021)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x026F, RGBDeviceType.Keyboard, "Blade 15 Base (Early 2021)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0270, RGBDeviceType.Keyboard, "Blade 14 (2021)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0271, RGBDeviceType.Keyboard, "BlackWidow V3 Mini Hyperspeed", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0276, RGBDeviceType.Keyboard, "Blade 15 Advanced (Mid 2021)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0279, RGBDeviceType.Keyboard, "Blade 17 Pro (Mid 2021)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x027A, RGBDeviceType.Keyboard, "Blade 15 Base (2022)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0282, RGBDeviceType.Keyboard, "Huntsman Mini Analog", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0287, RGBDeviceType.Keyboard, "BlackWidow V4", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x028A, RGBDeviceType.Keyboard, "Blade 15 Advanced (Early 2022)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x028B, RGBDeviceType.Keyboard, "Blade 17 (2022)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x028C, RGBDeviceType.Keyboard, "Blade 14 (2022)", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x029F, RGBDeviceType.Keyboard, "Blade 16 (2023)", LedMappings.Blade, RazerEndpointType.Keyboard },
        { 0x028D, RGBDeviceType.Keyboard, "BlackWidow V4 Pro", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0290, RGBDeviceType.Keyboard, "DeathStalker V2 Pro", LedMappings.Keyboard, RazerEndpointType.Keyboard }, // Wireless
        { 0x0292, RGBDeviceType.Keyboard, "DeathStalker V2 Pro", LedMappings.Keyboard, RazerEndpointType.Keyboard }, // Wired
        { 0x0293, RGBDeviceType.Keyboard, "BlackWidow V4 X", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0294, RGBDeviceType.Keyboard, "Ornata V3 X", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0295, RGBDeviceType.Keyboard, "DeathStalker V2", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0296, RGBDeviceType.Keyboard, "DeathStalker V2 Pro TKL", LedMappings.Keyboard, RazerEndpointType.Keyboard }, // Wireless
        { 0x0298, RGBDeviceType.Keyboard, "DeathStalker V2 Pro TKL", LedMappings.Keyboard, RazerEndpointType.Keyboard }, // Wired
        { 0x02A0, RGBDeviceType.Keyboard, "Blade 18", LedMappings.Blade, RazerEndpointType.Keyboard },
        { 0x02A1, RGBDeviceType.Keyboard, "Ornata V3", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x02A5, RGBDeviceType.Keyboard, "BlackWidow V4 75%", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x02A7, RGBDeviceType.Keyboard, "Huntsman V3 Pro TKL", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x0A24, RGBDeviceType.Keyboard, "BlackWidow V3 TKL", LedMappings.Keyboard, RazerEndpointType.Keyboard },
        { 0x02BA, RGBDeviceType.Keyboard, "BlackWidow V4 Mini HyperSpeed", LedMappings.Keyboard, RazerEndpointType.Keyboard },

        // Mice
        { 0x0013, RGBDeviceType.Mouse, "Orochi 2011", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0016, RGBDeviceType.Mouse, "DeathAdder 3.5G", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0020, RGBDeviceType.Mouse, "Abyssus 1800", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0024, RGBDeviceType.Mouse, "Mamba 2012 (Wired)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0025, RGBDeviceType.Mouse, "Mamba 2012 (Wireless)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0029, RGBDeviceType.Mouse, "DeathAdder V3 5G", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x002E, RGBDeviceType.Mouse, "Naga 2012", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x002F, RGBDeviceType.Mouse, "Imperator 2012", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0032, RGBDeviceType.Mouse, "Ouroboros 2012", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0034, RGBDeviceType.Mouse, "Taipan", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0036, RGBDeviceType.Mouse, "Naga Hex (Red)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0037, RGBDeviceType.Mouse, "DeathAdder 2013", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0038, RGBDeviceType.Mouse, "DeathAdder 1800", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0039, RGBDeviceType.Mouse, "Orochi 2013", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0040, RGBDeviceType.Mouse, "Naga 2014", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0041, RGBDeviceType.Mouse, "Naga Hex", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0042, RGBDeviceType.Mouse, "Abyssus 2014", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0043, RGBDeviceType.Mouse, "DeathAdder Chroma", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0044, RGBDeviceType.Mouse, "Mamba (Wired)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0045, RGBDeviceType.Mouse, "Mamba (Wireless)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0046, RGBDeviceType.Mouse, "Mamba Tournament Edition", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0048, RGBDeviceType.Mouse, "Orochi (Wired)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x004C, RGBDeviceType.Mouse, "Diamondback Chroma", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x004F, RGBDeviceType.Mouse, "DeathAdder 2000", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0050, RGBDeviceType.Mouse, "Naga Hex V2", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0053, RGBDeviceType.Mouse, "Naga Chroma", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0054, RGBDeviceType.Mouse, "DeathAdder 3500", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0059, RGBDeviceType.Mouse, "Lancehead (Wired)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x005A, RGBDeviceType.Mouse, "Lancehead (Wireless)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x005B, RGBDeviceType.Mouse, "Abyssus V2", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x005C, RGBDeviceType.Mouse, "DeathAdder Elite", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x005E, RGBDeviceType.Mouse, "Abyssus 2000", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0060, RGBDeviceType.Mouse, "Lancehead Tournament Edition", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0062, RGBDeviceType.Mouse, "Atheris (Receiver)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0064, RGBDeviceType.Mouse, "Basilisk", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0065, RGBDeviceType.Mouse, "Basilisk Essential", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0067, RGBDeviceType.Mouse, "Naga Trinity", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x006A, RGBDeviceType.Mouse, "Abyssus Elite (D.Va Edition)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x006B, RGBDeviceType.Mouse, "Abyssus Essential", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x006C, RGBDeviceType.Mouse, "Mamba Elite (Wired)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x006E, RGBDeviceType.Mouse, "DeathAdder Essential", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x006F, RGBDeviceType.Mouse, "Lancehead Wireless (Receiver)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0070, RGBDeviceType.Mouse, "Lancehead Wireless (Wired)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0071, RGBDeviceType.Mouse, "DeathAdder Essential (White Edition)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0072, RGBDeviceType.Mouse, "Mamba Wireless (Receiver)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0073, RGBDeviceType.Mouse, "Mamba Wireless (Wired)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0077, RGBDeviceType.Mouse, "Pro Click (Wireless)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0078, RGBDeviceType.Mouse, "Viper", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x007A, RGBDeviceType.Mouse, "Viper Ultimate (Wired)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x007B, RGBDeviceType.Mouse, "Viper Ultimate (Wireless)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x007C, RGBDeviceType.Mouse, "DeathAdder V2 Pro (Wired)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x007D, RGBDeviceType.Mouse, "DeathAdder V2 Pro (Wireless)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0080, RGBDeviceType.Mouse, "Pro Click (Wired)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0083, RGBDeviceType.Mouse, "Basilisk X HyperSpeed", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0084, RGBDeviceType.Mouse, "DeathAdder V2", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0085, RGBDeviceType.Mouse, "Basilisk V2", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0086, RGBDeviceType.Mouse, "Basilisk Ultimate (Wireless)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0088, RGBDeviceType.Mouse, "Basilisk Ultimate", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x008A, RGBDeviceType.Mouse, "Viper Mini", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x008C, RGBDeviceType.Mouse, "DeathAdder V2 Mini", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x008D, RGBDeviceType.Mouse, "Naga Left Handed Edition", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x008F, RGBDeviceType.Mouse, "Naga Pro", LedMappings.Mouse, RazerEndpointType.Mouse }, //this is via usb connection
        { 0x0090, RGBDeviceType.Mouse, "Naga Pro", LedMappings.Mouse, RazerEndpointType.Mouse }, //this is via bluetooth connection
        { 0x0091, RGBDeviceType.Mouse, "Viper 8khz", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0093, RGBDeviceType.Mouse, "Naga Classic", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0094, RGBDeviceType.Mouse, "Orochi V2", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0096, RGBDeviceType.Mouse, "Naga X", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x0099, RGBDeviceType.Mouse, "Basilisk v3", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x009A, RGBDeviceType.Mouse, "Pro Click Mini (Wireless)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x009C, RGBDeviceType.Mouse, "DeathAdder V2 X Hyperspeed", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x00A1, RGBDeviceType.Mouse, "DeathAdder V2 Lite", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x00A3, RGBDeviceType.Mouse, "Cobra", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x00A5, RGBDeviceType.Mouse, "Viper V2 Pro (Wired)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x00A6, RGBDeviceType.Mouse, "Viper V2 Pro (Wireless)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x00A7, RGBDeviceType.Mouse, "Naga V2 Pro", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x00A8, RGBDeviceType.Mouse, "Naga V2 Pro", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x00AA, RGBDeviceType.Mouse, "Basilisk V3 Pro (Wired)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x00AB, RGBDeviceType.Mouse, "Basilisk V3 Pro (Wireless)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x00B6, RGBDeviceType.Mouse, "DeathAdder V3 (Wired)", LedMappings.Mouse, RazerEndpointType.Mouse },
        { 0x00B7, RGBDeviceType.Mouse, "DeathAdder V3 (Wireless)", LedMappings.Mouse, RazerEndpointType.Mouse },
        
        // Mousepads
        { 0x0068, RGBDeviceType.Mousepad, "Firefly Hyperflux", LedMappings.Mousepad, RazerEndpointType.Mousepad },
        { 0x0C00, RGBDeviceType.Mousepad, "Firefly", LedMappings.Mousepad, RazerEndpointType.Mousepad },
        { 0x0C01, RGBDeviceType.Mousepad, "Goliathus", LedMappings.Mousepad, RazerEndpointType.ChromaLink },
        { 0x0C02, RGBDeviceType.Mousepad, "Goliathus Extended", LedMappings.Mousepad, RazerEndpointType.ChromaLink },
        { 0x0C04, RGBDeviceType.Mousepad, "Firefly v2", LedMappings.Mousepad, RazerEndpointType.Mousepad },
        { 0x0C08, RGBDeviceType.Mousepad, "Firefly v2 Pro", LedMappings.Mousepad, RazerEndpointType.Mousepad },

        // Headsets
        { 0x0501, RGBDeviceType.Headset, "Kraken 7.1", LedMappings.Headset, RazerEndpointType.Headset },
        { 0x0504, RGBDeviceType.Headset, "Kraken 7.1 Chroma", LedMappings.Headset, RazerEndpointType.Headset },
        { 0x0506, RGBDeviceType.Headset, "Kraken 7.1", LedMappings.Headset, RazerEndpointType.Headset },
        { 0x0510, RGBDeviceType.Headset, "Kraken 7.1 V2", LedMappings.Headset, RazerEndpointType.Headset },
        { 0x051A, RGBDeviceType.Headset, "Nari Ultimate", LedMappings.Headset, RazerEndpointType.Headset },
        { 0x051C, RGBDeviceType.Headset, "Nari", LedMappings.Headset, RazerEndpointType.Headset },
        { 0x0527, RGBDeviceType.Headset, "Kraken Ultimate", LedMappings.Headset, RazerEndpointType.Headset },
        { 0x0560, RGBDeviceType.Headset, "Kraken Kitty V2", LedMappings.Headset, RazerEndpointType.Headset },
        { 0x0F19, RGBDeviceType.Headset, "Kraken Kitty Edition", LedMappings.Headset, RazerEndpointType.Headset },

        // Keypads
        { 0x0111, RGBDeviceType.Keypad, "Nostromo", LedMappings.Keypad, RazerEndpointType.Keypad },
        { 0x0113, RGBDeviceType.Keypad, "Orbweaver", LedMappings.Keypad, RazerEndpointType.Keypad },
        { 0x0201, RGBDeviceType.Keypad, "Tartarus", LedMappings.Keypad, RazerEndpointType.Keypad },
        { 0x0207, RGBDeviceType.Keypad, "Orbweaver Chroma", LedMappings.Keypad, RazerEndpointType.Keypad },
        { 0x0208, RGBDeviceType.Keypad, "Tartarus Chroma", LedMappings.Keypad, RazerEndpointType.Keypad },
        { 0x022B, RGBDeviceType.Keypad, "Tartarus V2", LedMappings.Keypad, RazerEndpointType.Keypad },
        { 0x0244, RGBDeviceType.Keypad, "Tartarus Pro", LedMappings.Keypad, RazerEndpointType.Keypad },

        // Misc - guessing these are through ChromaLink
        { 0x0215, RGBDeviceType.GraphicsCard, "Core", LedMappings.ChromaLink, RazerEndpointType.ChromaLink },
        { 0x0F08, RGBDeviceType.HeadsetStand, "Base Station Chroma", LedMappings.Mousepad, RazerEndpointType.Mousepad }, // DarthAffe 16.12.2022: Not tested but based on the V2 I assume this is also a mousepad
        { 0x0F20, RGBDeviceType.HeadsetStand, "Base Station V2 Chroma", LedMappings.Mousepad, RazerEndpointType.Mousepad }, // DarthAffe 16.12.2022: Not sure why, but it's handled as a mousepad
        { 0x007E, RGBDeviceType.LedStripe, "Razer Mouse Dock Chroma", LedMappings.Mousepad, RazerEndpointType.Mousepad }, //roxaskeyheart 06.02.2023: Probably handled the same as a mousepad
        { 0x00A4, RGBDeviceType.LedStripe, "Mouse Dock Pro", LedMappings.Mousepad, RazerEndpointType.Mousepad }, // DarthAffe 22.09.2023: Most likely also a mousepad?
        { 0x0517, RGBDeviceType.Speaker, "Nommo Chroma", LedMappings.ChromaLink, RazerEndpointType.ChromaLink },
        { 0x0518, RGBDeviceType.Speaker, "Nommo Pro", LedMappings.ChromaLink, RazerEndpointType.ChromaLink },
        { 0x054A, RGBDeviceType.Speaker, "Leviathan V2 X", LedMappings.ChromaLink, RazerEndpointType.ChromaLink },
        { 0x0F07, RGBDeviceType.Unknown, "Chroma Mug Holder", LedMappings.ChromaLink, RazerEndpointType.ChromaLink },
        { 0x0F09, RGBDeviceType.LedController, "Chroma Hardware Development Kit (HDK)", LedMappings.ChromaLink, RazerEndpointType.ChromaLink },
        { 0x0F13, RGBDeviceType.Unknown, "Lian Li O11", LedMappings.ChromaLink, RazerEndpointType.ChromaLink },
        { 0x0F1D, RGBDeviceType.Unknown, "Mouse Bungee V3 Chroma", LedMappings.ChromaLink, RazerEndpointType.ChromaLink },
        { 0x0F1F, RGBDeviceType.LedController, "Addressable RGB Controller", LedMappings.ChromaLink, RazerEndpointType.ChromaLink },
        { 0x0F2C, RGBDeviceType.LedController, "Chroma Wireless ARGB Controller", LedMappings.ChromaLink, RazerEndpointType.ChromaLink },
    };

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RazerDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public RazerDeviceProvider()
    {
        lock (_lock)
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(RazerDeviceProvider)}");
            _instance = this;
        }
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void InitializeSDK()
    {
        TryUnInit();

        _RazerSDK.Reload();

        RazerError error;
        if (((error = _RazerSDK.Init()) != RazerError.Success) && Enum.IsDefined(typeof(RazerError), error)) //HACK DarthAffe 08.02.2018: The x86-SDK seems to have a problem here ...
            ThrowRazerError(error, true);
    }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> GetLoadedDevices(RGBDeviceType loadFilter)
    {
        DeviceDefinitions.LoadFilter = loadFilter;

        List<IRGBDevice> devices = base.GetLoadedDevices(loadFilter).ToList();

        if (LoadEmulatorDevices != RazerEndpointType.None)
        {
            if (loadFilter.HasFlag(RGBDeviceType.Keyboard) && (LoadEmulatorDevices.HasFlag(RazerEndpointType.Keyboard) || LoadEmulatorDevices.HasFlag(RazerEndpointType.LaptopKeyboard)) && devices.All(d => d is not RazerKeyboardRGBDevice))
                devices.Add(new RazerKeyboardRGBDevice(new RazerKeyboardRGBDeviceInfo("Emulator Keyboard", RazerEndpointType.Keyboard), GetUpdateTrigger(), LedMappings.Keyboard));

            if (loadFilter.HasFlag(RGBDeviceType.Mouse) && LoadEmulatorDevices.HasFlag(RazerEndpointType.Mouse) && devices.All(d => d is not RazerMouseRGBDevice))
                devices.Add(new RazerMouseRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Mouse, RazerEndpointType.Mouse, "Emulator Mouse"), GetUpdateTrigger(), LedMappings.Mouse));

            if (loadFilter.HasFlag(RGBDeviceType.Headset) && LoadEmulatorDevices.HasFlag(RazerEndpointType.Headset) && devices.All(d => d is not RazerHeadsetRGBDevice))
                devices.Add(new RazerHeadsetRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Headset, RazerEndpointType.Headset, "Emulator Headset"), GetUpdateTrigger()));

            if (loadFilter.HasFlag(RGBDeviceType.Mousepad) && LoadEmulatorDevices.HasFlag(RazerEndpointType.Mousepad) && devices.All(d => d is not RazerMousepadRGBDevice))
                devices.Add(new RazerMousepadRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Mousepad, RazerEndpointType.Mousepad, "Emulator Mousepad"), GetUpdateTrigger()));

            if (loadFilter.HasFlag(RGBDeviceType.Keypad) && LoadEmulatorDevices.HasFlag(RazerEndpointType.Keypad) && devices.All(d => d is not RazerMousepadRGBDevice))
                devices.Add(new RazerKeypadRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Keypad, RazerEndpointType.Keypad, "Emulator Keypad"), GetUpdateTrigger()));

            if (loadFilter.HasFlag(RGBDeviceType.Unknown) && LoadEmulatorDevices.HasFlag(RazerEndpointType.ChromaLink) && devices.All(d => d is not RazerChromaLinkRGBDevice))
                devices.Add(new RazerChromaLinkRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Unknown, RazerEndpointType.ChromaLink, "Emulator Chroma Link"), GetUpdateTrigger()));
        }

        return devices;
    }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        // Only take the first device of each endpoint type, the Razer SDK doesn't allow separate control over multiple devices using the same endpoint
        foreach ((HIDDeviceDefinition<int, RazerEndpointType> definition, _) in DeviceDefinitions.GetConnectedDevices(x => x.CustomData == RazerEndpointType.LaptopKeyboard ? RazerEndpointType.Keyboard : x.CustomData))
        {
            yield return definition.CustomData switch
            {
                RazerEndpointType.Keyboard => new RazerKeyboardRGBDevice(new RazerKeyboardRGBDeviceInfo(definition.Name, definition.CustomData), GetUpdateTrigger(), definition.LedMapping),
                RazerEndpointType.LaptopKeyboard => new RazerKeyboardRGBDevice(new RazerKeyboardRGBDeviceInfo(definition.Name, definition.CustomData), GetUpdateTrigger(), definition.LedMapping),
                RazerEndpointType.Mouse => new RazerMouseRGBDevice(new RazerRGBDeviceInfo(definition.DeviceType, definition.CustomData, definition.Name), GetUpdateTrigger(), definition.LedMapping),
                RazerEndpointType.Headset => new RazerHeadsetRGBDevice(new RazerRGBDeviceInfo(definition.DeviceType, definition.CustomData, definition.Name), GetUpdateTrigger()),
                RazerEndpointType.Mousepad => new RazerMousepadRGBDevice(new RazerRGBDeviceInfo(definition.DeviceType, definition.CustomData, definition.Name), GetUpdateTrigger()),
                RazerEndpointType.Keypad => new RazerKeypadRGBDevice(new RazerRGBDeviceInfo(definition.DeviceType, definition.CustomData, definition.Name), GetUpdateTrigger()),
                RazerEndpointType.ChromaLink => new RazerChromaLinkRGBDevice(new RazerRGBDeviceInfo(definition.DeviceType, definition.CustomData, definition.Name), GetUpdateTrigger()),
                _ => throw new RGBDeviceException($"Razer SDK does not support endpoint '{definition.CustomData}'")
            };
        }
    }

    private void ThrowRazerError(RazerError errorCode, bool isCritical) => Throw(new RazerException(errorCode), isCritical);

    private void TryUnInit()
    {
        try { _RazerSDK.UnInit(); }
        catch { /* We tried our best */ }
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        lock (_lock)
        {
            base.Dispose(disposing);

            TryUnInit();

            // DarthAffe 03.03.2020: Fails with an access-violation - verify if an unload is already triggered by uninit
            //try { _RazerSDK.UnloadRazerSDK(); }
            //catch { /* at least we tried */ }

            _instance = null;
        }
    }

    #endregion
}
