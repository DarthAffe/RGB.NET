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
        /// Gets the name of the serial-port to connect to.
        /// </summary>
        public string Port { get; }

        /// <summary>
        /// Gets the baud-rate used by the serial-connection.
        /// </summary>
        public int BaudRate { get; set; } = 115200;

        /// <summary>
        /// Gets or sets the name used by this device.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the amount of leds of this device.
        /// </summary>
        public int StripLength { get; set; } = 384;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BitwizardWS281XDeviceDefinition"/> class.
        /// </summary>
        /// <param name="portName">The name of the serial-port to connect to.</param>
        public BitwizardWS281XDeviceDefinition(string port)
        {
            this.Port = port;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> CreateDevices()
        {
            DeviceUpdateTrigger updateTrigger = new DeviceUpdateTrigger();

            BitwizardWS2812USBUpdateQueue queue = new BitwizardWS2812USBUpdateQueue(updateTrigger, Port, BaudRate);
            string name = Name ?? $"Bitwizard WS2812 USB ({Port})";
            BitwizardWS2812USBDevice device = new BitwizardWS2812USBDevice(new BitwizardWS2812USBDeviceInfo(name), queue);
            device.Initialize(StripLength);
            yield return device;

            updateTrigger.Start();
        }

        #endregion
    }
}
