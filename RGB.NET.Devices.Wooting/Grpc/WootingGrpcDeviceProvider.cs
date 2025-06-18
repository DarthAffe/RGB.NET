using System;
using System.Collections.Generic;
using System.Threading;
using Grpc.Net.Client;
using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Enum;
using RGB.NET.Devices.Wooting.Keyboard;
using RGB.NET.Devices.Wooting.Keypad;
using WootingRgbSdk;

namespace RGB.NET.Devices.Wooting.Grpc;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for Wooting devices.
/// </summary>
public sealed class WootingGrpcDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    // ReSharper disable once InconsistentNaming
    private static readonly Lock _lock = new();

    private GrpcChannel? _channel;
    private RgbSdkService.RgbSdkServiceClient? _client;

    private static WootingGrpcDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="WootingGrpcDeviceProvider"/> instance.
    /// </summary>
    public static WootingGrpcDeviceProvider Instance
    {
        get
        {
            lock (_lock)
                return _instance ?? new WootingGrpcDeviceProvider();
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WootingGrpcDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public WootingGrpcDeviceProvider()
    {
        lock (_lock)
        {
            if (_instance != null)
                throw new InvalidOperationException($"There can be only one instance of type {nameof(WootingGrpcDeviceProvider)}");
            _instance = this;
        }
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void InitializeSDK()
    {
        _channel = GrpcChannel.ForAddress("http://localhost:50051");
        _client = new RgbSdkService.RgbSdkServiceClient(_channel);
    }

    /// <inheritdoc /> 
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        ArgumentNullException.ThrowIfNull(_client, nameof(_client));
        
        int i = 0;
        foreach (RgbGetConnectedDevicesResponse.Types.RgbDevice? device in _client.GetConnectedDevices(new()).Devices)
        {
            if (device.DeviceType == RgbDeviceType.None)
                continue; // Skip devices that are not supported

            _client.Initialize(new RgbInitializeRequest { Id = device.Id });

            WootingGrpcUpdateQueue updateQueue = new(GetUpdateTrigger(i++), device, _client);

            WootingDeviceType deviceType = device.DeviceType switch
            {
                RgbDeviceType.Tkl => WootingDeviceType.KeyboardTKL,
                RgbDeviceType.FullSize => WootingDeviceType.Keyboard,
                RgbDeviceType.SixtyPercent => WootingDeviceType.KeyboardSixtyPercent,
                RgbDeviceType.ThreeKey => WootingDeviceType.Keypad3Keys,
                RgbDeviceType.EighyPercent => WootingDeviceType.KeyboardEightyPercent,
                _ or RgbDeviceType.None => throw new ArgumentOutOfRangeException()
            };
            KeyboardLayoutType layoutType = device.LayoutType switch
            {
                RgbDeviceLayout.Ansi => KeyboardLayoutType.ANSI,
                RgbDeviceLayout.Iso => KeyboardLayoutType.ISO,
                RgbDeviceLayout.Jis => KeyboardLayoutType.JIS,
                RgbDeviceLayout.AnsiSplitSpacebar => KeyboardLayoutType.ANSI,
                RgbDeviceLayout.IsoSplitSpacebar => KeyboardLayoutType.ISO,
                RgbDeviceLayout.Unknown => KeyboardLayoutType.Unknown,
                _ => throw new ArgumentOutOfRangeException()
            };

            //NOTE: this model name ends up kind of ugly, since `ModelName` is like `Wooting 60HE`. We cannot remove the `Wooting` prefix,
            //since that makes loadouts fail to load. The deviceName part, however, is fine to strip.
            string model = device.ModelName;
            string name =
                DeviceHelper.CreateDeviceName("Wooting", $"{device.ModelName.Replace("Wooting", "").Trim()} ({device.SerialNumber})");

            yield return deviceType switch
            {
                WootingDeviceType.Keypad3Keys => new WootingKeypadRGBDevice(deviceType, new(model, name), updateQueue),
                _ => new WootingKeyboardRGBDevice(deviceType, new(layoutType, model, name), updateQueue)
            };
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        lock (_lock)
        {
            base.Dispose(disposing);

            try
            {
                _client = null;
                _channel?.Dispose();
            }
            catch
            { /* at least we tried */
            }

            _instance = null;
        }
    }

    #endregion
}
