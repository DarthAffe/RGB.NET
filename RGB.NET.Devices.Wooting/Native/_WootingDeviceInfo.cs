using System.Runtime.InteropServices;
using RGB.NET.Devices.Wooting.Enum;

namespace RGB.NET.Devices.Wooting.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct _WootingDeviceInfo
    {
        public bool Connected { get; private set; }

        public string Model { get; private set; }

        public byte MaxRows { get; private set; }

        public byte MaxColumns { get; private set; }

        public byte KeycodeLimit { get; private set; }

        public WootingDeviceType DeviceType { get; private set; }
    }
}
