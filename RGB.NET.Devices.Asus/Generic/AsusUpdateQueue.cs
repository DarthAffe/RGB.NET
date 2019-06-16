using System;
using System.Collections.Generic;
using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the update-queue performing updates for asus devices.
    /// </summary>
    public class AsusUpdateQueue : UpdateQueue
    {
        #region Properties & Fields

        /// <summary>
        /// The device to be updated.
        /// </summary>
        protected IAuraSyncDevice Device { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsusUpdateQueue"/> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        public AsusUpdateQueue(IDeviceUpdateTrigger updateTrigger)
            : base(updateTrigger)
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the queue.
        /// </summary>
        /// <param name="device">The device to be updated.</param>
        public void Initialize(IAuraSyncDevice device)
        {
            Device = device;
        }

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            try
            {
                foreach (KeyValuePair<object, Color> data in dataSet)
                {
                    int index = (int)data.Key;
                    IAuraRgbLight light = Device.Lights[index];
                    (_, byte r, byte g, byte b) = data.Value.GetRGBBytes();
                    light.Red = r;
                    light.Green = g;
                    light.Blue = b;
                }

                Device.Apply();
            }
            catch (Exception ex)
            { /* "The server threw an exception." seems to be a thing here ... */ }
        }

        #endregion
    }
}
