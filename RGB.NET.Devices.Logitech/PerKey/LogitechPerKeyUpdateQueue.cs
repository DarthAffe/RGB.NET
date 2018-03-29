using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.Logitech.Native;

namespace RGB.NET.Devices.Logitech
{
    public class LogitechPerKeyUpdateQueue : UpdateQueue
    {
        #region Properties & Fields

        private readonly byte[] _bitmap;

        #endregion

        #region Constructors

        public LogitechPerKeyUpdateQueue(IUpdateTrigger updateTrigger)
            : base(updateTrigger)
        {
            _bitmap = BitmapMapping.CreateBitmap();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            _LogitechGSDK.LogiLedSetTargetDevice(LogitechDeviceCaps.PerKeyRGB);

            Array.Clear(_bitmap, 0, _bitmap.Length);
            bool usesBitmap = false;
            foreach (KeyValuePair<object, Color> data in dataSet)
            {
                (LedId id, LogitechLedId customData) = ((LedId, LogitechLedId))data.Key;

                // DarthAffe 26.03.2017: This is only needed since update by name doesn't work as expected for all keys ...
                if (BitmapMapping.BitmapOffset.TryGetValue(id, out int bitmapOffset))
                {
                    BitmapMapping.SetColor(_bitmap, bitmapOffset, data.Value);
                    usesBitmap = true;
                }
                else
                    _LogitechGSDK.LogiLedSetLightingForKeyWithKeyName((int)customData,
                                                                      (int)Math.Round(data.Value.RPercent * 100),
                                                                      (int)Math.Round(data.Value.GPercent * 100),
                                                                      (int)Math.Round(data.Value.BPercent * 100));
            }

            if (usesBitmap)
                _LogitechGSDK.LogiLedSetLightingFromBitmap(_bitmap);
        }

        #endregion
    }
}
