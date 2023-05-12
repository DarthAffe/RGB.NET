#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable NotAccessedField.Global

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming    
/// <summary>
/// iCUE-SDK: contains information about session state and client/server versions
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal sealed class _CorsairSessionStateChanged
{
    /// <summary>
    /// iCUE-SDK: new session state which SDK client has been transitioned to
    /// </summary>
    internal CorsairSessionState state;

    /// <summary>
    /// iCUE-SDK: information about client/server versions
    /// </summary>
    internal _CorsairSessionDetails details = new();
};
