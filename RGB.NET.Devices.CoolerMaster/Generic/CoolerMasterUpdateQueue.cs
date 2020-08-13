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
        private readonly _CoolerMasterColorMatrix _deviceMatrix;

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
            
            _deviceMatrix = new _CoolerMasterColorMatrix();
            _deviceMatrix.KeyColor = new _CoolerMasterKeyColor[_CoolerMasterColorMatrix.ROWS, _CoolerMasterColorMatrix.COLUMNS];
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            foreach (KeyValuePair<object, Color> data in dataSet)
            {
                (int row, int column) = ((int, int))data.Key;
                _deviceMatrix.KeyColor[row, column] = new _CoolerMasterKeyColor(data.Value.GetR(), data.Value.GetG(), data.Value.GetB());
            }

            _CoolerMasterSDK.SetAllLedColor(_deviceMatrix, _deviceIndex);
        }

        #endregion
    }
}
