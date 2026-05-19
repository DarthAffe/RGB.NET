using HidSharp;
using RGB.NET.Core;

namespace RGB.NET.Devices.PlayStation;

/// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a Sony DualSense controller (PS5 / DualSense Edge). Owns its
/// HID I/O directly — the open <see cref="HidStream"/>, the optional Win32
/// raw-write fallback, and the device path used for identity comparison
/// during hot-plug — and tears them down on <see cref="Dispose"/>.
/// </summary>
public sealed class DualSenseRGBDevice : AbstractRGBDevice<PlayStationDeviceInfo>, IPlayStationRGBDevice
{
    #region Properties & Fields

    private readonly DualSenseUpdateQueue _updateQueue;
    private readonly HidStream _stream;
    private readonly HidRawWriter? _rawWriter;

    /// <inheritdoc />
    public string DevicePath { get; }

    /// <inheritdoc />
    public bool IsKnownDisconnected { get; private set; }

    #endregion

    #region Constructors

    internal DualSenseRGBDevice(PlayStationDeviceInfo deviceInfo, DualSenseUpdateQueue updateQueue, HidStream stream, HidRawWriter? rawWriter, string devicePath)
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

    // DualSense LED layout (left→right when looking at the controller):
    //   - The lightbar runs along the bottom edge of the touchpad in two
    //     mirrored strips. Modelled as one wide rectangle (Custom1).
    //   - The 5 player indicator LEDs sit in a row directly below the
    //     touchpad. Bit 0 = leftmost, bit 4 = rightmost from the player's
    //     POV (matches Linux's player_leds bit ordering).
    //
    // Note: the mic-mute LED is intentionally NOT exposed. The controller
    // firmware drives that LED to track mic-mute toggle state — pressing
    // the mute button mutes the microphone AND lights the LED, regardless
    // of any host involvement. Taking host control of the LED would only
    // suppress that visual feedback for an action that still happens, so
    // we leave the firmware default in place. See DualSenseUpdateQueue
    // header for the protocol detail (we deliberately don't set the
    // MIC_MUTE_LED_CONTROL_ENABLE bit in valid_flag1).
    //
    // Coordinates are arbitrary visual approximations for layout consumers —
    // they don't drive any hardware addressing.
    private void InitializeLayout()
    {
        Led? lightbar = AddLed(LedId.Custom1, new Point(0, 0), new Size(80, 8));
        if (lightbar != null) lightbar.Shape = Shape.Rectangle;

        // Five player indicator dots, evenly spaced beneath the lightbar.
        for (int i = 0; i < 5; i++)
        {
            Led? led = AddLed((LedId)(LedId.Custom2 + i), new Point(20 + (i * 12), 16), new Size(6));
            if (led != null) led.Shape = Shape.Circle;
        }
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
        try { _updateQueue.Shutdown(sendOffFrame: !IsKnownDisconnected); } catch { /* best effort */ }
        try { _rawWriter?.Dispose(); } catch { /* best effort */ }
        try { _stream.Dispose(); } catch { /* best effort */ }

        base.Dispose();
    }

    #endregion
}
