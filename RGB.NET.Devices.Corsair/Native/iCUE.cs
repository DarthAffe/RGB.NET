using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair.Native;

/// <summary>
/// The Corsair Lighting SDK gives ability for third-party applications to control lightings on Corsair RGB devices. Corsair Lighting SDK interacts with hardware through CUE so it should be running in order for SDK to work properly.
/// </summary>
public static class iCUE
{
    private const string ICUESDK_X64_DLL = "x64\\iCUESDK.x64_2019.dll";

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SubscribeForEventsCallback(nint context, _CorsairEvent cEvent);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ConnectCallback(nint context, _CorsairSessionStateChanged cEvent);

    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairSetLedColors", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError SetLedColors(string deviceId, int size, nint ledColors);
    
    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairGetDevices", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError GetDevices(_CorsairDeviceFilter filter, int deviceCount, ref _CorsairDeviceInfo arrayPtr, out int size);

    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairGetDeviceInfo", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError GetDeviceInfo(string deviceId, ref _CorsairDeviceInfo deviceInfoPtr);
    
    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairGetDevicePropertyInfo", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError GetDevicePropertyInfo(string deviceId, CorsairDevicePropertyId property, uint index, out CorsairDataType daType, out CorsairPropertyFlag flags);

    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairReadDeviceProperty", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError ReadDeviceProperty(string deviceId, CorsairDevicePropertyId property, uint index, ref _CorsairProperty daType);
    
    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairFreeProperty", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError FreeProperty(ref _CorsairProperty daType);

    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairGetLedPositions", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError GetLedPositions(string deviceId, int sizeMax, ref _CorsairLedPosition ledPositions, out int size);

    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairRequestControl", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError RequestControl(string deviceId, CorsairAccessLevel accessMode);

    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairReleaseControl", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError ReleaseControl(string deviceId);

    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairSetLayerPriority", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError SetLayerPriority(uint priority);

    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairSubscribeForEvents", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError SubscribeForEvents(SubscribeForEventsCallback corsairEventHandler, nint context);

    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairUnsubscribeFromEvents", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError UnsubscribeFromEvents();

    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairConnect", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError Connect(ConnectCallback connectCallback, nint context);

    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairGetSessionDetails", CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe CorsairError GetSessionDetails(ref _CorsairSessionDetails sessionDetailsPtr);

    [DllImport(ICUESDK_X64_DLL, EntryPoint = "CorsairDisconnect", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CorsairError Disconnect();
}