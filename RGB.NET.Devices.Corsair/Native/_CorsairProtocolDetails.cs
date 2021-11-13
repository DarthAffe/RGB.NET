#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned

using System;
using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
/// <summary>
/// CUE-SDK: contains information about SDK and CUE versions
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct _CorsairProtocolDetails
{
    /// <summary>
    /// CUE-SDK: null - terminated string containing version of SDK(like “1.0.0.1”). Always contains valid value even if there was no CUE found
    /// </summary>
    internal IntPtr sdkVersion;

    /// <summary>
    /// CUE-SDK: null - terminated string containing version of CUE(like “1.0.0.1”) or NULL if CUE was not found.
    /// </summary>
    internal IntPtr serverVersion;

    /// <summary>
    /// CUE-SDK: integer number that specifies version of protocol that is implemented by current SDK.
    /// Numbering starts from 1. Always contains valid value even if there was no CUE found
    /// </summary>
    internal int sdkProtocolVersion;

    /// <summary>
    /// CUE-SDK: integer number that specifies version of protocol that is implemented by CUE.
    /// Numbering starts from 1. If CUE was not found then this value will be 0
    /// </summary>
    internal int serverProtocolVersion;

    /// <summary>
    /// CUE-SDK: boolean value that specifies if there were breaking changes between version of protocol implemented by server and client
    /// </summary>
    internal byte breakingChanges;
};