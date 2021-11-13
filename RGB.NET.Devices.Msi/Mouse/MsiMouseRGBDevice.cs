using RGB.NET.Core;
using RGB.NET.Devices.Msi.Native;

namespace RGB.NET.Devices.Msi;

/// <inheritdoc />
/// <summary>
/// Represents a MSI mouse.
/// </summary>
public class MsiMouseRGBDevice : MsiRGBDevice<MsiRGBDeviceInfo>
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Msi.MsiMouseRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by MSI for the mouse.</param>
    /// <param name="ledCount">The amount of leds on this device.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    internal MsiMouseRGBDevice(MsiRGBDeviceInfo info, int ledCount, IDeviceUpdateTrigger updateTrigger)
        : base(info, updateTrigger)
    {
        InitializeLayout(ledCount);
    }

    #endregion

    #region Methods

    private void InitializeLayout(int ledCount)
    {
        for (int i = 0; i < ledCount; i++)
        {
            //Hex3l: Should it be configurable in order to provide style access?
            //Hex3l: Sets led style to "Steady" in order to have a solid color output therefore a controllable led color
            //Hex3l: This is a string defined by the output of _MsiSDK.GetLedStyle, "Steady" should be always present
            const string LED_STYLE = "Steady";

            _MsiSDK.SetLedStyle(DeviceInfo.MsiDeviceType, i, LED_STYLE);
            AddLed(LedId.Mouse1 + i, new Point(i * 10, 0), new Size(10, 10));
        }
    }

    /// <inheritdoc />
    protected override object? GetLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Mouse1;

    #endregion
}