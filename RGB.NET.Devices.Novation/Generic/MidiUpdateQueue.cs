using System;
using System.Collections.Generic;
using RGB.NET.Core;
using Sanford.Multimedia.Midi;

namespace RGB.NET.Devices.Novation
{
    /// <inheritdoc cref="UpdateQueue" />
    /// <inheritdoc cref="IDisposable" />
    /// <summary>
    /// Represents the update-queue performing updates for midi devices.
    /// </summary>
    public abstract class MidiUpdateQueue : UpdateQueue, IDisposable
    {
        #region Properties & Fields

        private readonly OutputDevice _outputDevice;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Novation.MidiUpdateQueue" /> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        /// <param name="deviceId">The id of the device this queue is performing updates for.</param>
        protected MidiUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceId)
            : base(updateTrigger)
        {
            _outputDevice = new OutputDevice(deviceId);
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            foreach (KeyValuePair<object, Color> data in dataSet)
                SendMessage(CreateMessage(data));
        }

        /// <summary>
        /// Sends the specified message to the device this queue is performing updates for.
        /// </summary>
        /// <param name="message">The message to send.</param>
        protected virtual void SendMessage(ShortMessage? message)
        {
            if (message != null)
                _outputDevice.SendShort(message.Message);
        }


        /// <summary>
        /// Creates a update-message out of a given data set.
        /// </summary>
        /// <param name="data">The data set to create the message from.</param>
        /// <returns>The message created out of the data set.</returns>
        protected abstract ShortMessage? CreateMessage(KeyValuePair<object, Color> data);

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            _outputDevice.Dispose();
        }

        #endregion
    }
}
