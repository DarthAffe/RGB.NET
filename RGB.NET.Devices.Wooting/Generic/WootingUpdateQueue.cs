using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Native;

namespace RGB.NET.Devices.Wooting.Generic
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the update-queue performing updates for cooler master devices.
    /// </summary>
    public class WootingUpdateQueue : UpdateQueue
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WootingUpdateQueue"/> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        public WootingUpdateQueue(IDeviceUpdateTrigger updateTrigger)
            : base(updateTrigger)
        {
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            foreach (KeyValuePair<object, Color> data in dataSet)
            {
                (int row, int column) = ((int, int))data.Key;
                _WootingSDK.ArraySetSingle((byte)row, (byte)column, data.Value.GetR(), data.Value.GetG(), data.Value.GetB());
            }

            _WootingSDK.ArrayUpdateKeyboard();
        }

        #endregion
    }
}
