// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer;

/// <inheritdoc cref="RazerRGBDevice" />
/// <summary>
/// Represents a razer mousepad.
/// </summary>
public class RazerMousepadRGBDevice : RazerRGBDevice, IMousepad
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerMousepadRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the mousepad.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    internal RazerMousepadRGBDevice(RazerRGBDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
        : base(info, new RazerMousepadUpdateQueue(updateTrigger))
    {
        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        for (int i = 0; i < _Defines.MOUSEPAD_MAX_LEDS; i++)
            AddLed(LedId.Mousepad1 + i, new Point(i * 10, 0), new Size(10, 10));
    }

    /// <inheritdoc />
    protected override object? GetLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Mousepad1;
    #endregion
}