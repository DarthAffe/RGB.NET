#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable NotAccessedField.Global

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming    
/// <summary>
/// iCUE-SDK: contains information about version that consists of three components
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal sealed class _CorsairVersion
{
    #region Properties & Fields

    internal int major;
    internal int minor;
    internal int patch;

    #endregion

    #region Methods

    public override string ToString() => $"{major}.{minor}.{patch}";

    #endregion
};
