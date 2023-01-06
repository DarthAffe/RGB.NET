#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Razer.Native;

[StructLayout(LayoutKind.Sequential)]
internal struct _KeyboardCustomEffect
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = _Defines.KEYBOARD_MAX_LEDS)]
    public _Color[] Color;

    //diogotr7: I don't know what these "keys" mean, they were introduced in the Keyboard::v2
    //namespace. we need to put them here to give razer the struct size it expects
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = _Defines.KEYBOARD_MAX_LEDS)]
    public uint[] Key;
}