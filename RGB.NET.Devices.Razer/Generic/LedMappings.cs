using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer;

/// <summary>
/// Contains mappings for <see cref="LedId"/> to the matrix location.
/// </summary>
public static class LedMappings
{
    /// <summary>
    /// Gets the mapping for the full-size BlackWidow V4 Low-profile HyperSpeed (0x02CC).
    /// </summary>
    public static LedMapping<int> BlackWidowV4LowProfileHyperSpeed { get; } = new()
    {
        // Slot indices are zero-based (row * 24 + column). LedIds describe physical key positions.
        [LedId.Keyboard_Programmable1] = 3,
        [LedId.Keyboard_Programmable2] = 4,
        [LedId.Keyboard_Programmable3] = 5,
        [LedId.Keyboard_Programmable4] = 6,
        [LedId.Keyboard_MediaPreviousTrack] = 16,
        [LedId.Keyboard_MediaPlay] = 17,
        [LedId.Keyboard_MediaNextTrack] = 18,

        [LedId.Keyboard_Escape] = 26,
        [LedId.Keyboard_F1] = 27,
        [LedId.Keyboard_F2] = 28,
        [LedId.Keyboard_F3] = 29,
        [LedId.Keyboard_F4] = 30,
        [LedId.Keyboard_F5] = 31,
        [LedId.Keyboard_F6] = 32,
        [LedId.Keyboard_F7] = 33,
        [LedId.Keyboard_F8] = 34,
        [LedId.Keyboard_F9] = 36,
        [LedId.Keyboard_F10] = 37,
        [LedId.Keyboard_F11] = 38,
        [LedId.Keyboard_F12] = 39,
        [LedId.Keyboard_PrintScreen] = 40,
        [LedId.Keyboard_ScrollLock] = 41,
        [LedId.Keyboard_PauseBreak] = 42,
        [LedId.Custom1] = 43, // Wireless button
        [LedId.Custom2] = 44, // Bluetooth button
        [LedId.Custom3] = 45, // AI button
        [LedId.Custom4] = 46, // Battery indicator

        [LedId.Keyboard_GraveAccentAndTilde] = 49,
        [LedId.Keyboard_1] = 50,
        [LedId.Keyboard_2] = 51,
        [LedId.Keyboard_3] = 52,
        [LedId.Keyboard_4] = 53,
        [LedId.Keyboard_5] = 54,
        [LedId.Keyboard_6] = 55,
        [LedId.Keyboard_7] = 56,
        [LedId.Keyboard_8] = 57,
        [LedId.Keyboard_9] = 58,
        [LedId.Keyboard_0] = 59,
        [LedId.Keyboard_MinusAndUnderscore] = 60,
        [LedId.Keyboard_EqualsAndPlus] = 61,
        [LedId.Keyboard_Backspace] = 63,
        [LedId.Keyboard_Insert] = 64,
        [LedId.Keyboard_Home] = 65,
        [LedId.Keyboard_PageUp] = 66,
        [LedId.Keyboard_NumLock] = 67,
        [LedId.Keyboard_NumSlash] = 68,
        [LedId.Keyboard_NumAsterisk] = 69,
        [LedId.Keyboard_NumMinus] = 70,

        [LedId.Keyboard_Tab] = 73,
        [LedId.Keyboard_Q] = 74,
        [LedId.Keyboard_W] = 75,
        [LedId.Keyboard_E] = 76,
        [LedId.Keyboard_R] = 77,
        [LedId.Keyboard_T] = 78,
        [LedId.Keyboard_Y] = 79,
        [LedId.Keyboard_U] = 80,
        [LedId.Keyboard_I] = 81,
        [LedId.Keyboard_O] = 82,
        [LedId.Keyboard_P] = 83,
        [LedId.Keyboard_BracketLeft] = 84,
        [LedId.Keyboard_BracketRight] = 85,
        [LedId.Keyboard_Delete] = 88,
        [LedId.Keyboard_End] = 89,
        [LedId.Keyboard_PageDown] = 90,
        [LedId.Keyboard_Num7] = 91,
        [LedId.Keyboard_Num8] = 92,
        [LedId.Keyboard_Num9] = 93,
        [LedId.Keyboard_NumPlus] = 94,

        [LedId.Keyboard_CapsLock] = 97,
        [LedId.Keyboard_A] = 99,
        [LedId.Keyboard_S] = 100,
        [LedId.Keyboard_D] = 101,
        [LedId.Keyboard_F] = 102,
        [LedId.Keyboard_G] = 103,
        [LedId.Keyboard_H] = 104,
        [LedId.Keyboard_J] = 105,
        [LedId.Keyboard_K] = 106,
        [LedId.Keyboard_L] = 107,
        [LedId.Keyboard_SemicolonAndColon] = 108,
        [LedId.Keyboard_ApostropheAndDoubleQuote] = 109,
        [LedId.Keyboard_NonUsTilde] = 110,
        [LedId.Keyboard_Enter] = 111,
        [LedId.Keyboard_Num4] = 115,
        [LedId.Keyboard_Num5] = 116,
        [LedId.Keyboard_Num6] = 117,

        [LedId.Keyboard_LeftShift] = 121,
        [LedId.Keyboard_NonUsBackslash] = 122,
        [LedId.Keyboard_Z] = 123,
        [LedId.Keyboard_X] = 124,
        [LedId.Keyboard_C] = 125,
        [LedId.Keyboard_V] = 126,
        [LedId.Keyboard_B] = 127,
        [LedId.Keyboard_N] = 128,
        [LedId.Keyboard_M] = 129,
        [LedId.Keyboard_CommaAndLessThan] = 130,
        [LedId.Keyboard_PeriodAndBiggerThan] = 131,
        [LedId.Keyboard_SlashAndQuestionMark] = 132,
        [LedId.Keyboard_RightShift] = 134,
        [LedId.Keyboard_ArrowUp] = 137,
        [LedId.Keyboard_Num1] = 139,
        [LedId.Keyboard_Num2] = 140,
        [LedId.Keyboard_Num3] = 141,
        [LedId.Keyboard_NumEnter] = 142,

        [LedId.Keyboard_LeftCtrl] = 145,
        [LedId.Keyboard_LeftGui] = 146,
        [LedId.Keyboard_LeftAlt] = 147,
        [LedId.Keyboard_Space] = 151,
        [LedId.Keyboard_RightAlt] = 155,
        [LedId.Keyboard_Function] = 156,
        [LedId.Keyboard_Application] = 157,
        [LedId.Keyboard_RightCtrl] = 158,
        [LedId.Keyboard_ArrowLeft] = 159,
        [LedId.Keyboard_ArrowDown] = 160,
        [LedId.Keyboard_ArrowRight] = 161,
        [LedId.Keyboard_Num0] = 163,
        [LedId.Keyboard_NumPeriodAndDelete] = 165,
    };

    /// <summary>
    /// Gets the mapping for keyboards.
    /// </summary>
    public static LedMapping<int> Keyboard { get; } = new()
    {
        #region Row 0

        [LedId.LedStripe1] = (_Defines.KEYBOARD_MAX_COLUMN * 0) + 0,
        [LedId.LedStripe16] = (_Defines.KEYBOARD_MAX_COLUMN * 0) + 23,

        #endregion

        #region Row 1

        [LedId.LedStripe2] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 0,
        [LedId.Custom1] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 1,
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
        [LedId.Keyboard_MediaPreviousTrack] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 19,
        [LedId.Keyboard_MediaPlay] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 20,
        [LedId.Keyboard_MediaNextTrack] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 21,
        [LedId.Keyboard_MediaMute] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 22,
        [LedId.LedStripe15] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 23,

        #endregion

        #region Row 2

        [LedId.LedStripe3] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 0,
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
        [LedId.LedStripe14] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 23,

        #endregion

        #region Row 3

        [LedId.LedStripe4] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 0,
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
        [LedId.LedStripe13] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 23,

        #endregion

        #region Row 4

        [LedId.LedStripe5] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 0,
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
        [LedId.LedStripe12] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 23,

        #endregion

        #region Row 5

        [LedId.LedStripe6] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 0,
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
        [LedId.LedStripe11] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 23,

        #endregion

        #region Row 6

        [LedId.LedStripe7] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 0,
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
        [LedId.LedStripe9] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 19,
        [LedId.Keyboard_Num0] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 20,
        [LedId.Keyboard_NumPeriodAndDelete] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 21,
        [LedId.LedStripe10] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 23,

        #endregion

        #region Row 6

        [LedId.LedStripe8] = (_Defines.KEYBOARD_MAX_COLUMN * 7) + 0,

        #endregion
    };


    /// <summary>
    /// Gets the mapping for Blade keyboards.
    /// </summary>
    public static LedMapping<int> Blade { get; } = new()
    {
        //Row 0 is empty

        #region Row 1

        [LedId.Keyboard_Escape] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 2,
        [LedId.Keyboard_F1] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 3,
        [LedId.Keyboard_F2] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 4,
        [LedId.Keyboard_F3] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 5,
        [LedId.Keyboard_F4] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 6,
        [LedId.Keyboard_F5] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 7,
        [LedId.Keyboard_F6] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 8,
        [LedId.Keyboard_F7] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 9,
        [LedId.Keyboard_F8] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 10,
        [LedId.Keyboard_F9] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 11,
        [LedId.Keyboard_F10] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 12,
        [LedId.Keyboard_F11] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 13,
        [LedId.Keyboard_F12] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 14,
        [LedId.Keyboard_PrintScreen] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 15,
        [LedId.Keyboard_ScrollLock] = (_Defines.KEYBOARD_MAX_COLUMN * 1) + 16,

        #endregion

        #region Row 2

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
        [LedId.Keyboard_Backspace] = (_Defines.KEYBOARD_MAX_COLUMN * 2) + 16,

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
        [LedId.Keyboard_Backslash] = (_Defines.KEYBOARD_MAX_COLUMN * 3) + 16,

        #endregion

        #region Row 4

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
        [LedId.Keyboard_Enter] = (_Defines.KEYBOARD_MAX_COLUMN * 4) + 16,

        #endregion

        #region Row 5

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
        [LedId.Keyboard_RightShift] = (_Defines.KEYBOARD_MAX_COLUMN * 5) + 16,

        #endregion

        #region Row 6

        [LedId.Keyboard_LeftCtrl] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 2,
        // left gui lights left fn key
        [LedId.Keyboard_LeftGui] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 3,
        // left alt lights left gui key
        [LedId.Keyboard_LeftAlt] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 4,
        // space lights left alt key
        [LedId.Keyboard_Space] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 6,
        // see comment bellow
        [LedId.Keyboard_RightAlt] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 10,
        // right gui lights right fn key (this one (11) seems to light both right alt and right fn, but 8, 9 and 10 don't light anything)
        [LedId.Keyboard_RightGui] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 11,
        // application key lights right control to make room for up/down keys stacked in 1 key spot
        [LedId.Keyboard_Application] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 12,
        // right ctrl lights left arrow key
        [LedId.Keyboard_RightCtrl] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 13,
        // arrow left lights up arrow key
        [LedId.Keyboard_ArrowLeft] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 14,
        // these arrows appear to be flipped
        [LedId.Keyboard_ArrowDown] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 16,
        [LedId.Keyboard_ArrowRight] = (_Defines.KEYBOARD_MAX_COLUMN * 6) + 15,

        #endregion

        //Row 7 is also empty
    };

    /// <summary>
    /// Gets the mapping for mice.
    /// </summary>
    /// <remarks>
    /// The order of the ids is a bit messed up since the officially documented locations are 1-21 and 22 is know to be used even if it's not documented.
    /// 23-63 is there to not run into issues in the future.
    /// </remarks>
    public static LedMapping<int> Mouse { get; } = new()
    {
        //row 0 empty
        [LedId.Mouse24] = (_Defines.MOUSE_MAX_COLUMN * 0) + 0,
        [LedId.Mouse25] = (_Defines.MOUSE_MAX_COLUMN * 0) + 1,
        [LedId.Mouse26] = (_Defines.MOUSE_MAX_COLUMN * 0) + 2,
        [LedId.Mouse23] = (_Defines.MOUSE_MAX_COLUMN * 0) + 3,
        [LedId.Mouse27] = (_Defines.MOUSE_MAX_COLUMN * 0) + 4,
        [LedId.Mouse28] = (_Defines.MOUSE_MAX_COLUMN * 0) + 5,
        [LedId.Mouse29] = (_Defines.MOUSE_MAX_COLUMN * 0) + 6,

        //row 1
        [LedId.Mouse1] = (_Defines.MOUSE_MAX_COLUMN * 1) + 0,
        [LedId.Mouse30] = (_Defines.MOUSE_MAX_COLUMN * 1) + 1,
        [LedId.Mouse31] = (_Defines.MOUSE_MAX_COLUMN * 1) + 2,
        [LedId.Mouse32] = (_Defines.MOUSE_MAX_COLUMN * 1) + 3,
        [LedId.Mouse33] = (_Defines.MOUSE_MAX_COLUMN * 1) + 4,
        [LedId.Mouse34] = (_Defines.MOUSE_MAX_COLUMN * 1) + 5,
        [LedId.Mouse2] = (_Defines.MOUSE_MAX_COLUMN * 1) + 6,

        //row 2
        [LedId.Mouse3] = (_Defines.MOUSE_MAX_COLUMN * 2) + 0,
        [LedId.Mouse35] = (_Defines.MOUSE_MAX_COLUMN * 2) + 1,
        [LedId.Mouse36] = (_Defines.MOUSE_MAX_COLUMN * 2) + 2,
        [LedId.Mouse4] = (_Defines.MOUSE_MAX_COLUMN * 2) + 3,
        [LedId.Mouse37] = (_Defines.MOUSE_MAX_COLUMN * 2) + 4,
        [LedId.Mouse38] = (_Defines.MOUSE_MAX_COLUMN * 2) + 5,
        [LedId.Mouse5] = (_Defines.MOUSE_MAX_COLUMN * 2) + 6,

        //row 3
        [LedId.Mouse6] = (_Defines.MOUSE_MAX_COLUMN * 3) + 0,
        [LedId.Mouse39] = (_Defines.MOUSE_MAX_COLUMN * 3) + 1,
        [LedId.Mouse40] = (_Defines.MOUSE_MAX_COLUMN * 3) + 2,
        [LedId.Mouse41] = (_Defines.MOUSE_MAX_COLUMN * 3) + 3,
        [LedId.Mouse42] = (_Defines.MOUSE_MAX_COLUMN * 3) + 4,
        [LedId.Mouse43] = (_Defines.MOUSE_MAX_COLUMN * 3) + 5,
        [LedId.Mouse7] = (_Defines.MOUSE_MAX_COLUMN * 3) + 6,

        //row 4
        [LedId.Mouse8] = (_Defines.MOUSE_MAX_COLUMN * 4) + 0,
        [LedId.Mouse44] = (_Defines.MOUSE_MAX_COLUMN * 4) + 1,
        [LedId.Mouse45] = (_Defines.MOUSE_MAX_COLUMN * 4) + 2,
        [LedId.Mouse9] = (_Defines.MOUSE_MAX_COLUMN * 4) + 3,
        [LedId.Mouse46] = (_Defines.MOUSE_MAX_COLUMN * 4) + 4,
        [LedId.Mouse47] = (_Defines.MOUSE_MAX_COLUMN * 4) + 5,
        [LedId.Mouse10] = (_Defines.MOUSE_MAX_COLUMN * 4) + 6,

        //row 5
        [LedId.Mouse11] = (_Defines.MOUSE_MAX_COLUMN * 5) + 0,
        [LedId.Mouse48] = (_Defines.MOUSE_MAX_COLUMN * 5) + 1,
        [LedId.Mouse49] = (_Defines.MOUSE_MAX_COLUMN * 5) + 2,
        [LedId.Mouse50] = (_Defines.MOUSE_MAX_COLUMN * 5) + 3,
        [LedId.Mouse51] = (_Defines.MOUSE_MAX_COLUMN * 5) + 4,
        [LedId.Mouse52] = (_Defines.MOUSE_MAX_COLUMN * 5) + 5,
        [LedId.Mouse12] = (_Defines.MOUSE_MAX_COLUMN * 5) + 6,

        //row 6
        [LedId.Mouse13] = (_Defines.MOUSE_MAX_COLUMN * 6) + 0,
        [LedId.Mouse53] = (_Defines.MOUSE_MAX_COLUMN * 6) + 1,
        [LedId.Mouse54] = (_Defines.MOUSE_MAX_COLUMN * 6) + 2,
        [LedId.Mouse55] = (_Defines.MOUSE_MAX_COLUMN * 6) + 3,
        [LedId.Mouse56] = (_Defines.MOUSE_MAX_COLUMN * 6) + 4,
        [LedId.Mouse57] = (_Defines.MOUSE_MAX_COLUMN * 6) + 5,
        [LedId.Mouse14] = (_Defines.MOUSE_MAX_COLUMN * 6) + 6,

        //row 7
        [LedId.Mouse15] = (_Defines.MOUSE_MAX_COLUMN * 7) + 0,
        [LedId.Mouse58] = (_Defines.MOUSE_MAX_COLUMN * 7) + 1,
        [LedId.Mouse59] = (_Defines.MOUSE_MAX_COLUMN * 7) + 2,
        [LedId.Mouse16] = (_Defines.MOUSE_MAX_COLUMN * 7) + 3,
        [LedId.Mouse60] = (_Defines.MOUSE_MAX_COLUMN * 7) + 4,
        [LedId.Mouse61] = (_Defines.MOUSE_MAX_COLUMN * 7) + 5,
        [LedId.Mouse17] = (_Defines.MOUSE_MAX_COLUMN * 7) + 6,

        //row 8
        [LedId.Mouse62] = (_Defines.MOUSE_MAX_COLUMN * 8) + 0,
        [LedId.Mouse18] = (_Defines.MOUSE_MAX_COLUMN * 8) + 1,
        [LedId.Mouse19] = (_Defines.MOUSE_MAX_COLUMN * 8) + 2,
        [LedId.Mouse20] = (_Defines.MOUSE_MAX_COLUMN * 8) + 3,
        [LedId.Mouse21] = (_Defines.MOUSE_MAX_COLUMN * 8) + 4,
        [LedId.Mouse22] = (_Defines.MOUSE_MAX_COLUMN * 8) + 5,
        [LedId.Mouse63] = (_Defines.MOUSE_MAX_COLUMN * 8) + 6,
    };

    //TODO DarthAffe 27.04.2021: Are mappings for these possible?
    /// <summary>
    /// Gets the mapping for mousepads.
    /// </summary>
    public static LedMapping<int> Mousepad { get; } = [];

    /// <summary>
    /// Gets the mapping for headsets.
    /// </summary>
    public static LedMapping<int> Headset { get; } = [];

    /// <summary>
    /// Gets the mapping for keypads.
    /// </summary>
    public static LedMapping<int> Keypad { get; } = [];

    /// <summary>
    /// Gets the mapping for chroma link devices.
    /// </summary>
    public static LedMapping<int> ChromaLink { get; } = [];
}
