#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Events;

namespace RGB.NET.Devices.Corsair.Native;

// ReSharper disable once InconsistentNaming
internal static class _CUESDK
{
    #region Constants

    /// <summary>
    /// iCUE-SDK: medium string length
    /// </summary>
    internal const int CORSAIR_STRING_SIZE_M = 128;

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
    internal static CorsairSessionState SessionState { get; private set; } = CorsairSessionState.Invalid;

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
    }

    internal static void SubscribeToEvents()
    {
        iCUE.UnsubscribeFromEvents();
        iCUE.SubscribeForEvents(CorsairEventCallback, 0);
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

    internal static CorsairError CorsairConnect()
    {
        if (IsConnected) throw new RGBDeviceException("The Corsair-SDK is already connected.");
        return iCUE.Connect(CorsairSessionStateChangedCallback, 0);
    }

    internal static CorsairError CorsairGetSessionDetails(out _CorsairSessionDetails details)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");

        details = new _CorsairSessionDetails();
        return iCUE.GetSessionDetails(ref details);
    }

    internal static CorsairError CorsairDisconnect()
    {
        SessionState = CorsairSessionState.Invalid;
        return iCUE.Disconnect();
    }

    internal static CorsairError CorsairGetDevices(_CorsairDeviceFilter filter, out _CorsairDeviceInfo[] devices)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");

        _CorsairDeviceInfo[] buffer = new _CorsairDeviceInfo[CORSAIR_DEVICE_COUNT_MAX];
        CorsairError error = iCUE.GetDevices(filter, buffer.Length, ref buffer[0], out int size);

        devices = new _CorsairDeviceInfo[size];
        Array.Copy(buffer, devices, size);

        return error;
    }

    internal static CorsairError CorsairGetDeviceInfo(string deviceId, out _CorsairDeviceInfo deviceInfo)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");

        deviceInfo = new _CorsairDeviceInfo();
        return iCUE.GetDeviceInfo(deviceId, ref deviceInfo);
    }

    internal static CorsairError CorsairGetLedPositions(string deviceId, out _CorsairLedPosition[] ledPositions)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");

        _CorsairLedPosition[] buffer = new _CorsairLedPosition[CORSAIR_DEVICE_LEDCOUNT_MAX];
        CorsairError error = iCUE.GetLedPositions(deviceId, buffer.Length, ref buffer[0], out int size);
        
        ledPositions = new _CorsairLedPosition[size];
        Array.Copy(buffer, ledPositions, size);

        return error;
    }

    internal static CorsairError CorsairSetLedColors(string deviceId, int ledCount, nint ledColorsPtr)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");
        return iCUE.SetLedColors(deviceId, ledCount, ledColorsPtr);
    }

    internal static CorsairError CorsairSetLayerPriority(uint priority)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");
        return iCUE.SetLayerPriority(priority);
    }

    internal static CorsairError CorsairRequestControl(string deviceId, CorsairAccessLevel accessLevel)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");
        return iCUE.RequestControl(deviceId, accessLevel);
    }

    internal static CorsairError CorsairReleaseControl(string deviceId)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");
        return iCUE.ReleaseControl(deviceId);
    }

    internal static CorsairError GetDevicePropertyInfo(string deviceId, CorsairDevicePropertyId propertyId, uint index, out CorsairDataType dataType, out CorsairPropertyFlag flags)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");
        return iCUE.GetDevicePropertyInfo(deviceId, propertyId, index, out dataType, out flags);
    }

    internal static CorsairError ReadDeviceProperty(string deviceId, CorsairDevicePropertyId propertyId, uint index, out _CorsairProperty property)
    {
        if (!IsConnected) throw new RGBDeviceException("The Corsair-SDK is not connected.");

        property = new _CorsairProperty();
        return iCUE.ReadDeviceProperty(deviceId, propertyId, index, ref property);
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

        errorCode = ReadDeviceProperty(deviceId, propertyId, index, out _CorsairProperty property);
        if (errorCode != CorsairError.Success)
            throw new RGBDeviceException($"Failed to read device-property '{propertyId}' for corsair device '{deviceId}'. (Invalid return value)");

        if (property.type != expectedDataType)
            throw new RGBDeviceException($"Failed to read device-property '{propertyId}' for corsair device '{deviceId}'. (Wrong data-type '{dataType}', expected: '{expectedDataType}')");

        return property.value;
    }

    #endregion
}