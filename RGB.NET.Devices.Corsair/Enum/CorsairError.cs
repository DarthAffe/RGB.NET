// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace RGB.NET.Devices.Corsair;

/// <summary>
/// iCUE-SDK: Shared list of all errors which could happen during calling of Corsair* functions.
/// </summary>
public enum CorsairError
{
    /// <summary>
    /// iCUE-SDK: if previously called function completed successfully
    /// </summary>
    Success = 0,

    /// <summary>
    /// iCUE-SDK: if iCUE is not running or was shut down or third-party control is disabled in iCUE settings (runtime error), or if developer did not call CorsairConnect after calling CorsairDisconnect or on app start (developer error)
    /// </summary>
    NotConnected = 1,

    /// <summary>
    /// iCUE-SDK: if some other client has or took over exclusive control (runtime error)
    /// </summary>
    NoControl = 2,

    /// <summary>
    /// iCUE-SDK: if developer is calling the function that is not supported by the server (either because protocol has broken by server or client or because the function is new and server is too old. Check CorsairSessionDetails for details) (developer error)
    /// </summary>
    IncompatibleProtocol = 3,

    /// <summary>
    /// iCUE-SDK: if developer supplied invalid arguments to the function (for specifics look at function descriptions) (developer error)
    /// </summary>
    InvalidArguments = 4,

    /// <summary>
    /// iCUE-SDK: if developer is calling the function that is not allowed due to current state (reading improper properties from device, or setting callback when it has already been set) (developer error)
    /// </summary>
    InvalidOperation = 5,

    /// <summary>
    /// iCUE-SDK: if invalid device id has been supplied as an argument to the function (when device id refers to disconnected device) (runtime error)
    /// </summary>
    DeviceNotFound = 6,

    /// <summary>
    /// iCUE-SDK: if specific functionality (key interception) is disabled in iCUE settings (runtime error)
    /// </summary>
    NotAllowed = 7
};