using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using SimpleTCP;

namespace RGB.NET.Devices.SoIP.Server
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the update-queue performing updates for E131-DMX devices.
    /// </summary>
    public class SoIPServerUpdateQueue : UpdateQueue
    {
        #region Properties & Fields

        private readonly SimpleTcpServer _tcpServer;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.SoIP.Server.SoIPServerUpdateQueue" /> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        /// <param name="tcpServer">The hostname of the device this queue is performing updates for.</param>
        public SoIPServerUpdateQueue(IDeviceUpdateTrigger updateTrigger, SimpleTcpServer tcpServer)
            : base(updateTrigger)
        {
            this._tcpServer = tcpServer;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            if ((dataSet != null) && (dataSet.Count > 0))
            {
                string m = GetLedString(dataSet);
                _tcpServer.BroadcastLine(m);
            }
        }

        private string GetLedString(Dictionary<object, Color> dataSet) => string.Join(";", dataSet.Select(x => x.Key.ToString() + "|" + x.Value.AsARGBHexString()));

        #endregion
    }
}
