// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace RGB.NET.Devices.Corsair;

/// <summary>
/// iCUE-SDK: Represents an SDK access level.
/// </summary>
public enum CorsairAccessLevel
{
    /// <summary>
    /// iCUE-SDK: shared mode (default)
    /// </summary>
    Shared = 0,

    /// <summary>
    /// iCUE-SDK: exclusive lightings, but shared events
    /// </summary>
    ExclusiveLightingControl = 1,

    /// <summary>
    /// iCUE-SDK: exclusive key events, but shared lightings
    /// </summary>
    ExclusiveKeyEventsListening = 2,

    /// <summary>
    /// iCUE-SDK: exclusive mode
    /// </summary>
    ExclusiveLightingControlAndKeyEventsListening = 3
};