using Windows.Devices.Lights;
using RGB.NET.Core;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a generic Dynamic Lighting-device.
/// </summary>
public abstract class DynamicLightingRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IDynamicLightingRGBDevice
    where TDeviceInfo : DynamicLightingRGBDeviceInfo
{
    #region Properties & Fields

    /// <summary>
    /// Gets the mapping of <see cref="LedId"/> to the index of the led used to update the LEDs of this device.
    /// </summary>
    protected LedMapping<int> Mapping { get; private set; } = [];

    /// <summary>
    /// Gets the reference led id.
    /// </summary>
    protected abstract LedId ReferenceLedId { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicLightingRGBDevice{TDeviceInfo}"/> class.
    /// </summary>
    /// <param name="info">The meta data for this device.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    protected DynamicLightingRGBDevice(TDeviceInfo info, DynamicLightingDeviceUpdateQueue updateQueue)
        : base(info, updateQueue) { }

    #endregion

    #region Methods

    void IDynamicLightingRGBDevice.Initialize()
    {
        Mapping = CreateMapping();
        InitializeLayout();
    }

    /// <summary>
    /// Initializes the LEDs of the device based on the data provided by the SDK.
    /// </summary>
    protected virtual void InitializeLayout()
    {
        for (int i = 0; i < DeviceInfo.LedCount; i++)
        {
            LampInfo lampInfo = DeviceInfo.LampArray.GetLampInfo(i);

            LedId ledId = Mapping.TryGetValue(i, out LedId id) ? id : LedId.Invalid;
            Rectangle rectangle = new(new Point(lampInfo.Position.X, lampInfo.Position.Y), new Size(10, 10));
            AddLed(ledId, rectangle.Location, rectangle.Size);
        }
    }

    /// <summary>
    /// Creates a mapping for this device.
    /// </summary>
    /// <returns>The mapping.</returns>
    protected virtual LedMapping<int> CreateMapping()
    {
        LedMapping<int> mapping = [];
        for (int i = 0; i < DeviceInfo.LedCount; i++)
            mapping.Add(ReferenceLedId + i, i);

        return mapping;
    }

    /// <inheritdoc />
    protected override object GetLedCustomData(LedId ledId) => Mapping.TryGetValue(ledId, out int index) ? index : 0;

    #endregion
}