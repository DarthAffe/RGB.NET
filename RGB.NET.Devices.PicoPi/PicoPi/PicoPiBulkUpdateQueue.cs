using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.PicoPi;

/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for Pico-Pi bulk-devices.
///  </summary>
/// <remarks>
/// Using this requires the libusb driver to be installed!
/// </remarks>
public class PicoPiBulkUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private readonly PicoPiSDK _sdk;
    private readonly int _channel;

    private readonly byte[] _dataBuffer;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PicoPiBulkUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="sdk">The sdk used to access the device.</param>
    /// <param name="channel">The channel to update.</param>
    /// <param name="ledCount">The maximum amount of leds to update.</param>
    public PicoPiBulkUpdateQueue(IDeviceUpdateTrigger updateTrigger, PicoPiSDK sdk, int channel, int ledCount)
        : base(updateTrigger)
    {
        this._sdk = sdk;
        this._channel = channel;

        _dataBuffer = new byte[ledCount * 3];
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        Span<byte> buffer = _dataBuffer;
        foreach ((object key, Color color) in dataSet)
        {
            int index = key as int? ?? -1;
            if (index < 0) continue;

            (byte _, byte r, byte g, byte b) = color.GetRGBBytes();
            int offset = index * 3;
            buffer[offset] = r;
            buffer[offset + 1] = g;
            buffer[offset + 2] = b;
        }

        _sdk.SendBulkUpdate(buffer, _channel);
    }

    #endregion
}