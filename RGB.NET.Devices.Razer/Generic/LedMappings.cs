using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer;

/// <summary>
/// Contains mappings for <see cref="LedId"/> to the matrix location.
/// </summary>
public static class LedMappings
{
    /// <summary>
    /// Gets the mapping for keyboards.
    /// </summary>
    public static LedMapping<int> Keyboard { get; } = new()
    {
        //Row 0 is empty

        #region Row 1

        [LedId.Keyboard_Escape] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 2,
        [LedId.Keyboard_F1] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 4,
        [LedId.Keyboard_F2] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 5,
        [LedId.Keyboard_F3] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 6,
        [LedId.Keyboard_F4] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 7,
        [LedId.Keyboard_F5] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 8,
        [LedId.Keyboard_F6] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 9,
        [LedId.Keyboard_F7] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 10,
        [LedId.Keyboard_F8] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 11,
        [LedId.Keyboard_F9] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 12,
        [LedId.Keyboard_F10] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 13,
        [LedId.Keyboard_F11] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 14,
        [LedId.Keyboard_F12] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 15,
        [LedId.Keyboard_PrintScreen] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 16,
        [LedId.Keyboard_ScrollLock] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 17,
        [LedId.Keyboard_PauseBreak] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 18,
        [LedId.Logo] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 21,

        #endregion

        #region Row 2

        [LedId.Keyboard_Programmable1] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 1,
        [LedId.Keyboard_GraveAccentAndTilde] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 2,
        [LedId.Keyboard_1] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 3,
        [LedId.Keyboard_2] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 4,
        [LedId.Keyboard_3] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 5,
        [LedId.Keyboard_4] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 6,
        [LedId.Keyboard_5] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 7,
        [LedId.Keyboard_6] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 8,
        [LedId.Keyboard_7] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 9,
        [LedId.Keyboard_8] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 10,
        [LedId.Keyboard_9] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 11,
        [LedId.Keyboard_0] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 12,
        [LedId.Keyboard_MinusAndUnderscore] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 13,
        [LedId.Keyboard_EqualsAndPlus] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 14,
        [LedId.Keyboard_Backspace] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 15,
        [LedId.Keyboard_Insert] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 16,
        [LedId.Keyboard_Home] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 17,
        [LedId.Keyboard_PageUp] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 18,
        [LedId.Keyboard_NumLock] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 19,
        [LedId.Keyboard_NumSlash] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 20,
        [LedId.Keyboard_NumAsterisk] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 21,
        [LedId.Keyboard_NumMinus] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 22,

        #endregion

        #region Row 3

        [LedId.Keyboard_Programmable2] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 1,
        [LedId.Keyboard_Tab] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 2,
        [LedId.Keyboard_Q] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 3,
        [LedId.Keyboard_W] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 4,
        [LedId.Keyboard_E] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 5,
        [LedId.Keyboard_R] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 6,
        [LedId.Keyboard_T] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 7,
        [LedId.Keyboard_Y] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 8,
        [LedId.Keyboard_U] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 9,
        [LedId.Keyboard_I] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 10,
        [LedId.Keyboard_O] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 11,
        [LedId.Keyboard_P] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 12,
        [LedId.Keyboard_BracketLeft] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 13,
        [LedId.Keyboard_BracketRight] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 14,
        [LedId.Keyboard_Backslash] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 15,
        [LedId.Keyboard_Delete] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 16,
        [LedId.Keyboard_End] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 17,
        [LedId.Keyboard_PageDown] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 18,
        [LedId.Keyboard_Num7] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 19,
        [LedId.Keyboard_Num8] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 20,
        [LedId.Keyboard_Num9] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 21,
        [LedId.Keyboard_NumPlus] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 22,

        #endregion

        #region Row 4

        [LedId.Keyboard_Programmable3] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 1,
        [LedId.Keyboard_CapsLock] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 2,
        [LedId.Keyboard_A] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 3,
        [LedId.Keyboard_S] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 4,
        [LedId.Keyboard_D] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 5,
        [LedId.Keyboard_F] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 6,
        [LedId.Keyboard_G] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 7,
        [LedId.Keyboard_H] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 8,
        [LedId.Keyboard_J] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 9,
        [LedId.Keyboard_K] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 10,
        [LedId.Keyboard_L] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 11,
        [LedId.Keyboard_SemicolonAndColon] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 12,
        [LedId.Keyboard_ApostropheAndDoubleQuote] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 13,
        [LedId.Keyboard_NonUsTilde] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 14,
        [LedId.Keyboard_Enter] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 15,
        [LedId.Keyboard_Num4] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 19,
        [LedId.Keyboard_Num5] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 20,
        [LedId.Keyboard_Num6] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 21,

        #endregion

        #region Row 5

        [LedId.Keyboard_Programmable4] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 1,
        [LedId.Keyboard_LeftShift] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 2,
        [LedId.Keyboard_NonUsBackslash] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 3,
        [LedId.Keyboard_Z] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 4,
        [LedId.Keyboard_X] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 5,
        [LedId.Keyboard_C] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 6,
        [LedId.Keyboard_V] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 7,
        [LedId.Keyboard_B] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 8,
        [LedId.Keyboard_N] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 9,
        [LedId.Keyboard_M] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 10,
        [LedId.Keyboard_CommaAndLessThan] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 11,
        [LedId.Keyboard_PeriodAndBiggerThan] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 12,
        [LedId.Keyboard_SlashAndQuestionMark] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 13,
        [LedId.Keyboard_RightShift] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 15,
        [LedId.Keyboard_ArrowUp] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 17,
        [LedId.Keyboard_Num1] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 19,
        [LedId.Keyboard_Num2] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 20,
        [LedId.Keyboard_Num3] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 21,
        [LedId.Keyboard_NumEnter] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 22,

        #endregion

        #region Row 6

        [LedId.Keyboard_Programmable5] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 1,
        [LedId.Keyboard_LeftCtrl] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 2,
        [LedId.Keyboard_LeftGui] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 3,
        [LedId.Keyboard_LeftAlt] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 4,
        [LedId.Keyboard_Space] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 8,
        [LedId.Keyboard_RightAlt] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 12,
        [LedId.Keyboard_RightGui] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 13,
        [LedId.Keyboard_Application] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 14,
        [LedId.Keyboard_RightCtrl] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 15,
        [LedId.Keyboard_ArrowLeft] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 16,
        [LedId.Keyboard_ArrowDown] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 17,
        [LedId.Keyboard_ArrowRight] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 18,
        [LedId.Keyboard_Num0] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 20,
        [LedId.Keyboard_NumPeriodAndDelete] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 21,

        #endregion

        //Row 7 is also empty
    };

    /// <summary>
    /// Gets the mapping for laptop keyboards.
    /// </summary>
    public static LedMapping<int> LaptopKeyboard { get; } = new()
    {
        //Row 0 is empty

        #region Row 1

        [LedId.Keyboard_Escape] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 1,
        [LedId.Keyboard_F1] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 2,
        [LedId.Keyboard_F2] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 3,
        [LedId.Keyboard_F3] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 4,
        [LedId.Keyboard_F4] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 5,
        [LedId.Keyboard_F5] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 6,
        [LedId.Keyboard_F6] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 7,
        [LedId.Keyboard_F7] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 8,
        [LedId.Keyboard_F8] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 9,
        [LedId.Keyboard_F9] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 10,
        [LedId.Keyboard_F10] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 11,
        [LedId.Keyboard_F11] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 12,
        [LedId.Keyboard_F12] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 13,
        [LedId.Keyboard_Insert] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 14,
        [LedId.Keyboard_Delete] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 15,

        #endregion

        #region Row 2

        [LedId.Keyboard_GraveAccentAndTilde] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 1,
        [LedId.Keyboard_1] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 2,
        [LedId.Keyboard_2] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 3,
        [LedId.Keyboard_3] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 4,
        [LedId.Keyboard_4] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 5,
        [LedId.Keyboard_5] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 6,
        [LedId.Keyboard_6] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 7,
        [LedId.Keyboard_7] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 8,
        [LedId.Keyboard_8] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 9,
        [LedId.Keyboard_9] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 10,
        [LedId.Keyboard_0] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 11,
        [LedId.Keyboard_MinusAndUnderscore] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 12,
        [LedId.Keyboard_EqualsAndPlus] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 13,
        [LedId.Keyboard_Backspace] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 14,

        #endregion

        #region Row 3

        [LedId.Keyboard_Tab] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 2,
        [LedId.Keyboard_Q] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 3,
        [LedId.Keyboard_W] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 4,
        [LedId.Keyboard_E] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 5,
        [LedId.Keyboard_R] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 6,
        [LedId.Keyboard_T] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 7,
        [LedId.Keyboard_Y] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 8,
        [LedId.Keyboard_U] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 9,
        [LedId.Keyboard_I] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 10,
        [LedId.Keyboard_O] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 11,
        [LedId.Keyboard_P] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 12,
        [LedId.Keyboard_BracketLeft] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 13,
        [LedId.Keyboard_BracketRight] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 14,
        [LedId.Keyboard_Backslash] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 15,

        #endregion

        #region Row 4

        [LedId.Keyboard_CapsLock] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 1,
        [LedId.Keyboard_A] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 3,
        [LedId.Keyboard_S] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 4,
        [LedId.Keyboard_D] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 5,
        [LedId.Keyboard_F] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 6,
        [LedId.Keyboard_G] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 7,
        [LedId.Keyboard_H] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 8,
        [LedId.Keyboard_J] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 9,
        [LedId.Keyboard_K] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 10,
        [LedId.Keyboard_L] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 11,
        [LedId.Keyboard_SemicolonAndColon] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 12,
        [LedId.Keyboard_ApostropheAndDoubleQuote] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 13,
        //[LedId.Keyboard_NonUsTilde] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 14, //TODO diogotr7 15.04.2021: investigate
        [LedId.Keyboard_Enter] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 16,

        #endregion

        #region Row 5

        [LedId.Keyboard_LeftShift] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 2,
        [LedId.Keyboard_Z] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 3,
        [LedId.Keyboard_X] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 4,
        [LedId.Keyboard_C] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 5,
        [LedId.Keyboard_V] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 6,
        [LedId.Keyboard_B] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 7,
        [LedId.Keyboard_N] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 8,
        [LedId.Keyboard_M] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 9,
        [LedId.Keyboard_CommaAndLessThan] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 10,
        [LedId.Keyboard_PeriodAndBiggerThan] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 11,
        [LedId.Keyboard_SlashAndQuestionMark] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 12,
        [LedId.Keyboard_ArrowUp] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 13,
        [LedId.Keyboard_RightShift] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 14,

        #endregion

        #region Row 6

        [LedId.Keyboard_LeftCtrl] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 1,
        [LedId.Keyboard_Custom1] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 2,//left fn
        [LedId.Keyboard_LeftGui] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 3,
        [LedId.Keyboard_LeftAlt] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 4,
        [LedId.Keyboard_Space] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 7,
        [LedId.Keyboard_RightAlt] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 10,
        [LedId.Keyboard_RightCtrl] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 11,
        [LedId.Keyboard_ArrowLeft] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 12,
        [LedId.Keyboard_ArrowDown] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 13,
        [LedId.Keyboard_ArrowRight] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 14,
        [LedId.Keyboard_Custom2] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 15,//right fn

        #endregion

        //Row 7 is also empty
    };

    /// <summary>
    /// Gets the mapping for the Black-Widow V3 keyboard.
    /// </summary>
    public static LedMapping<int> KeyboardBlackWidowV3 { get; } = new()
    {
        //Row 0 is empty

        #region Row 1

        [LedId.Keyboard_Escape] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 2,
        [LedId.Keyboard_F1] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 4,
        [LedId.Keyboard_F2] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 5,
        [LedId.Keyboard_F3] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 6,
        [LedId.Keyboard_F4] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 7,
        [LedId.Keyboard_F5] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 8,
        [LedId.Keyboard_F6] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 9,
        [LedId.Keyboard_F7] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 10,
        [LedId.Keyboard_F8] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 11,
        [LedId.Keyboard_F9] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 12,
        [LedId.Keyboard_F10] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 13,
        [LedId.Keyboard_F11] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 14,
        [LedId.Keyboard_F12] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 15,
        [LedId.Keyboard_PrintScreen] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 16,
        [LedId.Keyboard_ScrollLock] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 17,
        [LedId.Keyboard_PauseBreak] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 18,
        [LedId.Logo] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 21,

        #endregion

        #region Row 2

        [LedId.Keyboard_Programmable1] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 1,
        [LedId.Keyboard_GraveAccentAndTilde] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 2,
        [LedId.Keyboard_1] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 3,
        [LedId.Keyboard_2] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 4,
        [LedId.Keyboard_3] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 5,
        [LedId.Keyboard_4] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 6,
        [LedId.Keyboard_5] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 7,
        [LedId.Keyboard_6] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 8,
        [LedId.Keyboard_7] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 9,
        [LedId.Keyboard_8] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 10,
        [LedId.Keyboard_9] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 11,
        [LedId.Keyboard_0] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 12,
        [LedId.Keyboard_MinusAndUnderscore] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 13,
        [LedId.Keyboard_EqualsAndPlus] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 14,
        [LedId.Keyboard_Backspace] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 15,
        [LedId.Keyboard_Insert] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 16,
        [LedId.Keyboard_Home] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 17,
        [LedId.Keyboard_PageUp] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 18,
        [LedId.Keyboard_NumLock] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 19,
        [LedId.Keyboard_NumSlash] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 20,
        [LedId.Keyboard_NumAsterisk] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 21,
        [LedId.Keyboard_NumMinus] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 22,

        #endregion

        #region Row 3

        [LedId.Keyboard_Programmable2] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 1,
        [LedId.Keyboard_Tab] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 2,
        [LedId.Keyboard_Q] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 3,
        [LedId.Keyboard_W] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 4,
        [LedId.Keyboard_E] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 5,
        [LedId.Keyboard_R] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 6,
        [LedId.Keyboard_T] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 7,
        [LedId.Keyboard_Y] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 8,
        [LedId.Keyboard_U] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 9,
        [LedId.Keyboard_I] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 10,
        [LedId.Keyboard_O] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 11,
        [LedId.Keyboard_P] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 12,
        [LedId.Keyboard_BracketLeft] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 13,
        [LedId.Keyboard_BracketRight] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 14,
        [LedId.Keyboard_Backslash] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 15,
        [LedId.Keyboard_Delete] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 16,
        [LedId.Keyboard_End] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 17,
        [LedId.Keyboard_PageDown] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 18,
        [LedId.Keyboard_Num7] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 19,
        [LedId.Keyboard_Num8] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 20,
        [LedId.Keyboard_Num9] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 21,
        [LedId.Keyboard_NumPlus] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 22,

        #endregion

        #region Row 4

        [LedId.Keyboard_Programmable3] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 1,
        [LedId.Keyboard_CapsLock] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 2,
        [LedId.Keyboard_A] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 3,
        [LedId.Keyboard_S] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 4,
        [LedId.Keyboard_D] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 5,
        [LedId.Keyboard_F] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 6,
        [LedId.Keyboard_G] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 7,
        [LedId.Keyboard_H] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 8,
        [LedId.Keyboard_J] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 9,
        [LedId.Keyboard_K] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 10,
        [LedId.Keyboard_L] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 11,
        [LedId.Keyboard_SemicolonAndColon] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 12,
        [LedId.Keyboard_ApostropheAndDoubleQuote] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 13,
        [LedId.Keyboard_NonUsTilde] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 14,
        [LedId.Keyboard_Enter] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 15,
        [LedId.Keyboard_Num4] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 19,
        [LedId.Keyboard_Num5] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 20,
        [LedId.Keyboard_Num6] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 21,

        #endregion

        #region Row 5

        [LedId.Keyboard_Programmable4] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 1,
        [LedId.Keyboard_LeftShift] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 2,
        [LedId.Keyboard_NonUsBackslash] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 3,
        [LedId.Keyboard_Z] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 4,
        [LedId.Keyboard_X] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 5,
        [LedId.Keyboard_C] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 6,
        [LedId.Keyboard_V] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 7,
        [LedId.Keyboard_B] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 8,
        [LedId.Keyboard_N] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 9,
        [LedId.Keyboard_M] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 10,
        [LedId.Keyboard_CommaAndLessThan] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 11,
        [LedId.Keyboard_PeriodAndBiggerThan] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 12,
        [LedId.Keyboard_SlashAndQuestionMark] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 13,
        [LedId.Keyboard_RightShift] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 15,
        [LedId.Keyboard_ArrowUp] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 17,
        [LedId.Keyboard_Num1] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 19,
        [LedId.Keyboard_Num2] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 20,
        [LedId.Keyboard_Num3] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 21,
        [LedId.Keyboard_NumEnter] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 22,

        #endregion

        #region Row 6

        [LedId.Keyboard_Programmable5] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 1,
        [LedId.Keyboard_LeftCtrl] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 2,
        [LedId.Keyboard_LeftGui] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 3,
        [LedId.Keyboard_LeftAlt] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 4,
        [LedId.Keyboard_Space] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 7,
        [LedId.Keyboard_RightAlt] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 11,
        [LedId.Keyboard_RightGui] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 13,
        [LedId.Keyboard_Application] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 14,
        [LedId.Keyboard_RightCtrl] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 15,
        [LedId.Keyboard_ArrowLeft] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 16,
        [LedId.Keyboard_ArrowDown] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 17,
        [LedId.Keyboard_ArrowRight] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 18,
        [LedId.Keyboard_Num0] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 20,
        [LedId.Keyboard_NumPeriodAndDelete] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 21,

        #endregion

        //Row 7 is also empty
    };

    /// <summary>
    /// Gets the mapping for mice.
    /// </summary>
    public static LedMapping<int> Mouse { get; } = new()
    {
        //row 0 empty

        //row 1
        [LedId.Mouse1] = (_Defines.MOUSE_MAX_COLUMN * 1) + 0,
        [LedId.Mouse2] = (_Defines.MOUSE_MAX_COLUMN * 1) + 6,

        //row 2
        [LedId.Mouse3] = (_Defines.MOUSE_MAX_COLUMN * 2) + 0,
        [LedId.Mouse4] = (_Defines.MOUSE_MAX_COLUMN * 2) + 3,
        [LedId.Mouse5] = (_Defines.MOUSE_MAX_COLUMN * 2) + 6,

        //row 3
        [LedId.Mouse6] = (_Defines.MOUSE_MAX_COLUMN * 3) + 0,
        [LedId.Mouse7] = (_Defines.MOUSE_MAX_COLUMN * 3) + 6,

        //row 4
        [LedId.Mouse8] = (_Defines.MOUSE_MAX_COLUMN * 4) + 0,
        [LedId.Mouse9] = (_Defines.MOUSE_MAX_COLUMN * 4) + 3,
        [LedId.Mouse10] = (_Defines.MOUSE_MAX_COLUMN * 4) + 6,

        //row 5
        [LedId.Mouse11] = (_Defines.MOUSE_MAX_COLUMN * 5) + 0,
        [LedId.Mouse12] = (_Defines.MOUSE_MAX_COLUMN * 5) + 6,

        //row 6
        [LedId.Mouse13] = (_Defines.MOUSE_MAX_COLUMN * 6) + 0,
        [LedId.Mouse14] = (_Defines.MOUSE_MAX_COLUMN * 6) + 6,

        //row 7
        [LedId.Mouse15] = (_Defines.MOUSE_MAX_COLUMN * 7) + 0,
        [LedId.Mouse16] = (_Defines.MOUSE_MAX_COLUMN * 7) + 3,
        [LedId.Mouse17] = (_Defines.MOUSE_MAX_COLUMN * 7) + 6,

        //row 8
        [LedId.Mouse18] = (_Defines.MOUSE_MAX_COLUMN * 8) + 1,
        [LedId.Mouse19] = (_Defines.MOUSE_MAX_COLUMN * 8) + 2,
        [LedId.Mouse20] = (_Defines.MOUSE_MAX_COLUMN * 8) + 3,
        [LedId.Mouse21] = (_Defines.MOUSE_MAX_COLUMN * 8) + 4,
        [LedId.Mouse22] = (_Defines.MOUSE_MAX_COLUMN * 8) + 5,
    };

    //TODO DarthAffe 27.04.2021: Are mappings for these possible?
    /// <summary>
    /// Gets the mapping for mousepads.
    /// </summary>
    public static LedMapping<int> Mousepad { get; } = new();

    /// <summary>
    /// Gets the mapping for headsets.
    /// </summary>
    public static LedMapping<int> Headset { get; } = new();

    /// <summary>
    /// Gets the mapping for keypads.
    /// </summary>
    public static LedMapping<int> Keypad { get; } = new();

    /// <summary>
    /// Gets the mapping for chroma link devices.
    /// </summary>
    public static LedMapping<int> ChromaLink { get; } = new();
}