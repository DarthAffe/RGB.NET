// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer;

/// <inheritdoc cref="RazerRGBDevice" />
/// <summary>
/// Represents a razer keypad.
/// </summary>
public class RazerKeypadRGBDevice : RazerRGBDevice, IKeypad
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerKeypadRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the keypad.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    internal RazerKeypadRGBDevice(RazerRGBDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
        : base(info, new RazerKeypadUpdateQueue(updateTrigger))
    {
        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        for (int row = 0; row < _Defines.KEYPAD_MAX_ROW; row++)
            for (int column = 0; column < _Defines.KEYPAD_MAX_COLUMN; column++)
                AddLed(LedId.Keypad1 + ((row * _Defines.KEYPAD_MAX_COLUMN) + column), new Point(column * 19, row * 19), new Size(19, 19));
    }

    /// <inheritdoc />
    protected override object? GetLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Keypad1;

    #endregion
}