using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.SoIP.Generic;

namespace RGB.NET.Devices.SoIP.Server
{
    public class SoIPServerDeviceDefinition : ISoIPDeviceDefinition
    {
        #region Properties & Fields

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

        /// <summary>
        /// Gets the IDs of the leds represented by this device.
        /// </summary>
        public List<LedId> Leds { get; }

        #endregion

        #region Constructors

        public SoIPServerDeviceDefinition(int port, params LedId[] ledIds)
        {
            this.Port = port;
            this.Leds = ledIds.ToList();
        }

        #endregion
    }
}
