using System;
using System.Diagnostics;
using System.Threading;
using HidSharp;
using RGB.NET.Core;

namespace RGB.NET.Devices.PlayStation;

// Builds and writes DualShock 4 main output reports.
//   USB report 0x05, 32 bytes total (incl. report ID).
//   BT  report 0x11, 78 bytes total + trailing little-endian CRC32.
//
// Byte layouts mirror struct dualshock4_output_report_{usb,bt} in the Linux
// hid-playstation driver. See PlayStationCrc32 for the CRC details.
//
// The DS4 only has a single RGB lightbar (no player indicator row, no mic LED),
// so this queue handles one Color slot.
internal sealed class DualShock4UpdateQueue : UpdateQueue
{
    #region Constants

    // valid_flag0 bits — DS4_OUTPUT_VALID_FLAG0_LED. We only update LEDs; rumble
    // & blink stay 0 so the controller doesn't vibrate when the queue runs.
    private const byte VALID_FLAG0_LED = 0x02;

    // hw_control bits for BT report 0x11. Driver always sets HID|CRC32 so the
    // controller knows the report carries main HID state and validates the CRC
    // tail. Lower 6 bits are the BT poll interval (0 = 1ms, fastest).
    private const byte BT_HW_CONTROL_HID = 0x80;
    private const byte BT_HW_CONTROL_CRC32 = 0x40;

    #endregion

    #region Properties & Fields

    private readonly HidStream _stream;
    private readonly HidRawWriter? _rawWriter;
    private readonly PlayStationTransport _transport;
    private readonly byte[] _buffer;
    private readonly string _devicePath;
    private readonly Lock _writeLock = new();
    private volatile bool _disposed;

    #endregion

    #region Constructors

    public DualShock4UpdateQueue(IDeviceUpdateTrigger trigger, HidStream stream, HidRawWriter? rawWriter, PlayStationTransport transport, string devicePath)
        : base(trigger)
    {
        _stream = stream;
        _rawWriter = rawWriter;
        _transport = transport;
        _devicePath = devicePath ?? string.Empty;
        _buffer = new byte[transport == PlayStationTransport.Bluetooth ? 78 : 32];
    }

    #endregion

    #region Methods

    protected override bool Update(ReadOnlySpan<(object key, Color color)> dataSet)
    {
        if (_disposed) return true;
        if (dataSet.IsEmpty) return true;

        // Per-frame liveness pre-check. The provider's PnP handler refreshes
        // this snapshot synchronously the moment the OS reports a change,
        // while we're consulted on the trigger thread. Skipping the Write
        // here avoids the IOException entirely on hot-unplug.
        if (!PlayStationDeviceProvider.IsDevicePathAlive(_devicePath))
        {
            _disposed = true;
            return false;
        }

        // The DualShock4 device exposes a single Lightbar LED. Take the first
        // colour we see — RGB.NET commits the painted colour for that LED each tick.
        Color color = dataSet[0].color;

        bool ok;
        lock (_writeLock)
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            BuildReport(color);
            ok = WriteBuffer();
        }

        if (!ok)
        {
            // Device went away mid-write or another tool grabbed exclusive access.
            // Suspend the queue so the next 30Hz tick short-circuits at the
            // `if (_disposed)` gate. The provider's hot-plug Reconcile will
            // RemoveDevice us shortly.
            Trace.WriteLine("[RGB.NET.PlayStation] DualShock4 write failed, suspending queue.");
            _disposed = true;
            return false;
        }
        return true;
    }

    // On Windows, prefer HidRawWriter (synchronous Win32 WriteFile) — HidSharp's
    // overlapped HidStream.Write fails on the second and subsequent USB writes
    // against the PlayStation HID minidriver. On non-Windows or if HidRawWriter
    // failed to open, fall back to HidStream.Write.
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
            Trace.WriteLine($"[RGB.NET.PlayStation] DualShock4 stream write threw: {ex.Message}");
            return false;
        }
    }

    private void BuildReport(Color color)
    {
        byte r = color.GetR();
        byte g = color.GetG();
        byte b = color.GetB();

        if (_transport == PlayStationTransport.Bluetooth)
        {
            // BT report 0x11. Layout: report_id, hw_control, audio_control,
            // then the common 9-byte block (valid_flag0, valid_flag1,
            // reserved, motor_right, motor_left, lightbar_red/green/blue,
            // blink_on, blink_off), then 61 bytes reserved padding, then
            // 4-byte LE CRC32 over [0xA2 || buffer[0..len-4]].
            _buffer[0] = 0x11;
            _buffer[1] = BT_HW_CONTROL_HID | BT_HW_CONTROL_CRC32; // hw_control
            _buffer[2] = 0;                                       // audio_control
            _buffer[3] = VALID_FLAG0_LED;                          // valid_flag0
            _buffer[4] = 0;                                       // valid_flag1
            _buffer[5] = 0;                                       // reserved
            _buffer[6] = 0;                                       // motor_right
            _buffer[7] = 0;                                       // motor_left
            _buffer[8] = r;                                       // lightbar_red
            _buffer[9] = g;                                       // lightbar_green
            _buffer[10] = b;                                      // lightbar_blue
            _buffer[11] = 0;                                      // blink_on
            _buffer[12] = 0;                                      // blink_off
            // Bytes 13..73 already zero from Array.Clear.
            PlayStationCrc32.AppendOutputCrc(_buffer);
        }
        else
        {
            // USB report 0x05. Layout: report_id, common(10 bytes), then
            // 21 bytes reserved padding. No CRC.
            _buffer[0] = 0x05;
            _buffer[1] = VALID_FLAG0_LED;
            _buffer[2] = 0;
            _buffer[3] = 0;   // reserved
            _buffer[4] = 0;   // motor_right
            _buffer[5] = 0;   // motor_left
            _buffer[6] = r;
            _buffer[7] = g;
            _buffer[8] = b;
            _buffer[9] = 0;
            _buffer[10] = 0;
        }
    }

    /// <summary>
    /// Sets the queue's disposed flag so subsequent ticks short-circuit before
    /// attempting any HidStream.Write. Called by the provider's immediate
    /// hot-unplug handler the moment the OS reports the device is gone, so the
    /// next trigger tick has nothing to write. Lighter than Shutdown — no off-
    /// frame attempt, no other state mutation. Idempotent.
    /// </summary>
    public void SuspendWrites() => _disposed = true;

    /// <summary>
    /// Tears down the queue. <paramref name="sendOffFrame"/> defaults to true for
    /// "voluntary" teardowns (provider unloaded by the user, app exit) where the
    /// controller is still connected and benefits from a clean off-state. Pass
    /// false from the hot-plug-disconnect path.
    /// </summary>
    public void Shutdown(bool sendOffFrame = true)
    {
        if (_disposed) return;
        _disposed = true;
        if (!sendOffFrame) return;
        // Send one final all-zero lightbar so the controller doesn't sit on our
        // last colour after we tear down. The controller's firmware restores
        // the OS-driven indicator (e.g. player number) shortly after we stop
        // sending reports anyway, but explicit black avoids the visible "stuck
        // on last colour" beat between shutdown and firmware reset. Best-effort —
        // WriteBuffer returns false silently if the handle has already been
        // invalidated.
        lock (_writeLock)
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            BuildReport(new Color(0, 0, 0));
            WriteBuffer();
        }
    }

    #endregion
}
