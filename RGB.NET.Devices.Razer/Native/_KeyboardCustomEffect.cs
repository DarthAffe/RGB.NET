using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Razer.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _KeyboardCustomEffect
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = _Defines.KEYBOARD_MAX_LEDS)]
        public _Color[] Color;
    }
}
