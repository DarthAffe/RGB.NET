#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;

namespace RGB.NET.Devices.CoolerMaster.Native;

// ReSharper disable once InconsistentNaming
internal static class _CoolerMasterSDK
{
    #region Libary Management

    private static IntPtr _dllHandle = IntPtr.Zero;

    /// <summary>
    /// Reloads the SDK.
    /// </summary>
    internal static void Reload()
    {
        if (_dllHandle != IntPtr.Zero)
        {
            foreach (CoolerMasterDevicesIndexes index in Enum.GetValues(typeof(CoolerMasterDevicesIndexes)))
                EnableLedControl(false, index);
        }
        else
            LoadCMSDK();
    }

    private static void LoadCMSDK()
    {
        if (_dllHandle != IntPtr.Zero) return;

        // HACK: Load library at runtime to support both, x86 and x64 with one managed dll
        List<string> possiblePathList = Environment.Is64BitProcess ? CoolerMasterDeviceProvider.PossibleX64NativePaths : CoolerMasterDeviceProvider.PossibleX86NativePaths;
        string? dllPath = possiblePathList.FirstOrDefault(File.Exists);
        if (dllPath == null) throw new RGBDeviceException($"Can't find the CoolerMaster-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

        _dllHandle = LoadLibrary(dllPath);
        if (_dllHandle == IntPtr.Zero) throw new RGBDeviceException($"CoolerMaster LoadLibrary failed with error code {Marshal.GetLastWin32Error()}");

        _getSDKVersionPointer = (GetSDKVersionPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "GetCM_SDK_DllVer"), typeof(GetSDKVersionPointer));
        _setControlDevicenPointer = (SetControlDevicePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetControlDevice"), typeof(SetControlDevicePointer));
        _isDevicePlugPointer = (IsDevicePlugPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "IsDevicePlug"), typeof(IsDevicePlugPointer));
        _getDeviceLayoutPointer = (GetDeviceLayoutPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "GetDeviceLayout"), typeof(GetDeviceLayoutPointer));
        _enableLedControlPointer = (EnableLedControlPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "EnableLedControl"), typeof(EnableLedControlPointer));
        _refreshLedPointer = (RefreshLedPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "RefreshLed"), typeof(RefreshLedPointer));
        _setLedColorPointer = (SetLedColorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetLedColor"), typeof(SetLedColorPointer));
        _setAllLedColorPointer = (SetAllLedColorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetAllLedColor"), typeof(SetAllLedColorPointer));
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr LoadLibrary(string dllToLoad);
        
    [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr GetProcAddress(IntPtr dllHandle, string name);

    #endregion

    #region SDK-METHODS

    #region Pointers

    private static GetSDKVersionPointer? _getSDKVersionPointer;
    private static SetControlDevicePointer? _setControlDevicenPointer;
    private static IsDevicePlugPointer? _isDevicePlugPointer;
    private static GetDeviceLayoutPointer? _getDeviceLayoutPointer;
    private static EnableLedControlPointer? _enableLedControlPointer;
    private static RefreshLedPointer? _refreshLedPointer;
    private static SetLedColorPointer? _setLedColorPointer;
    private static SetAllLedColorPointer? _setAllLedColorPointer;

    #endregion

    #region Delegates

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetSDKVersionPointer();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void SetControlDevicePointer(CoolerMasterDevicesIndexes devicesIndexes);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool IsDevicePlugPointer(CoolerMasterDevicesIndexes devIndex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate CoolerMasterPhysicalKeyboardLayout GetDeviceLayoutPointer(CoolerMasterDevicesIndexes devIndex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool EnableLedControlPointer(bool value, CoolerMasterDevicesIndexes devIndex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool RefreshLedPointer(bool autoRefresh, CoolerMasterDevicesIndexes devIndex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool SetLedColorPointer(int row, int column, byte r, byte g, byte b, CoolerMasterDevicesIndexes devIndex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool SetAllLedColorPointer(_CoolerMasterColorMatrix colorMatrix, CoolerMasterDevicesIndexes devIndex);

    #endregion

    // ReSharper disable EventExceptionNotDocumented

    /// <summary>
    /// CM-SDK: Get SDK Dll's Version.
    /// </summary>
    internal static int GetSDKVersion() => (_getSDKVersionPointer ?? throw new RGBDeviceException("The CoolerMaster-SDK is not initialized.")).Invoke();

    /// <summary>
    /// CM-SDK: set operating device
    /// </summary>
    internal static void SetControlDevice(CoolerMasterDevicesIndexes devicesIndexes)
        => (_setControlDevicenPointer ?? throw new RGBDeviceException("The CoolerMaster-SDK is not initialized.")).Invoke(devicesIndexes);

    /// <summary>
    /// CM-SDK: verify if the deviced is plugged in
    /// </summary>
    internal static bool IsDevicePlugged(CoolerMasterDevicesIndexes devIndex = CoolerMasterDevicesIndexes.Default)
        => (_isDevicePlugPointer ?? throw new RGBDeviceException("The CoolerMaster-SDK is not initialized.")).Invoke(devIndex);

    /// <summary>
    /// CM-SDK: Obtain current device layout
    /// </summary>
    internal static CoolerMasterPhysicalKeyboardLayout GetDeviceLayout(CoolerMasterDevicesIndexes devIndex = CoolerMasterDevicesIndexes.Default)
        => (_getDeviceLayoutPointer ?? throw new RGBDeviceException("The CoolerMaster-SDK is not initialized.")).Invoke(devIndex);

    /// <summary>
    /// CM-SDK: set control over device’s LED
    /// </summary>
    internal static bool EnableLedControl(bool value, CoolerMasterDevicesIndexes devIndex = CoolerMasterDevicesIndexes.Default)
        => (_enableLedControlPointer ?? throw new RGBDeviceException("The CoolerMaster-SDK is not initialized.")).Invoke(value, devIndex);

    /// <summary>
    /// CM-SDK: Print out the lights setting from Buffer to LED
    /// </summary>
    internal static bool RefreshLed(bool autoRefresh, CoolerMasterDevicesIndexes devIndex = CoolerMasterDevicesIndexes.Default)
        => (_refreshLedPointer ?? throw new RGBDeviceException("The CoolerMaster-SDK is not initialized.")).Invoke(autoRefresh, devIndex);

    /// <summary>
    /// CM-SDK: Set single Key LED color
    /// </summary>
    internal static bool SetLedColor(int row, int column, byte r, byte g, byte b, CoolerMasterDevicesIndexes devIndex = CoolerMasterDevicesIndexes.Default)
        => (_setLedColorPointer ?? throw new RGBDeviceException("The CoolerMaster-SDK is not initialized.")).Invoke(row, column, r, g, b, devIndex);

    /// <summary>
    /// CM-SDK: Set Keyboard "every LED" color
    /// </summary>
    internal static bool SetAllLedColor(_CoolerMasterColorMatrix colorMatrix, CoolerMasterDevicesIndexes devIndex = CoolerMasterDevicesIndexes.Default)
        => (_setAllLedColorPointer ?? throw new RGBDeviceException("The CoolerMaster-SDK is not initialized.")).Invoke(colorMatrix, devIndex);

    // ReSharper restore EventExceptionNotDocumented

    #endregion
}