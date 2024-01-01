#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
// ReSharper disable MemberCanBePrivate.Global

using System.Runtime.InteropServices;
using RGB.NET.Core;

namespace RGB.NET.Devices.Razer.Native;

// ReSharper disable once InconsistentNaming
[StructLayout(LayoutKind.Sequential, Size = sizeof(uint))]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Native-naming")]
internal struct _Color(byte red, byte green, byte blue)
{
    #region Properties & Fields

    public uint Color = red + ((uint)green << 8) + ((uint)blue << 16);

    #endregion

    #region Constructors

    public _Color(Color color)
        : this(color.GetR(), color.GetG(), color.GetB()) { }

    #endregion
}