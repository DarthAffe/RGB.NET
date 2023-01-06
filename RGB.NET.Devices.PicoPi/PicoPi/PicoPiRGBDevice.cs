using RGB.NET.Core;

namespace RGB.NET.Devices.PicoPi;

/// <inheritdoc cref="AbstractRGBDevice{PicoPiRGBDeviceInfo}" />
/// <summary>
/// Represents a device based on an Raspberry Pi Pico.
/// </summary>
public class PicoPiRGBDevice : AbstractRGBDevice<PicoPiRGBDeviceInfo>
{
    #region Properties & Fields

    private readonly LedMapping<int> _ledMapping;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="PicoPiRGBDevice" /> class.
    /// </summary>
    /// <param name="deviceInfo">The device info of this device.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    /// <param name="ledMapping">A mapping of leds this device is initialized with.</param>
    public PicoPiRGBDevice(PicoPiRGBDeviceInfo deviceInfo, IUpdateQueue updateQueue, LedMapping<int> ledMapping)
        : base(deviceInfo, updateQueue)
    {
        this._ledMapping = ledMapping;
    }

    #endregion

    #region Methods

    internal void Initialize()
    {
        for (int i = 0; i < DeviceInfo.LedCount; i++)
            AddLed(_ledMapping[i], new Point(i * 10, 0), new Size(10, 10), i);
    }

    /// <inheritdoc />
    protected override object GetLedCustomData(LedId ledId) => _ledMapping.TryGetValue(ledId, out int index) ? index : -1;

    #endregion
}