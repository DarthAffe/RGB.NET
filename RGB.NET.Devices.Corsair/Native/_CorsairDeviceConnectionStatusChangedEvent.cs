using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

[StructLayout(LayoutKind.Sequential)]
public class _CorsairDeviceConnectionStatusChangedEvent
{
    /// <summary>
    /// iCUE-SDK: null terminated Unicode string that contains unique device identifier
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = _CUESDK.CORSAIR_STRING_SIZE_M)]
    internal string? deviceId;

    /// <summary>
    /// iCUE-SDK: true if connected, false if disconnected
    /// </summary>
    [MarshalAs(UnmanagedType.U1)]
    internal bool isConnected;
}
