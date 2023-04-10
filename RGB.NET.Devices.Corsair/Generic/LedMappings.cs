using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair;

/// <summary>
/// Contains mappings for <see cref="LedId"/> to <see cref="CorsairLedId"/>.
/// </summary>
internal static class LedMappings
{
    #region Constants

    // ReSharper disable once InconsistentNaming
    private static LedMapping<CorsairLedId> KEYBOARD_MAPPING => new()
    {
        { LedId.Invalid, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Invalid) },
        { LedId.Keyboard_Escape, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Escape) },
        { LedId.Keyboard_F1, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.F1) },
        { LedId.Keyboard_F2, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.F2) },
        { LedId.Keyboard_F3, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.F3) },
        { LedId.Keyboard_F4, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.F4) },
        { LedId.Keyboard_F5, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.F5) },
        { LedId.Keyboard_F6, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.F6) },
        { LedId.Keyboard_F7, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.F7) },
        { LedId.Keyboard_F8, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.F8) },
        { LedId.Keyboard_F9, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.F9) },
        { LedId.Keyboard_F10, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.F10) },
        { LedId.Keyboard_F11, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.F11) },
        { LedId.Keyboard_GraveAccentAndTilde, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.GraveAccentAndTilde) },
        { LedId.Keyboard_1, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.One) },
        { LedId.Keyboard_2, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Two) },
        { LedId.Keyboard_3, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Three) },
        { LedId.Keyboard_4, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Four) },
        { LedId.Keyboard_5, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Five) },
        { LedId.Keyboard_6, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Six) },
        { LedId.Keyboard_7, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Seven) },
        { LedId.Keyboard_8, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Eight) },
        { LedId.Keyboard_9, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Nine) },
        { LedId.Keyboard_0, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Zero) },
        { LedId.Keyboard_MinusAndUnderscore, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.MinusAndUnderscore) },
        { LedId.Keyboard_Tab, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Tab) },
        { LedId.Keyboard_Q, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Q) },
        { LedId.Keyboard_W, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.W) },
        { LedId.Keyboard_E, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.E) },
        { LedId.Keyboard_R, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.R) },
        { LedId.Keyboard_T, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.T) },
        { LedId.Keyboard_Y, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Y) },
        { LedId.Keyboard_U, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.U) },
        { LedId.Keyboard_I, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.I) },
        { LedId.Keyboard_O, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.O) },
        { LedId.Keyboard_P, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.P) },
        { LedId.Keyboard_BracketLeft, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.BracketLeft) },
        { LedId.Keyboard_CapsLock, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.CapsLock) },
        { LedId.Keyboard_A, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.A) },
        { LedId.Keyboard_S, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.S) },
        { LedId.Keyboard_D, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.D) },
        { LedId.Keyboard_F, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.F) },
        { LedId.Keyboard_G, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.G) },
        { LedId.Keyboard_H, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.H) },
        { LedId.Keyboard_J, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.J) },
        { LedId.Keyboard_K, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.K) },
        { LedId.Keyboard_L, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.L) },
        { LedId.Keyboard_SemicolonAndColon, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.SemicolonAndColon) },
        { LedId.Keyboard_ApostropheAndDoubleQuote, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.ApostropheAndDoubleQuote) },
        { LedId.Keyboard_LeftShift, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.LeftShift) },
        { LedId.Keyboard_NonUsBackslash, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.NonUsBackslash) },
        { LedId.Keyboard_Z, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Z) },
        { LedId.Keyboard_X, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.X) },
        { LedId.Keyboard_C, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.C) },
        { LedId.Keyboard_V, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.V) },
        { LedId.Keyboard_B, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.B) },
        { LedId.Keyboard_N, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.N) },
        { LedId.Keyboard_M, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.M) },
        { LedId.Keyboard_CommaAndLessThan, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.CommaAndLessThan) },
        { LedId.Keyboard_PeriodAndBiggerThan, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.PeriodAndBiggerThan) },
        { LedId.Keyboard_SlashAndQuestionMark, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.SlashAndQuestionMark) },
        { LedId.Keyboard_LeftCtrl, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.LeftCtrl) },
        { LedId.Keyboard_LeftGui, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.LeftGui) },
        { LedId.Keyboard_LeftAlt, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.LeftAlt) },
        { LedId.Keyboard_Lang2, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Lang2) },
        { LedId.Keyboard_Space, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Space) },
        { LedId.Keyboard_Lang1, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Lang1) },
        { LedId.Keyboard_International2, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.International2) },
        { LedId.Keyboard_RightAlt, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.RightAlt) },
        { LedId.Keyboard_RightGui, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.RightGui) },
        { LedId.Keyboard_Application, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Application) },
        { LedId.Keyboard_Brightness, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Brightness) },
        { LedId.Keyboard_F12, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.F12) },
        { LedId.Keyboard_PrintScreen, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.PrintScreen) },
        { LedId.Keyboard_ScrollLock, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.ScrollLock) },
        { LedId.Keyboard_PauseBreak, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.PauseBreak) },
        { LedId.Keyboard_Insert, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Insert) },
        { LedId.Keyboard_Home, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Home) },
        { LedId.Keyboard_PageUp, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.PageUp) },
        { LedId.Keyboard_BracketRight, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.BracketRight) },
        { LedId.Keyboard_Backslash, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Backslash) },
        { LedId.Keyboard_NonUsTilde, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.NonUsTilde) },
        { LedId.Keyboard_Enter, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Enter) },
        { LedId.Keyboard_International1, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.International1) },
        { LedId.Keyboard_EqualsAndPlus, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.EqualsAndPlus) },
        { LedId.Keyboard_International3, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.International3) },
        { LedId.Keyboard_Backspace, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Backspace) },
        { LedId.Keyboard_Delete, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Delete) },
        { LedId.Keyboard_End, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.End) },
        { LedId.Keyboard_PageDown, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.PageDown) },
        { LedId.Keyboard_RightShift, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.RightShift) },
        { LedId.Keyboard_RightCtrl, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.RightCtrl) },
        { LedId.Keyboard_ArrowUp, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.UpArrow) },
        { LedId.Keyboard_ArrowLeft, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.LeftArrow) },
        { LedId.Keyboard_ArrowDown, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.DownArrow) },
        { LedId.Keyboard_ArrowRight, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.RightArrow) },
        { LedId.Keyboard_WinLock, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.WinLock) },
        { LedId.Keyboard_MediaMute, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Mute) },
        { LedId.Keyboard_MediaStop, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Stop) },
        { LedId.Keyboard_MediaPreviousTrack, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.ScanPreviousTrack) },
        { LedId.Keyboard_MediaPlay, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.PlayPause) },
        { LedId.Keyboard_MediaNextTrack, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.ScanNextTrack) },
        { LedId.Keyboard_NumLock, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.NumLock) },
        { LedId.Keyboard_NumSlash, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.KeypadSlash) },
        { LedId.Keyboard_NumAsterisk, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.KeypadAsterisk) },
        { LedId.Keyboard_NumMinus, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.KeypadMinus) },
        { LedId.Keyboard_NumPlus, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.KeypadPlus) },
        { LedId.Keyboard_NumEnter, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.KeypadEnter) },
        { LedId.Keyboard_Num7, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Keypad7) },
        { LedId.Keyboard_Num8, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Keypad8) },
        { LedId.Keyboard_Num9, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Keypad9) },
        { LedId.Keyboard_NumComma, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.KeypadComma) },
        { LedId.Keyboard_Num4, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Keypad4) },
        { LedId.Keyboard_Num5, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Keypad5) },
        { LedId.Keyboard_Num6, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Keypad6) },
        { LedId.Keyboard_Num1, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Keypad1) },
        { LedId.Keyboard_Num2, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Keypad2) },
        { LedId.Keyboard_Num3, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Keypad3) },
        { LedId.Keyboard_Num0, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Keypad0) },
        { LedId.Keyboard_NumPeriodAndDelete, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.KeypadPeriodAndDelete) },
        { LedId.Keyboard_MediaVolumeUp, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.VolumeUp) },
        { LedId.Keyboard_MediaVolumeDown, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.VolumeDown) },
        { LedId.Keyboard_MacroRecording, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.MR) },
        { LedId.Keyboard_Macro1, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.M1) },
        { LedId.Keyboard_Macro2, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.M2) },
        { LedId.Keyboard_Macro3, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.M3) },
        { LedId.Keyboard_International5, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.International5) },
        { LedId.Keyboard_International4, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.International4) },
        { LedId.Keyboard_LedProgramming, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.LedProgramming) },
        { LedId.Keyboard_Function, new CorsairLedId(CorsairLedGroup.Keyboard, CorsairLedIdKeyboard.Fn) }
    };

    #endregion

    #region Methods

    internal static LedMapping<CorsairLedId> CreateFanMapping(IEnumerable<CorsairLedId> ids) => CreateMapping(ids, LedId.Fan1);
    internal static LedMapping<CorsairLedId> CreateCoolerMapping(IEnumerable<CorsairLedId> ids) => CreateMapping(ids, LedId.Cooler1);
    internal static LedMapping<CorsairLedId> CreateLedStripMapping(IEnumerable<CorsairLedId> ids) => CreateMapping(ids, LedId.LedStripe1);
    internal static LedMapping<CorsairLedId> CreateGraphicsCardMapping(IEnumerable<CorsairLedId> ids) => CreateMapping(ids, LedId.GraphicsCard1);
    internal static LedMapping<CorsairLedId> CreateHeadsetStandMapping(IEnumerable<CorsairLedId> ids) => CreateMapping(ids, LedId.HeadsetStand1);
    internal static LedMapping<CorsairLedId> CreateMainboardMapping(IEnumerable<CorsairLedId> ids) => CreateMapping(ids, LedId.Mainboard1);
    internal static LedMapping<CorsairLedId> CreateMemoryMapping(IEnumerable<CorsairLedId> ids) => CreateMapping(ids, LedId.DRAM1);
    internal static LedMapping<CorsairLedId> CreateMousepadMapping(IEnumerable<CorsairLedId> ids) => CreateMapping(ids, LedId.Mousepad1);
    internal static LedMapping<CorsairLedId> CreateHeadsetMapping(IEnumerable<CorsairLedId> ids) => CreateMapping(ids, LedId.Headset1);
    internal static LedMapping<CorsairLedId> CreateMouseMapping(IEnumerable<CorsairLedId> ids) => CreateMapping(ids, LedId.Mouse1);
    internal static LedMapping<CorsairLedId> CreateUnknownMapping(IEnumerable<CorsairLedId> ids) => CreateMapping(ids, LedId.Unknown1);

    internal static LedMapping<CorsairLedId> CreateMapping(IEnumerable<CorsairLedId> ids, LedId referenceId)
    {
        LedMapping<CorsairLedId> mapping = new();
        int counter = 0;
        foreach (CorsairLedId corsairLedId in ids.OrderBy(x => x))
            mapping.Add(referenceId + counter++, corsairLedId);

        return mapping;
    }

    internal static LedMapping<CorsairLedId> CreateKeyboardMapping(IEnumerable<CorsairLedId> ids)
    {
        Dictionary<CorsairLedGroup, int> groupCounter = new()
        {
            [CorsairLedGroup.KeyboardOem] = 0,
            [CorsairLedGroup.KeyboardGKeys] = 0,
            [CorsairLedGroup.KeyboardEdge] = 0,
            [CorsairLedGroup.Keyboard] = 0 // Workaround for unknown keys
        };

        LedMapping<CorsairLedId> mapping = KEYBOARD_MAPPING;

        foreach (CorsairLedId corsairLedId in ids.OrderBy(x => x).Where(x => x.Group != CorsairLedGroup.Keyboard))
            switch (corsairLedId.Group)
            {
                case CorsairLedGroup.KeyboardOem:
                    mapping.Add(LedId.Keyboard_Custom1 + groupCounter[CorsairLedGroup.KeyboardOem]++, corsairLedId);
                    break;

                case CorsairLedGroup.KeyboardGKeys:
                    mapping.Add(LedId.Keyboard_Programmable1 + groupCounter[CorsairLedGroup.KeyboardGKeys]++, corsairLedId);
                    break;

                case CorsairLedGroup.KeyboardEdge:
                    mapping.Add(LedId.LedStripe1 + groupCounter[CorsairLedGroup.KeyboardEdge]++, corsairLedId);
                    break;

                default:
                    mapping.Add(LedId.Unknown1 + groupCounter[CorsairLedGroup.Keyboard]++, corsairLedId);
                    break;
            }

        return mapping;
    }

    #endregion
}