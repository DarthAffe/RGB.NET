using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HidSharp;
using RGB.NET.Core;

namespace RGB.NET.Devices.PlayStation;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for Sony PlayStation controllers —
/// DualShock 4 (PS4) and DualSense / DualSense Edge (PS5).
/// </summary>
/// <remarks>
/// Talks raw HID via HidSharp; no third-party drivers (no DS4Windows, no SignalRGB,
/// no HidHide). Both USB and Bluetooth transports are supported.
///
/// Lighting only — input reports continue to flow through the OS HID stack to games
/// normally. Sony's HID gamepads accept *shared* output writes by default, so
/// coexisting with Steam or a game's native lighting integration is the expected
/// case. Last-writer-wins per output report period; at the 30Hz cadence this
/// provider comfortably overrides most intermittent setters (Steam profile changes,
/// game state events).
///
/// Hot-plug: <see cref="DeviceList"/>'s Changed event fires on PnP events
/// (USB connect/disconnect, BT pair/unpair). Reconcile is debounced, then
/// the open set is diffed against the current HID enumeration — new
/// controllers get opened + AddDevice'd, removed ones are disposed and
/// RemoveDevice'd.
///
/// Known collisions:
/// <list type="bullet">
///   <item>DS4Windows / reWASD with "Exclusive Mode" enabled — they hold the HID
///         handle exclusive, so opens fail with UnauthorizedAccessException /
///         IOException.</item>
///   <item>HidHide hiding the controller from non-allow-listed apps — the device
///         never appears in HidSharp enumeration.</item>
/// </list>
/// </remarks>
public sealed class PlayStationDeviceProvider : AbstractRGBDeviceProvider
{
    #region Constants

    // Sony Interactive Entertainment's USB vendor id.
    private const int SONY_VENDOR_ID = 0x054C;

    // PlayStation HID product ids relevant for lighting.
    // DualShock 4 v1: 0x05C4 (original "JDM-001/011").
    // DualShock 4 v2: 0x09CC (revised "JDM-040/050/055" with lightbar visible
    //                          through touchpad).
    // DualSense:      0x0CE6 (PS5 launch model "CFI-ZCT1").
    // DualSense Edge: 0x0DF2 (PS5 pro variant "CFI-ZCP1").
    // The "Wireless Adapter" 0x0BA0 is the BT bridge for DS4 — also has the
    // Sony VID and reports as a DualShock 4. Treated like DS4 v2 (later
    // firmware, supports same lighting protocol).
    private const int PID_DUALSHOCK4_V1 = 0x05C4;
    private const int PID_DUALSHOCK4_V2 = 0x09CC;
    private const int PID_DUALSHOCK4_WIRELESS = 0x0BA0;
    private const int PID_DUALSENSE = 0x0CE6;
    private const int PID_DUALSENSE_EDGE = 0x0DF2;

    // 30Hz update rate. Faster than Steam's intermittent profile-change writes,
    // slower than USB full-speed bandwidth (would run fine at 250Hz but it's
    // wasted writes — perceptual change isn't there). Matches what OpenRGB's
    // DualSense plugin uses.
    private const double UPDATE_FREQUENCY_SECONDS = 1.0 / 30.0;

    // PnP can fire several Changed events for one logical connect (driver
    // initialisation, child interface enumeration, etc.). Coalesce them
    // AND wait long enough that the OS has finished setting up the HID
    // device — TryOpen can succeed against a partially-enumerated device
    // and the first write will then fail with "A device which does not
    // exist was specified".
    private const int HOTPLUG_DEBOUNCE_MS = 1500;

    #endregion

    #region Properties & Fields

    // ReSharper disable once InconsistentNaming
    private static readonly Lock _lock = new();

    private static PlayStationDeviceProvider? _instance;

    /// <summary>Gets the singleton <see cref="PlayStationDeviceProvider"/> instance.</summary>
    public static PlayStationDeviceProvider Instance
    {
        get
        {
            lock (_lock)
                return _instance ?? new PlayStationDeviceProvider();
        }
    }

    // Per-device state — HidStream, optional HidRawWriter, DevicePath — lives
    // on the device class itself (DualShock4RGBDevice / DualSenseRGBDevice)
    // and is disposed by the device's Dispose. The provider only needs to
    // know which devices it owns, which it gets via the inherited
    // <see cref="Devices"/> collection. Hot-plug iteration walks that
    // collection and reads each <see cref="IPlayStationRGBDevice.DevicePath"/>
    // off the device.

    // Snapshot of currently-alive Sony controller DevicePaths, refreshed
    // synchronously by SuspendDeadDevices on every DeviceList.Changed
    // and at the end of LoadDevices / Reconcile. UpdateQueues consult
    // it via IsDevicePathAlive before each HidStream.Write — this
    // closes the race between PnP unplug and the next 30Hz trigger
    // tick. Without a pre-check, even when the PnP handler runs
    // promptly, a tick already in flight can still call Write against
    // a now-invalid handle and throw IOException.
    private static volatile HashSet<string> _alivePathsSnapshot = new(StringComparer.OrdinalIgnoreCase);

    // Hot-plug bookkeeping: subscription flag (so re-init doesn't double-subscribe),
    // and a serial counter so debounced reconciles on stale enqueues short-circuit.
    private bool _hotplugSubscribed;
    private int _hotplugScheduleSeq;

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="PlayStationDeviceProvider"/> class.</summary>
    /// <exception cref="InvalidOperationException">Thrown if a second instance is constructed.</exception>
    public PlayStationDeviceProvider()
    {
        lock (_lock)
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(PlayStationDeviceProvider)}");
            _instance = this;
        }
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void InitializeSDK()
    {
        // Subscribe once for the lifetime of this provider instance. The
        // subscription is unhooked in Dispose. Guard against double-subscribe
        // in case Initialize is invoked twice.
        if (!_hotplugSubscribed)
        {
            DeviceList.Local.Changed += OnHidDeviceListChanged;
            _hotplugSubscribed = true;
        }
    }

    /// <inheritdoc />
    protected override IDeviceUpdateTrigger CreateUpdateTrigger(int id, double updateRateHardLimit)
        => new DeviceUpdateTrigger(UPDATE_FREQUENCY_SECONDS);

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        List<IRGBDevice> devices = [];

        HidDevice[] all;
        try
        {
            all = DeviceList.Local.GetHidDevices(vendorID: SONY_VENDOR_ID).ToArray();
        }
        catch (Exception ex)
        {
            Throw(ex);
            return devices;
        }

        foreach (HidDevice hid in all)
        {
            int pid = hid.ProductID;
            if (!IsSupportedPid(pid)) continue;

            if (TryOpenAndCreateDevice(hid, pid, out IRGBDevice? device) && device != null)
                devices.Add(device);
        }

        // Seed the alive-path snapshot so UpdateQueues' per-frame pre-check
        // answers correctly from the very first trigger tick.
        try { SuspendDeadDevices(); } catch { /* best effort */ }

        return devices;
    }

    private static bool IsSupportedPid(int pid)
        => pid is PID_DUALSHOCK4_V1
           or PID_DUALSHOCK4_V2
           or PID_DUALSHOCK4_WIRELESS
           or PID_DUALSENSE
           or PID_DUALSENSE_EDGE;

    // Centralised "open + construct + register" path used by both initial
    // enumeration and hot-plug. Handles the predictable failure modes
    // (TryOpen returns false, UnauthorizedAccessException, the broader
    // DeviceIOException family that HidSharp throws when the kernel
    // rejects the descriptor-query handle) and returns false silently —
    // caller doesn't need to distinguish "not openable" from "openable
    // but build failed".
    //
    // Only call HidDevice methods that are absolutely necessary, and only
    // call them in this order:
    //   1. DevicePath (cheap property, no descriptor query)
    //   2. TryOpen   (this also primes the ReportInfo cache as a side
    //                 effect)
    //   3. GetMaxOutputReportLength on the open stream (free — ReportInfo
    //      is now cached)
    //
    // We deliberately do NOT call GetSerialNumber. HidSharp's RequiresGetInfo
    // opens a *separate* read-info handle to satisfy any flag not already
    // cached — and on some hardware (DS4 v1 in particular, also any
    // controller whose descriptor query can't get a handle because Steam /
    // driver / power state is holding the device) this throws
    // DeviceIOException("Failed to get info."). Identity always comes from
    // a stable hash of DevicePath instead.
    private bool TryOpenAndCreateDevice(HidDevice hid, int pid, out IRGBDevice? device)
    {
        device = null;

        string devicePath;
        try { devicePath = hid.DevicePath ?? string.Empty; }
        catch { devicePath = string.Empty; }

        string serial = string.IsNullOrEmpty(devicePath) ? string.Empty : ShortHashOf(devicePath);

        HidStream opened;
        try
        {
            if (!hid.TryOpen(out opened!))
            {
                Trace.WriteLine($"[RGB.NET.PlayStation] Could not open controller (VID 0x{hid.VendorID:X4} PID 0x{pid:X4}). Another application may have exclusive HID access (DS4Windows / reWASD with exclusive mode enabled).");
                return false;
            }
        }
        catch (UnauthorizedAccessException)
        {
            Trace.WriteLine($"[RGB.NET.PlayStation] Access denied opening controller (VID 0x{hid.VendorID:X4} PID 0x{pid:X4}). Another application has exclusive HID access — close DS4Windows / reWASD or disable their exclusive mode.");
            return false;
        }
        catch (Exception ex)
        {
            // HidSharp.Exceptions.DeviceIOException ("Failed to get info.")
            // lands here when the kernel refuses the descriptor-query handle.
            Trace.WriteLine($"[RGB.NET.PlayStation] Failed to open controller (VID 0x{hid.VendorID:X4} PID 0x{pid:X4}): {ex.Message}");
            return false;
        }

        try
        {
            // Transport detection: DS4 USB max output report is 32 bytes (incl.
            // report ID), DS4 BT is 78. DS5 USB is 64, DS5 BT is 78. Any
            // controller that reports an output buffer of 78+ is on Bluetooth.
            // ReportInfo was cached by TryOpen above, so this call is free.
            int maxOut;
            try { maxOut = opened.Device.GetMaxOutputReportLength(); }
            catch { maxOut = 0; /* default to USB byte count */ }
            PlayStationTransport transport = maxOut >= 78 ? PlayStationTransport.Bluetooth : PlayStationTransport.Usb;

            PlayStationControllerType controllerType = pid switch
            {
                PID_DUALSENSE => PlayStationControllerType.DualSense,
                PID_DUALSENSE_EDGE => PlayStationControllerType.DualSenseEdge,
                _ => PlayStationControllerType.DualShock4,
            };

            PlayStationDeviceInfo info = new(controllerType, transport, serial);

            // On Windows, open a second handle for synchronous WriteFile use.
            // If this fails (rare — same flags as HidSharp's open which already
            // succeeded), log and continue with HidStream.Write fallback. On
            // non-Windows, rawWriter stays null and the queues use HidStream.
            HidRawWriter? rawWriter = null;
            if (OperatingSystem.IsWindows() && !string.IsNullOrEmpty(devicePath))
            {
                try { rawWriter = new HidRawWriter(devicePath); }
                catch (Exception writerEx)
                {
                    Trace.WriteLine($"[RGB.NET.PlayStation] Could not open raw write handle for {info.DeviceName}: {writerEx.Message} — falling back to HidStream.Write.");
                }
            }

            IRGBDevice newDevice;
            if (controllerType == PlayStationControllerType.DualShock4)
            {
                DualShock4UpdateQueue queue = new(GetUpdateTrigger(), opened, rawWriter, transport, devicePath);
                newDevice = new DualShock4RGBDevice(info, queue, opened, rawWriter, devicePath);
            }
            else
            {
                DualSenseUpdateQueue queue = new(GetUpdateTrigger(), opened, rawWriter, transport, devicePath);
                newDevice = new DualSenseRGBDevice(info, queue, opened, rawWriter, devicePath);
            }

            device = newDevice;
            return true;
        }
        catch (Exception ex)
        {
            try { opened.Dispose(); } catch { /* best effort */ }
            Trace.WriteLine($"[RGB.NET.PlayStation] Failed to construct device for VID 0x{hid.VendorID:X4} PID 0x{pid:X4}: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Hot-plug

    private void OnHidDeviceListChanged(object? sender, DeviceListChangedEventArgs e)
    {
        // Two-pass design.
        //
        // Pass 1 (immediate, no debounce): walk the open set against the
        // current HID enumeration and call SuspendWrites() on any device
        // that has disappeared. This sets the queue's _disposed flag
        // BEFORE the next 30Hz trigger tick fires, so the trigger never
        // reaches HidStream.Write — no IOException thrown at all.
        //
        // Pass 2 (debounced 1500ms): full Reconcile that handles
        //   - the slow-side cleanup (RemoveDevice + stream dispose +
        //     surface.Detach via the bookkeeping handler)
        //   - new-device opens (which need the debounce to let the OS
        //     finish enumerating — TryOpen on a partially-enumerated
        //     device succeeds but the first Write fails)
        // The seq counter cancels stale debounces so only the latest
        // PnP burst's Reconcile actually runs.
        try { SuspendDeadDevices(); }
        catch (Exception ex)
        {
            Trace.WriteLine($"[RGB.NET.PlayStation] Suspend-dead-devices pass threw: {ex.Message}");
        }

        int mySeq = System.Threading.Interlocked.Increment(ref _hotplugScheduleSeq);
        Task.Run(async () =>
        {
            await Task.Delay(HOTPLUG_DEBOUNCE_MS).ConfigureAwait(false);
            if (System.Threading.Volatile.Read(ref _hotplugScheduleSeq) != mySeq) return;
            try { Reconcile(); }
            catch (Exception ex)
            {
                Trace.WriteLine($"[RGB.NET.PlayStation] Hot-plug reconcile threw: {ex.Message}");
            }
        });
    }

    /// <summary>
    /// Per-frame pre-check used by the update queues. Queries HidSharp's device
    /// list LIVE — HidSharp invalidates its internal device-keys cache
    /// synchronously on WM_DEVICECHANGE inside DeviceMonitorWindowProc on the
    /// message-pump thread, BEFORE pulsing the notify thread that eventually
    /// fires the <see cref="DeviceList"/> Changed event. So a live
    /// GetHidDevices call sees the unplug ahead of any subscriber, which is
    /// the race that would otherwise leave the snapshot stale through the
    /// first post-unplug 30Hz tick.
    /// </summary>
    public static bool IsDevicePathAlive(string devicePath)
    {
        if (string.IsNullOrEmpty(devicePath)) return false;
        try
        {
            foreach (HidDevice hid in DeviceList.Local.GetHidDevices(vendorID: SONY_VENDOR_ID))
            {
                if (string.Equals(hid.DevicePath, devicePath, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
        catch
        {
            // Fail closed — if enumeration itself throws, skip the write
            // rather than fall through to HidStream.Write where the failure
            // mode is exactly the IOException we're trying to avoid.
            return false;
        }
    }

    // Immediate-pass companion to Reconcile. Compares currently-tracked device
    // paths to the live HID enumeration; for anything still held open that no
    // longer enumerates, mark the device as known-disconnected (suspends its
    // queue) AND refresh the alive-path snapshot the update queues consult
    // per frame.
    private void SuspendDeadDevices()
    {
        HashSet<string> currentPaths;
        try
        {
            currentPaths = DeviceList.Local.GetHidDevices(vendorID: SONY_VENDOR_ID)
                                           .Where(h => IsSupportedPid(h.ProductID))
                                           .Select(h => h.DevicePath ?? string.Empty)
                                           .Where(p => p.Length > 0)
                                           .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }
        catch
        {
            return;
        }

        // Publish the new snapshot atomically. UpdateQueues see the change
        // on the next trigger tick (volatile reference write).
        _alivePathsSnapshot = currentPaths;

        // Iterate a copy so concurrent removals during Reconcile don't
        // mutate the collection mid-enumeration.
        List<IPlayStationRGBDevice> snapshot = Devices.OfType<IPlayStationRGBDevice>().ToList();
        foreach (IPlayStationRGBDevice device in snapshot)
        {
            if (string.IsNullOrEmpty(device.DevicePath)) continue;
            if (currentPaths.Contains(device.DevicePath)) continue;
            if (device.IsKnownDisconnected) continue;

            device.MarkKnownDisconnected();
        }
    }

    // Compare current HID enumeration to the open set; add new ones, remove
    // gone ones. Called from the debounced PnP callback. Iterates the
    // device collection directly — each device knows its own DevicePath.
    private void Reconcile()
    {
        HashSet<string> currentPaths;
        try
        {
            currentPaths = DeviceList.Local.GetHidDevices(vendorID: SONY_VENDOR_ID)
                                           .Where(h => IsSupportedPid(h.ProductID))
                                           .Select(h => h.DevicePath ?? string.Empty)
                                           .Where(p => p.Length > 0)
                                           .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[RGB.NET.PlayStation] Reconcile enumeration failed: {ex.Message}");
            return;
        }

        List<IPlayStationRGBDevice> snapshot = Devices.OfType<IPlayStationRGBDevice>().ToList();

        // Removals first (devices held but no longer enumerated) — done before
        // adds so a controller that quickly reconnects on a different path can
        // be re-added cleanly.
        foreach (IPlayStationRGBDevice device in snapshot)
        {
            if (string.IsNullOrEmpty(device.DevicePath)) continue;
            if (currentPaths.Contains(device.DevicePath)) continue;

            if (!device.IsKnownDisconnected)
                device.MarkKnownDisconnected();
            RemoveDevice(device);
        }

        // Additions: any enumerated path not currently open. Re-snapshot Devices
        // after removals so a controller that disappeared and immediately
        // reconnected on the same path can be re-added.
        HashSet<string> openedPaths = Devices.OfType<IPlayStationRGBDevice>()
                                             .Select(d => d.DevicePath)
                                             .Where(p => !string.IsNullOrEmpty(p))
                                             .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (HidDevice hid in DeviceList.Local.GetHidDevices(vendorID: SONY_VENDOR_ID))
        {
            if (!IsSupportedPid(hid.ProductID)) continue;
            string path;
            try { path = hid.DevicePath ?? string.Empty; } catch { continue; }
            if (string.IsNullOrEmpty(path)) continue;
            if (openedPaths.Contains(path)) continue;

            if (TryOpenAndCreateDevice(hid, hid.ProductID, out IRGBDevice? newDevice) && newDevice != null)
            {
                // Refresh the alive-path snapshot BEFORE AddDevice so the first
                // trigger tick after AddDevice already sees the new device's
                // path.
                try { SuspendDeadDevices(); } catch { /* best effort */ }

                AddDevice(newDevice);

                // Make sure the DeviceUpdateTrigger is actually running.
                // AbstractRGBDeviceProvider.Initialize() calls Start() on every
                // trigger in UpdateTriggerMapping at the end of initial load —
                // but if no controllers were connected at launch, the trigger
                // wasn't created until just now. Start() is idempotent.
                try { GetUpdateTrigger().Start(); } catch { /* best effort */ }
            }
        }
    }

    #endregion

    #region Lifecycle

    /// <inheritdoc />
    protected override bool RemoveDevice(IRGBDevice device)
    {
        // Provider Dispose marks every device as known-disconnected first,
        // so the device's own Dispose skips the off-frame attempt in that
        // case. PnP-driven removal goes through Reconcile which also marks
        // first. Voluntary host-app removal of a still-connected device
        // (the rare case) leaves IsKnownDisconnected false, so the device
        // sends a graceful off-frame before tearing its stream down.
        bool removed = base.RemoveDevice(device);
        if (removed)
        {
            try { device.Dispose(); } catch { /* best effort */ }
        }
        return removed;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_hotplugSubscribed)
            {
                try { DeviceList.Local.Changed -= OnHidDeviceListChanged; } catch { /* best effort */ }
                _hotplugSubscribed = false;
            }

            // Voluntary teardown: the controller is still physically attached
            // and its HID handle is still valid (the OS doesn't invalidate
            // handles just because Dispose is being called). Leave
            // IsKnownDisconnected alone so each device's Dispose sees it as
            // false and sends a final all-zero output report — the lightbar
            // and player indicators blank out instead of freezing on the last
            // colour they were painted. HidRawWriter.TryWrite is non-throwing
            // anyway, so a stale handle just fails silently.
            //
            // Devices that were already torn down by the PnP path
            // (Reconcile / SuspendDeadDevices) have IsKnownDisconnected = true
            // and skip the off-frame correctly on their own.
            //
            // Iterate a copy because RemoveDevice mutates InternalDevices.
            List<IPlayStationRGBDevice> snapshot = Devices.OfType<IPlayStationRGBDevice>().ToList();
            foreach (IPlayStationRGBDevice d in snapshot)
            {
                try { RemoveDevice(d); } catch { /* best effort */ }
            }
        }

        base.Dispose(disposing);

        lock (_lock)
        {
            if (ReferenceEquals(_instance, this))
                _instance = null;
        }
    }

    #endregion

    #region Helpers

    // 12-char hex hash of an arbitrary string. Used to derive a stable
    // pseudo-serial from DevicePath when the controller's HID descriptor
    // doesn't expose a real serial — short enough to look reasonable in
    // the device name, long enough that two distinct USB instances of the
    // same product won't collide. Identity is the only requirement; cryptographic
    // strength is not.
    private static string ShortHashOf(string input)
    {
        byte[] hash = SHA1.HashData(Encoding.UTF8.GetBytes(input));
        StringBuilder sb = new(12);
        for (int i = 0; i < 6; i++) sb.Append(hash[i].ToString("X2"));
        return sb.ToString();
    }

    #endregion
}
