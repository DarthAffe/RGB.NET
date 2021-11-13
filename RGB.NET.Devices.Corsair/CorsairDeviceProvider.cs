// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for corsair (CUE) devices.
/// </summary>
public class CorsairDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static CorsairDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="CorsairDeviceProvider"/> instance.
    /// </summary>
    public static CorsairDeviceProvider Instance => _instance ?? new CorsairDeviceProvider();

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX86NativePaths { get; } = new() { "x86/CUESDK.dll", "x86/CUESDK_2015.dll", "x86/CUESDK_2013.dll" };

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX64NativePaths { get; } = new() { "x64/CUESDK.dll", "x64/CUESDK_2015.dll", "x64/CUESDK_2013.dll" };

    /// <summary>
    /// Gets the protocol details for the current SDK-connection.
    /// </summary>
    public CorsairProtocolDetails? ProtocolDetails { get; private set; }

    /// <summary>
    /// Gets the last error documented by CUE.
    /// </summary>
    public static CorsairError LastError => _CUESDK.CorsairGetLastError();

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CorsairDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public CorsairDeviceProvider()
    {
        if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(CorsairDeviceProvider)}");
        _instance = this;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void InitializeSDK()
    {
        _CUESDK.Reload();

        ProtocolDetails = new CorsairProtocolDetails(_CUESDK.CorsairPerformProtocolHandshake());

        CorsairError error = LastError;
        if (error != CorsairError.Success)
            Throw(new CUEException(error), true);

        if (ProtocolDetails.BreakingChanges)
            Throw(new RGBDeviceException("The SDK currently used isn't compatible with the installed version of CUE.\r\n"
                                       + $"CUE-Version: {ProtocolDetails.ServerVersion} (Protocol {ProtocolDetails.ServerProtocolVersion})\r\n"
                                       + $"SDK-Version: {ProtocolDetails.SdkVersion} (Protocol {ProtocolDetails.SdkProtocolVersion})"), true);

        // DarthAffe 02.02.2021: 127 is iCUE
        if (!_CUESDK.CorsairSetLayerPriority(128))
            Throw(new CUEException(LastError));
    }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        foreach (ICorsairRGBDevice corsairDevice in LoadCorsairDevices())
        {
            corsairDevice.Initialize();
            yield return corsairDevice;
        }
    }

    private IEnumerable<ICorsairRGBDevice> LoadCorsairDevices()
    {
        int deviceCount = _CUESDK.CorsairGetDeviceCount();
        for (int i = 0; i < deviceCount; i++)
        {
            _CorsairDeviceInfo nativeDeviceInfo = (_CorsairDeviceInfo)Marshal.PtrToStructure(_CUESDK.CorsairGetDeviceInfo(i), typeof(_CorsairDeviceInfo))!;
            if (!((CorsairDeviceCaps)nativeDeviceInfo.capsMask).HasFlag(CorsairDeviceCaps.Lighting))
                continue; // Everything that doesn't support lighting control is useless

            CorsairDeviceUpdateQueue updateQueue = new(GetUpdateTrigger(), i);
            switch (nativeDeviceInfo.type)
            {
                case CorsairDeviceType.Keyboard:
                    yield return new CorsairKeyboardRGBDevice(new CorsairKeyboardRGBDeviceInfo(i, nativeDeviceInfo), updateQueue);
                    break;

                case CorsairDeviceType.Mouse:
                    yield return new CorsairMouseRGBDevice(new CorsairMouseRGBDeviceInfo(i, nativeDeviceInfo), updateQueue);
                    break;

                case CorsairDeviceType.Headset:
                    yield return new CorsairHeadsetRGBDevice(new CorsairHeadsetRGBDeviceInfo(i, nativeDeviceInfo), updateQueue);
                    break;

                case CorsairDeviceType.Mousepad:
                    yield return new CorsairMousepadRGBDevice(new CorsairMousepadRGBDeviceInfo(i, nativeDeviceInfo), updateQueue);
                    break;

                case CorsairDeviceType.HeadsetStand:
                    yield return new CorsairHeadsetStandRGBDevice(new CorsairHeadsetStandRGBDeviceInfo(i, nativeDeviceInfo), updateQueue);
                    break;

                case CorsairDeviceType.MemoryModule:
                    yield return new CorsairMemoryRGBDevice(new CorsairMemoryRGBDeviceInfo(i, nativeDeviceInfo), updateQueue);
                    break;

                case CorsairDeviceType.Mainboard:
                    yield return new CorsairMainboardRGBDevice(new CorsairMainboardRGBDeviceInfo(i, nativeDeviceInfo), updateQueue);
                    break;

                case CorsairDeviceType.GraphicsCard:
                    yield return new CorsairGraphicsCardRGBDevice(new CorsairGraphicsCardRGBDeviceInfo(i, nativeDeviceInfo), updateQueue);
                    break;

                case CorsairDeviceType.Cooler:
                case CorsairDeviceType.CommanderPro:
                case CorsairDeviceType.LightningNodePro:
                    _CorsairChannelsInfo? channelsInfo = nativeDeviceInfo.channels;
                    if (channelsInfo != null)
                    {
                        IntPtr channelInfoPtr = channelsInfo.channels;
                        int ledOffset = 0;

                        for (int channel = 0; channel < channelsInfo.channelsCount; channel++)
                        {
                            CorsairLedId referenceLed = GetChannelReferenceId(nativeDeviceInfo.type, channel);
                            if (referenceLed == CorsairLedId.Invalid) continue;

                            _CorsairChannelInfo channelInfo = (_CorsairChannelInfo)Marshal.PtrToStructure(channelInfoPtr, typeof(_CorsairChannelInfo))!;

                            int channelDeviceInfoStructSize = Marshal.SizeOf(typeof(_CorsairChannelDeviceInfo));
                            IntPtr channelDeviceInfoPtr = channelInfo.devices;

                            for (int device = 0; (device < channelInfo.devicesCount) && (ledOffset < nativeDeviceInfo.ledsCount); device++)
                            {
                                _CorsairChannelDeviceInfo channelDeviceInfo = (_CorsairChannelDeviceInfo)Marshal.PtrToStructure(channelDeviceInfoPtr, typeof(_CorsairChannelDeviceInfo))!;

                                yield return new CorsairCustomRGBDevice(new CorsairCustomRGBDeviceInfo(i, nativeDeviceInfo, channelDeviceInfo, referenceLed, ledOffset), updateQueue);
                                referenceLed += channelDeviceInfo.deviceLedCount;

                                ledOffset += channelDeviceInfo.deviceLedCount;
                                channelDeviceInfoPtr = new IntPtr(channelDeviceInfoPtr.ToInt64() + channelDeviceInfoStructSize);
                            }

                            int channelInfoStructSize = Marshal.SizeOf(typeof(_CorsairChannelInfo));
                            channelInfoPtr = new IntPtr(channelInfoPtr.ToInt64() + channelInfoStructSize);
                        }
                    }
                    break;
                default:
                    Throw(new RGBDeviceException("Unknown Device-Type"));
                    break;
            }
        }
    }

    private static CorsairLedId GetChannelReferenceId(CorsairDeviceType deviceType, int channel)
    {
        if (deviceType == CorsairDeviceType.Cooler)
            return CorsairLedId.CustomLiquidCoolerChannel1Led1;

        return channel switch
        {
            0 => CorsairLedId.CustomDeviceChannel1Led1,
            1 => CorsairLedId.CustomDeviceChannel2Led1,
            2 => CorsairLedId.CustomDeviceChannel3Led1,
            _ => CorsairLedId.Invalid
        };
    }

    /// <inheritdoc />
    protected override void Reset()
    {
        ProtocolDetails = null;

        base.Reset();
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        try { _CUESDK.UnloadCUESDK(); }
        catch { /* at least we tried */ }

        GC.SuppressFinalize(this);
    }

    #endregion
}