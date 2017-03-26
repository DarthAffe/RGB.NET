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

        internal static Dictionary<LogitechLedIds, int> BitmapOffset { get; } = new Dictionary<LogitechLedIds, int>
        {
            { LogitechLedIds.ESC, 0 },
            { LogitechLedIds.F1, 4 },
            { LogitechLedIds.F2, 8 },
            { LogitechLedIds.F3, 12 },
            { LogitechLedIds.F4, 16 },
            { LogitechLedIds.F5, 20 },
            { LogitechLedIds.F6, 24 },
            { LogitechLedIds.F7, 28 },
            { LogitechLedIds.F8, 32 },
            { LogitechLedIds.F9, 36 },
            { LogitechLedIds.F10, 40 },
            { LogitechLedIds.F11, 44 },
            { LogitechLedIds.F12, 48 },
            { LogitechLedIds.PRINT_SCREEN, 52 },
            { LogitechLedIds.SCROLL_LOCK, 56 },
            { LogitechLedIds.PAUSE_BREAK, 60 },
            // { LogitechLedIds.?, 64 },
            // { LogitechLedIds.?, 68 },
            // { LogitechLedIds.?, 72 },
            // { LogitechLedIds.?, 76 },
            // { LogitechLedIds.?, 80 },

            { LogitechLedIds.TILDE, 84 },
            { LogitechLedIds.ONE, 88 },
            { LogitechLedIds.TWO, 92 },
            { LogitechLedIds.THREE, 96 },
            { LogitechLedIds.FOUR, 100 },
            { LogitechLedIds.FIVE, 104 },
            { LogitechLedIds.SIX, 108 },
            { LogitechLedIds.SEVEN, 112 },
            { LogitechLedIds.EIGHT, 116 },
            { LogitechLedIds.NINE, 120 },
            { LogitechLedIds.ZERO, 124 },
            { LogitechLedIds.MINUS, 128 },
            { LogitechLedIds.EQUALS, 132 },
            { LogitechLedIds.BACKSPACE, 136 },
            { LogitechLedIds.INSERT, 140 },
            { LogitechLedIds.HOME, 144 },
            { LogitechLedIds.PAGE_UP, 148 },
            { LogitechLedIds.NUM_LOCK, 152 },
            { LogitechLedIds.NUM_SLASH, 156 },
            { LogitechLedIds.NUM_ASTERISK, 160 },
            { LogitechLedIds.NUM_MINUS, 164 },

            { LogitechLedIds.TAB, 168 },
            { LogitechLedIds.Q, 172 },
            { LogitechLedIds.W, 176 },
            { LogitechLedIds.E, 180 },
            { LogitechLedIds.R, 184 },
            { LogitechLedIds.T, 188 },
            { LogitechLedIds.Y, 192 },
            { LogitechLedIds.U, 196 },
            { LogitechLedIds.I, 200 },
            { LogitechLedIds.O, 204 },
            { LogitechLedIds.P, 208 },
            { LogitechLedIds.OPEN_BRACKET, 212 },
            { LogitechLedIds.CLOSE_BRACKET, 216 },
            // { LogitechLedIds.?, 220 },
            { LogitechLedIds.KEYBOARD_DELETE, 224 },
            { LogitechLedIds.END, 228 },
            { LogitechLedIds.PAGE_DOWN, 232 },
            { LogitechLedIds.NUM_SEVEN, 236 },
            { LogitechLedIds.NUM_EIGHT, 240 },
            { LogitechLedIds.NUM_NINE, 244 },
            { LogitechLedIds.NUM_PLUS, 248 },

            { LogitechLedIds.CAPS_LOCK, 252 },
            { LogitechLedIds.A, 256 },
            { LogitechLedIds.S, 260 },
            { LogitechLedIds.D, 264 },
            { LogitechLedIds.F, 268 },
            { LogitechLedIds.G, 272 },
            { LogitechLedIds.H, 276 },
            { LogitechLedIds.J, 280 },
            { LogitechLedIds.K, 284 },
            { LogitechLedIds.L, 288 },
            { LogitechLedIds.SEMICOLON, 292 },
            { LogitechLedIds.APOSTROPHE, 296 },
            { LogitechLedIds.NonUsTilde, 300 }, //TODO DarthAffe 26.03.2017: Find the real ID/Name of this key - it's not documented ...
            { LogitechLedIds.ENTER, 304 },
            // { LogitechLedIds.?, 308 },
            // { LogitechLedIds.?, 312 },
            // { LogitechLedIds.?, 316 },
            { LogitechLedIds.NUM_FOUR, 320 },
            { LogitechLedIds.NUM_FIVE, 324 },
            { LogitechLedIds.NUM_SIX, 328 },
            // { LogitechLedIds.?, 332 },

            { LogitechLedIds.LEFT_SHIFT, 336 },
            { LogitechLedIds.BACKSLASH, 340 },
            { LogitechLedIds.Z, 344 },
            { LogitechLedIds.X, 348 },
            { LogitechLedIds.C, 352 },
            { LogitechLedIds.V, 356 },
            { LogitechLedIds.B, 360 },
            { LogitechLedIds.N, 364 },
            { LogitechLedIds.M, 368 },
            { LogitechLedIds.COMMA, 372 },
            { LogitechLedIds.PERIOD, 376 },
            { LogitechLedIds.FORWARD_SLASH, 380 },
            { LogitechLedIds.RIGHT_SHIFT, 388 },
            // { LogitechLedIds.?, 392 },
            { LogitechLedIds.ARROW_UP, 396 },
            // { LogitechLedIds.?, 400 },
            { LogitechLedIds.NUM_ONE, 404 },
            { LogitechLedIds.NUM_TWO, 408 },
            { LogitechLedIds.NUM_THREE, 412 },
            { LogitechLedIds.NUM_ENTER, 416 },

            { LogitechLedIds.LEFT_CONTROL, 420 },
            { LogitechLedIds.LEFT_WINDOWS, 424 },
            { LogitechLedIds.LEFT_ALT, 428 },
            // { LogitechLedIds.?, 432 },
            // { LogitechLedIds.?, 436 },
            { LogitechLedIds.SPACE, 440 },
            // { LogitechLedIds.?, 444 },
            // { LogitechLedIds.?, 448 },
            // { LogitechLedIds.?, 452 },
            // { LogitechLedIds.?, 456 },
            // { LogitechLedIds.?, 460 },
            { LogitechLedIds.RIGHT_ALT, 464 },
            { LogitechLedIds.RIGHT_WINDOWS, 468 },
            { LogitechLedIds.APPLICATION_SELECT, 472 },
            { LogitechLedIds.RIGHT_CONTROL, 476 },
            { LogitechLedIds.ARROW_LEFT, 480 },
            { LogitechLedIds.ARROW_DOWN, 484 },
            { LogitechLedIds.ARROW_RIGHT, 488 },
            { LogitechLedIds.NUM_ZERO, 492 },
            { LogitechLedIds.NUM_PERIOD, 496 },
            // { LogitechLedIds.?, 500 },
        };

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte[] CreateBitmap()
        {
            return new byte[BITMAP_SIZE];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SetColor(ref byte[] bitmap, int offset, Color color)
        {
            bitmap[offset] = color.B;
            bitmap[offset + 1] = color.G;
            bitmap[offset + 2] = color.R;
            bitmap[offset + 3] = color.A;
        }

        #endregion
    }
}
