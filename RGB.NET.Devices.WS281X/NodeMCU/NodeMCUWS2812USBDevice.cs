// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.NodeMCU;

// ReSharper disable once InconsistentNaming
/// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents an NodeMCU WS2812 device.
/// </summary>
public class NodeMCUWS2812USBDevice : AbstractRGBDevice<NodeMCUWS2812USBDeviceInfo>, ILedStripe
{
    #region Properties & Fields

    /// <summary>
    /// Gets the channel (as defined in the NodeMCU-sketch) this device is attached to.
    /// </summary>
    public int Channel { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="NodeMCUWS2812USBDevice"/> class.
    /// </summary>
    /// <param name="deviceInfo">The device info of this device.</param>
    /// <param name="updateQueue">The update queue performing updates for this device.</param>
    /// <param name="channel">The channel (as defined in the NodeMCU-sketch) this device is attached to.</param>
    /// <param name="ledCount">The amount of leds on this device.</param>
    public NodeMCUWS2812USBDevice(NodeMCUWS2812USBDeviceInfo deviceInfo, NodeMCUWS2812USBUpdateQueue updateQueue, int channel, int ledCount)
        : base(deviceInfo, updateQueue)
    {
        this.Channel = channel;

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
    protected override object GetLedCustomData(LedId ledId) => (Channel, (int)ledId - (int)LedId.LedStripe1);

    /// <inheritdoc />
    protected override IEnumerable<Led> GetLedsToUpdate(bool flushLeds) => (flushLeds || LedMapping.Values.Any(x => x.IsDirty)) ? LedMapping.Values : Enumerable.Empty<Led>();

    /// <inheritdoc />
    protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(GetUpdateData(ledsToUpdate));

    #endregion
}