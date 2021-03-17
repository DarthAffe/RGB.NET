using System.Collections.Generic;
using System.Linq;
using HidSharp;
using RGB.NET.Core;
using DeviceDataList =
    System.Collections.Generic.List<(string model, RGB.NET.Core.RGBDeviceType deviceType, RGB.NET.Devices.Razer.RazerEndpointType
        razerDeviceType, int id)>;

namespace RGB.NET.Devices.Razer.HID
{
    internal static class DeviceChecker
    {
        #region Constants

        private const int VENDOR_ID = 0x1532;

        private static readonly DeviceDataList DEVICES
            = new()
              {
                  // Keyboards
                  ("BlackWidow Ultimate 2012", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x010D),
                  ("BlackWidow Classic (Alternate)", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x010E),
                  ("Anansi", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x010F),
                  ("BlackWidow Ultimate 2013", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x011A),
                  ("BlackWidow Stealth", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x011B),
                  ("DeathStalker Expert", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0202),
                  ("BlackWidow Chroma", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0203),
                  ("DeathStalker Chroma", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0204),
                  ("Blade Stealth", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0205),
                  ("BlackWidow Tournament Edition Chroma", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0209),
                  ("Blade QHD", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x020F),
                  ("Blade Pro (Late 2016)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0210),
                  ("BlackWidow Chroma (Overwatch)", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0211),
                  ("BlackWidow Ultimate 2016", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0214),
                  ("BlackWidow X Chroma", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0216),
                  ("BlackWidow X Ultimate", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0217),
                  ("BlackWidow X Tournament Edition Chroma", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x021A),
                  ("Ornata Chroma", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x021E),
                  ("Ornata", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x021F),
                  ("Blade Stealth (Late 2016)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0220),
                  ("BlackWidow Chroma V2", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0221),
                  ("Blade (Late 2016)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0224),
                  ("Blade Pro (2017)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0225),
                  ("Huntsman Elite", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0226),
                  ("Huntsman", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0227),
                  ("BlackWidow Elite", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0228),
                  ("Cynosa Chroma", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x022A),
                  ("Blade Stealth (Mid 2017)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x022D),
                  ("Blade Pro FullHD (2017)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x022F),
                  ("Blade Stealth (Late 2017)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0232),
                  ("Blade 15 (2018)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0233),
                  ("Blade Pro 17 (2019)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0234),
                  ("BlackWidow Lite (2018)", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0235),
                  ("BlackWidow Essential", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0237),
                  ("Blade Stealth (2019)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0239),
                  ("Blade 15 (2019) Advanced", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x023A),
                  ("Blade 15 (2018) Base Model", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x023B),
                  ("Cynosa Lite", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x023F),
                  ("Blade 15 (2018) Mercury", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0240),
                  ("BlackWidow (2019)", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0241),
                  ("Huntsman Tournament Edition", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x0243),
                  ("Blade 15 (Mid 2019) Mercury", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0245),
                  ("Blade 15 (Mid 2019) Base", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0246),
                  ("Blade Stealth (Late 2019)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x024A),
                  ("Blade Pro (Late 2019)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x024C),
                  ("Blade 15 Studio Edition (2019)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x024D),
                  ("Blade Stealth (Early 2020)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0252),
                  ("Blade 15 Advanced (2020)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0253),
                  ("Blade 15 (Early 2020) Base", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0255),
                  ("Blade Stealth (Late 2020)", RGBDeviceType.Keyboard, RazerEndpointType.LaptopKeyboard, 0x0259),
                  ("BlackWidow V3 Pro", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x25A), // The keyboard, only present when connected with cable
                  ("BlackWidow V3 Pro", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x25C), // The dongle, may not be present when connected with cable
                  ("Ornata Chroma V2", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x025D),
                  ("Cynosa V2", RGBDeviceType.Keyboard, RazerEndpointType.Keyboard, 0x025E),

                  // Mice
                  ("Orochi 2011", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0013),
                  ("DeathAdder 3.5G", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0016),
                  ("Abyssus 1800", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0020),
                  ("Mamba 2012 (Wired)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0024),
                  ("Mamba 2012 (Wireless)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0025),
                  ("Naga 2012", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x002E),
                  ("Imperator 2012", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x002F),
                  ("Ouroboros 2012", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0032),
                  ("Taipan", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0034),
                  ("Naga Hex (Red)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0036),
                  ("DeathAdder 2013", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0037),
                  ("DeathAdder 1800", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0038),
                  ("Orochi 2013", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0039),
                  ("Naga 2014", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0040),
                  ("Naga Hex", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0041),
                  ("Abyssus 2014", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0042),
                  ("DeathAdder Chroma", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0043),
                  ("Mamba (Wired)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0044),
                  ("Mamba (Wireless)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0045),
                  ("Mamba Tournament Edition", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0046),
                  ("Orochi (Wired)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0048),
                  ("Diamondback Chroma", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x004C),
                  ("DeathAdder 2000", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x004F),
                  ("Naga Hex V2", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0050),
                  ("Naga Chroma", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0053),
                  ("DeathAdder 3500", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0054),
                  ("Lancehead (Wired)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0059),
                  ("Lancehead (Wireless)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x005A),
                  ("Abyssus V2", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x005B),
                  ("DeathAdder Elite", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x005C),
                  ("Abyssus 2000", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x005E),
                  ("Lancehead Tournament Edition", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0060),
                  ("Atheris (Receiver)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0062),
                  ("Basilisk", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0064),
                  ("Naga Trinity", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0067),
                  ("Abyssus Elite (D.Va Edition)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x006A),
                  ("Abyssus Essential", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x006B),
                  ("Mamba Elite (Wired)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x006C),
                  ("DeathAdder Essential", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x006E),
                  ("Lancehead Wireless (Receiver)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x006F),
                  ("Lancehead Wireless (Wired)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0070),
                  ("DeathAdder Essential (White Edition)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0071),
                  ("Mamba Wireless (Receiver)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0072),
                  ("Mamba Wireless (Wired)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0073),
                  ("Viper", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0078),
                  ("Viper Ultimate (Wired)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x007A),
                  ("Viper Ultimate (Wireless)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x007B),
                  ("DeathAdder V2 Pro (Wired)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x007C),
                  ("DeathAdder V2 Pro (Wireless)", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x007D),
                  ("Basilisk X HyperSpeed", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0083),
                  ("Basilisk Ultimate", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0088),
                  ("DeathAdder V2", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x0084),
                  ("Viper Mini", RGBDeviceType.Mouse, RazerEndpointType.Mouse, 0x008A),

                  // Mousepads
                  ("Firefly Hyperflux", RGBDeviceType.Mousepad, RazerEndpointType.Mousepad, 0x0068),
                  ("Firefly", RGBDeviceType.Mousepad, RazerEndpointType.Mousepad, 0x0C00),
                  ("Goliathus", RGBDeviceType.Mousepad, RazerEndpointType.ChromaLink, 0x0C01),
                  ("Goliathus Extended", RGBDeviceType.Mousepad, RazerEndpointType.ChromaLink, 0x0C02),
                  ("Firefly v2", RGBDeviceType.Mousepad, RazerEndpointType.Mousepad, 0x0C04),

                  // Headsets
                  ("Kraken 7.1", RGBDeviceType.Headset, RazerEndpointType.Headset, 0x0501),
                  ("Kraken 7.1 Chroma", RGBDeviceType.Headset, RazerEndpointType.Headset, 0x0504),
                  ("Kraken 7.1", RGBDeviceType.Headset, RazerEndpointType.Headset, 0x0506),
                  ("Kraken 7.1 V2", RGBDeviceType.Headset, RazerEndpointType.Headset, 0x0510),
                  ("Kraken Ultimate", RGBDeviceType.Headset, RazerEndpointType.Headset, 0x0527),
                  ("Kraken Kitty Edition", RGBDeviceType.Headset, RazerEndpointType.Headset, 0x0F19),

                  // Keypads
                  ("Nostromo", RGBDeviceType.Keypad, RazerEndpointType.Keypad, 0x0111),
                  ("Orbweaver", RGBDeviceType.Keypad, RazerEndpointType.Keypad, 0x0113),
                  ("Tartarus", RGBDeviceType.Keypad, RazerEndpointType.Keypad, 0x0201),
                  ("Orbweaver Chroma", RGBDeviceType.Keypad, RazerEndpointType.Keypad, 0x0207),
                  ("Tartarus Chroma", RGBDeviceType.Keypad, RazerEndpointType.Keypad, 0x0208),
                  ("Tartarus V2", RGBDeviceType.Keypad, RazerEndpointType.Keypad, 0x022B),
                  ("Tartarus Pro", RGBDeviceType.Keypad, RazerEndpointType.Keypad, 0x0244),

                  // Misc - guessing these are through ChromaLink
                  ("Core", RGBDeviceType.GraphicsCard, RazerEndpointType.ChromaLink, 0x0215),
                  ("Base Station Chroma", RGBDeviceType.HeadsetStand, RazerEndpointType.ChromaLink, 0x0F08),
                  ("Nommo Chroma", RGBDeviceType.Speaker, RazerEndpointType.ChromaLink, 0x0517),
                  ("Nommo Pro", RGBDeviceType.Speaker, RazerEndpointType.ChromaLink, 0x0518),
                  ("Chroma Mug Holder", RGBDeviceType.Unknown, RazerEndpointType.ChromaLink, 0x0F07),
                  ("Chroma Hardware Development Kit (HDK)", RGBDeviceType.Unknown, RazerEndpointType.ChromaLink, 0x0F09),
                  ("Mouse Bungee V3 Chroma", RGBDeviceType.Unknown, RazerEndpointType.ChromaLink, 0x0F1D),
                  ("Base Station V2 Chroma", RGBDeviceType.Unknown, RazerEndpointType.ChromaLink, 0x0F20)
              };

        #endregion

        #region Properties & Fields

        public static DeviceDataList ConnectedDevices { get; } = new();

        #endregion

        #region Methods

        internal static void LoadDeviceList(RGBDeviceType loadFilter)
        {
            ConnectedDevices.Clear();

            HashSet<int> ids = new(DeviceList.Local.GetHidDevices(VENDOR_ID).Select(x => x.ProductID).Distinct());
            DeviceDataList connectedDevices = DEVICES.Where(d => ids.Contains(d.id) && loadFilter.HasFlag(d.deviceType)).ToList();

            ConnectedDevices.AddRange(connectedDevices);
        }

        #endregion
    }
}
