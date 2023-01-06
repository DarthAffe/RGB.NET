// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.Arduino;

// ReSharper disable once InconsistentNaming
/// <inheritdoc />
/// <summary>
/// Represents a definition of an arduino WS2812 devices.
/// </summary>
public class ArduinoWS281XDeviceDefinition : IWS281XDeviceDefinition
{
    #region Properties & Fields

    /// <summary>
    /// Gets the serial-connection used for the device.
    /// </summary>
    public ISerialConnection SerialConnection { get; }

    /// <summary>
    /// Gets the name of the serial-port to connect to.
    /// </summary>
    public string Port => SerialConnection.Port;

    /// <summary>
    /// Gets the baud-rate used by the serial-connection.
    /// </summary>
    public int BaudRate => SerialConnection.BaudRate;

    /// <summary>
    /// Gets or sets the name used by this device.
    /// This allows to use {0} as a placeholder for a incrementing number if multiple devices are created.
    /// </summary>
    public string? Name { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ArduinoWS281XDeviceDefinition"/> class.
    /// </summary>
    /// <param name="serialConnection">The serial connection used for the device.</param>
    public ArduinoWS281XDeviceDefinition(ISerialConnection serialConnection)
    {
        this.SerialConnection = serialConnection;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArduinoWS281XDeviceDefinition"/> class.
    /// </summary>
    /// <param name="port">The name of the serial-port to connect to.</param>
    /// <param name="baudRate">The baud-rate of the serial-connection.</param>
    public ArduinoWS281XDeviceDefinition(string port, int baudRate = 115200)
    {
        SerialConnection = new SerialPortConnection(port, baudRate);
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public IEnumerable<IRGBDevice> CreateDevices(IDeviceUpdateTrigger updateTrigger)
    {
        //TODO DarthAffe 04.03.2021: one queue per device
        ArduinoWS2812USBUpdateQueue queue = new(updateTrigger, SerialConnection);
        IEnumerable<(int channel, int ledCount)> channels = queue.GetChannels();
        int counter = 0;
        foreach ((int channel, int ledCount) in channels)
        {
            string name = string.Format(Name ?? $"Arduino WS2812 USB ({Port}) [{{0}}]", ++counter);
            yield return new ArduinoWS2812USBDevice(new ArduinoWS2812USBDeviceInfo(name), queue, channel, ledCount);
        }
    }

    #endregion
}