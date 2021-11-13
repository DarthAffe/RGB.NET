using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.Bitwizard;

// ReSharper disable once InconsistentNaming
/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for a bitwizard WS2812 device.
/// </summary>
public class BitwizardWS2812USBUpdateQueue : SerialConnectionUpdateQueue<string>
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.WS281X.Bitwizard.BitwizardWS2812USBUpdateQueue" /> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="serialConnection">The serial connection used to access the device.</param>
    public BitwizardWS2812USBUpdateQueue(IDeviceUpdateTrigger updateTrigger, ISerialConnection serialConnection)
        : base(updateTrigger, serialConnection)
    { }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void OnStartup(object? sender, CustomUpdateData customData)
    {
        base.OnStartup(sender, customData);

        SendCommand(string.Empty); // Get initial prompt
    }

    /// <inheritdoc />
    protected override IEnumerable<string> GetCommands(IList<(object key, Color color)> dataSet)
    {
        foreach ((object key, Color value) in dataSet)
            yield return $"pix {(int)key} {value.AsRGBHexString(false)}";
    }

    #endregion
}