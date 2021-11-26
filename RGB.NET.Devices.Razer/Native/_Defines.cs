#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

namespace RGB.NET.Devices.Razer.Native;

internal static class _Defines
{
    internal const int EFFECT_ID = 7;
    internal const int HEADSET_EFFECT_ID = 4;
    internal const int CHROMALINK_EFFECT_ID = 1;
    internal const int KEYBOARD_EFFECT_ID = 9;
    internal const int MOUSEPAD_EFFECT_ID = 6;
    internal const int MOUSE_EFFECT_ID = 8;
    internal const int KEYPAD_EFFECT_ID = 2;

    internal const int KEYBOARD_MAX_ROW = 8;
    internal const int KEYBOARD_MAX_COLUMN = 24;
    internal const int KEYBOARD_MAX_LEDS = KEYBOARD_MAX_ROW * KEYBOARD_MAX_COLUMN;

    internal const int MOUSE_MAX_ROW = 9;
    internal const int MOUSE_MAX_COLUMN = 7;
    internal const int MOUSE_MAX_LEDS = MOUSE_MAX_ROW * MOUSE_MAX_COLUMN;

    internal const int HEADSET_MAX_LEDS = 5;

    internal const int MOUSEPAD_MAX_LEDS = 20;

    internal const int KEYPAD_MAX_ROW = 4;
    internal const int KEYPAD_MAX_COLUMN = 5;
    internal const int KEYPAD_MAX_LEDS = KEYPAD_MAX_ROW * KEYPAD_MAX_COLUMN;

    internal const int CHROMALINK_MAX_LEDS = 5;
}