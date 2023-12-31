#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;

namespace RGB.NET.Devices.CorsairLegacy.Native;

// ReSharper disable once InconsistentNaming
internal static class _CUESDK
{
    #region Libary Management

    private static nint _handle = 0;

    /// <summary>
    /// Reloads the SDK.
    /// </summary>
    internal static void Reload()
    {
        UnloadCUESDK();
        LoadCUESDK();
    }

    private static void LoadCUESDK()
    {
        if (_handle != 0) return;

        List<string> possiblePathList = GetPossibleLibraryPaths().ToList();

        string? dllPath = possiblePathList.FirstOrDefault(File.Exists);
        if (dllPath == null) throw new RGBDeviceException($"Can't find the CUE-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

        if (!NativeLibrary.TryLoad(dllPath, out _handle))
#if NET6_0
            throw new RGBDeviceException($"Corsair LoadLibrary failed with error code {Marshal.GetLastPInvokeError()}");
#else
            throw new RGBDeviceException($"Corsair LoadLibrary failed with error code {Marshal.GetLastWin32Error()}");
#endif

        if (!NativeLibrary.TryGetExport(_handle, "CorsairSetLedsColorsBufferByDeviceIndex", out _corsairSetLedsColorsBufferByDeviceIndexPointer)) throw new RGBDeviceException("Failed to load Corsair function 'CorsairSetLedsColorsBufferByDeviceIndex'");
        if (!NativeLibrary.TryGetExport(_handle, "CorsairSetLedsColorsFlushBuffer", out _corsairSetLedsColorsFlushBufferPointer)) throw new RGBDeviceException("Failed to load Corsair function 'CorsairSetLedsColorsFlushBuffer'");
        if (!NativeLibrary.TryGetExport(_handle, "CorsairGetLedsColorsByDeviceIndex", out _corsairGetLedsColorsByDeviceIndexPointer)) throw new RGBDeviceException("Failed to load Corsair function 'CorsairGetLedsColorsByDeviceIndex'");
        if (!NativeLibrary.TryGetExport(_handle, "CorsairSetLayerPriority", out _corsairSetLayerPriorityPointer)) throw new RGBDeviceException("Failed to load Corsair function 'CorsairSetLayerPriority'");
        if (!NativeLibrary.TryGetExport(_handle, "CorsairGetDeviceCount", out _corsairGetDeviceCountPointer)) throw new RGBDeviceException("Failed to load Corsair function 'CorsairGetDeviceCount'");
        if (!NativeLibrary.TryGetExport(_handle, "CorsairGetDeviceInfo", out _corsairGetDeviceInfoPointer)) throw new RGBDeviceException("Failed to load Corsair function 'CorsairGetDeviceInfo'");
        if (!NativeLibrary.TryGetExport(_handle, "CorsairGetLedIdForKeyName", out _corsairGetLedIdForKeyNamePointer)) throw new RGBDeviceException("Failed to load Corsair function 'CorsairGetLedIdForKeyName'");
        if (!NativeLibrary.TryGetExport(_handle, "CorsairGetLedPositionsByDeviceIndex", out _corsairGetLedPositionsByDeviceIndexPointer)) throw new RGBDeviceException("Failed to load Corsair function 'CorsairGetLedPositionsByDeviceIndex'");
        if (!NativeLibrary.TryGetExport(_handle, "CorsairRequestControl", out _corsairRequestControlPointer)) throw new RGBDeviceException("Failed to load Corsair function 'CorsairRequestControl'");
        if (!NativeLibrary.TryGetExport(_handle, "CorsairReleaseControl", out _corsairReleaseControlPointer)) throw new RGBDeviceException("Failed to load Corsair function 'CorsairReleaseControl'");
        if (!NativeLibrary.TryGetExport(_handle, "CorsairPerformProtocolHandshake", out _corsairPerformProtocolHandshakePointer)) throw new RGBDeviceException("Failed to load Corsair function 'CorsairPerformProtocolHandshake'");
        if (!NativeLibrary.TryGetExport(_handle, "CorsairGetLastError", out _corsairGetLastErrorPointer)) throw new RGBDeviceException("Failed to load Corsair function 'CorsairGetLastError'");
    }

    private static IEnumerable<string> GetPossibleLibraryPaths()
    {
        IEnumerable<string> possibleLibraryPaths;

        if (OperatingSystem.IsWindows())
            possibleLibraryPaths = Environment.Is64BitProcess ? CorsairLegacyDeviceProvider.PossibleX64NativePaths : CorsairLegacyDeviceProvider.PossibleX86NativePaths;
        else
            possibleLibraryPaths = Enumerable.Empty<string>();

        return possibleLibraryPaths.Select(Environment.ExpandEnvironmentVariables);
    }

    internal static void UnloadCUESDK()
    {
        if (_handle == 0) return;

        _corsairSetLedsColorsBufferByDeviceIndexPointer = 0;
        _corsairSetLedsColorsFlushBufferPointer = 0;
        _corsairGetLedsColorsByDeviceIndexPointer = 0;
        _corsairSetLayerPriorityPointer = 0;
        _corsairGetDeviceCountPointer = 0;
        _corsairGetDeviceInfoPointer = 0;
        _corsairGetLedIdForKeyNamePointer = 0;
        _corsairGetLedPositionsByDeviceIndexPointer = 0;
        _corsairRequestControlPointer = 0;
        _corsairReleaseControlPointer = 0;
        _corsairPerformProtocolHandshakePointer = 0;
        _corsairGetLastErrorPointer = 0;

        NativeLibrary.Free(_handle);
        _handle = 0;
    }

    #endregion

    #region SDK-METHODS

    #region Pointers

    private static nint _corsairSetLedsColorsBufferByDeviceIndexPointer;
    private static nint _corsairSetLedsColorsFlushBufferPointer;
    private static nint _corsairGetLedsColorsByDeviceIndexPointer;
    private static nint _corsairSetLayerPriorityPointer;
    private static nint _corsairGetDeviceCountPointer;
    private static nint _corsairGetDeviceInfoPointer;
    private static nint _corsairGetLedIdForKeyNamePointer;
    private static nint _corsairGetLedPositionsByDeviceIndexPointer;
    private static nint _corsairRequestControlPointer;
    private static nint _corsairReleaseControlPointer;
    private static nint _corsairPerformProtocolHandshakePointer;
    private static nint _corsairGetLastErrorPointer;

    #endregion

    /// <summary>
    /// CUE-SDK: set specified LEDs to some colors.
    /// This function set LEDs colors in the buffer which is written to the devices via CorsairSetLedsColorsFlushBuffer or CorsairSetLedsColorsFlushBufferAsync.
    /// Typical usecase is next: CorsairSetLedsColorsFlushBuffer or CorsairSetLedsColorsFlushBufferAsync is called to write LEDs colors to the device
    /// and follows after one or more calls of CorsairSetLedsColorsBufferByDeviceIndex to set the LEDs buffer.
    /// This function does not take logical layout into account.
    /// </summary>
    internal static unsafe bool CorsairSetLedsColorsBufferByDeviceIndex(int deviceIndex, int size, nint ledsColors)
        => ((delegate* unmanaged[Cdecl]<int, int, nint, bool>)ThrowIfZero(_corsairSetLedsColorsBufferByDeviceIndexPointer))(deviceIndex, size, ledsColors);

    /// <summary>
    /// CUE-SDK: writes to the devices LEDs colors buffer which is previously filled by the CorsairSetLedsColorsBufferByDeviceIndex function.
    /// This function executes synchronously, if you are concerned about delays consider using CorsairSetLedsColorsFlushBufferAsync
    /// </summary>
    internal static unsafe bool CorsairSetLedsColorsFlushBuffer() => ((delegate* unmanaged[Cdecl]<bool>)ThrowIfZero(_corsairSetLedsColorsFlushBufferPointer))();

    /// <summary>
    /// CUE-SDK: get current color for the list of requested LEDs.
    /// The color should represent the actual state of the hardware LED, which could be a combination of SDK and/or CUE input.
    /// This function works for keyboard, mouse, mousemat, headset, headset stand and DIY-devices.
    /// </summary>
    internal static unsafe bool CorsairGetLedsColorsByDeviceIndex(int deviceIndex, int size, nint ledsColors)
        => ((delegate* unmanaged[Cdecl]<int, int, nint, bool>)ThrowIfZero(_corsairGetLedsColorsByDeviceIndexPointer))(deviceIndex, size, ledsColors);

    /// <summary>
    /// CUE-SDK: set layer priority for this shared client.
    /// By default CUE has priority of 127 and all shared clients have priority of 128 if they don’t call this function.
    /// Layers with higher priority value are shown on top of layers with lower priority.
    /// </summary>
    internal static unsafe bool CorsairSetLayerPriority(int priority) => ((delegate* unmanaged[Cdecl]<int, bool>)ThrowIfZero(_corsairSetLayerPriorityPointer))(priority);

    /// <summary>
    /// CUE-SDK: returns number of connected Corsair devices that support lighting control.
    /// </summary>
    internal static unsafe int CorsairGetDeviceCount() => ((delegate* unmanaged[Cdecl]<int>)ThrowIfZero(_corsairGetDeviceCountPointer))();

    /// <summary>
    /// CUE-SDK: returns information about device at provided index.
    /// </summary>
    internal static unsafe nint CorsairGetDeviceInfo(int deviceIndex) => ((delegate* unmanaged[Cdecl]<int, nint>)ThrowIfZero(_corsairGetDeviceInfoPointer))(deviceIndex);

    /// <summary>
    /// CUE-SDK: provides list of keyboard or mousepad LEDs with their physical positions.
    /// </summary>
    internal static unsafe nint CorsairGetLedPositionsByDeviceIndex(int deviceIndex) => ((delegate* unmanaged[Cdecl]<int, nint>)ThrowIfZero(_corsairGetLedPositionsByDeviceIndexPointer))(deviceIndex);

    /// <summary>
    /// CUE-SDK: retrieves led id for key name taking logical layout into account.
    /// </summary>
    internal static unsafe CorsairLedId CorsairGetLedIdForKeyName(char keyName) => ((delegate* unmanaged[Cdecl]<char, CorsairLedId>)ThrowIfZero(_corsairGetLedIdForKeyNamePointer))(keyName);

    /// <summary>
    /// CUE-SDK: requestes control using specified access mode.
    /// By default client has shared control over lighting so there is no need to call CorsairRequestControl unless client requires exclusive control.
    /// </summary>
    internal static unsafe bool CorsairRequestControl(CorsairAccessMode accessMode) => ((delegate* unmanaged[Cdecl]<CorsairAccessMode, bool>)ThrowIfZero(_corsairRequestControlPointer))(accessMode);

    /// <summary>
    /// CUE-SDK: releases previously requested control for specified access mode.
    /// </summary>
    internal static unsafe bool CorsairReleaseControl(CorsairAccessMode accessMode) => ((delegate* unmanaged[Cdecl]<CorsairAccessMode, bool>)ThrowIfZero(_corsairReleaseControlPointer))(accessMode);

    /// <summary>
    /// CUE-SDK: checks file and protocol version of CUE to understand which of SDK functions can be used with this version of CUE.
    /// </summary>
    internal static unsafe _CorsairProtocolDetails CorsairPerformProtocolHandshake() => ((delegate* unmanaged[Cdecl]<_CorsairProtocolDetails>)ThrowIfZero(_corsairPerformProtocolHandshakePointer))();

    /// <summary>
    /// CUE-SDK: returns last error that occured while using any of Corsair* functions.
    /// </summary>
    internal static unsafe CorsairError CorsairGetLastError() => ((delegate* unmanaged[Cdecl]<CorsairError>)ThrowIfZero(_corsairGetLastErrorPointer))();

    private static nint ThrowIfZero(nint ptr)
    {
        if (ptr == 0) throw new RGBDeviceException("The Corsair-SDK is not initialized.");
        return ptr;
    }

    #endregion
}