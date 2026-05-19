[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for Sony PlayStation controllers (DualShock 4, DualSense, DualSense Edge).

## Usage
This provider follows the default pattern and does not require additional setup.

```csharp
surface.Load(PlayStationDeviceProvider.Instance);
```

The provider auto-detects connected controllers at load time and tracks hot-plug events for the lifetime of the provider — no rescan call is required when controllers are connected, disconnected, or paired/unpaired over Bluetooth.

# Required SDK
This provider does not require an additional SDK. It speaks raw HID via the bundled `HidSharp` dependency (transitive through `RGB.NET.HID`); no Sony driver, no DS4Windows, no SignalRGB, no HidHide, no DualSenseX.

## Supported devices

| Controller | USB | Bluetooth | Notes |
|---|---|---|---|
| DualShock 4 v1 (PID `0x05C4`) | yes | yes | Original "JDM-001/011" |
| DualShock 4 v2 (PID `0x09CC`) | yes | yes | Revised "JDM-040/050/055"; lightbar visible through touchpad |
| DualShock 4 Wireless Adapter (PID `0x0BA0`) | n/a | yes | Sony's official BT bridge — treated like DS4 v2 |
| DualSense (PID `0x0CE6`) | yes | yes | PS5 launch controller "CFI-ZCT1" |
| DualSense Edge (PID `0x0DF2`) | yes | yes | Pro variant "CFI-ZCP1" |

## LED layout

| `LedId` | DualShock 4 | DualSense / DualSense Edge |
|---|---|---|
| `Custom1` | Lightbar (RGB) | Lightbar (RGB) |
| `Custom2` | — | Player indicator 1 (leftmost, monochrome) |
| `Custom3` | — | Player indicator 2 (monochrome) |
| `Custom4` | — | Player indicator 3 (monochrome) |
| `Custom5` | — | Player indicator 4 (monochrome) |
| `Custom6` | — | Player indicator 5 (rightmost, monochrome) |

`Custom1` is the lightbar on both controller families, so a host-side mapping for "Custom 1" carries sensible meaning across DS4 and DS5.

The DualSense player indicator LEDs are monochrome — they don't accept colour, only on/off. Any non-black colour written to `Custom2`..`Custom6` lights the corresponding indicator at full brightness; pure black turns it off.

## Limitations

### Not exposed
- **DualSense mic-mute LED.** Deliberately left under firmware control. The mic-mute *button* mutes the microphone in hardware regardless of host activity — taking control of the LED would suppress visual feedback for an action that still happens. The provider does not set the `MIC_MUTE_LED_CONTROL_ENABLE` bit in the output report, so the firmware retains its default LED-tracks-mute-state behaviour.
- **Rumble, adaptive triggers, haptics.** This provider is lighting only. Output reports are written with rumble/audio/haptic fields zeroed and their corresponding `valid_flag` bits clear, so the firmware ignores those fields and games / Steam Input continue to drive them normally.
- **Audio routing, headset volume, speaker volume, microphone gain.** Same as above — fields are zeroed, flags are clear.

### Coexistence
- **Shared HID writes.** Sony's HID gamepads accept shared output writes by default on Windows. Steam Input, a game's native lighting integration, and this provider can all write to the same controller — last writer wins per output report period. At the provider's 30Hz cadence intermittent setters (Steam profile changes, in-game state events) get overridden quickly enough that the net visual is the host app's colour.
- **DS4Windows / reWASD with "Exclusive Mode" enabled** hold the HID handle exclusive. The provider's `TryOpen` call fails (`UnauthorizedAccessException` / `IOException`) and the controller is skipped. Diagnostic message logged via `Trace.WriteLine`. Disable exclusive mode in those tools or close them to restore lighting.
- **HidHide** hiding the controller from non-allow-listed apps means the device never appears in the HidSharp enumeration — indistinguishable from "controller not connected". Add the host application to HidHide's allow-list.
- **The DualSense lightbar boot animation.** A fresh DualSense connection plays a fade-in animation that overrides host-driven colours until released. The provider sends the `LIGHTBAR_SETUP_RELEASE_LEDS` setup-control bit on the first output report after open, so host control takes effect immediately on connect.

### Identity
- **Device identity is derived from the OS device path**, hashed to a 12-character hex tag and embedded in `DeviceName`. Stable across application restarts for the same physical controller in the same USB port / BT pairing. Switching the same controller between USB and Bluetooth produces a different `DeviceName`, so any host-side state keyed by device name won't auto-apply across transports.
- **`PlayStationDeviceInfo.SerialNumber` is the path-derived hash, not the controller's HID serial descriptor.** The descriptor query (`HidDevice.GetSerialNumber`) opens a separate read-info handle that throws `DeviceIOException("Failed to get info.")` on some hardware (DS4 v1 in particular, and any controller whose descriptor query is blocked by Steam / driver / power state). The path-derived hash is sufficient for identity and avoids the throw.

### Platform
- **Windows**: each opened controller gets a second synchronous Win32 `WriteFile` handle (`HidRawWriter`) alongside the HidSharp-managed `HidStream`. All output reports go through the synchronous handle. HidSharp's `HidStream.Write` opens its handle with `FILE_FLAG_OVERLAPPED` and runs an asynchronous `WriteFile` + `GetOverlappedResult` dance internally; against the PlayStation USB HID minidriver that overlapped path returns failure on the second and subsequent writes, manifesting as "lightbar updates once and then freezes". The synchronous handle sidesteps the issue entirely. The two-handle overhead is negligible — Sony HID gamepads accept shared writes by default. If the second handle fails to open (rare), the queue falls back to `HidStream.Write` and logs a diagnostic.
- **macOS / Linux**: HidSharp's platform implementations don't use the same overlapped Windows code path, so the provider uses `HidStream.Write` directly. The controllers' BT output report formats and PnP semantics have only been verified on Windows; macOS and Linux are expected to work for USB but Bluetooth has not been tested. Reports welcome.
- On Linux, `hid-playstation` (kernel 5.12+) drives most lighting itself and may compete with this provider for output reports — last writer wins, but the kernel's player-LED logic may overwrite host-driven indicators.

## Protocol references

The output report layouts are the same ones the Linux kernel `hid-playstation` driver uses. See [`drivers/hid/hid-playstation.c`](https://github.com/torvalds/linux/blob/master/drivers/hid/hid-playstation.c) — specifically `struct dualshock4_output_report_{usb,bt}` and `struct dualsense_output_report_common`. The `PlayStationCrc32` helper mirrors the same `crc32_le` seed-byte convention the kernel uses for BT reports.
