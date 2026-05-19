using System;
using System.Diagnostics;
using System.Threading;
using HidSharp;
using RGB.NET.Core;

namespace RGB.NET.Devices.PlayStation;

// Builds and writes DualSense main output reports.
//   USB report 0x02, 63 bytes total (incl. report ID).
//   BT  report 0x31, 78 bytes total + trailing little-endian CRC32.
//
// The DualSense exposes:
//   - one RGB lightbar (sides of the touchpad)
//   - five monochrome player indicator LEDs in a row below the touchpad
//   - one mic-mute LED (orange, in the centre of the mic-mute button) —
//     NOT exposed by this provider. We deliberately leave the firmware in
//     control so the LED keeps its default behaviour of tracking the
//     hardware mic-mute toggle (tap the button → firmware mutes the mic
//     AND lights the LED). The mute BUTTON itself remains firmware-driven
//     regardless of host activity, so taking control of the LED would
//     only suppress the visual feedback for an action that still happens.
//
// We model the controllable LEDs as Custom1 (lightbar) + Custom2..Custom6
// (P1..P5 left→right in bit-position order, see player_leds bit layout
// below). Player indicators are monochrome so any non-black colour turns
// them on at full brightness, and pure black turns them off.
//
// valid_flag1 gates which sub-systems the controller should accept updates
// for. Without those bits set, the controller ignores the corresponding
// bytes — so we must set them every report or e.g. the lightbar will stay
// on the firmware's default. We deliberately do NOT set the mic-mute-LED
// bit (BIT(0)), which keeps the firmware-driven default LED behaviour.
//
// The first report after open also sets LIGHTBAR_SETUP_CONTROL_ENABLE +
// lightbar_setup = 0x02 ("release leds"). On a fresh connect, the
// controller plays a fade-in animation on the lightbar that overrides
// host-driven colours until released. Without this one-shot, the first
// few seconds of host control look like nothing is happening.
internal sealed class DualSenseUpdateQueue : UpdateQueue
{
    #region Constants

    // valid_flag1 bits we want the controller to honour. Mic-mute LED is
    // intentionally NOT here — see file header for rationale.
    // BIT(0) = MIC_MUTE_LED_CONTROL_ENABLE remains clear; the firmware
    // ignores any value we'd put in mute_button_led and uses its own logic.
    private const byte VALID_FLAG1_LIGHTBAR_CONTROL = 0x04;          // BIT(2)
    private const byte VALID_FLAG1_PLAYER_INDICATOR_CONTROL = 0x10;  // BIT(4)
    private const byte VALID_FLAG1_ALL =
        VALID_FLAG1_LIGHTBAR_CONTROL | VALID_FLAG1_PLAYER_INDICATOR_CONTROL;

    // valid_flag2 bit for the one-shot "release lightbar from boot animation"
    // setup. Cleared after the first report.
    private const byte VALID_FLAG2_LIGHTBAR_SETUP_CONTROL = 0x02;    // BIT(1)
    private const byte LIGHTBAR_SETUP_RELEASE_LEDS = 0x02;

    // BT-specific tag — Sony driver requires a fixed value here. Lower 4
    // bits of seq_tag carry an alternate tag (0); upper 4 bits carry a
    // sequence number that increments per report.
    private const byte BT_TAG = 0x10;

    #endregion

    #region Properties & Fields

    private readonly HidStream _stream;
    private readonly HidRawWriter? _rawWriter;
    private readonly PlayStationTransport _transport;
    private readonly byte[] _buffer;
    private readonly string _devicePath;
    private readonly Lock _writeLock = new();
    private byte _btSeq; // 0..15 rolling
    private bool _firstReport = true;
    private volatile bool _disposed;

    #endregion

    #region Constructors

    public DualSenseUpdateQueue(IDeviceUpdateTrigger trigger, HidStream stream, HidRawWriter? rawWriter, PlayStationTransport transport, string devicePath)
        : base(trigger)
    {
        _stream = stream;
        _rawWriter = rawWriter;
        _transport = transport;
        _devicePath = devicePath ?? string.Empty;
        _buffer = new byte[transport == PlayStationTransport.Bluetooth ? 78 : 63];
    }

    #endregion

    #region Methods

    protected override bool Update(ReadOnlySpan<(object key, Color color)> dataSet)
    {
        if (_disposed) return true;
        if (dataSet.IsEmpty) return true;

        // Per-frame liveness pre-check; see DualShock4UpdateQueue.Update.
        if (!PlayStationDeviceProvider.IsDevicePathAlive(_devicePath))
        {
            _disposed = true;
            return false;
        }

        // Walk the painted LEDs and split them into the payload slots the
        // report cares about. dataSet entries arrive keyed by LedId, so we
        // can address them individually instead of trusting iteration order.
        Color lightbar = default;
        byte playerLedBits = 0;
        bool gotLightbar = false;

        foreach ((object key, Color color) in dataSet)
        {
            if (key is not LedId id) continue;
            switch (id)
            {
                case LedId.Custom1:
                    lightbar = color;
                    gotLightbar = true;
                    break;
                // Custom2..Custom6 = player indicators 1..5 (bits 0..4)
                case LedId.Custom2: if (IsLit(color)) playerLedBits |= 1 << 0; break;
                case LedId.Custom3: if (IsLit(color)) playerLedBits |= 1 << 1; break;
                case LedId.Custom4: if (IsLit(color)) playerLedBits |= 1 << 2; break;
                case LedId.Custom5: if (IsLit(color)) playerLedBits |= 1 << 3; break;
                case LedId.Custom6: if (IsLit(color)) playerLedBits |= 1 << 4; break;
            }
        }

        // If we somehow get a payload with no lightbar entry (should not
        // happen since RGB.NET commits every device LED each tick), keep
        // the lightbar at black instead of leaving uninitialised state.
        if (!gotLightbar) lightbar = new Color(0, 0, 0);

        bool ok;
        lock (_writeLock)
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            BuildReport(lightbar, playerLedBits);
            ok = WriteBuffer();
        }

        if (!ok)
        {
            Trace.WriteLine("[RGB.NET.PlayStation] DualSense write failed, suspending queue.");
            _disposed = true;
            return false;
        }
        _firstReport = false;
        return true;
    }

    // See DualShock4UpdateQueue.WriteBuffer for the rationale behind preferring
    // HidRawWriter over HidStream.Write on Windows.
    private bool WriteBuffer()
    {
        if (_rawWriter != null)
            return _rawWriter.TryWrite(_buffer);

        try
        {
            _stream.Write(_buffer);
            return true;
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[RGB.NET.PlayStation] DualSense stream write threw: {ex.Message}");
            return false;
        }
    }

    private static bool IsLit(Color c) => (c.R > 0) || (c.G > 0) || (c.B > 0);

    private void BuildReport(Color lightbar, byte playerLedBits)
    {
        byte r = lightbar.GetR();
        byte g = lightbar.GetG();
        byte b = lightbar.GetB();

        int commonOffset; // start of the 47-byte common block within _buffer

        if (_transport == PlayStationTransport.Bluetooth)
        {
            // BT report 0x31:
            //   [0]   report_id (0x31)
            //   [1]   seq_tag (high 4 bits = sequence number 0..15)
            //   [2]   tag (0x10)
            //   [3..49]  common (47 bytes)
            //   [50..73] reserved (24 bytes)
            //   [74..77] CRC32 (LE)
            _buffer[0] = 0x31;
            _buffer[1] = (byte)((_btSeq << 4) & 0xF0);
            _buffer[2] = BT_TAG;
            commonOffset = 3;
            _btSeq = (byte)((_btSeq + 1) & 0x0F);
        }
        else
        {
            // USB report 0x02:
            //   [0]      report_id (0x02)
            //   [1..47]  common (47 bytes)
            //   [48..62] reserved (15 bytes)
            _buffer[0] = 0x02;
            commonOffset = 1;
        }

        // dualsense_output_report_common offsets, 0-indexed from start of
        // common block. See struct dualsense_output_report_common in
        // Linux's hid-playstation.c.
        //   [0]  valid_flag0
        //   [1]  valid_flag1
        //   [2]  motor_right
        //   [3]  motor_left
        //   [4]  headphone_volume
        //   [5]  speaker_volume
        //   [6]  mic_volume
        //   [7]  audio_control
        //   [8]  mute_button_led
        //   [9]  power_save_control
        //   [10..36] reserved2 (27 bytes)
        //   [37] audio_control2
        //   [38] valid_flag2
        //   [39..40] reserved3
        //   [41] lightbar_setup
        //   [42] led_brightness
        //   [43] player_leds
        //   [44] lightbar_red
        //   [45] lightbar_green
        //   [46] lightbar_blue
        int c = commonOffset;
        _buffer[c + 0] = 0;                                // valid_flag0
        _buffer[c + 1] = VALID_FLAG1_ALL;                  // valid_flag1
        // motor_*, audio, power_save, mute_button_led left zero. The
        // mute_button_led byte (offset 8) is ignored by firmware because
        // we don't set MIC_MUTE_LED_CONTROL_ENABLE in valid_flag1, so
        // the firmware retains its default LED-tracks-mute-state behaviour.

        if (_firstReport)
        {
            _buffer[c + 38] = VALID_FLAG2_LIGHTBAR_SETUP_CONTROL; // valid_flag2
            _buffer[c + 41] = LIGHTBAR_SETUP_RELEASE_LEDS;        // lightbar_setup
        }

        _buffer[c + 42] = 0;                               // led_brightness (0 = full per Sony default)
        _buffer[c + 43] = (byte)(playerLedBits & 0x1F);    // player_leds (bits 0..4)
        _buffer[c + 44] = r;                               // lightbar_red
        _buffer[c + 45] = g;                               // lightbar_green
        _buffer[c + 46] = b;                               // lightbar_blue

        if (_transport == PlayStationTransport.Bluetooth)
            PlayStationCrc32.AppendOutputCrc(_buffer);
    }

    /// <summary>
    /// See <see cref="DualShock4UpdateQueue.SuspendWrites"/> for the contract.
    /// </summary>
    public void SuspendWrites() => _disposed = true;

    /// <summary>
    /// See <see cref="DualShock4UpdateQueue.Shutdown(bool)"/> for the contract.
    /// </summary>
    public void Shutdown(bool sendOffFrame = true)
    {
        if (_disposed) return;
        _disposed = true;
        if (!sendOffFrame) return;
        // Best-effort — WriteBuffer returns false silently if the handle has
        // already been invalidated.
        lock (_writeLock)
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            BuildReport(new Color(0, 0, 0), 0);
            WriteBuffer();
        }
    }

    #endregion
}
