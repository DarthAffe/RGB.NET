using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.Logitech.Native;

namespace RGB.NET.Devices.Logitech
{
    public class LogitechPerKeyUpdateQueue : UpdateQueue
    {
        #region Constructors

        public LogitechPerKeyUpdateQueue(IUpdateTrigger updateTrigger)
            : base(updateTrigger)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            _LogitechGSDK.LogiLedSetTargetDevice(LogitechDeviceCaps.PerKeyRGB);

            byte[] bitmap = null;
            foreach (KeyValuePair<object, Color> data in dataSet)
            {
                (LedId id, int customData) = ((LedId, int))data.Key;

                // DarthAffe 26.03.2017: This is only needed since update by name doesn't work as expected for all keys ...
                if (BitmapMapping.BitmapOffset.TryGetValue(id, out int bitmapOffset))
                {
                    if (bitmap == null)
                        bitmap = BitmapMapping.CreateBitmap();

                    BitmapMapping.SetColor(ref bitmap, bitmapOffset, data.Value);
                }
                else
                    _LogitechGSDK.LogiLedSetLightingForKeyWithKeyName(customData,
                                                                      (int)Math.Round(data.Value.RPercent * 100),
                                                                      (int)Math.Round(data.Value.GPercent * 100),
                                                                      (int)Math.Round(data.Value.BPercent * 100));
            }

            if (bitmap != null)
                _LogitechGSDK.LogiLedSetLightingFromBitmap(bitmap);
        }

        #endregion
    }
}
