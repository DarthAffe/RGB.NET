#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech.Native;

// ReSharper disable once InconsistentNaming
internal class _LogitechGSDK
{
    #region Libary Management

    private static IntPtr _dllHandle = IntPtr.Zero;
        
    /// <summary>
    /// Reloads the SDK.
    /// </summary>
    internal static void Reload()
    {
        UnloadLogitechGSDK();
        LoadLogitechGSDK();
    }

    private static void LoadLogitechGSDK()
    {
        if (_dllHandle != IntPtr.Zero) return;

        // HACK: Load library at runtime to support both, x86 and x64 with one managed dll
        List<string> possiblePathList = Environment.Is64BitProcess ? LogitechDeviceProvider.PossibleX64NativePaths : LogitechDeviceProvider.PossibleX86NativePaths;
        string? dllPath = possiblePathList.FirstOrDefault(File.Exists);
        if (dllPath == null) throw new RGBDeviceException($"Can't find the Logitech-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

        _dllHandle = LoadLibrary(dllPath);
        if (_dllHandle == IntPtr.Zero) throw new RGBDeviceException($"Logitech LoadLibrary failed with error code {Marshal.GetLastWin32Error()}");

        _logiLedInitPointer = (LogiLedInitPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedInit"), typeof(LogiLedInitPointer));
        _logiLedShutdownPointer = (LogiLedShutdownPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedShutdown"), typeof(LogiLedShutdownPointer));
        _logiLedSetTargetDevicePointer = (LogiLedSetTargetDevicePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedSetTargetDevice"), typeof(LogiLedSetTargetDevicePointer));
        _logiLedGetSdkVersionPointer = (LogiLedGetSdkVersionPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedGetSdkVersion"), typeof(LogiLedGetSdkVersionPointer));
        _lgiLedSaveCurrentLightingPointer = (LogiLedSaveCurrentLightingPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedSaveCurrentLighting"), typeof(LogiLedSaveCurrentLightingPointer));
        _logiLedRestoreLightingPointer = (LogiLedRestoreLightingPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedRestoreLighting"), typeof(LogiLedRestoreLightingPointer));
        _logiLedSetLightingPointer = (LogiLedSetLightingPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedSetLighting"), typeof(LogiLedSetLightingPointer));
        _logiLedSetLightingForKeyWithKeyNamePointer = (LogiLedSetLightingForKeyWithKeyNamePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedSetLightingForKeyWithKeyName"), typeof(LogiLedSetLightingForKeyWithKeyNamePointer));
        _logiLedSetLightingFromBitmapPointer = (LogiLedSetLightingFromBitmapPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedSetLightingFromBitmap"), typeof(LogiLedSetLightingFromBitmapPointer));
        _logiLedSetLightingForTargetZonePointer = (LogiLedSetLightingForTargetZonePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedSetLightingForTargetZone"), typeof(LogiLedSetLightingForTargetZonePointer));
    }

    internal static void UnloadLogitechGSDK()
    {
        if (_dllHandle == IntPtr.Zero) return;

        LogiLedShutdown();

        // ReSharper disable once EmptyEmbeddedStatement - DarthAffe 20.02.2016: We might need to reduce the internal reference counter more than once to set the library free
        while (FreeLibrary(_dllHandle)) ;
        _dllHandle = IntPtr.Zero;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr LoadLibrary(string dllToLoad);

    [DllImport("kernel32.dll")]
    private static extern bool FreeLibrary(IntPtr dllHandle);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr GetProcAddress(IntPtr dllHandle, string name);

    #endregion

    #region SDK-METHODS

    #region Pointers

    private static LogiLedInitPointer? _logiLedInitPointer;
    private static LogiLedShutdownPointer? _logiLedShutdownPointer;
    private static LogiLedSetTargetDevicePointer? _logiLedSetTargetDevicePointer;
    private static LogiLedGetSdkVersionPointer? _logiLedGetSdkVersionPointer;
    private static LogiLedSaveCurrentLightingPointer? _lgiLedSaveCurrentLightingPointer;
    private static LogiLedRestoreLightingPointer? _logiLedRestoreLightingPointer;
    private static LogiLedSetLightingPointer? _logiLedSetLightingPointer;
    private static LogiLedSetLightingForKeyWithKeyNamePointer? _logiLedSetLightingForKeyWithKeyNamePointer;
    private static LogiLedSetLightingFromBitmapPointer? _logiLedSetLightingFromBitmapPointer;
    private static LogiLedSetLightingForTargetZonePointer? _logiLedSetLightingForTargetZonePointer;

    #endregion

    #region Delegates

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool LogiLedInitPointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void LogiLedShutdownPointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool LogiLedSetTargetDevicePointer(int targetDevice);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool LogiLedGetSdkVersionPointer(ref int majorNum, ref int minorNum, ref int buildNum);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool LogiLedSaveCurrentLightingPointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool LogiLedRestoreLightingPointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool LogiLedSetLightingPointer(int redPercentage, int greenPercentage, int bluePercentage);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool LogiLedSetLightingForKeyWithKeyNamePointer(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool LogiLedSetLightingFromBitmapPointer(byte[] bitmap);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool LogiLedSetLightingForTargetZonePointer(LogitechDeviceType deviceType, int zone, int redPercentage, int greenPercentage, int bluePercentage);

    #endregion

    // ReSharper disable EventExceptionNotDocumented

    internal static bool LogiLedInit() => (_logiLedInitPointer ?? throw new RGBDeviceException("The Logitech-GSDK is not initialized.")).Invoke();

    internal static void LogiLedShutdown() => (_logiLedShutdownPointer ?? throw new RGBDeviceException("The Logitech-GSDK is not initialized.")).Invoke();

    internal static bool LogiLedSetTargetDevice(LogitechDeviceCaps targetDevice) => (_logiLedSetTargetDevicePointer ?? throw new RGBDeviceException("The Logitech-GSDK is not initialized.")).Invoke((int)targetDevice);

    internal static string LogiLedGetSdkVersion()
    {
        int major = 0;
        int minor = 0;
        int build = 0;
        LogiLedGetSdkVersion(ref major, ref minor, ref build);

        return $"{major}.{minor}.{build}";
    }

    internal static bool LogiLedGetSdkVersion(ref int majorNum, ref int minorNum, ref int buildNum) =>
        (_logiLedGetSdkVersionPointer ?? throw new RGBDeviceException("The Logitech-GSDK is not initialized.")).Invoke(ref majorNum, ref minorNum, ref buildNum);

    internal static bool LogiLedSaveCurrentLighting() => (_lgiLedSaveCurrentLightingPointer ?? throw new RGBDeviceException("The Logitech-GSDK is not initialized.")).Invoke();

    internal static bool LogiLedRestoreLighting() => (_logiLedRestoreLightingPointer ?? throw new RGBDeviceException("The Logitech-GSDK is not initialized.")).Invoke();

    internal static bool LogiLedSetLighting(int redPercentage, int greenPercentage, int bluePercentage) =>
        (_logiLedSetLightingPointer ?? throw new RGBDeviceException("The Logitech-GSDK is not initialized.")).Invoke(redPercentage, greenPercentage, bluePercentage);

    internal static bool LogiLedSetLightingForKeyWithKeyName(int keyCode, int redPercentage, int greenPercentage, int bluePercentage)
        => (_logiLedSetLightingForKeyWithKeyNamePointer ?? throw new RGBDeviceException("The Logitech-GSDK is not initialized.")).Invoke(keyCode, redPercentage, greenPercentage, bluePercentage);

    internal static bool LogiLedSetLightingFromBitmap(byte[] bitmap) => (_logiLedSetLightingFromBitmapPointer ?? throw new RGBDeviceException("The Logitech-GSDK is not initialized.")).Invoke(bitmap);

    internal static bool LogiLedSetLightingForTargetZone(LogitechDeviceType deviceType, int zone, int redPercentage, int greenPercentage, int bluePercentage)
        => (_logiLedSetLightingForTargetZonePointer ?? throw new RGBDeviceException("The Logitech-GSDK is not initialized.")).Invoke(deviceType, zone, redPercentage, greenPercentage, bluePercentage);

    // ReSharper restore EventExceptionNotDocumented

    #endregion
}