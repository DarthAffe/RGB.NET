// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.CorsairLegacy.Native;

namespace RGB.NET.Devices.CorsairLegacy;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for corsair (CUE) devices.
/// </summary>
public sealed class CorsairLegacyDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    // ReSharper disable once InconsistentNaming
    private static readonly object _lock = new();

    private static CorsairLegacyDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="CorsairLegacyDeviceProvider"/> instance.
    /// </summary>
    public static CorsairLegacyDeviceProvider Instance
    {
        get
        {
            lock (_lock)
                return _instance ?? new CorsairLegacyDeviceProvider();
        }
    }

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX86NativePaths { get; } = new() { "x86/CUESDK.dll", "x86/CUESDK_2019.dll", "x86/CUESDK_2017.dll", "x86/CUESDK_2015.dll", "x86/CUESDK_2013.dll" };

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX64NativePaths { get; } = new() { "x64/CUESDK.dll", "x64/CUESDK.x64_2019.dll", "x64/CUESDK.x64_2017.dll", "x64/CUESDK_2019.dll", "x64/CUESDK_2017.dll", "x64/CUESDK_2015.dll", "x64/CUESDK_2013.dll" };

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
    /// Initializes a new instance of the <see cref="CorsairLegacyDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public CorsairLegacyDeviceProvider()
    {
        lock (_lock)
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(CorsairLegacyDeviceProvider)}");
            _instance = this;
        }
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

                case CorsairDeviceType.Touchbar:
                    yield return new CorsairTouchbarRGBDevice(new CorsairTouchbarRGBDeviceInfo(i, nativeDeviceInfo), updateQueue);
                    break;

                case CorsairDeviceType.Cooler:
                case CorsairDeviceType.CommanderPro:
                case CorsairDeviceType.LightningNodePro:
                    List<_CorsairChannelInfo> channels = GetChannels(nativeDeviceInfo).ToList();
                    int channelsLedCount = channels.Sum(x => x.totalLedsCount);
                    int deviceLedCount = nativeDeviceInfo.ledsCount - channelsLedCount;

                    if (deviceLedCount > 0)
                        yield return new CorsairCustomRGBDevice(new CorsairCustomRGBDeviceInfo(i, nativeDeviceInfo, deviceLedCount), updateQueue);

                    int ledOffset = deviceLedCount;
                    foreach (_CorsairChannelInfo channelInfo in channels)
                    {
                        int channelDeviceInfoStructSize = Marshal.SizeOf(typeof(_CorsairChannelDeviceInfo));
                        IntPtr channelDeviceInfoPtr = channelInfo.devices;
                        for (int device = 0; (device < channelInfo.devicesCount) && (ledOffset < nativeDeviceInfo.ledsCount); device++)
                        {
                            _CorsairChannelDeviceInfo channelDeviceInfo = (_CorsairChannelDeviceInfo)Marshal.PtrToStructure(channelDeviceInfoPtr, typeof(_CorsairChannelDeviceInfo))!;

                            yield return new CorsairCustomRGBDevice(new CorsairCustomRGBDeviceInfo(i, nativeDeviceInfo, channelDeviceInfo, ledOffset), updateQueue);

                            ledOffset += channelDeviceInfo.deviceLedCount;
                            channelDeviceInfoPtr = new IntPtr(channelDeviceInfoPtr.ToInt64() + channelDeviceInfoStructSize);
                        }
                    }
                    break;

                default:
                    Throw(new RGBDeviceException("Unknown Device-Type"));
                    break;
            }
        }
    }

    private static IEnumerable<_CorsairChannelInfo> GetChannels(_CorsairDeviceInfo deviceInfo)
    {
        _CorsairChannelsInfo? channelsInfo = deviceInfo.channels;
        if (channelsInfo == null) yield break;

        IntPtr channelInfoPtr = channelsInfo.channels;
        for (int channel = 0; channel < channelsInfo.channelsCount; channel++)
        {
            yield return (_CorsairChannelInfo)Marshal.PtrToStructure(channelInfoPtr, typeof(_CorsairChannelInfo))!;

            int channelInfoStructSize = Marshal.SizeOf(typeof(_CorsairChannelInfo));
            channelInfoPtr = new IntPtr(channelInfoPtr.ToInt64() + channelInfoStructSize);
        }
    }

    /// <inheritdoc />
    protected override void Reset()
    {
        ProtocolDetails = null;

        base.Reset();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        lock (_lock)
        {
            base.Dispose(disposing);

            try { _CUESDK.UnloadCUESDK(); }
            catch { /* at least we tried */ }

            _instance = null;
        }
    }

    #endregion
}