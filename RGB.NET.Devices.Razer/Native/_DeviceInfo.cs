#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
// ReSharper disable MemberCanBePrivate.Global

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Razer.Native;

// ReSharper disable once InconsistentNaming
/// <summary>
/// Razer-SDK: Device info.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct _DeviceInfo
{
    /// <summary>
    /// Razer-SDK: Device types.
    /// </summary>
    internal DeviceType Type;

    /// <summary>
    /// Razer-SDK: Number of devices connected.
    /// </summary>
    internal int Connected;
}