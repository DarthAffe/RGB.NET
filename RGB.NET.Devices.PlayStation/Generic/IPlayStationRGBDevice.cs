using RGB.NET.Core;

namespace RGB.NET.Devices.PlayStation;

/// <summary>
/// Common contract for PlayStation controller RGB devices. Each device owns
/// its own HID I/O (HidStream + optional HidRawWriter) and DevicePath, and
/// is responsible for tearing those down on <see cref="System.IDisposable.Dispose"/>.
/// </summary>
internal interface IPlayStationRGBDevice : IRGBDevice
{
    /// <summary>The Windows / Linux HID device path the controller was opened on.</summary>
    string DevicePath { get; }

    /// <summary>
    /// Set by the provider's hot-plug pass when the controller has disappeared
    /// from HID enumeration. Causes Dispose to skip the polite off-frame write
    /// (which would just throw against the invalidated handle anyway).
    /// </summary>
    bool IsKnownDisconnected { get; }

    /// <summary>
    /// Records that the device is no longer reachable on its HID path AND
    /// suspends any further writes from the update queue. Called from the
    /// hot-plug callback before the debounced Reconcile fires, so the next
    /// 30Hz tick is a no-op rather than an exception.
    /// </summary>
    void MarkKnownDisconnected();
}
