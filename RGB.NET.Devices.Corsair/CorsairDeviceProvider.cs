// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
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

    // ReSharper disable once InconsistentNaming
    private static readonly Lock _lock = new();

    private static CorsairDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="CorsairDeviceProvider"/> instance.
    /// </summary>
    public static CorsairDeviceProvider Instance
    {
        get
        {
            lock (_lock)
                return _instance ?? new CorsairDeviceProvider();
        }
    }

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX86NativePaths { get; } = ["x86/iCUESDK.dll", "x86/CUESDK_2019.dll"];

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX64NativePaths { get; } = ["x64/iCUESDK.dll", "x64/iCUESDK.x64_2019.dll", "x64/CUESDK.dll", "x64/CUESDK.x64_2019.dll"];

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
        lock (_lock)
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(CorsairDeviceProvider)}");
            _instance = this;
        }
    }

    #endregion

    #region Methods

    protected override void InitializeSDK()
    {
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
                Throw(new RGBDeviceException($"Failed to initialized Corsair-SDK. (Timeout - Current connection state: {_CUESDK.SesionState})"));

            _CUESDK.CorsairGetSessionDetails(out _CorsairSessionDetails? details);
            if (errorCode != CorsairError.Success)
                Throw(new RGBDeviceException($"Failed to get session details. (ErrorCode: {errorCode})"));

            SessionDetails = new CorsairSessionDetails(details!);
        }
        finally
        {
            _CUESDK.SessionStateChanged -= OnInitializeSessionStateChanged;
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

        foreach (_CorsairDeviceInfo device in devices)
        {
            if (string.IsNullOrWhiteSpace(device.id)) continue;

            error = _CUESDK.CorsairRequestControl(device.id, ExclusiveAccess ? CorsairAccessLevel.ExclusiveLightingControl : CorsairAccessLevel.Shared);
            if (error != CorsairError.Success)
                Throw(new RGBDeviceException($"Failed to take control of device '{device.id}'. (ErrorCode: {error})"));

            CorsairDeviceUpdateQueue updateQueue = new(GetUpdateTrigger(), device);

            int channelLedCount = 0;
            for (int i = 0; i < device.channelCount; i++)
            {
                Console.WriteLine($"Channel {i}/{device.channelCount}");
                channelLedCount += _CUESDK.ReadDevicePropertySimpleInt32(device.id!, CorsairDevicePropertyId.ChannelLedCount, (uint)i);
            }

            int deviceLedCount = device.ledCount - channelLedCount;
            if (deviceLedCount > 0)
                switch (device.type)
                {
                    case CorsairDeviceType.Keyboard:
                        yield return new CorsairKeyboardRGBDevice(new CorsairKeyboardRGBDeviceInfo(device, deviceLedCount, 0), updateQueue);
                        break;

                    case CorsairDeviceType.Mouse:
                        yield return new CorsairMouseRGBDevice(new CorsairMouseRGBDeviceInfo(device, deviceLedCount, 0), updateQueue);
                        break;

                    case CorsairDeviceType.Headset:
                        yield return new CorsairHeadsetRGBDevice(new CorsairHeadsetRGBDeviceInfo(device, deviceLedCount, 0), updateQueue);
                        break;

                    case CorsairDeviceType.Mousemat:
                        yield return new CorsairMousepadRGBDevice(new CorsairMousepadRGBDeviceInfo(device, deviceLedCount, 0), updateQueue);
                        break;

                    case CorsairDeviceType.HeadsetStand:
                        yield return new CorsairHeadsetStandRGBDevice(new CorsairHeadsetStandRGBDeviceInfo(device, deviceLedCount, 0), updateQueue);
                        break;

                    case CorsairDeviceType.MemoryModule:
                        yield return new CorsairMemoryRGBDevice(new CorsairMemoryRGBDeviceInfo(device, deviceLedCount, 0), updateQueue);
                        break;

                    case CorsairDeviceType.Motherboard:
                        yield return new CorsairMainboardRGBDevice(new CorsairMainboardRGBDeviceInfo(device, deviceLedCount, 0), updateQueue);
                        break;

                    case CorsairDeviceType.GraphicsCard:
                        yield return new CorsairGraphicsCardRGBDevice(new CorsairGraphicsCardRGBDeviceInfo(device, deviceLedCount, 0), updateQueue);
                        break;

                    case CorsairDeviceType.Touchbar:
                        yield return new CorsairTouchbarRGBDevice(new CorsairTouchbarRGBDeviceInfo(device, deviceLedCount, 0), updateQueue);
                        break;

                    case CorsairDeviceType.Cooler:
                        yield return new CorsairCoolerRGBDevice(new CorsairCoolerRGBDeviceInfo(device, deviceLedCount, 0), updateQueue);
                        break;

                    case CorsairDeviceType.GameController:
                        yield return new CorsairGameControllerRGBDevice(new CorsairGameControllerRGBDeviceInfo(device, deviceLedCount, 0), updateQueue);
                        break;

                    case CorsairDeviceType.FanLedController:
                    case CorsairDeviceType.LedController:
                    case CorsairDeviceType.Unknown:
                        yield return new CorsairUnknownRGBDevice(new CorsairUnknownRGBDeviceInfo(device, deviceLedCount, 0), updateQueue);
                        break;

                    default:
                        Throw(new RGBDeviceException("Unknown Device-Type"));
                        break;
                }

            int offset = deviceLedCount;
            for (int i = 0; i < device.channelCount; i++)
            {
                int deviceCount = _CUESDK.ReadDevicePropertySimpleInt32(device.id!, CorsairDevicePropertyId.ChannelDeviceCount, (uint)i);
                if (deviceCount <= 0) continue; // DarthAffe 10.02.2023: There seem to be an issue in the SDK where it reports empty channels and fails when getting ledCounts and device types from them

                int[] ledCounts = _CUESDK.ReadDevicePropertySimpleInt32Array(device.id!, CorsairDevicePropertyId.ChannelDeviceLedCountArray, (uint)i);
                int[] deviceTypes = _CUESDK.ReadDevicePropertySimpleInt32Array(device.id!, CorsairDevicePropertyId.ChannelDeviceTypeArray, (uint)i);

                for (int j = 0; j < deviceCount; j++)
                {
                    CorsairChannelDeviceType deviceType = (CorsairChannelDeviceType)deviceTypes[j];
                    int ledCount = ledCounts[j];

                    switch (deviceType)
                    {
                        case CorsairChannelDeviceType.FanHD:
                            yield return new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "HD Fan"), updateQueue);
                            break;

                        case CorsairChannelDeviceType.FanSP:
                            yield return new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "SP Fan"), updateQueue);
                            break;

                        case CorsairChannelDeviceType.FanLL:
                            yield return new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "LL Fan"), updateQueue);
                            break;

                        case CorsairChannelDeviceType.FanML:
                            yield return new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "ML Fan"), updateQueue);
                            break;

                        case CorsairChannelDeviceType.FanQL:
                            yield return new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "QL Fan"), updateQueue);
                            break;

                        case CorsairChannelDeviceType.FanQX:
                            yield return new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "QX Fan"), updateQueue);
                            break;

                        case CorsairChannelDeviceType.EightLedSeriesFan:
                            string fanModelName = "8-Led-Series Fan";
                            
                            if (device.model == "iCUE LINK System Hub")
                                fanModelName = "RX Fan";
                            
                            yield return new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, fanModelName), updateQueue);
                            break;

                        case CorsairChannelDeviceType.DAP:
                            yield return new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "DAP Fan"), updateQueue);
                            break;

                        case CorsairChannelDeviceType.Pump:
                            yield return new CorsairCoolerRGBDevice(new CorsairCoolerRGBDeviceInfo(device, ledCount, offset, "Pump"), updateQueue);
                            break;

                        case CorsairChannelDeviceType.WaterBlock:
                            yield return new CorsairCoolerRGBDevice(new CorsairCoolerRGBDeviceInfo(device, ledCount, offset, "Water Block"), updateQueue);
                            break;

                        case CorsairChannelDeviceType.Strip:
                            string stripModelName = "LED Strip";

                            // LS100 Led Strips are reported as one big strip if configured in monitor mode in iCUE, 138 LEDs for dual monitor, 84 for single
                            if ((device.model == "LS100 Starter Kit") && (ledCount == 138))
                                stripModelName = "LS100 LED Strip (dual monitor)";
                            else if ((device.model == "LS100 Starter Kit") && (ledCount == 84))
                                stripModelName = "LS100 LED Strip (single monitor)";
                            // Any other value means an "External LED Strip" in iCUE, these are reported per-strip, 15 for short strips, 27 for long
                            else if ((device.model == "LS100 Starter Kit") && (ledCount == 15))
                                stripModelName = "LS100 LED Strip (short)";
                            else if ((device.model == "LS100 Starter Kit") && (ledCount == 27))
                                stripModelName = "LS100 LED Strip (long)";

                            yield return new CorsairLedStripRGBDevice(new CorsairLedStripRGBDeviceInfo(device, ledCount, offset, stripModelName), updateQueue);
                            break;

                        case CorsairChannelDeviceType.DRAM:
                            yield return new CorsairMemoryRGBDevice(new CorsairMemoryRGBDeviceInfo(device, ledCount, offset, "DRAM"), updateQueue);
                            break;

                        default:
                            
                            //Workaround to support LX Fans because they have an invalid ChannelDeviceType
                            if ((device.model == "iCUE LINK System Hub") && (ledCount == 18))
                            {
                                yield return new CorsairFanRGBDevice(new CorsairFanRGBDeviceInfo(device, ledCount, offset, "LX Fan"), updateQueue);
                                break;
                            }

                            Throw(new RGBDeviceException("Unknown Device-Type"));
                            break;
                    }

                    offset += ledCount;
                }
            }
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        lock (_lock)
        {
            base.Dispose(disposing);

            try { _CUESDK.CorsairDisconnect(); }
            catch { /* at least we tried */ }

            try { _CUESDK.UnloadCUESDK(); }
            catch { /* at least we tried */ }

            _instance = null;
        }
    }

    #endregion
}