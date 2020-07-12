// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.NodeMCU
{
    // ReSharper disable once InconsistentNaming
    /// <inheritdoc />
    /// <summary>
    /// Represents a definition of an NodeMCU WS2812 devices.
    /// </summary>
    public class NodeMCUWS281XDeviceDefinition : IWS281XDeviceDefinition
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the hostname to connect to.
        /// </summary>
        public string Hostname { get; }

        /// <summary>
        /// Gets the port of the UDP connection.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets or sets the name used by this device.
        /// This allows to use {0} as a placeholder for a incrementing number if multiple devices are created.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeMCUWS281XDeviceDefinition"/> class.
        /// </summary>
        /// <param name="hostname">The hostname to connect to.</param>
        /// <param name="port">The port of the UDP connection.</param>
        public NodeMCUWS281XDeviceDefinition(string hostname, int port = 1872)
        {
            this.Hostname = hostname;
            this.Port = port;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> CreateDevices(IDeviceUpdateTrigger updateTrigger)
        {
            NodeMCUWS2812USBUpdateQueue queue = new NodeMCUWS2812USBUpdateQueue(updateTrigger, Hostname, Port);
            IEnumerable<(int channel, int ledCount)> channels = queue.GetChannels();
            int counter = 0;
            foreach ((int channel, int ledCount) in channels)
            {
                string name = string.Format(Name ?? $"NodeMCU WS2812 WIFI ({Hostname}) [{{0}}]", ++counter);
                NodeMCUWS2812USBDevice device = new NodeMCUWS2812USBDevice(new NodeMCUWS2812USBDeviceInfo(name), queue, channel);
                device.Initialize(ledCount);
                yield return device;
            }
        }

        #endregion
    }
}
