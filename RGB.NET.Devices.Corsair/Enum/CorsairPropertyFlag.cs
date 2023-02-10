using System;

namespace RGB.NET.Devices.Corsair;

/// <summary>
/// iCUE-SDK: contains list of operations that can be applied to the property
/// </summary>
[Flags]
public enum CorsairPropertyFlag
{
    /// <summary>
    /// -
    /// </summary>
    None = 0x00,

    /// <summary>
    /// iCUE-SDK: describes readable property
    /// </summary>
    CanRead = 0x01,

    /// <summary>
    /// iCUE-SDK: describes writable property
    /// </summary>
    CanWrite = 0x02,

    /// <summary>
    /// iCUE-SDK: if flag is set, then index should be used to read/write multiple properties that share the same property identifier
    /// </summary>
    Indexed = 0x04
}