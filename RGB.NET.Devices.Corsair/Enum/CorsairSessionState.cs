// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace RGB.NET.Devices.Corsair;

/// <summary>
/// iCUE-SDK: contains a list of all possible session states
/// </summary>
public enum CorsairSessionState
{
    /// <summary>
    /// iCUE-SDK: dummy value
    /// </summary>
    Invalid = 0,

    /// <summary>
    /// iCUE-SDK: client not initialized or client closed connection (initial state)
    /// </summary>
    Closed = 1,

    /// <summary>
    /// iCUE-SDK: client initiated connection but not connected yet
    /// </summary>
    Connecting = 2,

    /// <summary>
    /// iCUE-SDK: server did not respond, sdk will try again
    /// </summary>
    Timeout = 3,

    /// <summary>
    /// iCUE-SDK: server did not allow connection
    /// </summary>
    ConnectionRefused = 4,

    /// <summary>
    /// iCUE-SDK: server closed connection
    /// </summary>
    ConnectionLost = 5,

    /// <summary>
    /// iCUE-SDK: successfully connected
    /// </summary>
    Connected = 6
};