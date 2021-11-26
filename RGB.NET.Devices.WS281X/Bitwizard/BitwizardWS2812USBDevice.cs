// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.Bitwizard;

// ReSharper disable once InconsistentNaming
/// <inheritdoc cref="AbstractRGBDevice{T}" />
/// <summary>
/// Represents an bitwizard WS2812 USB device.
/// </summary>
public class BitwizardWS2812USBDevice : AbstractRGBDevice<BitwizardWS2812USBDeviceInfo>, ILedStripe
{
    #region Properties & Fields

    private readonly int _ledOffset;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BitwizardWS2812USBDevice"/> class.
    /// </summary>
    /// <param name="deviceInfo">The device info of this device.</param>
    /// <param name="updateQueue">The update queue performing updates for this device.</param>
    /// <param name="ledOffset">The offset used to access the leds on this device.</param>
    /// <param name="ledCount">The amount of leds on this device.</param>
    public BitwizardWS2812USBDevice(BitwizardWS2812USBDeviceInfo deviceInfo, BitwizardWS2812USBUpdateQueue updateQueue, int ledOffset, int ledCount)
        : base(deviceInfo, updateQueue)
    {
        this._ledOffset = ledOffset;
        InitializeLayout(ledCount);
    }

    #endregion

    #region Methods

    private void InitializeLayout(int ledCount)
    {
        for (int i = 0; i < ledCount; i++)
            AddLed(LedId.LedStripe1 + i, new Point(i * 10, 0), new Size(10, 10));
    }

    /// <inheritdoc />
    protected override object GetLedCustomData(LedId ledId) => _ledOffset + ((int)ledId - (int)LedId.LedStripe1);

    /// <inheritdoc />
    protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(GetUpdateData(ledsToUpdate));

    #endregion
}