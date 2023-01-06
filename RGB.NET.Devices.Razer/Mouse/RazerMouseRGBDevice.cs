// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer;

/// <inheritdoc cref="RazerRGBDevice" />
/// <summary>
/// Represents a razer mouse.
/// </summary>
public class RazerMouseRGBDevice : RazerRGBDevice, IMouse
{
    #region Properties & Fields

    private readonly LedMapping<int> _ledMapping;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerMouseRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the mouse.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    /// <param name="ledMapping">A mapping of leds this device is initialized with.</param>
    internal RazerMouseRGBDevice(RazerRGBDeviceInfo info, IDeviceUpdateTrigger updateTrigger, LedMapping<int> ledMapping)
        : base(info, new RazerMouseUpdateQueue(updateTrigger))
    {
        this._ledMapping = ledMapping;

        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        for (int row = 0; row < _Defines.MOUSE_MAX_ROW; row++)
            for (int column = 0; column < _Defines.MOUSE_MAX_COLUMN; column++)
                if (_ledMapping.TryGetValue((row * _Defines.MOUSE_MAX_COLUMN) + column, out LedId ledId))
                    AddLed(ledId, new Point(column * 10, row * 10), new Size(10, 10));
    }

    /// <inheritdoc />
    protected override object GetLedCustomData(LedId ledId) => _ledMapping[ledId];

    #endregion
}