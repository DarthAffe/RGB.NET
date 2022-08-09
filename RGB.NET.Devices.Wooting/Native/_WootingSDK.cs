// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        // HACK: Load library at runtime to support both, x86 and x64 with one managed dll
        List<string> possiblePathList = (Environment.Is64BitProcess ? WootingDeviceProvider.PossibleX64NativePaths : WootingDeviceProvider.PossibleX86NativePaths)
                                        .Select(Environment.ExpandEnvironmentVariables)
                                        .ToList();
        string? dllPath = possiblePathList.FirstOrDefault(File.Exists);
        if (dllPath == null) throw new RGBDeviceException($"Can't find the Wooting-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

            
        _handle = NativeLibrary.Load(dllPath);
        if (_handle == IntPtr.Zero) throw new RGBDeviceException($"Wooting LoadLibrary failed with error code {Marshal.GetLastWin32Error()}");
        
        _getDeviceInfoPointer = (GetDeviceInfoPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "wooting_rgb_device_info"), typeof(GetDeviceInfoPointer));
        _keyboardConnectedPointer = (KeyboardConnectedPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "wooting_rgb_kbd_connected"), typeof(KeyboardConnectedPointer));
        _resetPointer = (ResetPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "wooting_rgb_reset_rgb"), typeof(ResetPointer));
        _closePointer = (ClosePointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "wooting_rgb_close"), typeof(ClosePointer));
        _arrayUpdateKeyboardPointer = (ArrayUpdateKeyboardPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "wooting_rgb_array_update_keyboard"), typeof(ArrayUpdateKeyboardPointer));
        _arraySetSinglePointer = (ArraySetSinglePointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "wooting_rgb_array_set_single"), typeof(ArraySetSinglePointer));
        _getDeviceCountPointer = (GetDeviceCountPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "wooting_usb_keyboard_count"), typeof(GetDeviceCountPointer));
        _selectDevicePointer = (SelectDevicePointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "wooting_usb_select_device"), typeof(SelectDevicePointer));
    }


    internal static void UnloadWootingSDK()
    {
        if (_handle == IntPtr.Zero) return;

        Reset();
        Close();

        _getDeviceInfoPointer = null;
        _keyboardConnectedPointer = null;
        _arrayUpdateKeyboardPointer = null;
        _arraySetSinglePointer = null;
        _resetPointer = null;
        _closePointer = null;

        // ReSharper disable once EmptyEmbeddedStatement - DarthAffe 20.02.2016: We might need to reduce the internal reference counter more than once to set the library free
        NativeLibrary.Free(_handle);
        _handle = IntPtr.Zero;
    }

    #endregion

    #region SDK-METHODS

    #region Pointers

    private static GetDeviceInfoPointer? _getDeviceInfoPointer;
    private static KeyboardConnectedPointer? _keyboardConnectedPointer;
    private static ResetPointer? _resetPointer;
    private static ClosePointer? _closePointer;
    private static ArrayUpdateKeyboardPointer? _arrayUpdateKeyboardPointer;
    private static ArraySetSinglePointer? _arraySetSinglePointer;
    private static GetDeviceCountPointer? _getDeviceCountPointer;
    private static SelectDevicePointer? _selectDevicePointer;

    #endregion

    #region Delegates

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr GetDeviceInfoPointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool KeyboardConnectedPointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool ResetPointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool ClosePointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool ArrayUpdateKeyboardPointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool ArraySetSinglePointer(byte row, byte column, byte red, byte green, byte blue);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate byte GetDeviceCountPointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void SelectDevicePointer(byte index);

    #endregion

    internal static IntPtr GetDeviceInfo() => (_getDeviceInfoPointer ?? throw new RGBDeviceException("The Wooting-SDK is not initialized.")).Invoke();
    internal static bool KeyboardConnected() => (_keyboardConnectedPointer ?? throw new RGBDeviceException("The Wooting-SDK is not initialized.")).Invoke();
    internal static bool Reset() => (_resetPointer ?? throw new RGBDeviceException("The Wooting-SDK is not initialized.")).Invoke();
    internal static bool Close() => (_closePointer ?? throw new RGBDeviceException("The Wooting-SDK is not initialized.")).Invoke();
    internal static bool ArrayUpdateKeyboard() => (_arrayUpdateKeyboardPointer ?? throw new RGBDeviceException("The Wooting-SDK is not initialized.")).Invoke();
    internal static bool ArraySetSingle(byte row, byte column, byte red, byte green, byte blue) => (_arraySetSinglePointer ?? throw new RGBDeviceException("The Wooting-SDK is not initialized.")).Invoke(row, column, red, green, blue);
    internal static byte GetDeviceCount() => (_getDeviceCountPointer ?? throw new RGBDeviceException("The Wooting-SDK is not initialized.")).Invoke();
    internal static void SelectDevice(byte index) => (_selectDevicePointer ?? throw new RGBDeviceException("The Wooting-SDK is not initialized.")).Invoke(index);
    #endregion
}