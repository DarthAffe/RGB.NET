using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Razer.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _MousepadCustomEffect
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = _Defines.MOUSEPAD_MAX_LEDS)]
        public _Color[] Color;
    }
}
