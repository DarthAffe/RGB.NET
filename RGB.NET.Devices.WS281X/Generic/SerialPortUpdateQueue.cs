using System.Collections.Generic;
using System.IO.Ports;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a update queue for serial devices.
    /// </summary>
    /// <typeparam name="TData">The type of data sent through the serial connection.</typeparam>
    public abstract class SerialPortUpdateQueue<TData> : UpdateQueue
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the prompt to wait for between sending commands.
        /// </summary>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        protected string Prompt { get; set; } = ">";

        /// <summary>
        /// Gets the serial port used by this queue.
        /// </summary>
        protected SerialPort SerialPort { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="SerialPortUpdateQueue{TData}"/> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        /// <param name="portName">The name of the serial-port to connect to.</param>
        /// <param name="baudRate">The baud-rate used by the serial-connection.</param>
        internal SerialPortUpdateQueue(IDeviceUpdateTrigger updateTrigger, string portName, int baudRate = 115200)
            : base(updateTrigger)
        {
            SerialPort = new SerialPort(portName, baudRate);
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnStartup(object sender, CustomUpdateData customData)
        {
            base.OnStartup(sender, customData);

            if (!SerialPort.IsOpen)
                SerialPort.Open();

            SerialPort.DiscardInBuffer();
        }

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            foreach (TData command in GetCommands(dataSet))
            {
                SerialPort.ReadTo(Prompt);
                SendCommand(command);
            }
        }

        /// <summary>
        /// Gets the commands that need to be sent to the device to update the requested colors.
        /// </summary>
        /// <param name="dataSet">The set of data that needs to be updated.</param>
        /// <returns>The commands to be sent.</returns>
        protected abstract IEnumerable<TData> GetCommands(Dictionary<object, Color> dataSet);

        /// <summary>
        /// Sends a command as a string followed by a line-break to the device.
        /// This most likely needs to be overwritten if the data-type isn't string.  
        /// </summary>
        /// <param name="command">The command to be sent.</param>
        protected virtual void SendCommand(TData command) => SerialPort.WriteLine((command as string) ?? string.Empty);

        #endregion
    }
}
