using System;
using System.Collections.Generic;
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
        /// Gets or sets the internal color-data cache.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected byte[] ColorData { get; private set; }

        private Action<IntPtr, byte[]> _updateAction;
        private IntPtr _handle;

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
        /// <param name="updateAction">The update-action called by the queue to perform updates.</param>
        /// <param name="handle">The handle of the device this queue performs updates for.</param>
        /// <param name="ledCount">The amount of leds of the device this queue performs updates for.</param>
        public void Initialize(Action<IntPtr, byte[]> updateAction, IntPtr handle, int ledCount)
        {
            _updateAction = updateAction;
            _handle = handle;

            ColorData = new byte[ledCount * 3];
        }

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            foreach (KeyValuePair<object, Color> data in dataSet)
            {
                int index = ((int)data.Key) * 3;
                ColorData[index] = data.Value.GetR();
                ColorData[index + 1] = data.Value.GetB();
                ColorData[index + 2] = data.Value.GetG();
            }

            _updateAction(_handle, ColorData);
        }

        #endregion
    }
}
