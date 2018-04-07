using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus.Generic
{
    public class AsusUpdateQueue : UpdateQueue
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the internal color-data cache.
        /// </summary>
        protected byte[] ColorData { get; private set; }

        private Action<IntPtr, byte[]> _updateAction;
        private IntPtr _handle;

        #endregion

        #region Constructors

        public AsusUpdateQueue(IDeviceUpdateTrigger updateTrigger)
            : base(updateTrigger)
        { }

        #endregion

        #region Methods

        public void Initialize(Action<IntPtr, byte[]> updateAction, IntPtr handle, int ledCount)
        {
            _updateAction = updateAction;
            _handle = handle;

            ColorData = new byte[ledCount * 3];
        }

        protected override void Update(Dictionary<object, Color> dataSet)
        {
            foreach (KeyValuePair<object, Color> data in dataSet)
            {
                int index = ((int)data.Key) * 3;
                ColorData[index] = data.Value.R;
                ColorData[index + 1] = data.Value.B;
                ColorData[index + 2] = data.Value.G;
            }

            _updateAction(_handle, ColorData);
        }

        #endregion
    }
}
