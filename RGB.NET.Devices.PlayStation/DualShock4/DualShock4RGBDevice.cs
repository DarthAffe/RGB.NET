using HidSharp;
using RGB.NET.Core;

namespace RGB.NET.Devices.PlayStation;

/// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a Sony DualShock 4 controller. Owns its HID I/O directly —
/// the open <see cref="HidStream"/>, the optional Win32 raw-write fallback,
/// and the device path used for identity comparison during hot-plug — and
/// tears them down on <see cref="Dispose"/>.
/// </summary>
public sealed class DualShock4RGBDevice : AbstractRGBDevice<PlayStationDeviceInfo>, IPlayStationRGBDevice
{
    #region Properties & Fields

    private readonly DualShock4UpdateQueue _updateQueue;
    private readonly HidStream _stream;
    private readonly HidRawWriter? _rawWriter;

    /// <inheritdoc />
    public string DevicePath { get; }

    /// <inheritdoc />
    public bool IsKnownDisconnected { get; private set; }

    #endregion

    #region Constructors

    internal DualShock4RGBDevice(PlayStationDeviceInfo deviceInfo, DualShock4UpdateQueue updateQueue, HidStream stream, HidRawWriter? rawWriter, string devicePath)
        : base(deviceInfo, updateQueue)
    {
        _updateQueue = updateQueue;
        _stream = stream;
        _rawWriter = rawWriter;
        DevicePath = devicePath ?? string.Empty;
        InitializeLayout();
    }

    #endregion

    #region Methods

    // DS4 has a single RGB lightbar above the touchpad. Custom1 keeps the LED
    // enum stable across DS4 / DS5 — DualSenseRGBDevice's Custom1 is also the
    // lightbar so a host mapping for "Custom 1" carries sensible meaning across
    // both controller types.
    private void InitializeLayout()
    {
        Led? lightbar = AddLed(LedId.Custom1, new Point(0, 0), new Size(60, 14));
        if (lightbar != null)
            lightbar.Shape = Shape.Rectangle;
    }

    /// <inheritdoc />
    public void MarkKnownDisconnected()
    {
        IsKnownDisconnected = true;
        try { _updateQueue.SuspendWrites(); } catch { /* best effort */ }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        // Off-frame is best-effort: send a final all-zero lightbar so the
        // controller doesn't sit on our last colour after we tear down.
        // Skipped when we've already been marked disconnected — the handle
        // is invalid and the write would just throw.
        try { _updateQueue.Shutdown(sendOffFrame: !IsKnownDisconnected); } catch { /* best effort */ }
        try { _rawWriter?.Dispose(); } catch { /* best effort */ }
        try { _stream.Dispose(); } catch { /* best effort */ }

        base.Dispose();
    }

    #endregion
}
