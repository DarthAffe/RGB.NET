using System;
using RGB.NET.Core;
using Sanford.Multimedia.Midi;

namespace RGB.NET.Devices.Novation;

/// <inheritdoc cref="UpdateQueue" />
/// <summary>
/// Represents the update-queue performing updates for midi devices.
/// </summary>
public abstract class MidiUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private readonly OutputDevice _outputDevice;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Novation.MidiUpdateQueue" /> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="deviceId">The id of the device this queue is performing updates for.</param>
    protected MidiUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceId)
        : base(updateTrigger)
    {
        _outputDevice = new OutputDevice(deviceId);
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        foreach ((object key, Color color) in dataSet)
            SendMessage(CreateMessage(key, color));
    }

    /// <summary>
    /// Sends the specified message to the device this queue is performing updates for.
    /// </summary>
    /// <param name="message">The message to send.</param>
    protected virtual void SendMessage(ShortMessage? message)
    {
        if (message != null)
            _outputDevice.SendShort(message.Message);
    }

    /// <summary>
    /// Creates a update-message out of a specified data set.
    /// </summary>
    /// <param name="key">The key used to identify the LED to update.</param>
    /// <param name="color">The color to send.</param>
    /// <returns>The message created out of the data set.</returns>
    protected abstract ShortMessage? CreateMessage(object key, in Color color);

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        _outputDevice.Dispose();

        GC.SuppressFinalize(this);
    }

    #endregion
}