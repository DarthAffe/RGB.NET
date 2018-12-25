// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.Arduino
{
    // ReSharper disable once InconsistentNaming
    /// <inheritdoc />
    /// <summary>
    /// Represents a definition of an arduino WS2812 devices.
    /// </summary>
    public class ArduinoWS281XDeviceDefinition : IWS281XDeviceDefinition
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the name of the serial-port to connect to.
        /// </summary>
        public string Port { get; }

        /// <summary>
        /// Gets the baud-rate used by the serial-connection.
        /// </summary>
        public int BaudRate { get; set; } = 115200;

        /// <summary>
        /// Gets or sets the name used by this device.
        /// This allows to use {0} as a placeholder for a incrementing number if multiple devices are created.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArduinoWS281XDeviceDefinition"/> class.
        /// </summary>
        /// <param name="portName">The name of the serial-port to connect to.</param>
        public ArduinoWS281XDeviceDefinition(string port)
        {
            this.Port = port;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> CreateDevices()
        {
            DeviceUpdateTrigger updateTrigger = new DeviceUpdateTrigger();

            ArduinoWS2812USBUpdateQueue queue = new ArduinoWS2812USBUpdateQueue(updateTrigger, Port, BaudRate);
            IEnumerable<(int channel, int ledCount)> channels = queue.GetChannels();
            int counter = 0;
            foreach ((int channel, int ledCount) in channels)
            {
                string name = string.Format(Name ?? $"Arduino WS2812 USB ({Port}) [{{0}}]", ++counter);
                ArduinoWS2812USBDevice device = new ArduinoWS2812USBDevice(new ArduinoWS2812USBDeviceInfo(name), queue, channel);
                device.Initialize(ledCount);
                yield return device;
            }

            updateTrigger.Start();
        }

        #endregion
    }
}
