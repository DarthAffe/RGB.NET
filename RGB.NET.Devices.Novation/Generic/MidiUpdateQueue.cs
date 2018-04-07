using System;
using System.Collections.Generic;
using RGB.NET.Core;
using Sanford.Multimedia.Midi;

namespace RGB.NET.Devices.Novation
{
    public abstract class MidiUpdateQueue : UpdateQueue, IDisposable
    {
        #region Properties & Fields

        private readonly OutputDevice _outputDevice;

        #endregion

        #region Constructors

        public MidiUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceId)
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

        protected virtual void SendMessage(ShortMessage message) => _outputDevice.SendShort(message.Message);

        protected abstract ShortMessage CreateMessage(KeyValuePair<object, Color> data);

        public void Dispose()
        {
            _outputDevice.Dispose();
        }

        #endregion
    }
}
