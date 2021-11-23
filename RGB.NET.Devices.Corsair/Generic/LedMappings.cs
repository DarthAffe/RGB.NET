using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair;

/// <summary>
/// Contains mappings for <see cref="LedId"/> to <see cref="CorsairLedId"/>.
/// </summary>
public static class LedMappings
{
    static LedMappings()
    {
        for (int i = 0; i <= (CorsairLedId.GPU50 - CorsairLedId.GPU1); i++)
            GraphicsCard.Add(LedId.GraphicsCard1 + i, CorsairLedId.GPU1 + i);

        for (int i = 0; i <= (CorsairLedId.HeadsetStandZone9 - CorsairLedId.HeadsetStandZone1); i++)
            HeadsetStand.Add(LedId.HeadsetStand1 + i, CorsairLedId.HeadsetStandZone1 + i);

        for (int i = 0; i <= (CorsairLedId.Mainboard100 - CorsairLedId.Mainboard1); i++)
            Mainboard.Add(LedId.Mainboard1 + i, CorsairLedId.Mainboard1 + i);

        for (int i = 0; i <= (CorsairLedId.DRAM12 - CorsairLedId.DRAM1); i++)
            Memory.Add(LedId.DRAM1 + i, CorsairLedId.DRAM1 + i);

        for (int i = 0; i <= (CorsairLedId.Zone15 - CorsairLedId.Zone1); i++)
            Mousepad.Add(LedId.Mousepad1 + i, CorsairLedId.Zone1 + i);

        for (int i = 0; i <= (CorsairLedId.OemLed100 - CorsairLedId.OemLed1); i++)
            Keyboard.Add(LedId.Custom1 + i, CorsairLedId.OemLed1 + i);

        for (int i = 0; i <= (CorsairLedId.OemLed250 - CorsairLedId.OemLed101); i++)
            Keyboard.Add(LedId.Custom101 + i, CorsairLedId.OemLed101 + i);
    }

    /// <summary>
    /// Gets the mapping for graphics cards.
    /// </summary>
    public static LedMapping<CorsairLedId> GraphicsCard { get; } = new();

    /// <summary>
    /// Gets the mapping for headsets.
    /// </summary>
    public static LedMapping<CorsairLedId> HeadsetStand { get; } = new();

    /// <summary>
    /// Gets the mapping for mainboards.
    /// </summary>
    public static LedMapping<CorsairLedId> Mainboard { get; } = new();

    /// <summary>
    /// Gets the mapping for memory.
    /// </summary>
    public static LedMapping<CorsairLedId> Memory { get; } = new();

    /// <summary>
    /// Gets the mapping for mousepads.
    /// </summary>
    public static LedMapping<CorsairLedId> Mousepad { get; } = new();

    /// <summary>
    /// Gets the mapping for headsets.
    /// </summary>
    public static LedMapping<CorsairLedId> Headset { get; } = new()
    {
        { LedId.Headset1, CorsairLedId.LeftLogo },
        { LedId.Headset2, CorsairLedId.RightLogo },
    };

    /// <summary>
    /// Gets the mapping for mice.
    /// </summary>
    public static LedMapping<CorsairLedId> Mouse { get; } = new()
    {
        { LedId.Mouse1, CorsairLedId.B1 },
        { LedId.Mouse2, CorsairLedId.B2 },
        { LedId.Mouse3, CorsairLedId.B3 },
        { LedId.Mouse4, CorsairLedId.B4 },
        { LedId.Mouse5, CorsairLedId.B5 },
        { LedId.Mouse6, CorsairLedId.B6 },
        { LedId.Mouse7, CorsairLedId.B7 },
        { LedId.Mouse8, CorsairLedId.B8 },
        { LedId.Mouse9, CorsairLedId.B9 },
        { LedId.Mouse10, CorsairLedId.B10 },
        { LedId.Mouse11, CorsairLedId.B11 },
        { LedId.Mouse12, CorsairLedId.B12 },
        { LedId.Mouse13, CorsairLedId.B13 },
        { LedId.Mouse14, CorsairLedId.B14 },
        { LedId.Mouse15, CorsairLedId.B15 },
        { LedId.Mouse16, CorsairLedId.B16 },
        { LedId.Mouse17, CorsairLedId.B17 },
        { LedId.Mouse18, CorsairLedId.B18 },
        { LedId.Mouse19, CorsairLedId.B19 },
        { LedId.Mouse20, CorsairLedId.B20 },
    };

    /// <summary>
    /// Gets the mapping for keyboards.
    /// </summary>
    public static LedMapping<CorsairLedId> Keyboard { get; } = new()
    {
        { LedId.Invalid, CorsairLedId.Invalid },
        { LedId.Logo, CorsairLedId.Logo },
        { LedId.Keyboard_Escape, CorsairLedId.Escape },
        { LedId.Keyboard_F1, CorsairLedId.F1 },
        { LedId.Keyboard_F2, CorsairLedId.F2 },
        { LedId.Keyboard_F3, CorsairLedId.F3 },
        { LedId.Keyboard_F4, CorsairLedId.F4 },
        { LedId.Keyboard_F5, CorsairLedId.F5 },
        { LedId.Keyboard_F6, CorsairLedId.F6 },
        { LedId.Keyboard_F7, CorsairLedId.F7 },
        { LedId.Keyboard_F8, CorsairLedId.F8 },
        { LedId.Keyboard_F9, CorsairLedId.F9 },
        { LedId.Keyboard_F10, CorsairLedId.F10 },
        { LedId.Keyboard_F11, CorsairLedId.F11 },
        { LedId.Keyboard_GraveAccentAndTilde, CorsairLedId.GraveAccentAndTilde },
        { LedId.Keyboard_1, CorsairLedId.D1 },
        { LedId.Keyboard_2, CorsairLedId.D2 },
        { LedId.Keyboard_3, CorsairLedId.D3 },
        { LedId.Keyboard_4, CorsairLedId.D4 },
        { LedId.Keyboard_5, CorsairLedId.D5 },
        { LedId.Keyboard_6, CorsairLedId.D6 },
        { LedId.Keyboard_7, CorsairLedId.D7 },
        { LedId.Keyboard_8, CorsairLedId.D8 },
        { LedId.Keyboard_9, CorsairLedId.D9 },
        { LedId.Keyboard_0, CorsairLedId.D0 },
        { LedId.Keyboard_MinusAndUnderscore, CorsairLedId.MinusAndUnderscore },
        { LedId.Keyboard_Tab, CorsairLedId.Tab },
        { LedId.Keyboard_Q, CorsairLedId.Q },
        { LedId.Keyboard_W, CorsairLedId.W },
        { LedId.Keyboard_E, CorsairLedId.E },
        { LedId.Keyboard_R, CorsairLedId.R },
        { LedId.Keyboard_T, CorsairLedId.T },
        { LedId.Keyboard_Y, CorsairLedId.Y },
        { LedId.Keyboard_U, CorsairLedId.U },
        { LedId.Keyboard_I, CorsairLedId.I },
        { LedId.Keyboard_O, CorsairLedId.O },
        { LedId.Keyboard_P, CorsairLedId.P },
        { LedId.Keyboard_BracketLeft, CorsairLedId.BracketLeft },
        { LedId.Keyboard_CapsLock, CorsairLedId.CapsLock },
        { LedId.Keyboard_A, CorsairLedId.A },
        { LedId.Keyboard_S, CorsairLedId.S },
        { LedId.Keyboard_D, CorsairLedId.D },
        { LedId.Keyboard_F, CorsairLedId.F },
        { LedId.Keyboard_G, CorsairLedId.G },
        { LedId.Keyboard_H, CorsairLedId.H },
        { LedId.Keyboard_J, CorsairLedId.J },
        { LedId.Keyboard_K, CorsairLedId.K },
        { LedId.Keyboard_L, CorsairLedId.L },
        { LedId.Keyboard_SemicolonAndColon, CorsairLedId.SemicolonAndColon },
        { LedId.Keyboard_ApostropheAndDoubleQuote, CorsairLedId.ApostropheAndDoubleQuote },
        { LedId.Keyboard_LeftShift, CorsairLedId.LeftShift },
        { LedId.Keyboard_NonUsBackslash, CorsairLedId.NonUsBackslash },
        { LedId.Keyboard_Z, CorsairLedId.Z },
        { LedId.Keyboard_X, CorsairLedId.X },
        { LedId.Keyboard_C, CorsairLedId.C },
        { LedId.Keyboard_V, CorsairLedId.V },
        { LedId.Keyboard_B, CorsairLedId.B },
        { LedId.Keyboard_N, CorsairLedId.N },
        { LedId.Keyboard_M, CorsairLedId.M },
        { LedId.Keyboard_CommaAndLessThan, CorsairLedId.CommaAndLessThan },
        { LedId.Keyboard_PeriodAndBiggerThan, CorsairLedId.PeriodAndBiggerThan },
        { LedId.Keyboard_SlashAndQuestionMark, CorsairLedId.SlashAndQuestionMark },
        { LedId.Keyboard_LeftCtrl, CorsairLedId.LeftCtrl },
        { LedId.Keyboard_LeftGui, CorsairLedId.LeftGui },
        { LedId.Keyboard_LeftAlt, CorsairLedId.LeftAlt },
        { LedId.Keyboard_Lang2, CorsairLedId.Lang2 },
        { LedId.Keyboard_Space, CorsairLedId.Space },
        { LedId.Keyboard_Lang1, CorsairLedId.Lang1 },
        { LedId.Keyboard_International2, CorsairLedId.International2 },
        { LedId.Keyboard_RightAlt, CorsairLedId.RightAlt },
        { LedId.Keyboard_RightGui, CorsairLedId.RightGui },
        { LedId.Keyboard_Application, CorsairLedId.Application },
        { LedId.Keyboard_Brightness, CorsairLedId.Brightness },
        { LedId.Keyboard_F12, CorsairLedId.F12 },
        { LedId.Keyboard_PrintScreen, CorsairLedId.PrintScreen },
        { LedId.Keyboard_ScrollLock, CorsairLedId.ScrollLock },
        { LedId.Keyboard_PauseBreak, CorsairLedId.PauseBreak },
        { LedId.Keyboard_Insert, CorsairLedId.Insert },
        { LedId.Keyboard_Home, CorsairLedId.Home },
        { LedId.Keyboard_PageUp, CorsairLedId.PageUp },
        { LedId.Keyboard_BracketRight, CorsairLedId.BracketRight },
        { LedId.Keyboard_Backslash, CorsairLedId.Backslash },
        { LedId.Keyboard_NonUsTilde, CorsairLedId.NonUsTilde },
        { LedId.Keyboard_Enter, CorsairLedId.Enter },
        { LedId.Keyboard_International1, CorsairLedId.International1 },
        { LedId.Keyboard_EqualsAndPlus, CorsairLedId.EqualsAndPlus },
        { LedId.Keyboard_International3, CorsairLedId.International3 },
        { LedId.Keyboard_Backspace, CorsairLedId.Backspace },
        { LedId.Keyboard_Delete, CorsairLedId.Delete },
        { LedId.Keyboard_End, CorsairLedId.End },
        { LedId.Keyboard_PageDown, CorsairLedId.PageDown },
        { LedId.Keyboard_RightShift, CorsairLedId.RightShift },
        { LedId.Keyboard_RightCtrl, CorsairLedId.RightCtrl },
        { LedId.Keyboard_ArrowUp, CorsairLedId.UpArrow },
        { LedId.Keyboard_ArrowLeft, CorsairLedId.LeftArrow },
        { LedId.Keyboard_ArrowDown, CorsairLedId.DownArrow },
        { LedId.Keyboard_ArrowRight, CorsairLedId.RightArrow },
        { LedId.Keyboard_WinLock, CorsairLedId.WinLock },
        { LedId.Keyboard_MediaMute, CorsairLedId.Mute },
        { LedId.Keyboard_MediaStop, CorsairLedId.Stop },
        { LedId.Keyboard_MediaPreviousTrack, CorsairLedId.ScanPreviousTrack },
        { LedId.Keyboard_MediaPlay, CorsairLedId.PlayPause },
        { LedId.Keyboard_MediaNextTrack, CorsairLedId.ScanNextTrack },
        { LedId.Keyboard_NumLock, CorsairLedId.NumLock },
        { LedId.Keyboard_NumSlash, CorsairLedId.KeypadSlash },
        { LedId.Keyboard_NumAsterisk, CorsairLedId.KeypadAsterisk },
        { LedId.Keyboard_NumMinus, CorsairLedId.KeypadMinus },
        { LedId.Keyboard_NumPlus, CorsairLedId.KeypadPlus },
        { LedId.Keyboard_NumEnter, CorsairLedId.KeypadEnter },
        { LedId.Keyboard_Num7, CorsairLedId.Keypad7 },
        { LedId.Keyboard_Num8, CorsairLedId.Keypad8 },
        { LedId.Keyboard_Num9, CorsairLedId.Keypad9 },
        { LedId.Keyboard_NumComma, CorsairLedId.KeypadComma },
        { LedId.Keyboard_Num4, CorsairLedId.Keypad4 },
        { LedId.Keyboard_Num5, CorsairLedId.Keypad5 },
        { LedId.Keyboard_Num6, CorsairLedId.Keypad6 },
        { LedId.Keyboard_Num1, CorsairLedId.Keypad1 },
        { LedId.Keyboard_Num2, CorsairLedId.Keypad2 },
        { LedId.Keyboard_Num3, CorsairLedId.Keypad3 },
        { LedId.Keyboard_Num0, CorsairLedId.Keypad0 },
        { LedId.Keyboard_NumPeriodAndDelete, CorsairLedId.KeypadPeriodAndDelete },
        { LedId.Keyboard_Programmable1, CorsairLedId.G1 },
        { LedId.Keyboard_Programmable2, CorsairLedId.G2 },
        { LedId.Keyboard_Programmable3, CorsairLedId.G3 },
        { LedId.Keyboard_Programmable4, CorsairLedId.G4 },
        { LedId.Keyboard_Programmable5, CorsairLedId.G5 },
        { LedId.Keyboard_Programmable6, CorsairLedId.G6 },
        { LedId.Keyboard_Programmable7, CorsairLedId.G7 },
        { LedId.Keyboard_Programmable8, CorsairLedId.G8 },
        { LedId.Keyboard_Programmable9, CorsairLedId.G9 },
        { LedId.Keyboard_Programmable10, CorsairLedId.G10 },
        { LedId.Keyboard_MediaVolumeUp, CorsairLedId.VolumeUp },
        { LedId.Keyboard_MediaVolumeDown, CorsairLedId.VolumeDown },
        { LedId.Keyboard_MacroRecording, CorsairLedId.MR },
        { LedId.Keyboard_Macro1, CorsairLedId.M1 },
        { LedId.Keyboard_Macro2, CorsairLedId.M2 },
        { LedId.Keyboard_Macro3, CorsairLedId.M3 },
        { LedId.Keyboard_Programmable11, CorsairLedId.G11 },
        { LedId.Keyboard_Programmable12, CorsairLedId.G12 },
        { LedId.Keyboard_Programmable13, CorsairLedId.G13 },
        { LedId.Keyboard_Programmable14, CorsairLedId.G14 },
        { LedId.Keyboard_Programmable15, CorsairLedId.G15 },
        { LedId.Keyboard_Programmable16, CorsairLedId.G16 },
        { LedId.Keyboard_Programmable17, CorsairLedId.G17 },
        { LedId.Keyboard_Programmable18, CorsairLedId.G18 },
        { LedId.Keyboard_International5, CorsairLedId.International5 },
        { LedId.Keyboard_International4, CorsairLedId.International4 },
        { LedId.Keyboard_Profile, CorsairLedId.Profile },
        { LedId.Keyboard_LedProgramming, CorsairLedId.LedProgramming },
        { LedId.Keyboard_Function, CorsairLedId.Fn },

        { LedId.LedStripe1, CorsairLedId.Lightbar1 },
        { LedId.LedStripe2, CorsairLedId.Lightbar2 },
        { LedId.LedStripe3, CorsairLedId.Lightbar3 },
        { LedId.LedStripe4, CorsairLedId.Lightbar4 },
        { LedId.LedStripe5, CorsairLedId.Lightbar5 },
        { LedId.LedStripe6, CorsairLedId.Lightbar6 },
        { LedId.LedStripe7, CorsairLedId.Lightbar7 },
        { LedId.LedStripe8, CorsairLedId.Lightbar8 },
        { LedId.LedStripe9, CorsairLedId.Lightbar9 },
        { LedId.LedStripe10, CorsairLedId.Lightbar10 },
        { LedId.LedStripe11, CorsairLedId.Lightbar11 },
        { LedId.LedStripe12, CorsairLedId.Lightbar12 },
        { LedId.LedStripe13, CorsairLedId.Lightbar13 },
        { LedId.LedStripe14, CorsairLedId.Lightbar14 },
        { LedId.LedStripe15, CorsairLedId.Lightbar15 },
        { LedId.LedStripe16, CorsairLedId.Lightbar16 },
        { LedId.LedStripe17, CorsairLedId.Lightbar17 },
        { LedId.LedStripe18, CorsairLedId.Lightbar18 },
        { LedId.LedStripe19, CorsairLedId.Lightbar19 },
        { LedId.LedStripe20, CorsairLedId.Lightbar20 },
        { LedId.LedStripe21, CorsairLedId.Lightbar21 },
        { LedId.LedStripe22, CorsairLedId.Lightbar22 },
        { LedId.LedStripe23, CorsairLedId.Lightbar23 },
        { LedId.LedStripe24, CorsairLedId.Lightbar24 },
        { LedId.LedStripe25, CorsairLedId.Lightbar25 },
        { LedId.LedStripe26, CorsairLedId.Lightbar26 },
        { LedId.LedStripe27, CorsairLedId.Lightbar27 },
        { LedId.LedStripe28, CorsairLedId.Lightbar28 },
        { LedId.LedStripe29, CorsairLedId.Lightbar29 },
        { LedId.LedStripe30, CorsairLedId.Lightbar30 },
        { LedId.LedStripe31, CorsairLedId.Lightbar31 },
        { LedId.LedStripe32, CorsairLedId.Lightbar32 },
        { LedId.LedStripe33, CorsairLedId.Lightbar33 },
        { LedId.LedStripe34, CorsairLedId.Lightbar34 },
        { LedId.LedStripe35, CorsairLedId.Lightbar35 },
        { LedId.LedStripe36, CorsairLedId.Lightbar36 },
        { LedId.LedStripe37, CorsairLedId.Lightbar37 },
        { LedId.LedStripe38, CorsairLedId.Lightbar38 },
        { LedId.LedStripe39, CorsairLedId.Lightbar39 },
        { LedId.LedStripe40, CorsairLedId.Lightbar40 },
        { LedId.LedStripe41, CorsairLedId.Lightbar41 },
        { LedId.LedStripe42, CorsairLedId.Lightbar42 },
        { LedId.LedStripe43, CorsairLedId.Lightbar43 },
        { LedId.LedStripe44, CorsairLedId.Lightbar44 },
        { LedId.LedStripe45, CorsairLedId.Lightbar45 },
        { LedId.LedStripe46, CorsairLedId.Lightbar46 },
        { LedId.LedStripe47, CorsairLedId.Lightbar47 },
        { LedId.LedStripe48, CorsairLedId.Lightbar48 },
        { LedId.LedStripe49, CorsairLedId.Lightbar49 },
        { LedId.LedStripe50, CorsairLedId.Lightbar50 },
    };
}