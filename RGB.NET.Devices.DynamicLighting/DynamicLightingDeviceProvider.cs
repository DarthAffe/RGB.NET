// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Threading;
using RGB.NET.Core;
using Windows.Devices.Enumeration;
using Windows.Devices.Lights;

namespace RGB.NET.Devices.DynamicLighting;

internal record LampArrayInfo(string Id, string DisplayName, LampArray LampArray);

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for Microsoft Dynamic Lighting devices.
/// </summary>
public sealed class DynamicLightingDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    // ReSharper disable once InconsistentNaming
    private static readonly object _lock = new();

    private static DynamicLightingDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="DynamicLightingDeviceProvider"/> instance.
    /// </summary>
    public static DynamicLightingDeviceProvider Instance
    {
        get
        {
            lock (_lock)
                return _instance ?? new DynamicLightingDeviceProvider();
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicLightingDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public DynamicLightingDeviceProvider()
    {
        lock (_lock)
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(DynamicLightingDeviceProvider)}");
            _instance = this;
        }
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void InitializeSDK() { }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        ManualResetEventSlim waitEvent = new(false);
        List<LampArrayInfo> lampArrays = [];

        DeviceWatcher? watcher = DeviceInformation.CreateWatcher(LampArray.GetDeviceSelector());
        watcher.EnumerationCompleted += OnEnumerationCompleted;
        watcher.Added += OnDeviceAdded;
        watcher.Start();

        waitEvent.Wait();

        watcher.Stop();
        watcher.EnumerationCompleted -= OnEnumerationCompleted;
        watcher.Added -= OnDeviceAdded;

        int updateTriggerId = 0;

        foreach (LampArrayInfo lampArrayInfo in lampArrays)
        {
            IDynamicLightingRGBDevice? device = null;
            try
            {
                device = CreateDevice(lampArrayInfo);
                device.Initialize();
            }
            catch (Exception ex) { Throw(ex); }

            if (device != null)
                yield return device;
        }

        yield break;

        IDynamicLightingRGBDevice CreateDevice(LampArrayInfo lampArrayInfo)
            => lampArrayInfo.LampArray.LampArrayKind switch
            {
                LampArrayKind.Undefined => new DynamicLightingUndefinedRGBDevice(new DynamicLightingUndefinedRGBDeviceInfo(lampArrayInfo), new DynamicLightingDeviceUpdateQueue(GetUpdateTrigger(updateTriggerId++, lampArrayInfo.LampArray.MinUpdateInterval.TotalSeconds), lampArrayInfo.LampArray)),
                LampArrayKind.Keyboard => new DynamicLightingKeyboardRGBDevice(new DynamicLightingKeyboardRGBDeviceInfo(lampArrayInfo), new DynamicLightingDeviceUpdateQueue(GetUpdateTrigger(updateTriggerId++, lampArrayInfo.LampArray.MinUpdateInterval.TotalSeconds), lampArrayInfo.LampArray)),
                LampArrayKind.Mouse => new DynamicLightingMouseRGBDevice(new DynamicLightingMouseRGBDeviceInfo(lampArrayInfo), new DynamicLightingDeviceUpdateQueue(GetUpdateTrigger(updateTriggerId++, lampArrayInfo.LampArray.MinUpdateInterval.TotalSeconds), lampArrayInfo.LampArray)),
                LampArrayKind.GameController => new DynamicLightingGameControllerRGBDevice(new DynamicLightingGameControllerRGBDeviceInfo(lampArrayInfo), new DynamicLightingDeviceUpdateQueue(GetUpdateTrigger(updateTriggerId++, lampArrayInfo.LampArray.MinUpdateInterval.TotalSeconds), lampArrayInfo.LampArray)),
                LampArrayKind.Peripheral => new DynamicLightingPeripheralRGBDevice(new DynamicLightingPeripheralRGBDeviceInfo(lampArrayInfo), new DynamicLightingDeviceUpdateQueue(GetUpdateTrigger(updateTriggerId++, lampArrayInfo.LampArray.MinUpdateInterval.TotalSeconds), lampArrayInfo.LampArray)),
                LampArrayKind.Scene => new DynamicLightingSceneRGBDevice(new DynamicLightingSceneRGBDeviceInfo(lampArrayInfo), new DynamicLightingDeviceUpdateQueue(GetUpdateTrigger(updateTriggerId++, lampArrayInfo.LampArray.MinUpdateInterval.TotalSeconds), lampArrayInfo.LampArray)),
                LampArrayKind.Notification => new DynamicLightingNotificationRGBDevice(new DynamicLightingNotificationRGBDeviceInfo(lampArrayInfo), new DynamicLightingDeviceUpdateQueue(GetUpdateTrigger(updateTriggerId++, lampArrayInfo.LampArray.MinUpdateInterval.TotalSeconds), lampArrayInfo.LampArray)),
                LampArrayKind.Chassis => new DynamicLightingChassisRGBDevice(new DynamicLightingChassisRGBDeviceInfo(lampArrayInfo), new DynamicLightingDeviceUpdateQueue(GetUpdateTrigger(updateTriggerId++, lampArrayInfo.LampArray.MinUpdateInterval.TotalSeconds), lampArrayInfo.LampArray)),
                LampArrayKind.Wearable => new DynamicLightingWearableRGBDevice(new DynamicLightingWearableRGBDeviceInfo(lampArrayInfo), new DynamicLightingDeviceUpdateQueue(GetUpdateTrigger(updateTriggerId++, lampArrayInfo.LampArray.MinUpdateInterval.TotalSeconds), lampArrayInfo.LampArray)),
                LampArrayKind.Furniture => new DynamicLightingFurnitureRGBDevice(new DynamicLightingFurnitureRGBDeviceInfo(lampArrayInfo), new DynamicLightingDeviceUpdateQueue(GetUpdateTrigger(updateTriggerId++, lampArrayInfo.LampArray.MinUpdateInterval.TotalSeconds), lampArrayInfo.LampArray)),
                LampArrayKind.Art => new DynamicLightingArtRGBDevice(new DynamicLightingArtRGBDeviceInfo(lampArrayInfo), new DynamicLightingDeviceUpdateQueue(GetUpdateTrigger(updateTriggerId++, lampArrayInfo.LampArray.MinUpdateInterval.TotalSeconds), lampArrayInfo.LampArray)),
                _ => throw new ArgumentOutOfRangeException()
            };

        void OnEnumerationCompleted(DeviceWatcher sender, object o) => waitEvent.Set();

        void OnDeviceAdded(DeviceWatcher sender, DeviceInformation args)
        {
            try { lampArrays.Add(new LampArrayInfo(args.Id, args.Name, LampArray.FromIdAsync(args.Id).GetAwaiter().GetResult())); }
            catch (Exception ex) { Throw(ex); }
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        lock (_lock)
        {
            base.Dispose(disposing);

            _instance = null;
        }
    }

    #endregion
}