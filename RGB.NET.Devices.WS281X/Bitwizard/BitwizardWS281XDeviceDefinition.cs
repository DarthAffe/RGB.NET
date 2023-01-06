// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.Bitwizard;

// ReSharper disable once InconsistentNaming
/// <inheritdoc />
/// <summary>
/// Represents a definition of an bitwizard WS2812 devices.
/// </summary>
public class BitwizardWS281XDeviceDefinition : IWS281XDeviceDefinition
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
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets a list of LED-strips configured on this device.
    /// </summary>
    public List<(int pin, int stripLength)> Strips { get; } = new();

    /// <summary>
    /// Gets or sets the amount of leds controlled by one pin.
    /// This only needs to be changed if the firmware on the controller is updated.
    /// </summary>
    public int MaxStripLength { get; set; } = 384;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BitwizardWS281XDeviceDefinition"/> class.
    /// </summary>
    /// <param name="serialConnection">The serial connection used for the device.</param>
    /// <param name="strips">A list of LED-strips connected to this device.</param>
    public BitwizardWS281XDeviceDefinition(ISerialConnection serialConnection, params (int pin, int stripLength)[] strips)
    {
        this.SerialConnection = serialConnection;

        Strips.AddRange(strips);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BitwizardWS281XDeviceDefinition"/> class.
    /// </summary>
    /// <param name="port">The name of the serial-port to connect to.</param>
    /// <param name="baudRate">The baud-rate of the serial-connection.</param>
    /// <param name="strips">A list of LED-strips connected to this device.</param>
    public BitwizardWS281XDeviceDefinition(string port, int baudRate = 115200, params (int pin, int stripLength)[] strips)
    {
        SerialConnection = new SerialPortConnection(port, baudRate);

        Strips.AddRange(strips);
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public IEnumerable<IRGBDevice> CreateDevices(IDeviceUpdateTrigger updateTrigger)
    {
        foreach ((int pin, int stripLength) in Strips)
        {
            BitwizardWS2812USBUpdateQueue queue = new(updateTrigger, SerialConnection);
            string name = Name ?? $"Bitwizard WS2812 USB ({Port}) Pin {pin}";
            int ledOffset = pin * MaxStripLength;
            yield return new BitwizardWS2812USBDevice(new BitwizardWS2812USBDeviceInfo(name), queue, ledOffset, stripLength);
        }
    }

    #endregion
}