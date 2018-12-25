using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.Bitwizard
{
    // ReSharper disable once InconsistentNaming
    /// <inheritdoc />
    /// <summary>
    /// Represents the update-queue performing updates for a bitwizard WS2812 device.
    /// </summary>
    public class BitwizardWS2812USBUpdateQueue : SerialPortUpdateQueue<string>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.WS281X.Bitwizard.BitwizardWS2812USBUpdateQueue" /> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        /// <param name="portName">The name of the serial-port to connect to.</param>
        /// <param name="baudRate">The baud-rate used by the serial-connection.</param>
        public BitwizardWS2812USBUpdateQueue(IDeviceUpdateTrigger updateTrigger, string portName, int baudRate = 115200)
            : base(updateTrigger, portName, baudRate)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnStartup(object sender, CustomUpdateData customData)
        {
            base.OnStartup(sender, customData);

            SendCommand(string.Empty); // Get initial prompt
        }

        /// <inheritdoc />
        protected override IEnumerable<string> GetCommands(Dictionary<object, Color> dataSet)
        {
            foreach (KeyValuePair<object, Color> data in dataSet)
                yield return $"pix {(int)data.Key} {data.Value.AsRGBHexString()}";
        }

        #endregion
    }
}
