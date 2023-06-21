// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for corsair (CUE) devices.
/// </summary>
public sealed class CorsairDeviceProvider : AbstractRGBDeviceProvider
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
    public static List<string> PossibleX86NativePaths { get; } = new() { "x86/iCUESDK.dll", "x86/CUESDK_2019.dll" };

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX64NativePaths { get; } = new() { "x64/iCUESDK.dll", "x64/iCUESDK.x64_2019.dll", "x64/CUESDK.dll", "x64/CUESDK.x64_2019.dll" };

    /// <summary>
    /// Gets or sets the timeout used when connecting to the SDK.
    /// </summary>
    public static TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromMilliseconds(500);

    /// <summary>
    /// Gets or sets a bool indicating if exclusive request should be requested through the iCUE-SDK.
    /// </summary>
    public static bool ExclusiveAccess { get; set; } = false;

    /// <summary>
    /// Gets the details for the current SDK-session.
    /// </summary>
    public CorsairSessionDetails SessionDetails { get; private set; } = new();

    private CorsairSessionState _sessionState = CorsairSessionState.Invalid;
    public CorsairSessionState SessionState
    {
        get => _sessionState;
        private set
        {
            _sessionState = value;

            try { SessionStateChanged?.Invoke(this, SessionState); }
            catch { /* catch faulty event-handlers*/ }
        }
    }

    #endregion

    #region Events

    // ReSharper disable once UnassignedField.Global
    public EventHandler<CorsairSessionState>? SessionStateChanged;

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

    protected override void InitializeSDK()
    {
        _CUESDK.SessionStateChanged -= CancelOnConnect;
        _CUESDK.Reload();

        using ManualResetEventSlim waitEvent = new(false);

        void OnInitializeSessionStateChanged(object? sender, CorsairSessionState state)
        {
            if (state == CorsairSessionState.Connected)
                // ReSharper disable once AccessToDisposedClosure
                waitEvent.Set();
        }

        try
        {
            _CUESDK.SessionStateChanged += OnSessionStateChanged;
            _CUESDK.SessionStateChanged += OnInitializeSessionStateChanged;

            CorsairError errorCode = _CUESDK.CorsairConnect();
            if (errorCode != CorsairError.Success)
                Throw(new RGBDeviceException($"Failed to initialized Corsair-SDK. (ErrorCode: {errorCode})"));

            if (!waitEvent.Wait(ConnectionTimeout))
            {
                _CUESDK.SessionStateChanged += CancelOnConnect; //We can't cancel connection. All we can do is disconnect after connection
                Throw(new RGBDeviceException($"Failed to initialized Corsair-SDK. (Timeout - Current connection state: {_CUESDK.SessionState})"));
            }

            _CUESDK.CorsairGetSessionDetails(out _CorsairSessionDetails? details);
            if (errorCode != CorsairError.Success)
                Throw(new RGBDeviceException($"Failed to get session details. (ErrorCode: {errorCode})"));

            SessionDetails = new CorsairSessionDetails(details!);
            _CUESDK.DeviceConnectionEvent += OnDeviceConnectionEvent;
        }
        finally
        {
            _CUESDK.SessionStateChanged -= OnInitializeSessionStateChanged;
        }
    }

    private void CancelOnConnect(object? sender, CorsairSessionState e)
    {
        if (e != CorsairSessionState.Connected) return;
        _CUESDK.SessionStateChanged -= CancelOnConnect;
        _CUESDK.CorsairDisconnect();
    }

    private void OnDeviceConnectionEvent(object? sender, _CorsairDeviceConnectionStatusChangedEvent connectionStatusChangedEvent)
    {
        string? deviceId = connectionStatusChangedEvent.deviceId;
        if (string.IsNullOrWhiteSpace(deviceId)) return;
        if (connectionStatusChangedEvent.isConnected)
        {
            _CUESDK.CorsairGetDeviceInfo(deviceId, out _CorsairDeviceInfo deviceInfo);
            IDeviceUpdateTrigger deviceUpdateTrigger = GetUpdateTrigger();
            IEnumerable<ICorsairRGBDevice> device = LoadDevice(deviceInfo, deviceUpdateTrigger);
            foreach (ICorsairRGBDevice corsairRGBDevice in device)
            {
                corsairRGBDevice.Initialize();
                if (AddDevice(corsairRGBDevice))
                {
                    deviceUpdateTrigger.Start();
                }
            }
        }
        else
        {
            IRGBDevice? removedDevice = Devices.FirstOrDefault(device => ((ICorsairRGBDevice)device).DeviceId == deviceId);
            if (removedDevice == null) return;
            RemoveDevice(removedDevice);    //TODO disposing the device disposes device queue!
        }
    }

    private void OnSessionStateChanged(object? sender, CorsairSessionState state) => SessionState = state;

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
        CorsairError error = _CUESDK.CorsairGetDevices(new _CorsairDeviceFilter(CorsairDeviceType.All), out _CorsairDeviceInfo[] devices);
        if (error != CorsairError.Success)
            Throw(new RGBDeviceException($"Failed to load devices. (ErrorCode: {error})"));

        return devices.SelectMany(LoadDevice);
    }

    private IEnumerable<ICorsairRGBDevice> LoadDevice(_CorsairDeviceInfo device)
    {
        return LoadDevice(device, GetUpdateTrigger());
    }

    private IEnumerable<ICorsairRGBDevice> LoadDevice(_CorsairDeviceInfo device, IDeviceUpdateTrigger updateTrigger)
    {
        if (string.IsNullOrWhiteSpace(device.id)) yield break;

        // sometimes it is okay to fail :) (can cause problems with reconnections)
        _CUESDK.CorsairRequestControl(device.id, ExclusiveAccess ? CorsairAccessLevel.ExclusiveLightingControl : CorsairAccessLevel.Shared);

        CorsairDeviceUpdateQueue updateQueue = new(updateTrigger, device);

        int channelLedCount = 0;
        for (int i = 0; i < device.channelCount; i++)
        {
            Console.WriteLine($"Channel {i}/{device.channelCount}");
            channelLedCount += _CUESDK.ReadDevicePropertySimpleInt32(device.id!, CorsairDevicePropertyId.ChannelLedCount, (uint)i);
        }

        int deviceLedCount = device.ledCount - channelLedCount;
        if (deviceLedCount > 0)
        {
            ICorsairRGBDevice singleChannelDevice = CreateSingleChannelDevice(device, deviceLedCount, updateQueue);
            yield return singleChannelDevice;
        }


        int offset = deviceLedCount;
        for (int i = 0; i < device.channelCount; i++)
        {
            int deviceCount = _CUESDK.ReadDevicePropertySimpleInt32(device.id!, CorsairDevicePropertyId.ChannelDeviceCount, (uint)i);
            if (deviceCount <= 0)
                continue; // DarthAffe 10.02.2023: There seem to be an issue in the SDK where it reports empty channels and fails
                          // when getting ledCounts and device types from them

            int[] ledCounts =
                _CUESDK.ReadDevicePropertySimpleInt32Array(device.id!, CorsairDevicePropertyId.ChannelDeviceLedCountArray, (uint)i);
            int[] deviceTypes = _CUESDK.ReadDevicePropertySimpleInt32Array(device.id!, CorsairDevicePropertyId.ChannelDeviceTypeArray, (uint)i);

            for (int j = 0; j < deviceCount; j++)
            {
                CorsairChannelDeviceType deviceType = (CorsairChannelDeviceType)deviceTypes[j];
                int ledCount = ledCounts[j];

                yield return CreateCorsairDeviceChannel(device, deviceType, ledCount, offset, updateQueue);

                offset += ledCount;
            }
        }
    }

    private static ICorsairRGBDevice CreateSingleChannelDevice(_CorsairDeviceInfo device, int deviceLedCount, CorsairDeviceUpdateQueue updateQueue)
    {
        return device.type switch
        {
            CorsairDeviceType.Keyboard =>
                new CorsairKeyboardRGBDevice(new CorsairKeyboardRGBDeviceInfo(device, deviceLedCount, 0), updateQueue),

            CorsairDeviceType.Mouse =>
                new CorsairMouseRGBDevice(new CorsairMouseRGBDeviceInfo(device, deviceLedCount, 0), updateQueue),

            CorsairDeviceType.Headset =>
                new CorsairHeadsetRGBDevice(new CorsairHeadsetRGBDeviceInfo(device, deviceLedCount, 0), updateQueue),

            CorsairDeviceType.Mousemat =>
                new CorsairMousepadRGBDevice(new CorsairMousepadRGBDeviceInfo(device, deviceLedCount, 0), updateQueue),

            CorsairDeviceType.HeadsetStand =>
                new CorsairHeadsetStandRGBDevice(new CorsairHeadsetStandRGBDeviceInfo(device, deviceLedCount, 0), updateQueue),

            CorsairDeviceType.MemoryModule =>
                new CorsairMemoryRGBDevice(new CorsairMemoryRGBDeviceInfo(device, deviceLedCount, 0), updateQueue),

            CorsairDeviceType.Motherboard =>
                new CorsairMainboardRGBDevice(new CorsairMainboardRGBDeviceInfo(device, deviceLedCount, 0), updateQueue),

            CorsairDeviceType.GraphicsCard =>
                new CorsairGraphicsCardRGBDevice(new CorsairGraphicsCardRGBDeviceInfo(device, deviceLedCount, 0), updateQueue),

            CorsairDeviceType.Touchbar =>
                new CorsairTouchbarRGBDevice(new CorsairTouchbarRGBDeviceInfo(device, deviceLedCount, 0), updateQueue),

            CorsairDeviceType.Cooler =>
                new CorsairCoolerRGBDevice(new CorsairCoolerRGBDeviceInfo(device, deviceLedCount, 0), updateQueue),

            CorsairDeviceType.FanLedController or 
                CorsairDeviceType.LedController or 
                CorsairDeviceType.Unknown =>
                new CorsairUnknownRGBDevice(new CorsairUnknownRGBDeviceInfo(device, deviceLedCount, 0), updateQueue),

            _ => throw new RGBDeviceException("Unknown Device-Type")
        };
    }

    private static ICorsairRGBDevice CreateCorsairDeviceChannel(_CorsairDeviceInfo device, CorsairChannelDeviceType deviceType,
                                                                int ledCount, int offset, CorsairDeviceUpdateQueue updateQueue)
    {
        return deviceType switch
        {
            CorsairChannelDeviceType.FanHD =>
                new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "HD Fan"), updateQueue),

            CorsairChannelDeviceType.FanSP =>
                new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "SP Fan"), updateQueue),

            CorsairChannelDeviceType.FanLL =>
                new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "LL Fan"), updateQueue),

            CorsairChannelDeviceType.FanML =>
                new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "ML Fan"), updateQueue),

            CorsairChannelDeviceType.FanQL =>
                new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "QL Fan"), updateQueue),

            CorsairChannelDeviceType.EightLedSeriesFan =>
                new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "8-Led-Series Fan Fan"), updateQueue),

            CorsairChannelDeviceType.DAP =>
                new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "DAP Fan"), updateQueue),

            CorsairChannelDeviceType.Pump =>
                new CorsairCoolerRGBDevice(new CorsairCoolerRGBDeviceInfo(device, ledCount, offset, "Pump"), updateQueue),

            CorsairChannelDeviceType.WaterBlock =>
                new CorsairCoolerRGBDevice(new CorsairCoolerRGBDeviceInfo(device, ledCount, offset, "Water Block"), updateQueue),

            CorsairChannelDeviceType.Strip => CreateLedStripDevice(device, ledCount, offset, updateQueue),

            CorsairChannelDeviceType.DRAM =>
                new CorsairMemoryRGBDevice(new CorsairMemoryRGBDeviceInfo(device, ledCount, offset, "DRAM"), updateQueue),

            _ => throw new RGBDeviceException("Unknown Device-Type")
        };
    }

    private static CorsairLedStripRGBDevice CreateLedStripDevice(_CorsairDeviceInfo device, int ledCount, int offset,
                                                                 CorsairDeviceUpdateQueue updateQueue)
    {
        string modelName = "LED Strip";

        // LS100 Led Strips are reported as one big strip if configured in monitor mode in iCUE, 138 LEDs for dual monitor, 84 for single
        if ((device.model == "LS100 Starter Kit") && (ledCount == 138))
            modelName = "LS100 LED Strip (dual monitor)";
        else if ((device.model == "LS100 Starter Kit") && (ledCount == 84))
            modelName = "LS100 LED Strip (single monitor)";
        // Any other value means an "External LED Strip" in iCUE, these are reported per-strip, 15 for short strips, 27 for long
        else if ((device.model == "LS100 Starter Kit") && (ledCount == 15))
            modelName = "LS100 LED Strip (short)";
        else if ((device.model == "LS100 Starter Kit") && (ledCount == 27))
            modelName = "LS100 LED Strip (long)";

        CorsairLedStripRGBDevice ledStripDevice = new(new CorsairLedStripRGBDeviceInfo(device, ledCount, offset, modelName), updateQueue);
        return ledStripDevice;
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        try { _CUESDK.CorsairDisconnect(); } catch { /* at least we tried */ }
        try { _CUESDK.UnloadCUESDK(); } catch { /* at least we tried */ }
    }

    #endregion
}