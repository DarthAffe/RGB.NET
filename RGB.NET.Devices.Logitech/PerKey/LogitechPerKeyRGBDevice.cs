using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.Logitech.Native;

namespace RGB.NET.Devices.Logitech
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a logitech per-key-lightable device.
    /// </summary>
    public class LogitechPerKeyRGBDevice : LogitechRGBDevice<LogitechRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Logitech.LogitechPerKeyRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by logitech for the per-key-lightable device</param>
        internal LogitechPerKeyRGBDevice(LogitechRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => PerKeyIdMapping.DEFAULT.TryGetValue(ledId, out LogitechLedId logitechLedId) ? logitechLedId : LogitechLedId.Invalid;

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate)
        {
            List<Led> leds = ledsToUpdate.Where(x => x.Color.A > 0).ToList();
            if (leds.Count <= 0) return;

            _LogitechGSDK.LogiLedSetTargetDevice(LogitechDeviceCaps.PerKeyRGB);

            byte[] bitmap = null;
            foreach (Led led in leds)
            {
                // DarthAffe 26.03.2017: This is only needed since update by name doesn't work as expected for all keys ...
                if (BitmapMapping.BitmapOffset.TryGetValue(led.Id, out int bitmapOffset))
                {
                    if (bitmap == null)
                        bitmap = BitmapMapping.CreateBitmap();

                    BitmapMapping.SetColor(ref bitmap, bitmapOffset, led.Color);
                }
                else
                    _LogitechGSDK.LogiLedSetLightingForKeyWithKeyName((int)led.CustomData,
                                                                      (int)Math.Round(led.Color.RPercent * 100),
                                                                      (int)Math.Round(led.Color.GPercent * 100),
                                                                      (int)Math.Round(led.Color.BPercent * 100));
            }

            if (bitmap != null)
                _LogitechGSDK.LogiLedSetLightingFromBitmap(bitmap);
        }

        #endregion
    }
}
