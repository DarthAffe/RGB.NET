﻿#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;

namespace RGB.NET.Devices.Msi.Native;

// ReSharper disable once InconsistentNaming
internal static class _MsiSDK
{
    #region Libary Management

    private static nint _handle = 0;

    /// <summary>
    /// Reloads the SDK.
    /// </summary>
    internal static void Reload()
    {
        UnloadMsiSDK();
        LoadMsiSDK();
    }

    private static void LoadMsiSDK()
    {
        if (_handle != 0) return;

        List<string> possiblePathList = GetPossibleLibraryPaths().ToList();

        string? dllPath = possiblePathList.FirstOrDefault(File.Exists);
        if (dllPath == null) throw new RGBDeviceException($"Can't find the CUE-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");
        
        SetDllDirectory(Path.GetDirectoryName(Path.GetFullPath(dllPath))!);

        if (!NativeLibrary.TryLoad(dllPath, out _handle))
#if NET6_0
            throw new RGBDeviceException($"MSI LoadLibrary failed with error code {Marshal.GetLastPInvokeError()}");
#else
            throw new RGBDeviceException($"MSI LoadLibrary failed with error code {Marshal.GetLastWin32Error()}");
#endif

        _initializePointer = (InitializePointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_Initialize"), typeof(InitializePointer));
        _getDeviceInfoPointer = (GetDeviceInfoPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_GetDeviceInfo"), typeof(GetDeviceInfoPointer));
        _getLedInfoPointer = (GetLedInfoPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_GetLedInfo"), typeof(GetLedInfoPointer));
        _getLedColorPointer = (GetLedColorPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_GetLedColor"), typeof(GetLedColorPointer));
        _getLedStylePointer = (GetLedStylePointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_GetLedStyle"), typeof(GetLedStylePointer));
        _getLedMaxBrightPointer = (GetLedMaxBrightPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_GetLedMaxBright"), typeof(GetLedMaxBrightPointer));
        _getLedBrightPointer = (GetLedBrightPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_GetLedBright"), typeof(GetLedBrightPointer));
        _getLedMaxSpeedPointer = (GetLedMaxSpeedPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_GetLedMaxSpeed"), typeof(GetLedMaxSpeedPointer));
        _getLedSpeedPointer = (GetLedSpeedPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_GetLedSpeed"), typeof(GetLedSpeedPointer));
        _setLedColorPointer = (SetLedColorPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_SetLedColor"), typeof(SetLedColorPointer));
        _setLedStylePointer = (SetLedStylePointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_SetLedStyle"), typeof(SetLedStylePointer));
        _setLedBrightPointer = (SetLedBrightPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_SetLedBright"), typeof(SetLedBrightPointer));
        _setLedSpeedPointer = (SetLedSpeedPointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_SetLedSpeed"), typeof(SetLedSpeedPointer));
        _getErrorMessagePointer = (GetErrorMessagePointer)Marshal.GetDelegateForFunctionPointer(NativeLibrary.GetExport(_handle, "MLAPI_GetErrorMessage"), typeof(GetErrorMessagePointer));
    }

    private static IEnumerable<string> GetPossibleLibraryPaths()
    {
        IEnumerable<string> possibleLibraryPaths;

        if (OperatingSystem.IsWindows())
            possibleLibraryPaths = Environment.Is64BitProcess ? MsiDeviceProvider.PossibleX64NativePaths : MsiDeviceProvider.PossibleX86NativePaths;
        else
            possibleLibraryPaths = [];

        return possibleLibraryPaths.Select(Environment.ExpandEnvironmentVariables);
    }

    internal static void UnloadMsiSDK()
    {
        if (_handle == 0) return;

        _initializePointer = null;
        _getDeviceInfoPointer = null;
        _getLedColorPointer = null;
        _getLedColorPointer = null;
        _getLedStylePointer = null;
        _getLedMaxBrightPointer = null;
        _getLedBrightPointer = null;
        _getLedMaxSpeedPointer = null;
        _getLedSpeedPointer = null;
        _setLedColorPointer = null;
        _setLedStylePointer = null;
        _setLedBrightPointer = null;
        _setLedSpeedPointer = null;
        _getErrorMessagePointer = null;

        NativeLibrary.Free(_handle);
        _handle = 0;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern bool SetDllDirectory(string lpPathName);

    #endregion

    #region SDK-METHODS

    #region Pointers

    private static InitializePointer? _initializePointer;
    private static GetDeviceInfoPointer? _getDeviceInfoPointer;
    private static GetLedInfoPointer? _getLedInfoPointer;
    private static GetLedColorPointer? _getLedColorPointer;
    private static GetLedStylePointer? _getLedStylePointer;
    private static GetLedMaxBrightPointer? _getLedMaxBrightPointer;
    private static GetLedBrightPointer? _getLedBrightPointer;
    private static GetLedMaxSpeedPointer? _getLedMaxSpeedPointer;
    private static GetLedSpeedPointer? _getLedSpeedPointer;
    private static SetLedColorPointer? _setLedColorPointer;
    private static SetLedStylePointer? _setLedStylePointer;
    private static SetLedBrightPointer? _setLedBrightPointer;
    private static SetLedSpeedPointer? _setLedSpeedPointer;
    private static GetErrorMessagePointer? _getErrorMessagePointer;

    #endregion

    #region Delegates

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int InitializePointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetDeviceInfoPointer(
        [Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] pDevType,
        [Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] pLedCount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetLedInfoPointer(
        [In, MarshalAs(UnmanagedType.BStr)] string type,
        [In, MarshalAs(UnmanagedType.I4)] int index,
        [Out, MarshalAs(UnmanagedType.BStr)] out string pName,
        [Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] pLedStyles);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetLedColorPointer(
        [In, MarshalAs(UnmanagedType.BStr)] string type,
        [In, MarshalAs(UnmanagedType.I4)] int index,
        [Out, MarshalAs(UnmanagedType.I4)] out int r,
        [Out, MarshalAs(UnmanagedType.I4)] out int g,
        [Out, MarshalAs(UnmanagedType.I4)] out int b);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetLedStylePointer(
        [In, MarshalAs(UnmanagedType.BStr)] string type,
        [In, MarshalAs(UnmanagedType.I4)] int index,
        [Out, MarshalAs(UnmanagedType.BStr)] out string style);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetLedMaxBrightPointer(
        [In, MarshalAs(UnmanagedType.BStr)] string type,
        [In, MarshalAs(UnmanagedType.I4)] int index,
        [Out, MarshalAs(UnmanagedType.I4)] out int maxLevel);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetLedBrightPointer(
        [In, MarshalAs(UnmanagedType.BStr)] string type,
        [In, MarshalAs(UnmanagedType.I4)] int index,
        [Out, MarshalAs(UnmanagedType.I4)] out int currentLevel);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetLedMaxSpeedPointer(
        [In, MarshalAs(UnmanagedType.BStr)] string type,
        [In, MarshalAs(UnmanagedType.I4)] int index,
        [Out, MarshalAs(UnmanagedType.I4)] out int maxSpeed);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetLedSpeedPointer(
        [In, MarshalAs(UnmanagedType.BStr)] string type,
        [In, MarshalAs(UnmanagedType.I4)] int index,
        [Out, MarshalAs(UnmanagedType.I4)] out int currentSpeed);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SetLedColorPointer(
        [In, MarshalAs(UnmanagedType.BStr)] string type,
        [In, MarshalAs(UnmanagedType.I4)] int index,
        [In, MarshalAs(UnmanagedType.I4)] int r,
        [In, MarshalAs(UnmanagedType.I4)] int g,
        [In, MarshalAs(UnmanagedType.I4)] int b);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SetLedStylePointer(
        [In, MarshalAs(UnmanagedType.BStr)] string type,
        [In, MarshalAs(UnmanagedType.I4)] int index,
        [In, MarshalAs(UnmanagedType.BStr)] string style);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SetLedBrightPointer(
        [In, MarshalAs(UnmanagedType.BStr)] string type,
        [In, MarshalAs(UnmanagedType.I4)] int index,
        [In, MarshalAs(UnmanagedType.I4)] int level);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SetLedSpeedPointer(
        [In, MarshalAs(UnmanagedType.BStr)] string type,
        [In, MarshalAs(UnmanagedType.I4)] int index,
        [In, MarshalAs(UnmanagedType.I4)] int speed);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetErrorMessagePointer(
        [In, MarshalAs(UnmanagedType.I4)] int errorCode,
        [Out, MarshalAs(UnmanagedType.BStr)] out string pDesc);

    #endregion

    internal static int Initialize() => (_initializePointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke();
    internal static int GetDeviceInfo(out string[] pDevType, out int[] pLedCount)
    {
        // HACK - SDK GetDeviceInfo returns a string[] for ledCount, so we'll parse that to int.
        int result = (_getDeviceInfoPointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke(out pDevType, out string[] ledCount);
        pLedCount = new int[ledCount.Length];

        for (int i = 0; i < ledCount.Length; i++)
            pLedCount[i] = int.Parse(ledCount[i]);

        return result;
    }

    internal static int GetLedInfo(string type, int index, out string pName, out string[] pLedStyles) => (_getLedInfoPointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke(type, index, out pName, out pLedStyles);
    internal static int GetLedColor(string type, int index, out int r, out int g, out int b) => (_getLedColorPointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke(type, index, out r, out g, out b);
    internal static int GetLedStyle(string type, int index, out string style) => (_getLedStylePointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke(type, index, out style);
    internal static int GetLedMaxBright(string type, int index, out int maxLevel) => (_getLedMaxBrightPointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke(type, index, out maxLevel);
    internal static int GetLedBright(string type, int index, out int currentLevel) => (_getLedBrightPointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke(type, index, out currentLevel);
    internal static int GetLedMaxSpeed(string type, int index, out int maxSpeed) => (_getLedMaxSpeedPointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke(type, index, out maxSpeed);
    internal static int GetLedSpeed(string type, int index, out int currentSpeed) => (_getLedSpeedPointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke(type, index, out currentSpeed);
    internal static int SetLedColor(string type, int index, int r, int g, int b) => (_setLedColorPointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke(type, index, r, g, b);
    internal static int SetLedStyle(string type, int index, string style) => (_setLedStylePointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke(type, index, style);
    internal static int SetLedBright(string type, int index, int level) => (_setLedBrightPointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke(type, index, level);
    internal static int SetLedSpeed(string type, int index, int speed) => (_setLedSpeedPointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke(type, index, speed);

    internal static string GetErrorMessage(int errorCode)
    {
        (_getErrorMessagePointer ?? throw new RGBDeviceException("The MSI-SDK is not initialized.")).Invoke(errorCode, out string description);
        return description;
    }

    #endregion
}