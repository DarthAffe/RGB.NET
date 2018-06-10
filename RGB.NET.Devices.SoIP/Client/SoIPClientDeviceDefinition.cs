using RGB.NET.Devices.SoIP.Generic;

namespace RGB.NET.Devices.SoIP.Client
{
    public class SoIPClientDeviceDefinition : ISoIPDeviceDefinition
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the hostname of the device.
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the port to device is listening to.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer of the device.
        /// </summary>
        public string Manufacturer { get; set; } = "Unknown";

        /// <summary>
        /// Gets or sets the model name of the device.
        /// </summary>
        public string Model { get; set; } = "Generic SoIP-Device";

        #endregion

        #region Constructors

        public SoIPClientDeviceDefinition(string hostname, int port)
        {
            this.Hostname = hostname;
            this.Port = port;
        }

        #endregion
    }
}
