using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.CoolerMaster.Native;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the update-queue performing updates for cooler master devices.
    /// </summary>
    public class CoolerMasterUpdateQueue : UpdateQueue
    {
        #region Properties & Fields

        private CoolerMasterDevicesIndexes _deviceIndex;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoolerMasterUpdateQueue"/> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        /// <param name="deviceIndex">The <see cref="CoolerMasterDevicesIndexes"/> of the device this queue performs updates for.</param>
        public CoolerMasterUpdateQueue(IDeviceUpdateTrigger updateTrigger, CoolerMasterDevicesIndexes deviceIndex)
            : base(updateTrigger)
        {
            this._deviceIndex = deviceIndex;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            _CoolerMasterSDK.SetControlDevice(_deviceIndex);

            foreach (KeyValuePair<object, Color> data in dataSet)
            {
                (int row, int column) = ((int, int))data.Key;
                _CoolerMasterSDK.SetLedColor(row, column, data.Value.R, data.Value.G, data.Value.B);
            }

            _CoolerMasterSDK.RefreshLed(false);
        }

        #endregion
    }
}
