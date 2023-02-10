#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable NotAccessedField.Global

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming    
/// <summary>
/// iCUE-SDK: contains information about SDK and iCUE versions
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal class _CorsairSessionDetails
{
    /// <summary>
    /// iCUE-SDK: version of SDK client (like {4,0,1}). Always contains valid value even if there was no iCUE found. Must comply with the semantic versioning rules.
    /// </summary>
    internal _CorsairVersion clientVersion = new();

    /// <summary>
    /// iCUE-SDK: version of SDK server (like {4,0,1}) or empty struct ({0,0,0}) if the iCUE was not found. Must comply with the semantic versioning rules.
    /// </summary>
    internal _CorsairVersion serverVersion = new();

    /// <summary>
    /// iCUE-SDK: version of iCUE (like {3,33,100}) or empty struct ({0,0,0}) if the iCUE was not found.
    /// </summary>
    internal _CorsairVersion serverHostVersion = new();
};
