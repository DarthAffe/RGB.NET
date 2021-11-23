// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer;

/// <inheritdoc cref="RazerRGBDevice" />
/// <summary>
/// Represents a razer keyboard.
/// </summary>
public class RazerKeyboardRGBDevice : RazerRGBDevice, IKeyboard
{
    #region Properties & Fields

    IKeyboardDeviceInfo IKeyboard.DeviceInfo => (IKeyboardDeviceInfo)DeviceInfo;

    private readonly LedMapping<int> _ledMapping;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerKeyboardRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the keyboard.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    /// <param name="ledMapping">A mapping of leds this device is initialized with.</param>
    internal RazerKeyboardRGBDevice(RazerKeyboardRGBDeviceInfo info, IDeviceUpdateTrigger updateTrigger, LedMapping<int> ledMapping)
        : base(info, new RazerKeyboardUpdateQueue(updateTrigger))
    {
        this._ledMapping = ledMapping;

        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        for (int row = 0; row < _Defines.KEYBOARD_MAX_ROW; row++)
            for (int column = 0; column < _Defines.KEYBOARD_MAX_COLUMN; column++)
                if (_ledMapping.TryGetValue((row * _Defines.KEYBOARD_MAX_COLUMN) + column, out LedId id))
                    AddLed(id, new Point(column * 19, row * 19), new Size(19, 19));
    }

    /// <inheritdoc />
    protected override object GetLedCustomData(LedId ledId) => _ledMapping[ledId];

    #endregion
}