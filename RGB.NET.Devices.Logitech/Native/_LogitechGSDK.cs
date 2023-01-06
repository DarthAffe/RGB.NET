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
internal static class _LogitechGSDK
{
    #region Libary Management

    private static IntPtr _handle = IntPtr.Zero;

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
        if (_handle != IntPtr.Zero) return;

        List<string> possiblePathList = GetPossibleLibraryPaths().ToList();

        string? dllPath = possiblePathList.FirstOrDefault(File.Exists);
        if (dllPath == null) throw new RGBDeviceException($"Can't find the Logitech-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

        if (!NativeLibrary.TryLoad(dllPath, out _handle))
#if NET6_0
            throw new RGBDeviceException($"Logitech LoadLibrary failed with error code {Marshal.GetLastPInvokeError()}");
#else
            throw new RGBDeviceException($"Logitech LoadLibrary failed with error code {Marshal.GetLastWin32Error()}");
#endif

        if (!NativeLibrary.TryGetExport(_handle, "LogiLedInit", out _logiLedInitPointer)) throw new RGBDeviceException("Failed to load Logitech function 'LogiLedInit'");
        if (!NativeLibrary.TryGetExport(_handle, "LogiLedShutdown", out _logiLedShutdownPointer)) throw new RGBDeviceException("Failed to load Logitech function 'LogiLedShutdown'");
        if (!NativeLibrary.TryGetExport(_handle, "LogiLedSetTargetDevice", out _logiLedSetTargetDevicePointer)) throw new RGBDeviceException("Failed to load Logitech function 'LogiLedSetTargetDevice'");
        if (!NativeLibrary.TryGetExport(_handle, "LogiLedGetSdkVersion", out _logiLedGetSdkVersionPointer)) throw new RGBDeviceException("Failed to load Logitech function 'LogiLedGetSdkVersion'");
        if (!NativeLibrary.TryGetExport(_handle, "LogiLedSaveCurrentLighting", out _lgiLedSaveCurrentLightingPointer)) throw new RGBDeviceException("Failed to load Logitech function 'LogiLedSaveCurrentLighting'");
        if (!NativeLibrary.TryGetExport(_handle, "LogiLedRestoreLighting", out _logiLedRestoreLightingPointer)) throw new RGBDeviceException("Failed to load Logitech function 'LogiLedRestoreLighting'");
        if (!NativeLibrary.TryGetExport(_handle, "LogiLedSetLighting", out _logiLedSetLightingPointer)) throw new RGBDeviceException("Failed to load Logitech function 'LogiLedSetLighting'");
        if (!NativeLibrary.TryGetExport(_handle, "LogiLedSetLightingForKeyWithKeyName", out _logiLedSetLightingForKeyWithKeyNamePointer)) throw new RGBDeviceException("Failed to load Logitech function 'LogiLedSetLightingForKeyWithKeyName'");
        if (!NativeLibrary.TryGetExport(_handle, "LogiLedSetLightingFromBitmap", out _logiLedSetLightingFromBitmapPointer)) throw new RGBDeviceException("Failed to load Logitech function 'LogiLedSetLightingFromBitmap'");
        if (!NativeLibrary.TryGetExport(_handle, "LogiLedSetLightingForTargetZone", out _logiLedSetLightingForTargetZonePointer)) throw new RGBDeviceException("Failed to load Logitech function 'LogiLedSetLightingForTargetZone'");
    }

    private static IEnumerable<string> GetPossibleLibraryPaths()
    {
        IEnumerable<string> possibleLibraryPaths;

        if (OperatingSystem.IsWindows())
            possibleLibraryPaths = Environment.Is64BitProcess ? LogitechDeviceProvider.PossibleX64NativePaths : LogitechDeviceProvider.PossibleX86NativePaths;
        else
            possibleLibraryPaths = Enumerable.Empty<string>();

        return possibleLibraryPaths.Select(Environment.ExpandEnvironmentVariables);
    }

    internal static void UnloadLogitechGSDK()
    {
        if (_handle == IntPtr.Zero) return;

        _logiLedInitPointer = IntPtr.Zero;
        _logiLedShutdownPointer = IntPtr.Zero;
        _logiLedSetTargetDevicePointer = IntPtr.Zero;
        _logiLedGetSdkVersionPointer = IntPtr.Zero;
        _lgiLedSaveCurrentLightingPointer = IntPtr.Zero;
        _logiLedRestoreLightingPointer = IntPtr.Zero;
        _logiLedSetLightingPointer = IntPtr.Zero;
        _logiLedSetLightingForKeyWithKeyNamePointer = IntPtr.Zero;
        _logiLedSetLightingFromBitmapPointer = IntPtr.Zero;
        _logiLedSetLightingForTargetZonePointer = IntPtr.Zero;

        NativeLibrary.Free(_handle);
        _handle = IntPtr.Zero;
    }

    #endregion

    #region SDK-METHODS

    #region Pointers

    private static IntPtr _logiLedInitPointer;
    private static IntPtr _logiLedShutdownPointer;
    private static IntPtr _logiLedSetTargetDevicePointer;
    private static IntPtr _logiLedGetSdkVersionPointer;
    private static IntPtr _lgiLedSaveCurrentLightingPointer;
    private static IntPtr _logiLedRestoreLightingPointer;
    private static IntPtr _logiLedSetLightingPointer;
    private static IntPtr _logiLedSetLightingForKeyWithKeyNamePointer;
    private static IntPtr _logiLedSetLightingFromBitmapPointer;
    private static IntPtr _logiLedSetLightingForTargetZonePointer;

    #endregion

    internal static unsafe bool LogiLedInit()
        => ((delegate* unmanaged[Cdecl]<bool>)ThrowIfZero(_logiLedInitPointer))();

    internal static unsafe void LogiLedShutdown()
        => ((delegate* unmanaged[Cdecl]<void>)ThrowIfZero(_logiLedShutdownPointer))();

    internal static unsafe bool LogiLedSetTargetDevice(LogitechDeviceCaps targetDevice)
        => ((delegate* unmanaged[Cdecl]<int, bool>)ThrowIfZero(_logiLedSetTargetDevicePointer))((int)targetDevice);

    internal static string LogiLedGetSdkVersion()
    {
        int major = 0;
        int minor = 0;
        int build = 0;
        LogiLedGetSdkVersion(ref major, ref minor, ref build);

        return $"{major}.{minor}.{build}";
    }

    internal static unsafe bool LogiLedGetSdkVersion(ref int majorNum, ref int minorNum, ref int buildNum)
        => ((delegate* unmanaged[Cdecl]<ref int, ref int, ref int, bool>)ThrowIfZero(_logiLedGetSdkVersionPointer))(ref majorNum, ref minorNum, ref buildNum);

    internal static unsafe bool LogiLedSaveCurrentLighting()
        => ((delegate* unmanaged[Cdecl]<bool>)ThrowIfZero(_lgiLedSaveCurrentLightingPointer))();

    internal static unsafe bool LogiLedRestoreLighting()
        => ((delegate* unmanaged[Cdecl]<bool>)ThrowIfZero(_logiLedRestoreLightingPointer))();

    internal static unsafe bool LogiLedSetLighting(int redPercentage, int greenPercentage, int bluePercentage)
        => ((delegate* unmanaged[Cdecl]<int, int, int, bool>)ThrowIfZero(_logiLedSetLightingPointer))(redPercentage, greenPercentage, bluePercentage);

    internal static unsafe bool LogiLedSetLightingForKeyWithKeyName(int keyCode, int redPercentage, int greenPercentage, int bluePercentage)
        => ((delegate* unmanaged[Cdecl]<int, int, int, int, bool>)ThrowIfZero(_logiLedSetLightingForKeyWithKeyNamePointer))(keyCode, redPercentage, greenPercentage, bluePercentage);

    internal static unsafe bool LogiLedSetLightingFromBitmap(byte[] bitmap)
        => ((delegate* unmanaged[Cdecl]<byte[], bool>)ThrowIfZero(_logiLedSetLightingFromBitmapPointer))(bitmap);

    internal static unsafe bool LogiLedSetLightingForTargetZone(LogitechDeviceType deviceType, int zone, int redPercentage, int greenPercentage, int bluePercentage)
        => ((delegate* unmanaged[Cdecl]<LogitechDeviceType, int, int, int, int, bool>)ThrowIfZero(_logiLedSetLightingForTargetZonePointer))(deviceType, zone, redPercentage, greenPercentage, bluePercentage);

    private static IntPtr ThrowIfZero(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero) throw new RGBDeviceException("The Logitech-SDK is not initialized.");
        return ptr;
    }

    #endregion
}