using RGB.NET.Core;

namespace RGB.NET.Devices.Asus;

/// <inheritdoc cref="AsusRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a Asus headset.
/// </summary>
public class AsusUnspecifiedRGBDevice : AsusRGBDevice<AsusRGBDeviceInfo>, IUnknownDevice
{
    #region Properties & Fields

    private LedId _baseLedId;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusHeadsetRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by Asus for the headset.</param>
    /// <param name="baseLedId">The ledId of the first led of this device. All other leds are created by incrementing this base-id by 1.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    internal AsusUnspecifiedRGBDevice(AsusRGBDeviceInfo info, LedId baseLedId, IDeviceUpdateTrigger updateTrigger)
        : base(info, updateTrigger)
    {
        this._baseLedId = baseLedId;

        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        int ledCount = DeviceInfo.Device.Lights.Count;
        for (int i = 0; i < ledCount; i++)
            AddLed(_baseLedId + i, new Point(i * 10, 0), new Size(10, 10));
    }

    /// <inheritdoc />
    protected override object? GetLedCustomData(LedId ledId) => (int)ledId - (int)_baseLedId;

    #endregion
}