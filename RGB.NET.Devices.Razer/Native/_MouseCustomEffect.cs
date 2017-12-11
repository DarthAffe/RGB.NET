using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Razer.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _MouseCustomEffect
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = _Defines.MOUSE_MAX_LEDS)]
        public _Color[] Color;
    }
}
