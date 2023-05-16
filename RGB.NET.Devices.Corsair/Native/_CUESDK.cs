#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Events;

namespace RGB.NET.Devices.Corsair.Native;

internal delegate void CorsairSessionStateChangedHandler(nint context, _CorsairSessionStateChanged eventData);

internal delegate void CorsairEventHandler(nint context, _CorsairEvent corsairEvent);

// ReSharper disable once InconsistentNaming
internal static unsafe class _CUESDK
{
    #region Constants

    /// <summary>
    /// iCUE-SDK: small string length
    /// </summary>
    internal const int CORSAIR_STRING_SIZE_S = 64;

    /// <summary>
    /// iCUE-SDK: medium string length
    /// </summary>
    internal const int CORSAIR_STRING_SIZE_M = 128;

    /// <summary>
    /// iCUE-SDK: maximum level of layer’s priority that can be used in CorsairSetLayerPriority
    /// </summary>
    internal const int CORSAIR_LAYER_PRIORITY_MAX = 255;

    /// <summary>
    /// iCUE-SDK: maximum number of devices to be discovered
    /// </summary>
    internal const int CORSAIR_DEVICE_COUNT_MAX = 64;

    /// <summary>
    /// iCUE-SDK: maximum number of LEDs controlled by device
    /// </summary>
    internal const int CORSAIR_DEVICE_LEDCOUNT_MAX = 512;

    #endregion

    #region Properties & Fields

    internal static bool IsConnected => SessionState == CorsairSessionState.Connected;
    internal static CorsairSessionState SessionState { get; private set; }

    #endregion

    #region Events

    internal static event EventHandler<CorsairSessionState>? SessionStateChanged;

    internal static event EventHandler<CorsairDeviceConnectionStatusChangedEvent>? DeviceConnectionEvent; 

    #endregion

    #region Methods

    private static void CorsairSessionStateChangedCallback(nint context, _CorsairSessionStateChanged eventData)
    {
        SessionState = eventData.state;
        try
        {
            SessionStateChanged?.Invoke(null, eventData.state);
        }
        catch { /* dont let exception go to sdk */ }

        switch (eventData.state)
        {
            case CorsairSessionState.Connected:
                _corsairSubscribeForEvents(CorsairEventCallback, 0);
                break;
            case CorsairSessionState.Closed:
                _corsairUnsubscribeForEvents();
                break;
        }
    }

    private static void CorsairEventCallback(nint context, _CorsairEvent eventData)
    {
        if (eventData.id != CorsairEventId.DeviceConnectionStatusChangedEvent)
        {
            return;
        }

        try
        {
            if (eventData.eventPointer == 0)
            {
                return;
            }

            CorsairDeviceConnectionStatusChangedEvent connectionStatusChangedEvent =
                Marshal.PtrToStructure<CorsairDeviceConnectionStatusChangedEvent>(eventData.eventPointer)!;

            DeviceConnectionEvent?.Invoke(null, connectionStatusChangedEvent);
        }catch { /* dont let exception go to sdk */ }
    }

    #endregion

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
            throw new RGBDeviceException($"Corsair LoadLibrary failed with error code {Marshal.GetLastPInvokeError()}");

        _corsairConnectPtr = (delegate* unmanaged[Cdecl]<CorsairSessionStateChangedHandler, nint, CorsairError>)LoadFunction("CorsairConnect");
        _corsairGetSessionDetails = (delegate* unmanaged[Cdecl]<nint, CorsairError>)LoadFunction("CorsairGetSessionDetails");
        _corsairDisconnect = (delegate* unmanaged[Cdecl]<CorsairError>)LoadFunction("CorsairDisconnect");
        _corsairGetDevices = (delegate* unmanaged[Cdecl]<_CorsairDeviceFilter, int, nint, out int, CorsairError>)LoadFunction("CorsairGetDevices");
        _corsairGetDeviceInfo = (delegate* unmanaged[Cdecl]<string, nint, CorsairError>)LoadFunction("CorsairGetDeviceInfo");
        _corsairGetLedPositions = (delegate* unmanaged[Cdecl]<string, int, nint, out int, CorsairError>)LoadFunction("CorsairGetLedPositions");
        _corsairSetLedColors = (delegate* unmanaged[Cdecl]<string, int, nint, CorsairError>)LoadFunction("CorsairSetLedColors");
        _corsairSetLayerPriority = (delegate* unmanaged[Cdecl]<uint, CorsairError>)LoadFunction("CorsairSetLayerPriority");
        _corsairGetLedLuidForKeyName = (delegate* unmanaged[Cdecl]<string, char, out uint, CorsairError>)LoadFunction("CorsairGetLedLuidForKeyName");
        _corsairRequestControl = (delegate* unmanaged[Cdecl]<string, CorsairAccessLevel, CorsairError>)LoadFunction("CorsairRequestControl");
        _corsairReleaseControl = (delegate* unmanaged[Cdecl]<string, CorsairError>)LoadFunction("CorsairReleaseControl");
        _getDevicePropertyInfo = (delegate* unmanaged[Cdecl]<string, CorsairDevicePropertyId, uint, out CorsairDataType, out CorsairPropertyFlag, CorsairError>)LoadFunction("CorsairGetDevicePropertyInfo");
        _readDeviceProperty = (delegate* unmanaged[Cdecl]<string, CorsairDevicePropertyId, uint, nint, CorsairError>)LoadFunction("CorsairReadDeviceProperty");
        _corsairSubscribeForEvents = (delegate* unmanaged[Cdecl]<CorsairEventHandler, nint, CorsairError>)LoadFunction("CorsairSubscribeForEvents");
        _corsairUnsubscribeForEvents = (delegate* unmanaged[Cdecl]<CorsairError>)LoadFunction("CorsairSubscribeForEvents");
    }

    private static nint LoadFunction(string function)
    {
        if (!NativeLibrary.TryGetExport(_handle, function, out nint ptr)) throw new RGBDeviceException($"Failed to load Corsair function '{function}'");
        return ptr;
    }

    private static IEnumerable<string> GetPossibleLibraryPaths()
    {
        IEnumerable<string> possibleLibraryPaths;

        if (OperatingSystem.IsWindows())
            possibleLibraryPaths = Environment.Is64BitProcess ? CorsairDeviceProvider.PossibleX64NativePaths : CorsairDeviceProvider.PossibleX86NativePaths;
        else
            possibleLibraryPaths = Enumerable.Empty<string>();

        return possibleLibraryPaths.Select(Environment.ExpandEnvironmentVariables);
    }

    internal static void UnloadCUESDK()
    {
        if (_handle == 0) return;

        _corsairConnectPtr = null;
        _corsairGetSessionDetails = null;
        _corsairDisconnect = null;
        _corsairGetDevices = null;
        _corsairGetDeviceInfo = null;
        _corsairGetLedPositions = null;
        _corsairSetLedColors = null;
        _corsairSetLayerPriority = null;
        _corsairGetLedLuidForKeyName = null;
        _corsairRequestControl = null;
        _corsairReleaseControl = null;
        _getDevicePropertyInfo = null;
        _readDeviceProperty = null;
        _corsairSubscribeForEvents = null;
        _corsairUnsubscribeForEvents = null;

        NativeLibrary.Free(_handle);
        _handle = 0;
    }

    #endregion

    #region SDK-METHODS

    #region Pointers

    private static delegate* unmanaged[Cdecl]<CorsairSessionStateChangedHandler, nint, CorsairError> _corsairConnectPtr;
    private static delegate* unmanaged[Cdecl]<nint, CorsairError> _corsairGetSessionDetails;
    private static delegate* unmanaged[Cdecl]<CorsairError> _corsairDisconnect;
    private static delegate* unmanaged[Cdecl]<_CorsairDeviceFilter, int, nint, out int, CorsairError> _corsairGetDevices;
    private static delegate* unmanaged[Cdecl]<string, nint, CorsairError> _corsairGetDeviceInfo;
    private static delegate* unmanaged[Cdecl]<string, int, nint, out int, CorsairError> _corsairGetLedPositions;
    private static delegate* unmanaged[Cdecl]<string, int, nint, CorsairError> _corsairSetLedColors;
    private static delegate* unmanaged[Cdecl]<uint, CorsairError> _corsairSetLayerPriority;
    private static delegate* unmanaged[Cdecl]<string, char, out uint, CorsairError> _corsairGetLedLuidForKeyName;
    private static delegate* unmanaged[Cdecl]<string, CorsairAccessLevel, CorsairError> _corsairRequestControl;
    private static delegate* unmanaged[Cdecl]<string, CorsairError> _corsairReleaseControl;
    private static delegate* unmanaged[Cdecl]<string, CorsairDevicePropertyId, uint, out CorsairDataType, out CorsairPropertyFlag, CorsairError> _getDevicePropertyInfo;
    private static delegate* unmanaged[Cdecl]<string, CorsairDevicePropertyId, uint, nint, CorsairError> _readDeviceProperty;
    private static delegate* unmanaged[Cdecl]<CorsairEventHandler, nint, CorsairError> _corsairSubscribeForEvents;
    private static delegate* unmanaged[Cdecl]<CorsairError> _corsairUnsubscribeForEvents;

    #endregion

    internal static CorsairError CorsairConnect()
    {
        if (_corsairConnectPtr == null) throw new RGBDeviceException("The Corsair-SDK is not initialized.");
        if (IsConnected) throw new RGBDeviceException("The Corsair-SDK is already connected.");
        return _corsairConnectPtr(CorsairSessionStateChangedCallback, 0);
    }

    internal static CorsairError CorsairGetSessionDetails(out _CorsairSessionDetails? details)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");

        nint sessionDetailPtr = Marshal.AllocHGlobal(Marshal.SizeOf<_CorsairSessionDetails>());
        try
        {
            CorsairError error = _corsairGetSessionDetails(sessionDetailPtr);
            details = Marshal.PtrToStructure<_CorsairSessionDetails>(sessionDetailPtr);

            return error;
        }
        finally
        {
            Marshal.FreeHGlobal(sessionDetailPtr);
        }
    }

    internal static CorsairError CorsairDisconnect()
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");
        return _corsairDisconnect();
    }

    internal static CorsairError CorsairGetDevices(_CorsairDeviceFilter filter, out _CorsairDeviceInfo[] devices)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");

        int structSize = Marshal.SizeOf<_CorsairDeviceInfo>();
        nint devicePtr = Marshal.AllocHGlobal(structSize * CORSAIR_DEVICE_COUNT_MAX);
        try
        {
            CorsairError error = _corsairGetDevices(filter, CORSAIR_DEVICE_COUNT_MAX, devicePtr, out int size);
            devices = devicePtr.ToArray<_CorsairDeviceInfo>(size);

            return error;
        }
        finally
        {
            Marshal.FreeHGlobal(devicePtr);
        }
    }

    internal static CorsairError CorsairGetDeviceInfo(string deviceId, _CorsairDeviceInfo deviceInfo)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");

        nint myStructPtr = Marshal.AllocHGlobal(Marshal.SizeOf<_CorsairDeviceInfo>());
        Marshal.StructureToPtr(deviceInfo, myStructPtr, false);
        try
        {
            CorsairError error = _corsairGetDeviceInfo(deviceId, myStructPtr);
            Marshal.PtrToStructure(myStructPtr, deviceInfo);
            return error;
        }
        finally
        {
            Marshal.FreeHGlobal(myStructPtr);
        }
    }

    internal static CorsairError CorsairGetLedPositions(string deviceId, out _CorsairLedPosition[] ledPositions)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");

        int structSize = Marshal.SizeOf<_CorsairLedPosition>();
        nint ledPositionsPtr = Marshal.AllocHGlobal(structSize * CORSAIR_DEVICE_LEDCOUNT_MAX);
        try
        {
            CorsairError error = _corsairGetLedPositions(deviceId, CORSAIR_DEVICE_LEDCOUNT_MAX, ledPositionsPtr, out int size);
            ledPositions = ledPositionsPtr.ToArray<_CorsairLedPosition>(size);

            return error;
        }
        finally
        {
            Marshal.FreeHGlobal(ledPositionsPtr);
        }
    }

    internal static CorsairError CorsairSetLedColors(string deviceId, int ledCount, nint ledColorsPtr)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");
        return _corsairSetLedColors(deviceId, ledCount, ledColorsPtr);
    }

    internal static CorsairError CorsairSetLayerPriority(uint priority)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");
        return _corsairSetLayerPriority(priority);
    }

    internal static CorsairError CorsairGetLedLuidForKeyName(string deviceId, char keyName, out uint ledId)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");
        return _corsairGetLedLuidForKeyName(deviceId, keyName, out ledId);
    }

    internal static CorsairError CorsairRequestControl(string deviceId, CorsairAccessLevel accessLevel)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");
        return _corsairRequestControl(deviceId, accessLevel);
    }

    internal static CorsairError CorsairReleaseControl(string deviceId)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");
        return _corsairReleaseControl(deviceId);
    }

    internal static CorsairError GetDevicePropertyInfo(string deviceId, CorsairDevicePropertyId propertyId, uint index, out CorsairDataType dataType, out CorsairPropertyFlag flags)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");
        return _getDevicePropertyInfo(deviceId, propertyId, index, out dataType, out flags);
    }

    internal static CorsairError ReadDeviceProperty(string deviceId, CorsairDevicePropertyId propertyId, uint index, out _CorsairProperty? property)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");

        nint propertyPtr = Marshal.AllocHGlobal(Marshal.SizeOf<_CorsairProperty>());
        try
        {
            CorsairError error = _readDeviceProperty(deviceId, propertyId, index, propertyPtr);
            property = Marshal.PtrToStructure<_CorsairProperty>(propertyPtr);

            return error;
        }
        finally
        {
            Marshal.FreeHGlobal(propertyPtr);
        }
    }

    internal static int ReadDevicePropertySimpleInt32(string deviceId, CorsairDevicePropertyId propertyId, uint index = 0) => ReadDevicePropertySimple(deviceId, propertyId, CorsairDataType.Int32, index).int32;

    internal static int[] ReadDevicePropertySimpleInt32Array(string deviceId, CorsairDevicePropertyId propertyId, uint index = 0)
    {
        _CorsairDataValue dataValue = ReadDevicePropertySimple(deviceId, propertyId, CorsairDataType.Int32Array, index);
        return dataValue.int32Array.items.ToArray<int>((int)dataValue.int32Array.count);
    }

    internal static _CorsairDataValue ReadDevicePropertySimple(string deviceId, CorsairDevicePropertyId propertyId, CorsairDataType expectedDataType, uint index = 0)
    {
        CorsairError errorCode = GetDevicePropertyInfo(deviceId, propertyId, index, out CorsairDataType dataType, out CorsairPropertyFlag flags);
        if (errorCode != CorsairError.Success)
            throw new RGBDeviceException($"Failed to read device-property-info '{propertyId}' for corsair device '{deviceId}'. (ErrorCode: {errorCode})");

        if (dataType != expectedDataType)
            throw new RGBDeviceException($"Failed to read device-property-info '{propertyId}' for corsair device '{deviceId}'. (Wrong data-type '{dataType}', expected: '{expectedDataType}')");

        if (!flags.HasFlag(CorsairPropertyFlag.CanRead))
            throw new RGBDeviceException($"Failed to read device-property-info '{propertyId}' for corsair device '{deviceId}'. (Not readable)");

        errorCode = ReadDeviceProperty(deviceId, propertyId, index, out _CorsairProperty? property);
        if (errorCode != CorsairError.Success)
            throw new RGBDeviceException($"Failed to read device-property '{propertyId}' for corsair device '{deviceId}'. (ErrorCode: {errorCode})");

        if (property == null)
            throw new RGBDeviceException($"Failed to read device-property '{propertyId}' for corsair device '{deviceId}'. (Invalid return value)");

        if (property.Value.type != expectedDataType)
            throw new RGBDeviceException($"Failed to read device-property '{propertyId}' for corsair device '{deviceId}'. (Wrong data-type '{dataType}', expected: '{expectedDataType}')");

        return property.Value.value;
    }

    #endregion
}