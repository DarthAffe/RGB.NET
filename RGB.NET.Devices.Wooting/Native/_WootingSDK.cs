// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;

namespace RGB.NET.Devices.Wooting.Native;

// ReSharper disable once InconsistentNaming
internal static class _WootingSDK
{
    #region Library management

    private static IntPtr _handle = IntPtr.Zero;
    internal static object SdkLock = new();

    /// <summary>
    /// Reloads the SDK.
    /// </summary>
    internal static void Reload()
    {
        UnloadWootingSDK();
        LoadWootingSDK();
    }

    private static void LoadWootingSDK()
    {
        if (_handle != IntPtr.Zero) return;

        List<string> possiblePathList = GetPossibleLibraryPaths().ToList();

        string? dllPath = possiblePathList.FirstOrDefault(File.Exists);
        if (dllPath == null) throw new RGBDeviceException($"Can't find the Wooting-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

        if (!NativeLibrary.TryLoad(dllPath, out _handle)) throw new RGBDeviceException($"Wooting LoadLibrary failed with error code {Marshal.GetLastWin32Error()}");

        if (!NativeLibrary.TryGetExport(_handle, "wooting_rgb_device_info", out _getDeviceInfoPointer)) throw new RGBDeviceException("Failed to load wooting function 'wooting_rgb_device_info'");
        if (!NativeLibrary.TryGetExport(_handle, "wooting_rgb_kbd_connected", out _keyboardConnectedPointer)) throw new RGBDeviceException("Failed to load wooting function 'wooting_rgb_kbd_connected'");
        if (!NativeLibrary.TryGetExport(_handle, "wooting_rgb_reset_rgb", out _resetPointer)) throw new RGBDeviceException("Failed to load wooting function 'wooting_rgb_reset_rgb'");
        if (!NativeLibrary.TryGetExport(_handle, "wooting_rgb_close", out _closePointer)) throw new RGBDeviceException("Failed to load wooting function 'wooting_rgb_close'");
        if (!NativeLibrary.TryGetExport(_handle, "wooting_rgb_array_update_keyboard", out _arrayUpdateKeyboardPointer)) throw new RGBDeviceException("Failed to load wooting function 'wooting_rgb_array_update_keyboard'");
        if (!NativeLibrary.TryGetExport(_handle, "wooting_rgb_array_set_single", out _arraySetSinglePointer)) throw new RGBDeviceException("Failed to load wooting function 'wooting_rgb_array_set_single'");
        if (!NativeLibrary.TryGetExport(_handle, "wooting_usb_keyboard_count", out _getDeviceCountPointer)) throw new RGBDeviceException("Failed to load wooting function 'wooting_usb_keyboard_count'");
        if (!NativeLibrary.TryGetExport(_handle, "wooting_usb_select_device", out _selectDevicePointer)) throw new RGBDeviceException("Failed to load wooting function 'wooting_usb_select_device'");
    }

    private static IEnumerable<string> GetPossibleLibraryPaths()
    {
        IEnumerable<string> possibleLibraryPaths;

        if (OperatingSystem.IsWindows())
            possibleLibraryPaths = Environment.Is64BitProcess ? WootingDeviceProvider.PossibleX64NativePaths : WootingDeviceProvider.PossibleX86NativePaths;
        else if (OperatingSystem.IsLinux())
            possibleLibraryPaths = Environment.Is64BitProcess ? WootingDeviceProvider.PossibleX64NativePathsLinux : WootingDeviceProvider.PossibleX86NativePathsLinux;
        else
            possibleLibraryPaths = Enumerable.Empty<string>();

        return possibleLibraryPaths.Select(Environment.ExpandEnvironmentVariables);
    }

    internal static void UnloadWootingSDK()
    {
        if (_handle == IntPtr.Zero) return;

        Close();

        _getDeviceInfoPointer = IntPtr.Zero;
        _keyboardConnectedPointer = IntPtr.Zero;
        _resetPointer = IntPtr.Zero;
        _closePointer = IntPtr.Zero;
        _arrayUpdateKeyboardPointer = IntPtr.Zero;
        _arraySetSinglePointer = IntPtr.Zero;
        _getDeviceCountPointer = IntPtr.Zero;
        _selectDevicePointer = IntPtr.Zero;

        NativeLibrary.Free(_handle);
        _handle = IntPtr.Zero;
    }

    #endregion

    #region SDK-METHODS

    #region Pointers

    private static IntPtr _getDeviceInfoPointer;
    private static IntPtr _keyboardConnectedPointer;
    private static IntPtr _resetPointer;
    private static IntPtr _closePointer;
    private static IntPtr _arrayUpdateKeyboardPointer;
    private static IntPtr _arraySetSinglePointer;
    private static IntPtr _getDeviceCountPointer;
    private static IntPtr _selectDevicePointer;

    #endregion

    internal static unsafe IntPtr GetDeviceInfo() => ((delegate* unmanaged[Cdecl]<IntPtr>)ThrowIfZero(_getDeviceInfoPointer))();
    internal static unsafe bool KeyboardConnected() => ((delegate* unmanaged[Cdecl]<bool>)ThrowIfZero(_keyboardConnectedPointer))();
    internal static unsafe bool Reset() => ((delegate* unmanaged[Cdecl]<bool>)ThrowIfZero(_resetPointer))();
    internal static unsafe bool Close() => ((delegate* unmanaged[Cdecl]<bool>)ThrowIfZero(_closePointer))();
    internal static unsafe bool ArrayUpdateKeyboard() => ((delegate* unmanaged[Cdecl]<bool>)ThrowIfZero(_arrayUpdateKeyboardPointer))();
    internal static unsafe bool ArraySetSingle(byte row, byte column, byte red, byte green, byte blue)
        => ((delegate* unmanaged[Cdecl]<byte, byte, byte, byte, byte, bool>)ThrowIfZero(_arraySetSinglePointer))(row, column, red, green, blue);
    internal static unsafe byte GetDeviceCount() => ((delegate* unmanaged[Cdecl]<byte>)ThrowIfZero(_getDeviceCountPointer))();
    internal static unsafe void SelectDevice(byte index) => ((delegate* unmanaged[Cdecl]<byte, void>)ThrowIfZero(_selectDevicePointer))(index);

    private static IntPtr ThrowIfZero(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero) throw new RGBDeviceException("The Wooting-SDK is not initialized.");
        return ptr;
    }

    #endregion
}