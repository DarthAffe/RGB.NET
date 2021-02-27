using System.Collections.Generic;
using System.Linq;
using HidSharp;
using RGB.NET.Core;
using DeviceDataList = System.Collections.Generic.List<(string model, RGB.NET.Core.RGBDeviceType deviceType, int id)>;

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
                  ("BlackWidow Ultimate 2012", RGBDeviceType.Keyboard, 0x010D),
                  ("BlackWidow Classic (Alternate)", RGBDeviceType.Keyboard, 0x010E),
                  ("Anansi", RGBDeviceType.Keyboard, 0x010F),
                  ("BlackWidow Ultimate 2013", RGBDeviceType.Keyboard, 0x011A),
                  ("BlackWidow Stealth", RGBDeviceType.Keyboard, 0x011B),
                  ("DeathStalker Expert", RGBDeviceType.Keyboard, 0x0202),
                  ("BlackWidow Chroma", RGBDeviceType.Keyboard, 0x0203),
                  ("DeathStalker Chroma", RGBDeviceType.Keyboard, 0x0204),
                  ("Blade Stealth", RGBDeviceType.Keyboard, 0x0205),
                  ("BlackWidow Tournament Edition Chroma", RGBDeviceType.Keyboard, 0x0209),
                  ("Blade QHD", RGBDeviceType.Keyboard, 0x020F),
                  ("Blade Pro (Late 2016)", RGBDeviceType.Keyboard, 0x0210),
                  ("BlackWidow Chroma (Overwatch)", RGBDeviceType.Keyboard, 0x0211),
                  ("BlackWidow Ultimate 2016", RGBDeviceType.Keyboard, 0x0214),
                  ("BlackWidow X Chroma", RGBDeviceType.Keyboard, 0x0216),
                  ("BlackWidow X Ultimate", RGBDeviceType.Keyboard, 0x0217),
                  ("BlackWidow X Tournament Edition Chroma", RGBDeviceType.Keyboard, 0x021A),
                  ("Ornata Chroma", RGBDeviceType.Keyboard, 0x021E),
                  ("Ornata", RGBDeviceType.Keyboard, 0x021F),
                  ("Blade Stealth (Late 2016)", RGBDeviceType.Keyboard, 0x0220),
                  ("BlackWidow Chroma V2", RGBDeviceType.Keyboard, 0x0221),
                  ("Blade (Late 2016)", RGBDeviceType.Keyboard, 0x0224),
                  ("Blade Pro (2017)", RGBDeviceType.Keyboard, 0x0225),
                  ("Huntsman Elite", RGBDeviceType.Keyboard, 0x0226),
                  ("Huntsman", RGBDeviceType.Keyboard, 0x0227),
                  ("BlackWidow Elite", RGBDeviceType.Keyboard, 0x0228),
                  ("Cynosa Chroma", RGBDeviceType.Keyboard, 0x022A),
                  ("Blade Stealth (Mid 2017)", RGBDeviceType.Keyboard, 0x022D),
                  ("Blade Pro FullHD (2017)", RGBDeviceType.Keyboard, 0x022F),
                  ("Blade Stealth (Late 2017)", RGBDeviceType.Keyboard, 0x0232),
                  ("Blade 15 (2018)", RGBDeviceType.Keyboard, 0x0233),
                  ("Blade Pro 17 (2019)", RGBDeviceType.Keyboard, 0x0234),
                  ("BlackWidow Lite (2018)", RGBDeviceType.Keyboard, 0x0235),
                  ("BlackWidow Essential", RGBDeviceType.Keyboard, 0x0237),
                  ("Blade Stealth (2019)", RGBDeviceType.Keyboard, 0x0239),
                  ("Blade 15 (2019) Advanced", RGBDeviceType.Keyboard, 0x023A),
                  ("Blade 15 (2018) Base Model", RGBDeviceType.Keyboard, 0x023B),
                  ("Cynosa Lite", RGBDeviceType.Keyboard, 0x023F),
                  ("Blade 15 (2018) Mercury", RGBDeviceType.Keyboard, 0x0240),
                  ("BlackWidow (2019)", RGBDeviceType.Keyboard, 0x0241),
                  ("Huntsman Tournament Edition", RGBDeviceType.Keyboard, 0x0243),
                  ("Blade 15 (Mid 2019) Mercury", RGBDeviceType.Keyboard, 0x0245),
                  ("Blade 15 (Mid 2019) Base", RGBDeviceType.Keyboard, 0x0246),
                  ("Blade Stealth (Late 2019)", RGBDeviceType.Keyboard, 0x024A),
                  ("Blade Pro (Late 2019)", RGBDeviceType.Keyboard, 0x024C),
                  ("Blade 15 Studio Edition (2019)", RGBDeviceType.Keyboard, 0x024D),
                  ("Blade Stealth (Early 2020)", RGBDeviceType.Keyboard, 0x0252),
                  ("Blade 15 Advanced (2020)", RGBDeviceType.Keyboard, 0x0253),
                  ("Blade 15 (Early 2020) Base", RGBDeviceType.Keyboard, 0x0255),
                  ("Blade Stealth (Late 2020)", RGBDeviceType.Keyboard, 0x0259),
                  ("Ornata Chroma V2", RGBDeviceType.Keyboard, 0x025D),
                  ("Cynosa V2", RGBDeviceType.Keyboard, 0x025E),

                  // Mice
                  ("Orochi 2011", RGBDeviceType.Mouse, 0x0013),
                  ("DeathAdder 3.5G", RGBDeviceType.Mouse, 0x0016),
                  ("Abyssus 1800", RGBDeviceType.Mouse, 0x0020),
                  ("Mamba 2012 (Wired)", RGBDeviceType.Mouse, 0x0024),
                  ("Mamba 2012 (Wireless)", RGBDeviceType.Mouse, 0x0025),
                  ("Naga 2012", RGBDeviceType.Mouse, 0x002E),
                  ("Imperator 2012", RGBDeviceType.Mouse, 0x002F),
                  ("Ouroboros 2012", RGBDeviceType.Mouse, 0x0032),
                  ("Taipan", RGBDeviceType.Mouse, 0x0034),
                  ("Naga Hex (Red)", RGBDeviceType.Mouse, 0x0036),
                  ("DeathAdder 2013", RGBDeviceType.Mouse, 0x0037),
                  ("DeathAdder 1800", RGBDeviceType.Mouse, 0x0038),
                  ("Orochi 2013", RGBDeviceType.Mouse, 0x0039),
                  ("Naga 2014", RGBDeviceType.Mouse, 0x0040),
                  ("Naga Hex", RGBDeviceType.Mouse, 0x0041),
                  ("Abyssus 2014", RGBDeviceType.Mouse, 0x0042),
                  ("DeathAdder Chroma", RGBDeviceType.Mouse, 0x0043),
                  ("Mamba (Wired)", RGBDeviceType.Mouse, 0x0044),
                  ("Mamba (Wireless)", RGBDeviceType.Mouse, 0x0045),
                  ("Mamba Tournament Edition", RGBDeviceType.Mouse, 0x0046),
                  ("Orochi (Wired)", RGBDeviceType.Mouse, 0x0048),
                  ("Diamondback Chroma", RGBDeviceType.Mouse, 0x004C),
                  ("DeathAdder 2000", RGBDeviceType.Mouse, 0x004F),
                  ("Naga Hex V2", RGBDeviceType.Mouse, 0x0050),
                  ("Naga Chroma", RGBDeviceType.Mouse, 0x0053),
                  ("DeathAdder 3500", RGBDeviceType.Mouse, 0x0054),
                  ("Lancehead (Wired)", RGBDeviceType.Mouse, 0x0059),
                  ("Lancehead (Wireless)", RGBDeviceType.Mouse, 0x005A),
                  ("Abyssus V2", RGBDeviceType.Mouse, 0x005B),
                  ("DeathAdder Elite", RGBDeviceType.Mouse, 0x005C),
                  ("Abyssus 2000", RGBDeviceType.Mouse, 0x005E),
                  ("Lancehead Tournament Edition", RGBDeviceType.Mouse, 0x0060),
                  ("Atheris (Receiver)", RGBDeviceType.Mouse, 0x0062),
                  ("Basilisk", RGBDeviceType.Mouse, 0x0064),
                  ("Naga Trinity", RGBDeviceType.Mouse, 0x0067),
                  ("Abyssus Elite (D.Va Edition)", RGBDeviceType.Mouse, 0x006A),
                  ("Abyssus Essential", RGBDeviceType.Mouse, 0x006B),
                  ("Mamba Elite (Wired)", RGBDeviceType.Mouse, 0x006C),
                  ("DeathAdder Essential", RGBDeviceType.Mouse, 0x006E),
                  ("Lancehead Wireless (Receiver)", RGBDeviceType.Mouse, 0x006F),
                  ("Lancehead Wireless (Wired)", RGBDeviceType.Mouse, 0x0070),
                  ("DeathAdder Essential (White Edition)", RGBDeviceType.Mouse, 0x0071),
                  ("Mamba Wireless (Receiver)", RGBDeviceType.Mouse, 0x0072),
                  ("Mamba Wireless (Wired)", RGBDeviceType.Mouse, 0x0073),
                  ("Viper", RGBDeviceType.Mouse, 0x0078),
                  ("Viper Ultimate (Wired)", RGBDeviceType.Mouse, 0x007A),
                  ("Viper Ultimate (Wireless)", RGBDeviceType.Mouse, 0x007B),
                  ("DeathAdder V2 Pro (Wired)", RGBDeviceType.Mouse, 0x007C),
                  ("DeathAdder V2 Pro (Wireless)", RGBDeviceType.Mouse, 0x007D),
                  ("Basilisk X HyperSpeed", RGBDeviceType.Mouse, 0x0083),
                  ("Basilisk Ultimate", RGBDeviceType.Mouse, 0x0088),
                  ("DeathAdder V2", RGBDeviceType.Mouse, 0x0084),
                  ("Viper Mini", RGBDeviceType.Mouse, 0x008A),

                  // Mousepads
                  ("Firefly Hyperflux", RGBDeviceType.Mousepad, 0x0068),
                  ("Firefly", RGBDeviceType.Mousepad, 0x0C00),
                  ("Goliathus", RGBDeviceType.Mousepad, 0x0C01),
                  ("Goliathus Extended", RGBDeviceType.Mousepad, 0x0C02),
                  ("Firefly v2", RGBDeviceType.Mousepad, 0x0C04),

                  // Headsets
                  ("Kraken 7.1", RGBDeviceType.Headset, 0x0501),
                  ("Kraken 7.1 Chroma", RGBDeviceType.Headset, 0x0504),
                  ("Kraken 7.1", RGBDeviceType.Headset, 0x0506),
                  ("Kraken 7.1 V2", RGBDeviceType.Headset, 0x0510),
                  ("Kraken Ultimate", RGBDeviceType.Headset, 0x0527),
                  ("Kraken Kitty Edition", RGBDeviceType.Headset, 0x0F19),

                  // Keypads
                  ("Nostromo", RGBDeviceType.Keypad, 0x0111),
                  ("Orbweaver", RGBDeviceType.Keypad, 0x0113),
                  ("Tartarus", RGBDeviceType.Keypad, 0x0201),
                  ("Orbweaver Chroma", RGBDeviceType.Keypad, 0x0207),
                  ("Tartarus Chroma", RGBDeviceType.Keypad, 0x0208),
                  ("Tartarus V2", RGBDeviceType.Keypad, 0x022B),
                  ("Tartarus Pro", RGBDeviceType.Keypad, 0x0244),

                  // Misc - unsupported through the effects API
                  // ("Core", RGBDeviceType.GraphicsCard, 0x0215),
                  // ("Base Station Chroma", RGBDeviceType.HeadsetStand, 0x0F08),
                  // ("Nommo Chroma", RGBDeviceType.Unknown, 0x0517),
                  // ("Nommo Pro", RGBDeviceType.Unknown, 0x0518),
                  // ("Chroma Mug Holder", RGBDeviceType.Unknown, 0x0F07),
                  // ("Chroma Hardware Development Kit (HDK)", RGBDeviceType.Unknown, 0x0F09),
                  // ("Mouse Bungee V3 Chroma", RGBDeviceType.Unknown, 0x0F1D),
                  // ("Base Station V2 Chroma", RGBDeviceType.Unknown, 0x0F20)
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
