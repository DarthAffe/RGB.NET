using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace RGB.NET.Devices.PlayStation;

// Direct Win32 WriteFile wrapper for HID output reports — Windows only.
//
// On Windows, HidSharp's HidStream opens its underlying handle with
// FILE_FLAG_OVERLAPPED and runs an asynchronous WriteFile + GetOverlappedResult
// dance internally. Field reports against the PlayStation USB minidriver
// indicate that overlapped path can return non-ERROR_IO_PENDING failure on the
// second and subsequent writes, which HidSharp surfaces as IOException — and
// our queue's catch handler turns the exception into a queue suspension. End
// result: lightbar updates once, then freezes.
//
// To sidestep the overlapped I/O path entirely we open OUR OWN kernel handle
// alongside HidSharp's, with shared read/write access and dwFlagsAndAttributes
// = 0 (synchronous I/O, no overlapped). WriteFile then returns BOOL — false on
// failure surfaces as a return value, no exception. HidSharp keeps the handle
// it opens during TryOpen (still useful for detecting exclusive-access
// conflicts via DS4Windows / reWASD at open time). Two handles per controller
// is fine — Sony HID gamepads accept shared writes by default on Windows.
//
// The class is intentionally minimal: open with shared read/write access,
// write a buffer, dispose. No reads, no overlapped I/O, no internal locking
// (the caller's UpdateQueue already serialises via its own write lock).
//
// Windows-only at runtime — the kernel32 P/Invokes will throw
// DllNotFoundException on Linux/macOS. Construction is gated on
// OperatingSystem.IsWindows() inside the provider; the class itself is not
// platform-attributed to keep call sites readable across the assembly.
internal sealed class HidRawWriter : IDisposable
{
    #region Win32

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern SafeFileHandle CreateFileW(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool WriteFile(
        SafeFileHandle hFile,
        byte[] lpBuffer,
        uint nNumberOfBytesToWrite,
        out uint lpNumberOfBytesWritten,
        IntPtr lpOverlapped);

    private const uint GENERIC_WRITE = 0x40000000;
    private const uint FILE_SHARE_READ = 0x00000001;
    private const uint FILE_SHARE_WRITE = 0x00000002;
    private const uint OPEN_EXISTING = 3;

    #endregion

    #region Properties & Fields

    private readonly SafeFileHandle _handle;
    private volatile bool _closed;

    #endregion

    #region Constructors

    public HidRawWriter(string devicePath)
    {
        if (string.IsNullOrEmpty(devicePath))
            throw new ArgumentException("Device path is required.", nameof(devicePath));

        // Shared read/write so we coexist with HidSharp's handle and any other
        // app (Steam Input, game native lighting, etc.) that may also be
        // opening the device. dwFlagsAndAttributes = 0 → synchronous I/O. We
        // don't need overlapped — writes are small (78 bytes max) and our
        // caller is already on a dedicated trigger thread.
        _handle = CreateFileW(
            devicePath,
            GENERIC_WRITE,
            FILE_SHARE_READ | FILE_SHARE_WRITE,
            IntPtr.Zero,
            OPEN_EXISTING,
            0,
            IntPtr.Zero);

        if (_handle == null || _handle.IsInvalid)
        {
            int err = Marshal.GetLastWin32Error();
            throw new IOException($"CreateFile failed for HID device ({devicePath}). Win32 error: {err}");
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// True on successful write, false on any failure (handle invalid, device
    /// gone, partial write, etc.). Never throws — that's the whole point.
    /// Caller checks the return value and decides whether to log, retry, or
    /// self-suspend the queue.
    ///
    /// The first byte of <paramref name="buffer"/> must be the HID report ID,
    /// matching the convention HidStream.Write uses.
    /// </summary>
    public bool TryWrite(byte[] buffer)
    {
        if (_closed) return false;
        if (buffer == null || buffer.Length == 0) return false;

        try
        {
            if (_handle == null || _handle.IsClosed || _handle.IsInvalid)
                return false;

            return WriteFile(_handle, buffer, (uint)buffer.Length, out _, IntPtr.Zero);
        }
        catch
        {
            // P/Invoke marshalling could conceivably fault on a pathological
            // handle state; swallow and report failure rather than escape the
            // contract.
            return false;
        }
    }

    public void Dispose()
    {
        if (_closed) return;
        _closed = true;
        try { _handle?.Dispose(); } catch { /* best effort */ }
    }

    #endregion
}
