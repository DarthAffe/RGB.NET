using RGB.NET.Core;

namespace RGB.NET.Devices.SteelSeries;

/// <summary>
/// Contains mappings for <see cref="LedId"/> to <see cref="SteelSeriesLedId"/>.
/// </summary>
public static class LedMappings
{
    /// <summary>
    /// Gets the uk-mapping for keyboards.
    /// </summary>
    public static LedMapping<SteelSeriesLedId> KeyboardMappingUk { get; } = new()
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

    /// <summary>
    /// Gets the uk-tkl-mapping for keyboards.
    /// </summary>
    public static LedMapping<SteelSeriesLedId> KeyboardTklMappingUk { get; } = new()
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

    /// <summary>
    /// Gets the mapping for one-zone mice.
    /// </summary>
    public static LedMapping<SteelSeriesLedId> MouseOneZone { get; } = new()
    {
        { LedId.Mouse1, SteelSeriesLedId.ZoneOne }
    };

    /// <summary>
    /// Gets the mapping for two-zone mice.
    /// </summary>
    public static LedMapping<SteelSeriesLedId> MouseTwoZone { get; } = new()
    {
        { LedId.Mouse1, SteelSeriesLedId.ZoneOne },
        { LedId.Mouse2, SteelSeriesLedId.ZoneTwo }
    };

    /// <summary>
    /// Gets the mapping for three-zone mice.
    /// </summary>
    public static LedMapping<SteelSeriesLedId> MouseThreeZone { get; } = new()
    {
        { LedId.Mouse1, SteelSeriesLedId.ZoneOne },
        { LedId.Mouse2, SteelSeriesLedId.ZoneTwo },
        { LedId.Mouse3, SteelSeriesLedId.ZoneThree }
    };

    /// <summary>
    /// Gets the mapping for eight-zone mice.
    /// </summary>
    public static LedMapping<SteelSeriesLedId> MouseEightZone { get; } = new()
    {
        { LedId.Mouse1, SteelSeriesLedId.ZoneOne },
        { LedId.Mouse2, SteelSeriesLedId.ZoneTwo },
        { LedId.Mouse3, SteelSeriesLedId.ZoneThree },
        { LedId.Mouse4, SteelSeriesLedId.ZoneFour },
        { LedId.Mouse5, SteelSeriesLedId.ZoneFive },
        { LedId.Mouse6, SteelSeriesLedId.ZoneSix },
        { LedId.Mouse7, SteelSeriesLedId.ZoneSeven },
        { LedId.Mouse8, SteelSeriesLedId.ZoneEight }
    };

    /// <summary>
    /// Gets the mapping for two-zone headsets.
    /// </summary>
    public static LedMapping<SteelSeriesLedId> HeadsetTwoZone { get; } = new()
    {
        { LedId.Headset1, SteelSeriesLedId.ZoneOne },
        { LedId.Headset2, SteelSeriesLedId.ZoneTwo }
    };

    /// <summary>
    /// Gets the mapping for twelve-zone mousepads
    /// </summary>
    public static LedMapping<SteelSeriesLedId> MousepadTwelveZone { get; } = new()
    {
        { LedId.Mousepad1, SteelSeriesLedId.ZoneOne },
        { LedId.Mousepad2, SteelSeriesLedId.ZoneTwo },
        { LedId.Mousepad3, SteelSeriesLedId.ZoneThree },
        { LedId.Mousepad4, SteelSeriesLedId.ZoneFour },
        { LedId.Mousepad5, SteelSeriesLedId.ZoneFive },
        { LedId.Mousepad6, SteelSeriesLedId.ZoneSix },
        { LedId.Mousepad7, SteelSeriesLedId.ZoneSeven },
        { LedId.Mousepad8, SteelSeriesLedId.ZoneEight },
        { LedId.Mousepad9, SteelSeriesLedId.ZoneNine },
        { LedId.Mousepad10, SteelSeriesLedId.ZoneTen },
        { LedId.Mousepad11, SteelSeriesLedId.ZoneEleven },
        { LedId.Mousepad12, SteelSeriesLedId.ZoneTwelve },
    };

    /// <summary>
    /// Gets the mapping for two-zone mousepads
    /// </summary>
    public static LedMapping<SteelSeriesLedId> MousepadTwoZone { get; } = new()
    {
        { LedId.Mousepad1, SteelSeriesLedId.ZoneOne },
        { LedId.Mousepad2, SteelSeriesLedId.ZoneTwo }
    };

    /// <summary>
    /// Gets the mapping for 103-zone led strip devices (monitor).
    /// </summary>
    public static LedMapping<SteelSeriesLedId> MonitorOnehundredandthreeZone { get; } = new()
    {
        { LedId.LedStripe1, SteelSeriesLedId.ZoneOne },
        { LedId.LedStripe2, SteelSeriesLedId.ZoneTwo },
        { LedId.LedStripe3, SteelSeriesLedId.ZoneThree },
        { LedId.LedStripe4, SteelSeriesLedId.ZoneFour },
        { LedId.LedStripe5, SteelSeriesLedId.ZoneFive },
        { LedId.LedStripe6, SteelSeriesLedId.ZoneSix },
        { LedId.LedStripe7, SteelSeriesLedId.ZoneSeven },
        { LedId.LedStripe8, SteelSeriesLedId.ZoneEight },
        { LedId.LedStripe9, SteelSeriesLedId.ZoneNine },
        { LedId.LedStripe10, SteelSeriesLedId.ZoneTen },
        { LedId.LedStripe11, SteelSeriesLedId.ZoneEleven },
        { LedId.LedStripe12, SteelSeriesLedId.ZoneTwelve },
        { LedId.LedStripe13, SteelSeriesLedId.ZoneThirteen },
        { LedId.LedStripe14, SteelSeriesLedId.ZoneFourteen },
        { LedId.LedStripe15, SteelSeriesLedId.ZoneFifteen },
        { LedId.LedStripe16, SteelSeriesLedId.ZoneSixteen },
        { LedId.LedStripe17, SteelSeriesLedId.ZoneSeventeen },
        { LedId.LedStripe18, SteelSeriesLedId.ZoneEighteen },
        { LedId.LedStripe19, SteelSeriesLedId.ZoneNineteen },
        { LedId.LedStripe20, SteelSeriesLedId.ZoneTwenty },
        { LedId.LedStripe21, SteelSeriesLedId.ZoneTwentyOne },
        { LedId.LedStripe22, SteelSeriesLedId.ZoneTwentyTwo },
        { LedId.LedStripe23, SteelSeriesLedId.ZoneTwentyThree },
        { LedId.LedStripe24, SteelSeriesLedId.ZoneTwentyFour },
        { LedId.LedStripe25, SteelSeriesLedId.ZoneTwentyFive },
        { LedId.LedStripe26, SteelSeriesLedId.ZoneTwentySix },
        { LedId.LedStripe27, SteelSeriesLedId.ZoneTwentySeven },
        { LedId.LedStripe28, SteelSeriesLedId.ZoneTwentyEight },
        { LedId.LedStripe29, SteelSeriesLedId.ZoneTwentyNine },
        { LedId.LedStripe30, SteelSeriesLedId.ZoneThirty },
        { LedId.LedStripe31, SteelSeriesLedId.ZoneThirtyOne },
        { LedId.LedStripe32, SteelSeriesLedId.ZoneThirtyTwo },
        { LedId.LedStripe33, SteelSeriesLedId.ZoneThirtyThree },
        { LedId.LedStripe34, SteelSeriesLedId.ZoneThirtyFour },
        { LedId.LedStripe35, SteelSeriesLedId.ZoneThirtyFive },
        { LedId.LedStripe36, SteelSeriesLedId.ZoneThirtySix },
        { LedId.LedStripe37, SteelSeriesLedId.ZoneThirtySeven },
        { LedId.LedStripe38, SteelSeriesLedId.ZoneThirtyEight },
        { LedId.LedStripe39, SteelSeriesLedId.ZoneThirtyNine },
        { LedId.LedStripe40, SteelSeriesLedId.ZoneForty },
        { LedId.LedStripe41, SteelSeriesLedId.ZoneFortyOne },
        { LedId.LedStripe42, SteelSeriesLedId.ZoneFortyTwo },
        { LedId.LedStripe43, SteelSeriesLedId.ZoneFortyThree },
        { LedId.LedStripe44, SteelSeriesLedId.ZoneFortyFour },
        { LedId.LedStripe45, SteelSeriesLedId.ZoneFortyFive },
        { LedId.LedStripe46, SteelSeriesLedId.ZoneFortySix },
        { LedId.LedStripe47, SteelSeriesLedId.ZoneFortySeven },
        { LedId.LedStripe48, SteelSeriesLedId.ZoneFortyEight },
        { LedId.LedStripe49, SteelSeriesLedId.ZoneFortyNine },
        { LedId.LedStripe50, SteelSeriesLedId.ZoneFifty },
        { LedId.LedStripe51, SteelSeriesLedId.ZoneFiftyOne },
        { LedId.LedStripe52, SteelSeriesLedId.ZoneFiftyTwo },
        { LedId.LedStripe53, SteelSeriesLedId.ZoneFiftyThree },
        { LedId.LedStripe54, SteelSeriesLedId.ZoneFiftyFour },
        { LedId.LedStripe55, SteelSeriesLedId.ZoneFiftyFive },
        { LedId.LedStripe56, SteelSeriesLedId.ZoneFiftySix },
        { LedId.LedStripe57, SteelSeriesLedId.ZoneFiftySeven },
        { LedId.LedStripe58, SteelSeriesLedId.ZoneFiftyEight },
        { LedId.LedStripe59, SteelSeriesLedId.ZoneFiftyNine },
        { LedId.LedStripe60, SteelSeriesLedId.ZoneSixty },
        { LedId.LedStripe61, SteelSeriesLedId.ZoneSixtyOne },
        { LedId.LedStripe62, SteelSeriesLedId.ZoneSixtyTwo },
        { LedId.LedStripe63, SteelSeriesLedId.ZoneSixtyThree },
        { LedId.LedStripe64, SteelSeriesLedId.ZoneSixtyFour },
        { LedId.LedStripe65, SteelSeriesLedId.ZoneSixtyFive },
        { LedId.LedStripe66, SteelSeriesLedId.ZoneSixtySix },
        { LedId.LedStripe67, SteelSeriesLedId.ZoneSixtySeven },
        { LedId.LedStripe68, SteelSeriesLedId.ZoneSixtyEight },
        { LedId.LedStripe69, SteelSeriesLedId.ZoneSixtyNine },
        { LedId.LedStripe70, SteelSeriesLedId.ZoneSeventy },
        { LedId.LedStripe71, SteelSeriesLedId.ZoneSeventyOne },
        { LedId.LedStripe72, SteelSeriesLedId.ZoneSeventyTwo },
        { LedId.LedStripe73, SteelSeriesLedId.ZoneSeventyThree },
        { LedId.LedStripe74, SteelSeriesLedId.ZoneSeventyFour },
        { LedId.LedStripe75, SteelSeriesLedId.ZoneSeventyFive },
        { LedId.LedStripe76, SteelSeriesLedId.ZoneSeventySix },
        { LedId.LedStripe77, SteelSeriesLedId.ZoneSeventySeven },
        { LedId.LedStripe78, SteelSeriesLedId.ZoneSeventyEight },
        { LedId.LedStripe79, SteelSeriesLedId.ZoneSeventyNine },
        { LedId.LedStripe80, SteelSeriesLedId.ZoneEighty },
        { LedId.LedStripe81, SteelSeriesLedId.ZoneEightyOne },
        { LedId.LedStripe82, SteelSeriesLedId.ZoneEightyTwo },
        { LedId.LedStripe83, SteelSeriesLedId.ZoneEightyThree },
        { LedId.LedStripe84, SteelSeriesLedId.ZoneEightyFour },
        { LedId.LedStripe85, SteelSeriesLedId.ZoneEightyFive },
        { LedId.LedStripe86, SteelSeriesLedId.ZoneEightySix },
        { LedId.LedStripe87, SteelSeriesLedId.ZoneEightySeven },
        { LedId.LedStripe88, SteelSeriesLedId.ZoneEightyEight },
        { LedId.LedStripe89, SteelSeriesLedId.ZoneEightyNine },
        { LedId.LedStripe90, SteelSeriesLedId.ZoneNinety },
        { LedId.LedStripe91, SteelSeriesLedId.ZoneNinetyOne },
        { LedId.LedStripe92, SteelSeriesLedId.ZoneNinetyTwo },
        { LedId.LedStripe93, SteelSeriesLedId.ZoneNinetyThree },
        { LedId.LedStripe94, SteelSeriesLedId.ZoneNinetyFour },
        { LedId.LedStripe95, SteelSeriesLedId.ZoneNinetyFive },
        { LedId.LedStripe96, SteelSeriesLedId.ZoneNinetySix },
        { LedId.LedStripe97, SteelSeriesLedId.ZoneNinetySeven },
        { LedId.LedStripe98, SteelSeriesLedId.ZoneNinetyEight },
        { LedId.LedStripe99, SteelSeriesLedId.ZoneNinetyNine },
        { LedId.LedStripe100, SteelSeriesLedId.ZoneOneHundred },
        { LedId.LedStripe101, SteelSeriesLedId.ZoneOneHundredOne },
        { LedId.LedStripe102, SteelSeriesLedId.ZoneOneHundredTwo },
        { LedId.LedStripe103, SteelSeriesLedId.ZoneOneHundredThree }
    };
}