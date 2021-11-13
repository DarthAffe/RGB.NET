using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X;

/// <inheritdoc />
/// <summary>
/// Represents a update queue for serial devices.
/// </summary>
/// <typeparam name="TData">The type of data sent through the serial connection.</typeparam>
public abstract class SerialConnectionUpdateQueue<TData> : UpdateQueue
{
    #region Properties & Fields

    /// <summary>
    /// Gets or sets the prompt to wait for between sending commands.
    /// </summary>
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    protected char Prompt { get; set; } = '>';

    /// <summary>
    /// Gets the serial port used by this queue.
    /// </summary>
    protected ISerialConnection SerialConnection { get; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="SerialConnectionUpdateQueue{TData}"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="serialConnection">The serial connection used to access the device.</param>
    internal SerialConnectionUpdateQueue(IDeviceUpdateTrigger updateTrigger, ISerialConnection serialConnection)
        : base(updateTrigger)
    {
        SerialConnection = serialConnection;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void OnStartup(object? sender, CustomUpdateData customData)
    {
        base.OnStartup(sender, customData);

        if (!SerialConnection.IsOpen)
            SerialConnection.Open();

        SerialConnection.DiscardInBuffer();
    }

    /// <inheritdoc />
    protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        foreach (TData command in GetCommands(dataSet.ToArray()))
        {
            SerialConnection.ReadTo(Prompt);
            SendCommand(command);
        }
    }

    /// <summary>
    /// Gets the commands that need to be sent to the device to update the requested colors.
    /// </summary>
    /// <param name="dataSet">The set of data that needs to be updated.</param>
    /// <returns>The commands to be sent.</returns>
    protected abstract IEnumerable<TData> GetCommands(IList<(object key, Color color)> dataSet);

    /// <summary>
    /// Sends a command as a string followed by a line-break to the device.
    /// This most likely needs to be overwritten if the data-type isn't string.  
    /// </summary>
    /// <param name="command">The command to be sent.</param>
    protected virtual void SendCommand(TData command) => SerialConnection.WriteLine((command as string) ?? string.Empty);

    #endregion
}