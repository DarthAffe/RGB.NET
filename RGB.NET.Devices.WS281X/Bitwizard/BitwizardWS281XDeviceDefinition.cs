// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.Bitwizard
{
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
        /// Gets or sets the pin sed to control the leds.
        /// </summary>
        public int Pin { get; set; } = 0;

        /// <summary>
        /// Gets or sets the amount of leds of this device.
        /// </summary>
        public int StripLength { get; set; } = 384;

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
        public BitwizardWS281XDeviceDefinition(ISerialConnection serialConnection)
        {
            this.SerialConnection = serialConnection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitwizardWS281XDeviceDefinition"/> class.
        /// </summary>
        /// <param name="port">The name of the serial-port to connect to.</param>
        /// <param name="baudRate">The baud-rate of the serial-connection.</param>
        public BitwizardWS281XDeviceDefinition(string port, int baudRate = 115200)
        {
            SerialConnection = new SerialPortConnection(port, baudRate);
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> CreateDevices(IDeviceUpdateTrigger updateTrigger)
        {
            BitwizardWS2812USBUpdateQueue queue = new(updateTrigger, SerialConnection);
            string name = Name ?? $"Bitwizard WS2812 USB ({Port})";
            int ledOffset = Pin * MaxStripLength;
            BitwizardWS2812USBDevice device = new(new BitwizardWS2812USBDeviceInfo(name), queue, ledOffset);
            device.Initialize(StripLength);
            yield return device;
        }

        #endregion
    }
}
