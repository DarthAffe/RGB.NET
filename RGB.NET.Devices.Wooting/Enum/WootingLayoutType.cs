// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace RGB.NET.Devices.Wooting.Enum;

/// <summary>
/// Contains list of available physical layouts for Wooting keyboards.
/// </summary>
/// <remarks>
/// Shop states ANSI (US) and ISO (UK/German/Nodics) - https://wooting.store/collections/wooting-keyboards/products/wooting-two
/// </remarks>
public enum WootingLayoutType
{
    Unknown = -1,
    ANSI = 0,
    ISO = 1,
    JIS = 2,
    ANSI_SPLIT_SPACEBAR = 3,
    ISO_SPLIT_SPACEBAR = 4,
}
