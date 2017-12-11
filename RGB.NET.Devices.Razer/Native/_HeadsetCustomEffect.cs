using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Razer.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _HeadsetCustomEffect
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = _Defines.HEADSET_MAX_LEDS)]
        public _Color[] Color;
    }
}
