using System.Collections.Generic;
using System.Linq;
using HidSharp;
using RGB.NET.Core;
using DeviceDataList = System.Collections.Generic.List<(string model, RGB.NET.Core.RGBDeviceType deviceType, int id, RGB.NET.Devices.SteelSeries.SteelSeriesDeviceType steelSeriesDeviceType, string imageLayout, string layoutPath, System.Collections.Generic.Dictionary<RGB.NET.Core.LedId, RGB.NET.Devices.SteelSeries.SteelSeriesLedId> ledMapping)>;
using LedMapping = System.Collections.Generic.Dictionary<RGB.NET.Core.LedId, RGB.NET.Devices.SteelSeries.SteelSeriesLedId>;

namespace RGB.NET.Devices.SteelSeries.HID
{
    internal static class DeviceChecker
    {
        #region Constants

        private static readonly LedMapping KEYBOARD_MAPPING_UK = new LedMapping
        {
            { LedId.Logo, SteelSeriesLedId.Logo },
            { LedId.Keyboard_Escape, SteelSeriesLedId.Escape },
            { LedId.Keyboard_F1, SteelSeriesLedId.F1 },
            { LedId.Keyboard_F2, SteelSeriesLedId.F2 },
            { LedId.Keyboard_F3, SteelSeriesLedId.F3 },
            { LedId.Keyboard_F4, SteelSeriesLedId.F4 },
            { LedId.Keyboard_F5, SteelSeriesLedId.F5 },
            { LedId.Keyboard_F6, SteelSeriesLedId.F6 },
            { LedId.Keyboard_F7, SteelSeriesLedId.F7 },
            { LedId.Keyboard_F8, SteelSeriesLedId.F8 },
            { LedId.Keyboard_F9, SteelSeriesLedId.F9 },
            { LedId.Keyboard_F10, SteelSeriesLedId.F10 },
            { LedId.Keyboard_F11, SteelSeriesLedId.F11 },
            { LedId.Keyboard_GraveAccentAndTilde, SteelSeriesLedId.Backqoute },
            { LedId.Keyboard_1, SteelSeriesLedId.Keyboard1 },
            { LedId.Keyboard_2, SteelSeriesLedId.Keyboard2 },
            { LedId.Keyboard_3, SteelSeriesLedId.Keyboard3 },
            { LedId.Keyboard_4, SteelSeriesLedId.Keyboard4 },
            { LedId.Keyboard_5, SteelSeriesLedId.Keyboard5 },
            { LedId.Keyboard_6, SteelSeriesLedId.Keyboard6 },
            { LedId.Keyboard_7, SteelSeriesLedId.Keyboard7 },
            { LedId.Keyboard_8, SteelSeriesLedId.Keyboard8 },
            { LedId.Keyboard_9, SteelSeriesLedId.Keyboard9 },
            { LedId.Keyboard_0, SteelSeriesLedId.Keyboard0 },
            { LedId.Keyboard_MinusAndUnderscore, SteelSeriesLedId.Dash },
            { LedId.Keyboard_Tab, SteelSeriesLedId.Tab },
            { LedId.Keyboard_Q, SteelSeriesLedId.Q },
            { LedId.Keyboard_W, SteelSeriesLedId.W },
            { LedId.Keyboard_E, SteelSeriesLedId.E },
            { LedId.Keyboard_R, SteelSeriesLedId.R },
            { LedId.Keyboard_T, SteelSeriesLedId.T },
            { LedId.Keyboard_Y, SteelSeriesLedId.Y },
            { LedId.Keyboard_U, SteelSeriesLedId.U },
            { LedId.Keyboard_I, SteelSeriesLedId.I },
            { LedId.Keyboard_O, SteelSeriesLedId.O },
            { LedId.Keyboard_P, SteelSeriesLedId.P },
            { LedId.Keyboard_BracketLeft, SteelSeriesLedId.LBracket },
            { LedId.Keyboard_CapsLock, SteelSeriesLedId.Caps },
            { LedId.Keyboard_A, SteelSeriesLedId.A },
            { LedId.Keyboard_S, SteelSeriesLedId.S },
            { LedId.Keyboard_D, SteelSeriesLedId.D },
            { LedId.Keyboard_F, SteelSeriesLedId.F },
            { LedId.Keyboard_G, SteelSeriesLedId.G },
            { LedId.Keyboard_H, SteelSeriesLedId.H },
            { LedId.Keyboard_J, SteelSeriesLedId.J },
            { LedId.Keyboard_K, SteelSeriesLedId.K },
            { LedId.Keyboard_L, SteelSeriesLedId.L },
            { LedId.Keyboard_SemicolonAndColon, SteelSeriesLedId.Semicolon },
            { LedId.Keyboard_ApostropheAndDoubleQuote, SteelSeriesLedId.Quote },
            { LedId.Keyboard_LeftShift, SteelSeriesLedId.LShift },
            { LedId.Keyboard_NonUsTilde, SteelSeriesLedId.Pound },
            { LedId.Keyboard_Z, SteelSeriesLedId.Z },
            { LedId.Keyboard_X, SteelSeriesLedId.X },
            { LedId.Keyboard_C, SteelSeriesLedId.C },
            { LedId.Keyboard_V, SteelSeriesLedId.V },
            { LedId.Keyboard_B, SteelSeriesLedId.B },
            { LedId.Keyboard_N, SteelSeriesLedId.N },
            { LedId.Keyboard_M, SteelSeriesLedId.M },
            { LedId.Keyboard_CommaAndLessThan, SteelSeriesLedId.Comma },
            { LedId.Keyboard_PeriodAndBiggerThan, SteelSeriesLedId.Period },
            { LedId.Keyboard_SlashAndQuestionMark, SteelSeriesLedId.Slash },
            { LedId.Keyboard_LeftCtrl, SteelSeriesLedId.LCtrl },
            { LedId.Keyboard_LeftGui, SteelSeriesLedId.LWin },
            { LedId.Keyboard_LeftAlt, SteelSeriesLedId.LAlt },
            { LedId.Keyboard_Space, SteelSeriesLedId.Spacebar },
            { LedId.Keyboard_RightAlt, SteelSeriesLedId.RAlt },
            { LedId.Keyboard_RightGui, SteelSeriesLedId.RWin },
            { LedId.Keyboard_Application, SteelSeriesLedId.SSKey },
            { LedId.Keyboard_F12, SteelSeriesLedId.F12 },
            { LedId.Keyboard_PrintScreen, SteelSeriesLedId.PrintScreen },
            { LedId.Keyboard_ScrollLock, SteelSeriesLedId.ScrollLock },
            { LedId.Keyboard_PauseBreak, SteelSeriesLedId.Pause },
            { LedId.Keyboard_Insert, SteelSeriesLedId.Insert },
            { LedId.Keyboard_Home, SteelSeriesLedId.Home },
            { LedId.Keyboard_PageUp, SteelSeriesLedId.PageUp },
            { LedId.Keyboard_BracketRight, SteelSeriesLedId.RBracket },
            { LedId.Keyboard_Backslash, SteelSeriesLedId.Backslash },
            { LedId.Keyboard_Enter, SteelSeriesLedId.Return },
            { LedId.Keyboard_EqualsAndPlus, SteelSeriesLedId.Equal },
            { LedId.Keyboard_Backspace, SteelSeriesLedId.Backspace },
            { LedId.Keyboard_Delete, SteelSeriesLedId.Delete },
            { LedId.Keyboard_End, SteelSeriesLedId.End },
            { LedId.Keyboard_PageDown, SteelSeriesLedId.PageDown },
            { LedId.Keyboard_RightShift, SteelSeriesLedId.RShift },
            { LedId.Keyboard_RightCtrl, SteelSeriesLedId.RCtrl },
            { LedId.Keyboard_ArrowUp, SteelSeriesLedId.UpArrow },
            { LedId.Keyboard_ArrowLeft, SteelSeriesLedId.LeftArrow },
            { LedId.Keyboard_ArrowDown, SteelSeriesLedId.DownArrow },
            { LedId.Keyboard_ArrowRight, SteelSeriesLedId.RightArrow },
            { LedId.Keyboard_NumLock, SteelSeriesLedId.KeypadNumLock },
            { LedId.Keyboard_NumSlash, SteelSeriesLedId.KeypadDivide },
            { LedId.Keyboard_NumAsterisk, SteelSeriesLedId.KeypadTimes },
            { LedId.Keyboard_NumMinus, SteelSeriesLedId.KeypadMinus },
            { LedId.Keyboard_NumPlus, SteelSeriesLedId.KeypadPlus },
            { LedId.Keyboard_NumEnter, SteelSeriesLedId.KeypadEnter },
            { LedId.Keyboard_Num7, SteelSeriesLedId.Keypad7 },
            { LedId.Keyboard_Num8, SteelSeriesLedId.Keypad8 },
            { LedId.Keyboard_Num9, SteelSeriesLedId.Keypad9 },
            { LedId.Keyboard_Num4, SteelSeriesLedId.Keypad4 },
            { LedId.Keyboard_Num5, SteelSeriesLedId.Keypad5 },
            { LedId.Keyboard_Num6, SteelSeriesLedId.Keypad6 },
            { LedId.Keyboard_Num1, SteelSeriesLedId.Keypad1 },
            { LedId.Keyboard_Num2, SteelSeriesLedId.Keypad2 },
            { LedId.Keyboard_Num3, SteelSeriesLedId.Keypad3 },
            { LedId.Keyboard_Num0, SteelSeriesLedId.Keypad0 },
            { LedId.Keyboard_NumPeriodAndDelete, SteelSeriesLedId.KeypadPeriod }
        };

        private static readonly LedMapping KEYBOARD_TKL_MAPPING_UK = new LedMapping
        {
            { LedId.Logo, SteelSeriesLedId.Logo },
            { LedId.Keyboard_Escape, SteelSeriesLedId.Escape },
            { LedId.Keyboard_F1, SteelSeriesLedId.F1 },
            { LedId.Keyboard_F2, SteelSeriesLedId.F2 },
            { LedId.Keyboard_F3, SteelSeriesLedId.F3 },
            { LedId.Keyboard_F4, SteelSeriesLedId.F4 },
            { LedId.Keyboard_F5, SteelSeriesLedId.F5 },
            { LedId.Keyboard_F6, SteelSeriesLedId.F6 },
            { LedId.Keyboard_F7, SteelSeriesLedId.F7 },
            { LedId.Keyboard_F8, SteelSeriesLedId.F8 },
            { LedId.Keyboard_F9, SteelSeriesLedId.F9 },
            { LedId.Keyboard_F10, SteelSeriesLedId.F10 },
            { LedId.Keyboard_F11, SteelSeriesLedId.F11 },
            { LedId.Keyboard_GraveAccentAndTilde, SteelSeriesLedId.Backqoute },
            { LedId.Keyboard_1, SteelSeriesLedId.Keyboard1 },
            { LedId.Keyboard_2, SteelSeriesLedId.Keyboard2 },
            { LedId.Keyboard_3, SteelSeriesLedId.Keyboard3 },
            { LedId.Keyboard_4, SteelSeriesLedId.Keyboard4 },
            { LedId.Keyboard_5, SteelSeriesLedId.Keyboard5 },
            { LedId.Keyboard_6, SteelSeriesLedId.Keyboard6 },
            { LedId.Keyboard_7, SteelSeriesLedId.Keyboard7 },
            { LedId.Keyboard_8, SteelSeriesLedId.Keyboard8 },
            { LedId.Keyboard_9, SteelSeriesLedId.Keyboard9 },
            { LedId.Keyboard_0, SteelSeriesLedId.Keyboard0 },
            { LedId.Keyboard_MinusAndUnderscore, SteelSeriesLedId.Dash },
            { LedId.Keyboard_Tab, SteelSeriesLedId.Tab },
            { LedId.Keyboard_Q, SteelSeriesLedId.Q },
            { LedId.Keyboard_W, SteelSeriesLedId.W },
            { LedId.Keyboard_E, SteelSeriesLedId.E },
            { LedId.Keyboard_R, SteelSeriesLedId.R },
            { LedId.Keyboard_T, SteelSeriesLedId.T },
            { LedId.Keyboard_Y, SteelSeriesLedId.Y },
            { LedId.Keyboard_U, SteelSeriesLedId.U },
            { LedId.Keyboard_I, SteelSeriesLedId.I },
            { LedId.Keyboard_O, SteelSeriesLedId.O },
            { LedId.Keyboard_P, SteelSeriesLedId.P },
            { LedId.Keyboard_BracketLeft, SteelSeriesLedId.LBracket },
            { LedId.Keyboard_CapsLock, SteelSeriesLedId.Caps },
            { LedId.Keyboard_A, SteelSeriesLedId.A },
            { LedId.Keyboard_S, SteelSeriesLedId.S },
            { LedId.Keyboard_D, SteelSeriesLedId.D },
            { LedId.Keyboard_F, SteelSeriesLedId.F },
            { LedId.Keyboard_G, SteelSeriesLedId.G },
            { LedId.Keyboard_H, SteelSeriesLedId.H },
            { LedId.Keyboard_J, SteelSeriesLedId.J },
            { LedId.Keyboard_K, SteelSeriesLedId.K },
            { LedId.Keyboard_L, SteelSeriesLedId.L },
            { LedId.Keyboard_SemicolonAndColon, SteelSeriesLedId.Semicolon },
            { LedId.Keyboard_ApostropheAndDoubleQuote, SteelSeriesLedId.Quote },
            { LedId.Keyboard_LeftShift, SteelSeriesLedId.LShift },
            { LedId.Keyboard_NonUsTilde, SteelSeriesLedId.Pound },
            { LedId.Keyboard_Z, SteelSeriesLedId.Z },
            { LedId.Keyboard_X, SteelSeriesLedId.X },
            { LedId.Keyboard_C, SteelSeriesLedId.C },
            { LedId.Keyboard_V, SteelSeriesLedId.V },
            { LedId.Keyboard_B, SteelSeriesLedId.B },
            { LedId.Keyboard_N, SteelSeriesLedId.N },
            { LedId.Keyboard_M, SteelSeriesLedId.M },
            { LedId.Keyboard_CommaAndLessThan, SteelSeriesLedId.Comma },
            { LedId.Keyboard_PeriodAndBiggerThan, SteelSeriesLedId.Period },
            { LedId.Keyboard_SlashAndQuestionMark, SteelSeriesLedId.Slash },
            { LedId.Keyboard_LeftCtrl, SteelSeriesLedId.LCtrl },
            { LedId.Keyboard_LeftGui, SteelSeriesLedId.LWin },
            { LedId.Keyboard_LeftAlt, SteelSeriesLedId.LAlt },
            { LedId.Keyboard_Space, SteelSeriesLedId.Spacebar },
            { LedId.Keyboard_RightAlt, SteelSeriesLedId.RAlt },
            { LedId.Keyboard_RightGui, SteelSeriesLedId.RWin },
            { LedId.Keyboard_Application, SteelSeriesLedId.SSKey },
            { LedId.Keyboard_F12, SteelSeriesLedId.F12 },
            { LedId.Keyboard_PrintScreen, SteelSeriesLedId.PrintScreen },
            { LedId.Keyboard_ScrollLock, SteelSeriesLedId.ScrollLock },
            { LedId.Keyboard_PauseBreak, SteelSeriesLedId.Pause },
            { LedId.Keyboard_Insert, SteelSeriesLedId.Insert },
            { LedId.Keyboard_Home, SteelSeriesLedId.Home },
            { LedId.Keyboard_PageUp, SteelSeriesLedId.PageUp },
            { LedId.Keyboard_BracketRight, SteelSeriesLedId.RBracket },
            { LedId.Keyboard_Backslash, SteelSeriesLedId.Backslash },
            { LedId.Keyboard_Enter, SteelSeriesLedId.Return },
            { LedId.Keyboard_EqualsAndPlus, SteelSeriesLedId.Equal },
            { LedId.Keyboard_Backspace, SteelSeriesLedId.Backspace },
            { LedId.Keyboard_Delete, SteelSeriesLedId.Delete },
            { LedId.Keyboard_End, SteelSeriesLedId.End },
            { LedId.Keyboard_PageDown, SteelSeriesLedId.PageDown },
            { LedId.Keyboard_RightShift, SteelSeriesLedId.RShift },
            { LedId.Keyboard_RightCtrl, SteelSeriesLedId.RCtrl },
            { LedId.Keyboard_ArrowUp, SteelSeriesLedId.UpArrow },
            { LedId.Keyboard_ArrowLeft, SteelSeriesLedId.LeftArrow },
            { LedId.Keyboard_ArrowDown, SteelSeriesLedId.DownArrow },
            { LedId.Keyboard_ArrowRight, SteelSeriesLedId.RightArrow }
        };

        private static readonly LedMapping MOUSE_TWO_ZONE = new LedMapping
                                                               {
                                                                   {LedId.Mouse1, SteelSeriesLedId.ZoneOne},
                                                                   {LedId.Mouse2, SteelSeriesLedId.ZoneTwo}
                                                               };

        private static readonly LedMapping MOUSE_THREE_ZONE = new LedMapping
                                                               {
            {LedId.Mouse1, SteelSeriesLedId.ZoneOne},
            {LedId.Mouse2, SteelSeriesLedId.ZoneTwo},
            {LedId.Mouse3, SteelSeriesLedId.ZoneThree}
                                                               };

        private static readonly LedMapping MOUSE_EIGHT_ZONE = new LedMapping
                                                              {
                                                                  { LedId.Mouse1, SteelSeriesLedId.ZoneOne},
                                                                  { LedId.Mouse2, SteelSeriesLedId.ZoneTwo},
                                                                  { LedId.Mouse3, SteelSeriesLedId.ZoneThree},
                                                                  { LedId.Mouse4, SteelSeriesLedId.ZoneFour},
                                                                  { LedId.Mouse5, SteelSeriesLedId.ZoneFive},
                                                                  { LedId.Mouse6, SteelSeriesLedId.ZoneSix},
                                                                  { LedId.Mouse7, SteelSeriesLedId.ZoneSeven},
                                                                  { LedId.Mouse8, SteelSeriesLedId.ZoneEight}
                                                               };

        private static readonly LedMapping HEADSET_TWO_ZONE = new LedMapping
        {
            {LedId.Headset1, SteelSeriesLedId.ZoneOne},
            {LedId.Headset2, SteelSeriesLedId.ZoneTwo}
        };
        
        private const int VENDOR_ID = 0x1038;

        //TODO DarthAffe 16.02.2019: Add devices
        private static readonly DeviceDataList DEVICES = new DeviceDataList
        {
            ("Rival 600", RGBDeviceType.Mouse, 0x1724, SteelSeriesDeviceType.EightZone, "default", @"Mice\Rival600", MOUSE_EIGHT_ZONE),
            ("Rival 500", RGBDeviceType.Mouse, 0x170E, SteelSeriesDeviceType.TwoZone, "default", @"Mice\Rival500", MOUSE_TWO_ZONE),
            ("Rival 310", RGBDeviceType.Mouse, 0x1720, SteelSeriesDeviceType.TwoZone, "default", @"Mice\Rival310", MOUSE_TWO_ZONE),
            ("Rival 3 (Old Firmware)", RGBDeviceType.Mouse, 0x1824, SteelSeriesDeviceType.ThreeZone, "default", @"Mice\Rival3", MOUSE_THREE_ZONE),
            ("Rival 3", RGBDeviceType.Mouse, 0x184C, SteelSeriesDeviceType.ThreeZone, "default", @"Mice\Rival3", MOUSE_THREE_ZONE),

            ("Apex 5", RGBDeviceType.Keyboard, 0x161C, SteelSeriesDeviceType.PerKey, "UK", @"Keyboards\5\UK", KEYBOARD_MAPPING_UK),
            ("Apex 7", RGBDeviceType.Keyboard, 0x1612, SteelSeriesDeviceType.PerKey, "UK", @"Keyboards\7\UK", KEYBOARD_MAPPING_UK),
            ("Apex 7 TKL", RGBDeviceType.Keyboard, 0x1618, SteelSeriesDeviceType.PerKey, "UK", @"Keyboards\7TKL\UK", KEYBOARD_TKL_MAPPING_UK),
            ("Apex M750", RGBDeviceType.Keyboard, 0x0616, SteelSeriesDeviceType.PerKey, "UK", @"Keyboards\M750\UK", KEYBOARD_MAPPING_UK),

            ("Arctis 5", RGBDeviceType.Headset, 0x12AA, SteelSeriesDeviceType.TwoZone, "default", @"Headsets\Artis5", HEADSET_TWO_ZONE),
            ("Arctis 5 Game", RGBDeviceType.Headset, 0x1250, SteelSeriesDeviceType.TwoZone, "default", @"Headsets\Artis5", HEADSET_TWO_ZONE),
            ("Arctis 5 Game - Dota 2 edition", RGBDeviceType.Headset, 0x1251, SteelSeriesDeviceType.TwoZone, "default", @"Headsets\Artis5", HEADSET_TWO_ZONE),
            ("Arctis 5 Pro Game", RGBDeviceType.Headset, 0x1252, SteelSeriesDeviceType.TwoZone, "default", @"Headsets\Artis5", HEADSET_TWO_ZONE),
            ("Arctis 5 Game - PUBG edition", RGBDeviceType.Headset, 0x12A8, SteelSeriesDeviceType.TwoZone, "default", @"Headsets\Artis5", HEADSET_TWO_ZONE),
        };

        #endregion

        #region Properties & Fields

        public static DeviceDataList ConnectedDevices { get; } = new DeviceDataList();

        #endregion

        #region Methods

        internal static void LoadDeviceList(RGBDeviceType loadFilter)
        {
            ConnectedDevices.Clear();

            HashSet<int> ids = new HashSet<int>(DeviceList.Local.GetHidDevices(VENDOR_ID).Select(x => x.ProductID).Distinct());
            DeviceDataList connectedDevices = DEVICES.Where(d => ids.Contains(d.id) && loadFilter.HasFlag(d.deviceType)).ToList();

            List<SteelSeriesDeviceType> connectedDeviceTypes = connectedDevices.Select(d => d.steelSeriesDeviceType).ToList();
            foreach (SteelSeriesDeviceType deviceType in connectedDeviceTypes)
                ConnectedDevices.Add(connectedDevices.Where(d => d.steelSeriesDeviceType == deviceType).OrderByDescending(d => d.ledMapping.Count).First());
        }

        #endregion
    }
}
