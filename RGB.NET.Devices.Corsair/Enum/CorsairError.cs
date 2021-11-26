// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace RGB.NET.Devices.Corsair;

/// <summary>
/// Shared list of all errors which could happen during calling of Corsair* functions.
/// </summary>
public enum CorsairError
{
    /// <summary>
    /// If previously called function completed successfully.
    /// </summary>
    Success,

    /// <summary>
    /// CUE is not running or was shut down or third-party control is disabled in CUE settings. (runtime error)
    /// </summary>
    ServerNotFound,

    /// <summary>
    /// If some other client has or took over exclusive control. (runtime error)
    /// </summary>
    NoControl,

    /// <summary>
    /// If developer did not perform protocol handshake. (developer error)
    /// </summary>
    ProtocolHandshakeMissing,

    /// <summary>
    /// If developer is calling the function that is not supported by the server (either because protocol has broken by server or client or because the function is new and server is too old.
    /// Check CorsairProtocolDetails for details). (developer error)
    /// </summary>
    IncompatibleProtocol,

    /// <summary>
    /// If developer supplied invalid arguments to the function (for specifics look at function descriptions). (developer error)
    /// </summary>
    InvalidArguments
};