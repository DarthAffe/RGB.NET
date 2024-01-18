using RGB.NET.Core;

namespace RGB.NET.Devices.WLED;

/// <inheritdoc cref="AbstractRGBDevice{WledRGBDeviceInfo}" />
/// <inheritdoc cref="IWledRGBDevice" />
/// <summary>
/// Represents a WLED-device.
/// </summary>
public sealed class WledRGBDevice : AbstractRGBDevice<WledRGBDeviceInfo>, IWledRGBDevice, ILedStripe
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WledRGBDevice"/> class.
    /// </summary>
    internal WledRGBDevice(WledRGBDeviceInfo info, string address, IDeviceUpdateTrigger updateTrigger)
        : base(info, new WledDeviceUpdateQueue(updateTrigger, address, info.Info.UDPPort, info.Info.Leds.Count))
    {
        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        for (int i = 0; i < DeviceInfo.Info.Leds.Count; i++)
            AddLed(LedId.LedStripe1 + i, new Point(i * 10, 0), new Size(10, 10));
    }

    /// <inheritdoc />
    protected override object GetLedCustomData(LedId ledId) => ledId - LedId.LedStripe1;

    #endregion
}