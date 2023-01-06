#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;

namespace RGB.NET.Devices.Razer.Native;

// ReSharper disable once InconsistentNaming
internal static class _RazerSDK
{
    #region Libary Management

    private static IntPtr _handle = IntPtr.Zero;

    /// <summary>
    /// Reloads the SDK.
    /// </summary>
    internal static void Reload()
    {
        UnloadRazerSDK();
        LoadRazerSDK();
    }

    private static void LoadRazerSDK()
    {
        if (_handle != IntPtr.Zero) return;

        List<string> possiblePathList = GetPossibleLibraryPaths().ToList();

        string? dllPath = possiblePathList.FirstOrDefault(File.Exists);
        if (dllPath == null) throw new RGBDeviceException($"Can't find the Razer-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

        if (!NativeLibrary.TryLoad(dllPath, out _handle))
#if NET6_0
            throw new RGBDeviceException($"Razer LoadLibrary failed with error code {Marshal.GetLastPInvokeError()}");
#else
            throw new RGBDeviceException($"Razer LoadLibrary failed with error code {Marshal.GetLastWin32Error()}");
#endif

        if (!NativeLibrary.TryGetExport(_handle, "Init", out _initPointer)) throw new RGBDeviceException("Failed to load Razer function 'Init'");
        if (!NativeLibrary.TryGetExport(_handle, "UnInit", out _unInitPointer)) throw new RGBDeviceException("Failed to load Razer function 'UnInit'");
        if (!NativeLibrary.TryGetExport(_handle, "QueryDevice", out _queryDevicePointer)) throw new RGBDeviceException("Failed to load Razer function 'QueryDevice'");
        if (!NativeLibrary.TryGetExport(_handle, "CreateEffect", out _createEffectPointer)) throw new RGBDeviceException("Failed to load Razer function 'CreateEffect'");
        if (!NativeLibrary.TryGetExport(_handle, "CreateHeadsetEffect", out _createHeadsetEffectPointer)) throw new RGBDeviceException("Failed to load Razer function 'CreateHeadsetEffect'");
        if (!NativeLibrary.TryGetExport(_handle, "CreateChromaLinkEffect", out _createChromaLinkEffectPointer)) throw new RGBDeviceException("Failed to load Razer function 'CreateChromaLinkEffect'");
        if (!NativeLibrary.TryGetExport(_handle, "CreateKeyboardEffect", out _createKeyboardEffectPointer)) throw new RGBDeviceException("Failed to load Razer function 'CreateKeyboardEffect'");
        if (!NativeLibrary.TryGetExport(_handle, "CreateKeypadEffect", out _createKeypadEffectPointer)) throw new RGBDeviceException("Failed to load Razer function 'CreateKeypadEffect'");
        if (!NativeLibrary.TryGetExport(_handle, "CreateMouseEffect", out _createMouseEffectPointer)) throw new RGBDeviceException("Failed to load Razer function 'CreateMouseEffect'");
        if (!NativeLibrary.TryGetExport(_handle, "CreateMousepadEffect", out _createMousepadEffectPointer)) throw new RGBDeviceException("Failed to load Razer function 'CreateMousepadEffect'");
        if (!NativeLibrary.TryGetExport(_handle, "SetEffect", out _setEffectPointer)) throw new RGBDeviceException("Failed to load Razer function 'SetEffect'");
        if (!NativeLibrary.TryGetExport(_handle, "DeleteEffect", out _deleteEffectPointer)) throw new RGBDeviceException("Failed to load Razer function 'DeleteEffect'");
    }

    private static IEnumerable<string> GetPossibleLibraryPaths()
    {
        IEnumerable<string> possibleLibraryPaths;

        if (OperatingSystem.IsWindows())
            possibleLibraryPaths = Environment.Is64BitProcess ? RazerDeviceProvider.PossibleX64NativePaths : RazerDeviceProvider.PossibleX86NativePaths;
        else
            possibleLibraryPaths = Enumerable.Empty<string>();

        return possibleLibraryPaths.Select(Environment.ExpandEnvironmentVariables);
    }

    internal static void UnloadRazerSDK()
    {
        if (_handle == IntPtr.Zero) return;

        _initPointer = IntPtr.Zero;
        _unInitPointer = IntPtr.Zero;
        _queryDevicePointer = IntPtr.Zero;
        _createEffectPointer = IntPtr.Zero;
        _createHeadsetEffectPointer = IntPtr.Zero;
        _createChromaLinkEffectPointer = IntPtr.Zero;
        _createKeyboardEffectPointer = IntPtr.Zero;
        _createKeypadEffectPointer = IntPtr.Zero;
        _createMouseEffectPointer = IntPtr.Zero;
        _createMousepadEffectPointer = IntPtr.Zero;
        _setEffectPointer = IntPtr.Zero;
        _deleteEffectPointer = IntPtr.Zero;

        NativeLibrary.Free(_handle);
        _handle = IntPtr.Zero;
    }

    #endregion

    #region SDK-METHODS

    #region Pointers

    private static IntPtr _initPointer;
    private static IntPtr _unInitPointer;
    private static IntPtr _queryDevicePointer;
    private static IntPtr _createEffectPointer;
    private static IntPtr _createHeadsetEffectPointer;
    private static IntPtr _createChromaLinkEffectPointer;
    private static IntPtr _createKeyboardEffectPointer;
    private static IntPtr _createKeypadEffectPointer;
    private static IntPtr _createMouseEffectPointer;
    private static IntPtr _createMousepadEffectPointer;
    private static IntPtr _setEffectPointer;
    private static IntPtr _deleteEffectPointer;

    #endregion

    /// <summary>
    /// Razer-SDK: Initialize Chroma SDK.
    /// </summary>
    internal static unsafe RazerError Init() => ((delegate* unmanaged[Cdecl]<RazerError>)ThrowIfZero(_initPointer))();

    /// <summary>
    /// Razer-SDK: UnInitialize Chroma SDK.
    /// </summary>
    internal static unsafe RazerError UnInit()
        => ((delegate* unmanaged[Cdecl]<RazerError>)ThrowIfZero(_unInitPointer))();

    /// <summary>
    /// Razer-SDK: Query for device information. 
    /// </summary>
    internal static unsafe RazerError QueryDevice(Guid deviceId, out _DeviceInfo deviceInfo)
    {
        int structSize = Marshal.SizeOf(typeof(_DeviceInfo));
        IntPtr deviceInfoPtr = Marshal.AllocHGlobal(structSize);

        RazerError error = ((delegate* unmanaged[Cdecl]<Guid, IntPtr, RazerError>)ThrowIfZero(_queryDevicePointer))(deviceId, deviceInfoPtr);

        deviceInfo = (_DeviceInfo)Marshal.PtrToStructure(deviceInfoPtr, typeof(_DeviceInfo))!;
        Marshal.FreeHGlobal(deviceInfoPtr);

        return error;
    }

    internal static unsafe RazerError CreateEffect(Guid deviceId, int effectType, IntPtr param, ref Guid effectId)
        => ((delegate* unmanaged[Cdecl]<Guid, int, IntPtr, ref Guid, RazerError>)ThrowIfZero(_createEffectPointer))(deviceId, effectType, param, ref effectId);

    internal static unsafe RazerError CreateHeadsetEffect(int effectType, IntPtr param, ref Guid effectId)
        => ((delegate* unmanaged[Cdecl]<int, IntPtr, ref Guid, RazerError>)ThrowIfZero(_createHeadsetEffectPointer))(effectType, param, ref effectId);

    internal static unsafe RazerError CreateChromaLinkEffect(int effectType, IntPtr param, ref Guid effectId)
        => ((delegate* unmanaged[Cdecl]<int, IntPtr, ref Guid, RazerError>)ThrowIfZero(_createChromaLinkEffectPointer))(effectType, param, ref effectId);

    internal static unsafe RazerError CreateKeyboardEffect(int effectType, IntPtr param, ref Guid effectId)
        => ((delegate* unmanaged[Cdecl]<int, IntPtr, ref Guid, RazerError>)ThrowIfZero(_createKeyboardEffectPointer))(effectType, param, ref effectId);

    internal static unsafe RazerError CreateKeypadEffect(int effectType, IntPtr param, ref Guid effectId)
        => ((delegate* unmanaged[Cdecl]<int, IntPtr, ref Guid, RazerError>)ThrowIfZero(_createKeypadEffectPointer))(effectType, param, ref effectId);

    internal static unsafe RazerError CreateMouseEffect(int effectType, IntPtr param, ref Guid effectId)
        => ((delegate* unmanaged[Cdecl]<int, IntPtr, ref Guid, RazerError>)ThrowIfZero(_createMouseEffectPointer))(effectType, param, ref effectId);

    internal static unsafe RazerError CreateMousepadEffect(int effectType, IntPtr param, ref Guid effectId)
        => ((delegate* unmanaged[Cdecl]<int, IntPtr, ref Guid, RazerError>)ThrowIfZero(_createMousepadEffectPointer))(effectType, param, ref effectId);

    internal static unsafe RazerError SetEffect(Guid effectId)
        => ((delegate* unmanaged[Cdecl]<Guid, RazerError>)ThrowIfZero(_setEffectPointer))(effectId);

    internal static unsafe RazerError DeleteEffect(Guid effectId)
        => ((delegate* unmanaged[Cdecl]<Guid, RazerError>)ThrowIfZero(_deleteEffectPointer))(effectId);

    private static IntPtr ThrowIfZero(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero) throw new RGBDeviceException("The Razer-SDK is not initialized.");
        return ptr;
    }

    #endregion
}