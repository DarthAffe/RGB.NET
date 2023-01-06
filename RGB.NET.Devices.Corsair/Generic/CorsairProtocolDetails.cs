// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;
using System.Runtime.InteropServices;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <summary>
/// Managed wrapper for CorsairProtocolDetails.
/// </summary>
public class CorsairProtocolDetails
{
    #region Properties & Fields

    /// <summary>
    /// String containing version of SDK(like "1.0.0.1").
    /// Always contains valid value even if there was no CUE found.
    /// </summary>
    public string? SdkVersion { get; }

    /// <summary>
    /// String containing version of CUE(like "1.0.0.1") or NULL if CUE was not found.
    /// </summary>
    public string? ServerVersion { get; }

    /// <summary>
    /// Integer that specifies version of protocol that is implemented by current SDK.
    /// Numbering starts from 1.
    /// Always contains valid value even if there was no CUE found.
    /// </summary>
    public int SdkProtocolVersion { get; }

    /// <summary>
    /// Integer that specifies version of protocol that is implemented by CUE.
    /// Numbering starts from 1.
    /// If CUE was not found then this value will be 0.
    /// </summary>
    public int ServerProtocolVersion { get; }

    /// <summary>
    /// Boolean that specifies if there were breaking changes between version of protocol implemented by server and client.
    /// </summary>
    public bool BreakingChanges { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Internal constructor of managed CorsairProtocolDetails.
    /// </summary>
    /// <param name="nativeDetails">The native CorsairProtocolDetails-struct</param>
    internal CorsairProtocolDetails(_CorsairProtocolDetails nativeDetails)
    {
        this.SdkVersion = nativeDetails.sdkVersion == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(nativeDetails.sdkVersion);
        this.ServerVersion = nativeDetails.serverVersion == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(nativeDetails.serverVersion);
        this.SdkProtocolVersion = nativeDetails.sdkProtocolVersion;
        this.ServerProtocolVersion = nativeDetails.serverProtocolVersion;
        this.BreakingChanges = nativeDetails.breakingChanges != 0;
    }

    #endregion
}