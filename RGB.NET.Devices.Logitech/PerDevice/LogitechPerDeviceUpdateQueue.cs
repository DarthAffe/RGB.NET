using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.Logitech.Native;

namespace RGB.NET.Devices.Logitech
{
    public class LogitechPerDeviceUpdateQueue : UpdateQueue
    {
        #region Constructors

        public LogitechPerDeviceUpdateQueue(IDeviceUpdateTrigger updateTrigger)
            : base(updateTrigger)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            Color color = dataSet.Values.First();

            _LogitechGSDK.LogiLedSetTargetDevice(LogitechDeviceCaps.DeviceRGB);
            _LogitechGSDK.LogiLedSetLighting((int)Math.Round(color.RPercent * 100),
                                             (int)Math.Round(color.GPercent * 100),
                                             (int)Math.Round(color.BPercent * 100));
        }

        #endregion
    }
}
