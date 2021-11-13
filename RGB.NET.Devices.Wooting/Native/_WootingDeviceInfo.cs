#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;
using RGB.NET.Devices.Wooting.Enum;

namespace RGB.NET.Devices.Wooting.Native;

[StructLayout(LayoutKind.Sequential)]
internal struct _WootingDeviceInfo
{
    internal bool Connected { get; private set; }

    internal string Model { get; private set; }

    internal byte MaxRows { get; private set; }

    internal byte MaxColumns { get; private set; }

    internal byte KeycodeLimit { get; private set; }

    internal WootingDeviceType DeviceType { get; private set; }

    internal bool V2Interface { get; set; }

    internal WootingLayoutType LayoutType { get; private set; }
}