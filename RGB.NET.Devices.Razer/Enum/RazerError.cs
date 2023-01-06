namespace RGB.NET.Devices.Razer;

/// <summary>
/// Razer-SDK: Error codes for Chroma SDK. If the error is not defined here, refer to WinError.h from the Windows SDK.
/// </summary>
public enum RazerError
{
    /// <summary>
    /// Razer-SDK: Invalid.
    /// </summary>
    Invalid = -1,

    /// <summary>
    /// Razer-SDK: Success.
    /// </summary>
    Success = 0,

    /// <summary>
    /// Razer-SDK: Access denied.
    /// </summary>
    AccessDenied = 5,

    /// <summary>
    /// Razer-SDK: Invalid handle.
    /// </summary>
    InvalidHandle = 6,

    /// <summary>
    /// Razer-SDK: Not supported.
    /// </summary>
    NotSupported = 50,

    /// <summary>
    /// Razer-SDK: Invalid parameter.
    /// </summary>
    InvalidParameter = 87,

    /// <summary>
    /// Razer-SDK: The service has not been started.
    /// </summary>
    ServiceNotActive = 1062,

    /// <summary>
    /// Razer-SDK: Cannot start more than one instance of the specified program.
    /// </summary>
    SingleInstanceApp = 1152,

    /// <summary>
    /// Razer-SDK: Device not connected.
    /// </summary>
    DeviceNotConnected = 1167,

    /// <summary>
    /// Razer-SDK: Element not found.
    /// </summary>
    NotFound = 1168,

    /// <summary>
    /// Razer-SDK: Request aborted.
    /// </summary>
    RequestAborted = 1235,

    /// <summary>
    /// Razer-SDK: An attempt was made to perform an initialization operation when initialization has already been completed.
    /// </summary>
    AlreadyInitialized = 1247,

    /// <summary>
    /// Razer-SDK: Resource not available or disabled.
    /// </summary>
    ResourceDisabled = 4309,

    /// <summary>
    /// Razer-SDK: Device not available or supported.
    /// </summary>
    DeviceNotAvailable = 4319,

    /// <summary>
    /// Razer-SDK: The group or resource is not in the correct state to perform the requested operation.
    /// </summary>
    NotValidState = 5023,

    /// <summary>
    /// Razer-SDK: No more items.
    /// </summary>
    NoMoreItems = 259,

    /// <summary>
    /// Razer-SDK: General failure.
    /// </summary>
    Failed = unchecked(-2147467259)
}