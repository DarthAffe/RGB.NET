using RGB.NET.Core;

namespace RGB.NET.Devices.Asus;

/// <inheritdoc cref="AsusRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a Asus mainboard.
/// </summary>
public class AsusMainboardRGBDevice : AsusRGBDevice<AsusRGBDeviceInfo>, IMainboard
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusMainboardRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by Asus for the mainboard.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    internal AsusMainboardRGBDevice(AsusRGBDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
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
            AddLed(LedId.Mainboard1 + i, new Point(i * 40, 0), new Size(40, 8));
    }

    /// <inheritdoc />
    protected override object? GetLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Mainboard1;

    #endregion
}