#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned
#pragma warning disable IDE1006 // Naming Styles

using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
/// <summary>
/// iCUE-SDK: a union of all property data types
/// </summary>
[StructLayout(LayoutKind.Explicit)]
internal struct _CorsairDataValue
{
    #region Properties & Fields

    /// <summary>
    /// iCUE-SDK: actual property value if it’s type is CPDT_Boolean
    /// </summary>
    [FieldOffset(0)]
    internal bool boolean;

    /// <summary>
    /// iCUE-SDK: actual property value if it’s type is CPDT_Int32
    /// </summary>
    [FieldOffset(0)]
    internal int int32;

    /// <summary>
    /// iCUE-SDK: actual property value if it’s type is CPDT_Float64
    /// </summary>
    [FieldOffset(0)]
    internal double float64;

    /// <summary>
    /// iCUE-SDK: actual property value if it’s type is CPDT_String
    /// </summary>
    [FieldOffset(0)]
    internal nint @string;

    /// <summary>
    /// iCUE-SDK: actual property value if it’s type is CPDT_Boolean_Array
    /// </summary>
    [FieldOffset(0)]
    internal CorsairDataTypeBooleanArray booleanArray;

    /// <summary>
    /// iCUE-SDK: actual property value if it’s type is CPDT_Int32_Array
    /// </summary>
    [FieldOffset(0)]
    internal CorsairDataTypeInt32Array int32Array;

    /// <summary>
    /// iCUE-SDK: actual property value if it’s type is CPDT_Float64_Array
    /// </summary>
    [FieldOffset(0)]
    internal CorsairDataTypeFloat64Array float64Array;

    /// <summary>
    /// iCUE-SDK: actual property value if it’s type is CPDT_String_Array
    /// </summary>
    [FieldOffset(0)]
    internal CorsairDataTypeStringArray stringArray;

    #endregion

    #region Data Types

    /// <summary>
    /// iCUE: represents an array of boolean values
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct CorsairDataTypeBooleanArray
    {
        /// <summary>
        /// iCUE: an array of boolean values
        /// </summary>
        internal nint items;

        /// <summary>
        /// iCUE: number of items array elements
        /// </summary>
        internal uint count;
    };

    /// <summary>
    /// iCUE: represents an array of integer values
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct CorsairDataTypeInt32Array
    {
        /// <summary>
        /// iCUE: an array of integer values
        /// </summary>
        internal nint items;

        /// <summary>
        /// iCUE: number of items array elements
        /// </summary>
        internal uint count;
    };

    /// <summary>
    /// iCUE: represents an array of double values
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct CorsairDataTypeFloat64Array
    {
        /// <summary>
        /// iCUE: an array of double values
        /// </summary>
        internal nint items;

        /// <summary>
        /// iCUE: number of items array elements
        /// </summary>
        internal uint count;
    };

    /// <summary>
    /// iCUE: represents an array of pointers to null terminated strings
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct CorsairDataTypeStringArray
    {
        /// <summary>
        /// iCUE: an array of pointers to null terminated strings
        /// </summary>
        internal nint items;

        /// <summary>
        /// iCUE: number of items array elements
        /// </summary>
        internal uint count;
    };

    #endregion
}
