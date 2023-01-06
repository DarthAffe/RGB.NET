#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Razer.Native;

[StructLayout(LayoutKind.Sequential)]
internal struct _KeypadCustomEffect
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = _Defines.KEYPAD_MAX_LEDS)]
    public _Color[] Color;
}