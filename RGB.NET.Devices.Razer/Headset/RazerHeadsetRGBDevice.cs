// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer;

/// <inheritdoc cref="RazerRGBDevice" />
/// <summary>
/// Represents a razer headset.
/// </summary>
public class RazerHeadsetRGBDevice : RazerRGBDevice, IHeadset
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerHeadsetRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the headset.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    internal RazerHeadsetRGBDevice(RazerRGBDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
        : base(info, new RazerHeadsetUpdateQueue(updateTrigger))
    {
        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        for (int i = 0; i < _Defines.HEADSET_MAX_LEDS; i++)
            AddLed(LedId.Headset1 + i, new Point(i * 10, 0), new Size(10, 10));
    }

    /// <inheritdoc />
    protected override object? GetLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Headset1;

    #endregion
}