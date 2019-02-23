using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.SteelSeries.API;

namespace RGB.NET.Devices.SteelSeries
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the update-queue performing updates for steelseries devices.
    /// </summary>
    internal class SteelSeriesDeviceUpdateQueue : UpdateQueue
    {
        #region Properties & Fields

        private string _deviceType;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SteelSeriesDeviceUpdateQueue"/> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        /// <param name="deviceType">The device type used to identify the device.</param>
        public SteelSeriesDeviceUpdateQueue(IDeviceUpdateTrigger updateTrigger, string deviceType)
            : base(updateTrigger)
        {
            this._deviceType = deviceType;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            SteelSeriesSDK.UpdateLeds(_deviceType, dataSet.ToDictionary(x => ((SteelSeriesLedId)x.Key).GetAPIName(), x => new int[] { x.Value.R, x.Value.G, x.Value.B }));
        }

        #endregion
    }
}
