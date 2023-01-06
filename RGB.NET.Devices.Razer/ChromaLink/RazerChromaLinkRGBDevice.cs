// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer;

/// <inheritdoc cref="RazerRGBDevice" />
/// <summary>
/// Represents a razer chroma link.
/// </summary>
public class RazerChromaLinkRGBDevice : RazerRGBDevice, IUnknownDevice
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerChromaLinkRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by CUE for the chroma link.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    internal RazerChromaLinkRGBDevice(RazerRGBDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
        : base(info, new RazerChromaLinkUpdateQueue(updateTrigger))
    {
        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        for (int i = 0; i < _Defines.CHROMALINK_MAX_LEDS; i++)
            AddLed(LedId.Custom1 + i, new Point(i * 10, 0), new Size(10, 10));
    }

    /// <inheritdoc />
    protected override object? GetLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Custom1;

    #endregion
}