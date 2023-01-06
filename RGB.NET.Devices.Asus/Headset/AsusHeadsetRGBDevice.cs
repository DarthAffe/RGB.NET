using RGB.NET.Core;

namespace RGB.NET.Devices.Asus;

/// <inheritdoc cref="AsusRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a Asus headset.
/// </summary>
public class AsusHeadsetRGBDevice : AsusRGBDevice<AsusRGBDeviceInfo>, IHeadset
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusHeadsetRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by Asus for the headset.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    internal AsusHeadsetRGBDevice(AsusRGBDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
        : base(info, updateTrigger)
    {
        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        int ledCount = DeviceInfo.Device.Lights.Count;
        for (int i = 0; i < ledCount; i++)
            AddLed(LedId.Headset1 + i, new Point(i * 40, 0), new Size(40, 8));
    }

    /// <inheritdoc />
    protected override object? GetLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Headset1;

    #endregion
}