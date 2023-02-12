#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
/// <summary>
/// iCUE-SDK: contains information about device property type and value
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct _CorsairProperty
{
    #region Properties & Fields

    /// <summary>
    /// iCUE-SDK: type of property
    /// </summary>
    internal CorsairDataType type;

    /// <summary>
    /// iCUE-SDK: property value
    /// </summary>
    internal _CorsairDataValue value;

    #endregion
}
