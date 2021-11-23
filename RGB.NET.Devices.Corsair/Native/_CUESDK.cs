#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
internal static class _CUESDK
{
    #region Libary Management

    private static IntPtr _dllHandle = IntPtr.Zero;
        
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
        if (_dllHandle != IntPtr.Zero) return;

        // HACK: Load library at runtime to support both, x86 and x64 with one managed dll
        List<string> possiblePathList = Environment.Is64BitProcess ? CorsairDeviceProvider.PossibleX64NativePaths : CorsairDeviceProvider.PossibleX86NativePaths;
        string? dllPath = possiblePathList.FirstOrDefault(File.Exists);
        if (dllPath == null) throw new RGBDeviceException($"Can't find the CUE-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

        _dllHandle = LoadLibrary(dllPath);
        if (_dllHandle == IntPtr.Zero) throw new RGBDeviceException($"Corsair LoadLibrary failed with error code {Marshal.GetLastWin32Error()}");

        _corsairSetLedsColorsBufferByDeviceIndexPointer = (CorsairSetLedsColorsBufferByDeviceIndexPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CorsairSetLedsColorsBufferByDeviceIndex"), typeof(CorsairSetLedsColorsBufferByDeviceIndexPointer));
        _corsairSetLedsColorsFlushBufferPointer = (CorsairSetLedsColorsFlushBufferPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CorsairSetLedsColorsFlushBuffer"), typeof(CorsairSetLedsColorsFlushBufferPointer));
        _corsairGetLedsColorsByDeviceIndexPointer = (CorsairGetLedsColorsByDeviceIndexPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CorsairGetLedsColorsByDeviceIndex"), typeof(CorsairGetLedsColorsByDeviceIndexPointer));
        _corsairSetLayerPriorityPointer = (CorsairSetLayerPriorityPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CorsairSetLayerPriority"), typeof(CorsairSetLayerPriorityPointer));
        _corsairGetDeviceCountPointer = (CorsairGetDeviceCountPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CorsairGetDeviceCount"), typeof(CorsairGetDeviceCountPointer));
        _corsairGetDeviceInfoPointer = (CorsairGetDeviceInfoPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CorsairGetDeviceInfo"), typeof(CorsairGetDeviceInfoPointer));
        _corsairGetLedPositionsByDeviceIndexPointer = (CorsairGetLedPositionsByDeviceIndexPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CorsairGetLedPositionsByDeviceIndex"), typeof(CorsairGetLedPositionsByDeviceIndexPointer));
        _corsairGetLedIdForKeyNamePointer = (CorsairGetLedIdForKeyNamePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CorsairGetLedIdForKeyName"), typeof(CorsairGetLedIdForKeyNamePointer));
        _corsairRequestControlPointer = (CorsairRequestControlPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CorsairRequestControl"), typeof(CorsairRequestControlPointer));
        _corsairReleaseControlPointer = (CorsairReleaseControlPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CorsairReleaseControl"), typeof(CorsairReleaseControlPointer));
        _corsairPerformProtocolHandshakePointer = (CorsairPerformProtocolHandshakePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CorsairPerformProtocolHandshake"), typeof(CorsairPerformProtocolHandshakePointer));
        _corsairGetLastErrorPointer = (CorsairGetLastErrorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CorsairGetLastError"), typeof(CorsairGetLastErrorPointer));
    }

    internal static void UnloadCUESDK()
    {
        if (_dllHandle == IntPtr.Zero) return;

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

    private static CorsairSetLedsColorsBufferByDeviceIndexPointer? _corsairSetLedsColorsBufferByDeviceIndexPointer;
    private static CorsairSetLedsColorsFlushBufferPointer? _corsairSetLedsColorsFlushBufferPointer;
    private static CorsairGetLedsColorsByDeviceIndexPointer? _corsairGetLedsColorsByDeviceIndexPointer;
    private static CorsairSetLayerPriorityPointer? _corsairSetLayerPriorityPointer;
    private static CorsairGetDeviceCountPointer? _corsairGetDeviceCountPointer;
    private static CorsairGetDeviceInfoPointer? _corsairGetDeviceInfoPointer;
    private static CorsairGetLedIdForKeyNamePointer? _corsairGetLedIdForKeyNamePointer;
    private static CorsairGetLedPositionsByDeviceIndexPointer? _corsairGetLedPositionsByDeviceIndexPointer;
    private static CorsairRequestControlPointer? _corsairRequestControlPointer;
    private static CorsairReleaseControlPointer? _corsairReleaseControlPointer;
    private static CorsairPerformProtocolHandshakePointer? _corsairPerformProtocolHandshakePointer;
    private static CorsairGetLastErrorPointer? _corsairGetLastErrorPointer;

    #endregion

    #region Delegates

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool CorsairSetLedsColorsBufferByDeviceIndexPointer(int deviceIndex, int size, IntPtr ledsColors);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool CorsairSetLedsColorsFlushBufferPointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool CorsairGetLedsColorsByDeviceIndexPointer(int deviceIndex, int size, IntPtr ledsColors);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool CorsairSetLayerPriorityPointer(int priority);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CorsairGetDeviceCountPointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr CorsairGetDeviceInfoPointer(int deviceIndex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr CorsairGetLedPositionsByDeviceIndexPointer(int deviceIndex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate CorsairLedId CorsairGetLedIdForKeyNamePointer(char keyName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool CorsairRequestControlPointer(CorsairAccessMode accessMode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool CorsairReleaseControlPointer(CorsairAccessMode accessMode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate _CorsairProtocolDetails CorsairPerformProtocolHandshakePointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate CorsairError CorsairGetLastErrorPointer();

    #endregion

    // ReSharper disable EventExceptionNotDocumented

    /// <summary>
    /// CUE-SDK: set specified LEDs to some colors.
    /// This function set LEDs colors in the buffer which is written to the devices via CorsairSetLedsColorsFlushBuffer or CorsairSetLedsColorsFlushBufferAsync.
    /// Typical usecase is next: CorsairSetLedsColorsFlushBuffer or CorsairSetLedsColorsFlushBufferAsync is called to write LEDs colors to the device
    /// and follows after one or more calls of CorsairSetLedsColorsBufferByDeviceIndex to set the LEDs buffer.
    /// This function does not take logical layout into account.
    /// </summary>
    internal static bool CorsairSetLedsColorsBufferByDeviceIndex(int deviceIndex, int size, IntPtr ledsColors)
        => (_corsairSetLedsColorsBufferByDeviceIndexPointer ?? throw new RGBDeviceException("The Corsair-SDK is not initialized.")).Invoke(deviceIndex, size, ledsColors);

    /// <summary>
    /// CUE-SDK: writes to the devices LEDs colors buffer which is previously filled by the CorsairSetLedsColorsBufferByDeviceIndex function.
    /// This function executes synchronously, if you are concerned about delays consider using CorsairSetLedsColorsFlushBufferAsync
    /// </summary>
    internal static bool CorsairSetLedsColorsFlushBuffer() => (_corsairSetLedsColorsFlushBufferPointer ?? throw new RGBDeviceException("The Corsair-SDK is not initialized.")).Invoke();

    /// <summary>
    /// CUE-SDK: get current color for the list of requested LEDs.
    /// The color should represent the actual state of the hardware LED, which could be a combination of SDK and/or CUE input.
    /// This function works for keyboard, mouse, mousemat, headset, headset stand and DIY-devices.
    /// </summary>
    internal static bool CorsairGetLedsColorsByDeviceIndex(int deviceIndex, int size, IntPtr ledsColors)
        => (_corsairGetLedsColorsByDeviceIndexPointer ?? throw new RGBDeviceException("The Corsair-SDK is not initialized.")).Invoke(deviceIndex, size, ledsColors);

    /// <summary>
    /// CUE-SDK: set layer priority for this shared client.
    /// By default CUE has priority of 127 and all shared clients have priority of 128 if they don’t call this function.
    /// Layers with higher priority value are shown on top of layers with lower priority.
    /// </summary>
    internal static bool CorsairSetLayerPriority(int priority) => (_corsairSetLayerPriorityPointer ?? throw new RGBDeviceException("The Corsair-SDK is not initialized.")).Invoke(priority);

    /// <summary>
    /// CUE-SDK: returns number of connected Corsair devices that support lighting control.
    /// </summary>
    internal static int CorsairGetDeviceCount() => (_corsairGetDeviceCountPointer ?? throw new RGBDeviceException("The Corsair-SDK is not initialized.")).Invoke();

    /// <summary>
    /// CUE-SDK: returns information about device at provided index.
    /// </summary>
    internal static IntPtr CorsairGetDeviceInfo(int deviceIndex) => (_corsairGetDeviceInfoPointer ?? throw new RGBDeviceException("The Corsair-SDK is not initialized.")).Invoke(deviceIndex);

    /// <summary>
    /// CUE-SDK: provides list of keyboard or mousepad LEDs with their physical positions.
    /// </summary>
    internal static IntPtr CorsairGetLedPositionsByDeviceIndex(int deviceIndex) => (_corsairGetLedPositionsByDeviceIndexPointer ?? throw new RGBDeviceException("The Corsair-SDK is not initialized.")).Invoke(deviceIndex);

    /// <summary>
    /// CUE-SDK: retrieves led id for key name taking logical layout into account.
    /// </summary>
    internal static CorsairLedId CorsairGetLedIdForKeyName(char keyName) => (_corsairGetLedIdForKeyNamePointer ?? throw new RGBDeviceException("The Corsair-SDK is not initialized.")).Invoke(keyName);

    /// <summary>
    /// CUE-SDK: requestes control using specified access mode.
    /// By default client has shared control over lighting so there is no need to call CorsairRequestControl unless client requires exclusive control.
    /// </summary>
    internal static bool CorsairRequestControl(CorsairAccessMode accessMode) => (_corsairRequestControlPointer ?? throw new RGBDeviceException("The Corsair-SDK is not initialized.")).Invoke(accessMode);

    /// <summary>
    /// CUE-SDK: releases previously requested control for specified access mode.
    /// </summary>
    internal static bool CorsairReleaseControl(CorsairAccessMode accessMode) => (_corsairReleaseControlPointer ?? throw new RGBDeviceException("The Corsair-SDK is not initialized.")).Invoke(accessMode);

    /// <summary>
    /// CUE-SDK: checks file and protocol version of CUE to understand which of SDK functions can be used with this version of CUE.
    /// </summary>
    internal static _CorsairProtocolDetails CorsairPerformProtocolHandshake() => (_corsairPerformProtocolHandshakePointer ?? throw new RGBDeviceException("The Corsair-SDK is not initialized.")).Invoke();

    /// <summary>
    /// CUE-SDK: returns last error that occured while using any of Corsair* functions.
    /// </summary>
    internal static CorsairError CorsairGetLastError() => (_corsairGetLastErrorPointer ?? throw new RGBDeviceException("The Corsair-SDK is not initialized.")).Invoke();

    // ReSharper restore EventExceptionNotDocumented

    #endregion
}