// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.Linq;
using HidSharp;
using RGB.NET.Core;
using RGB.NET.Devices.PicoPi.Enum;
using RGB.NET.HID;

namespace RGB.NET.Devices.PicoPi;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for PicoPi-devices.
/// </summary>
// ReSharper disable once InconsistentNaming
public class PicoPiDeviceProvider : AbstractRGBDeviceProvider
{
    #region Constants

    private const int AUTO_UPDATE_MODE_CHUNK_THRESHOLD = 2;

    #endregion

    #region Properties & Fields

    private static PicoPiDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="PicoPiDeviceProvider"/> instance.
    /// </summary>
    public static PicoPiDeviceProvider Instance => _instance ?? new PicoPiDeviceProvider();

    /// <summary>
    /// Gets the HID-definitions for PicoPi-devices.
    /// </summary>
    public static HIDLoader<int, int> DeviceDefinitions { get; } = new(PicoPiSDK.VENDOR_ID)
    {
        { PicoPiSDK.HID_BULK_CONTROLLER_PID, RGBDeviceType.LedStripe, "WS2812B-Controller", LedMappings.StripeMapping, 0 },
    };

    private readonly List<PicoPiSDK> _sdks = new();

    /// <summary>
    /// Gets or sets the endpoint used to update devices. (default <see cref="PicoPi.Enum.UpdateMode.Auto"/>).
    /// If auto is set it automatically is using bulk-updates for devies with more than 40 LEDs if supported. Else HID is used.
    /// </summary>
    public UpdateMode UpdateMode { get; set; } = UpdateMode.Auto;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PicoPiDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public PicoPiDeviceProvider()
    {
        if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(PicoPiDeviceProvider)}");
        _instance = this;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void InitializeSDK() { }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> GetLoadedDevices(RGBDeviceType loadFilter)
    {
        DeviceDefinitions.LoadFilter = loadFilter;

        return base.GetLoadedDevices(loadFilter);
    }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        IEnumerable<(HIDDeviceDefinition<int, int> definition, HidDevice device)> devices = DeviceDefinitions.GetConnectedDevices();
        foreach ((HIDDeviceDefinition<int, int> definition, HidDevice device) in devices)
        {
            PicoPiSDK sdk = new(device);
            _sdks.Add(sdk);
            IDeviceUpdateTrigger updateTrigger = GetUpdateTrigger(sdk.Id.GetHashCode());
            foreach ((int channel, int ledCount, _) in sdk.Channels.Where(c => c.ledCount > 0))
            {
                PicoPiRGBDevice picoPiDevice = new(new PicoPiRGBDeviceInfo(definition.DeviceType, definition.Name, sdk.Id, sdk.Version, channel, ledCount), GetUpdateQueue(updateTrigger, sdk, channel, ledCount), definition.LedMapping);
                picoPiDevice.Initialize();
                yield return picoPiDevice;
            }

            if (sdk.IsBulkSupported)
                updateTrigger.Update += (_, _) => sdk.FlushBulk();
        }
    }

    private IUpdateQueue GetUpdateQueue(IDeviceUpdateTrigger updateTrigger, PicoPiSDK sdk, int channel, int ledCount)
    {
        switch (UpdateMode)
        {
            case UpdateMode.HID:
                return new PicoPiHIDUpdateQueue(updateTrigger, sdk, channel, ledCount);

            case UpdateMode.BULK:
                if (!sdk.IsBulkSupported) throw new NotSupportedException("Bulk updates aren't supported for this device. Make sure the firmware is built with bulk support and the libusb driver is installed.");
                return new PicoPiBulkUpdateQueue(updateTrigger, sdk, channel, ledCount);

            case UpdateMode.Auto:
                if (!sdk.IsBulkSupported || (sdk.Channels.Sum(c => (int)Math.Ceiling(c.ledCount / 20.0)) <= AUTO_UPDATE_MODE_CHUNK_THRESHOLD)) return new PicoPiHIDUpdateQueue(updateTrigger, sdk, channel, ledCount);
                return new PicoPiBulkUpdateQueue(updateTrigger, sdk, channel, ledCount);

            default: throw new IndexOutOfRangeException($"Update mode {UpdateMode} is not supported.");
        }
    }

    /// <inheritdoc />
    protected override void Reset()
    {
        base.Reset();

        foreach (PicoPiSDK sdk in _sdks)
            sdk.Dispose();
        _sdks.Clear();
    }

    #endregion
}