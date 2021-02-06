using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech
{
    internal static class BitmapMapping
    {
        #region Constants

        private const int BITMAP_SIZE = 21 * 6 * 4;

        #endregion

        #region Properties & Fields

        internal static Dictionary<LedId, int> BitmapOffset { get; } = new()
                                                                       {
            { LedId.Keyboard_Escape, 0 },
            { LedId.Keyboard_F1, 4 },
            { LedId.Keyboard_F2, 8 },
            { LedId.Keyboard_F3, 12 },
            { LedId.Keyboard_F4, 16 },
            { LedId.Keyboard_F5, 20 },
            { LedId.Keyboard_F6, 24 },
            { LedId.Keyboard_F7, 28 },
            { LedId.Keyboard_F8, 32 },
            { LedId.Keyboard_F9, 36 },
            { LedId.Keyboard_F10, 40 },
            { LedId.Keyboard_F11, 44 },
            { LedId.Keyboard_F12, 48 },
            { LedId.Keyboard_PrintScreen, 52 },
            { LedId.Keyboard_ScrollLock, 56 },
            { LedId.Keyboard_PauseBreak, 60 },
            // { LedId.Keyboard_?, 64 },
            // { LedId.Keyboard_?, 68 },
            // { LedId.Keyboard_?, 72 },
            // { LedId.Keyboard_?, 76 },
            // { LedId.Keyboard_?, 80 },

            { LedId.Keyboard_GraveAccentAndTilde, 84 },
            { LedId.Keyboard_1, 88 },
            { LedId.Keyboard_2, 92 },
            { LedId.Keyboard_3, 96 },
            { LedId.Keyboard_4, 100 },
            { LedId.Keyboard_5, 104 },
            { LedId.Keyboard_6, 108 },
            { LedId.Keyboard_7, 112 },
            { LedId.Keyboard_8, 116 },
            { LedId.Keyboard_9, 120 },
            { LedId.Keyboard_0, 124 },
            { LedId.Keyboard_MinusAndUnderscore, 128 },
            { LedId.Keyboard_EqualsAndPlus, 132 },
            { LedId.Keyboard_Backspace, 136 },
            { LedId.Keyboard_Insert, 140 },
            { LedId.Keyboard_Home, 144 },
            { LedId.Keyboard_PageUp, 148 },
            { LedId.Keyboard_NumLock, 152 },
            { LedId.Keyboard_NumSlash, 156 },
            { LedId.Keyboard_NumAsterisk, 160 },
            { LedId.Keyboard_NumMinus, 164 },

            { LedId.Keyboard_Tab, 168 },
            { LedId.Keyboard_Q, 172 },
            { LedId.Keyboard_W, 176 },
            { LedId.Keyboard_E, 180 },
            { LedId.Keyboard_R, 184 },
            { LedId.Keyboard_T, 188 },
            { LedId.Keyboard_Y, 192 },
            { LedId.Keyboard_U, 196 },
            { LedId.Keyboard_I, 200 },
            { LedId.Keyboard_O, 204 },
            { LedId.Keyboard_P, 208 },
            { LedId.Keyboard_BracketLeft, 212 },
            { LedId.Keyboard_BracketRight, 216 },
            // { LedId.Keyboard_?, 220 },
            { LedId.Keyboard_Delete, 224 },
            { LedId.Keyboard_End, 228 },
            { LedId.Keyboard_PageDown, 232 },
            { LedId.Keyboard_Num7, 236 },
            { LedId.Keyboard_Num8, 240 },
            { LedId.Keyboard_Num9, 244 },
            { LedId.Keyboard_NumPlus, 248 },

            { LedId.Keyboard_CapsLock, 252 },
            { LedId.Keyboard_A, 256 },
            { LedId.Keyboard_S, 260 },
            { LedId.Keyboard_D, 264 },
            { LedId.Keyboard_F, 268 },
            { LedId.Keyboard_G, 272 },
            { LedId.Keyboard_H, 276 },
            { LedId.Keyboard_J, 280 },
            { LedId.Keyboard_K, 284 },
            { LedId.Keyboard_L, 288 },
            { LedId.Keyboard_SemicolonAndColon, 292 },
            { LedId.Keyboard_ApostropheAndDoubleQuote, 296 },
            { LedId.Keyboard_NonUsTilde, 300 }, //TODO DarthAffe 26.03.2017: Find the real ID/Name of this key - it's not documented ...
            { LedId.Keyboard_Enter, 304 },
            // { LedId.Keyboard_?, 308 },
            // { LedId.Keyboard_?, 312 },
            // { LedId.Keyboard_?, 316 },
            { LedId.Keyboard_Num4, 320 },
            { LedId.Keyboard_Num5, 324 },
            { LedId.Keyboard_Num6, 328 },
            // { LedId.Keyboard_?, 332 },

            { LedId.Keyboard_LeftShift, 336 },
            { LedId.Keyboard_Backslash, 340 },
            { LedId.Keyboard_Z, 344 },
            { LedId.Keyboard_X, 348 },
            { LedId.Keyboard_C, 352 },
            { LedId.Keyboard_V, 356 },
            { LedId.Keyboard_B, 360 },
            { LedId.Keyboard_N, 364 },
            { LedId.Keyboard_M, 368 },
            { LedId.Keyboard_CommaAndLessThan, 372 },
            { LedId.Keyboard_PeriodAndBiggerThan, 376 },
            { LedId.Keyboard_SlashAndQuestionMark, 380 },
            { LedId.Keyboard_RightShift, 388 },
            // { LedId.Keyboard_?, 392 },
            { LedId.Keyboard_ArrowUp, 396 },
            // { LedId.Keyboard_?, 400 },
            { LedId.Keyboard_Num1, 404 },
            { LedId.Keyboard_Num2, 408 },
            { LedId.Keyboard_Num3, 412 },
            { LedId.Keyboard_NumEnter, 416 },

            { LedId.Keyboard_LeftCtrl, 420 },
            { LedId.Keyboard_LeftGui, 424 },
            { LedId.Keyboard_LeftAlt, 428 },
            // { LedId.Keyboard_?, 432 },
            // { LedId.Keyboard_?, 436 },
            { LedId.Keyboard_Space, 440 },
            // { LedId.Keyboard_?, 444 },
            // { LedId.Keyboard_?, 448 },
            // { LedId.Keyboard_?, 452 },
            // { LedId.Keyboard_?, 456 },
            // { LedId.Keyboard_?, 460 },
            { LedId.Keyboard_RightAlt, 464 },
            { LedId.Keyboard_RightGui, 468 },
            { LedId.Keyboard_Application, 472 },
            { LedId.Keyboard_RightCtrl, 476 },
            { LedId.Keyboard_ArrowLeft, 480 },
            { LedId.Keyboard_ArrowDown, 484 },
            { LedId.Keyboard_ArrowRight, 488 },
            { LedId.Keyboard_Num0, 492 },
            { LedId.Keyboard_NumPeriodAndDelete, 496 },
            // { LedId.Keyboard_?, 500 },
        };

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte[] CreateBitmap() => new byte[BITMAP_SIZE];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SetColor(byte[] bitmap, int offset, Color color)
        {
            bitmap[offset] = color.GetB();
            bitmap[offset + 1] = color.GetG();
            bitmap[offset + 2] = color.GetR();
            bitmap[offset + 3] = color.GetA();
        }

        #endregion
    }
}
