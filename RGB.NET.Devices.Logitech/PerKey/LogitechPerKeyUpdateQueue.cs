using System;
using RGB.NET.Core;
using RGB.NET.Devices.Logitech.Native;

namespace RGB.NET.Devices.Logitech
{
    /// <summary>
    /// Represents the update-queue performing updates for logitech per-key devices.
    /// </summary>
    public class LogitechPerKeyUpdateQueue : UpdateQueue
    {
        #region Properties & Fields

        private readonly byte[] _bitmap;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogitechPerKeyUpdateQueue"/> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        public LogitechPerKeyUpdateQueue(IDeviceUpdateTrigger updateTrigger)
            : base(updateTrigger)
        {
            _bitmap = BitmapMapping.CreateBitmap();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
        {
            _LogitechGSDK.LogiLedSetTargetDevice(LogitechDeviceCaps.PerKeyRGB);

            Array.Clear(_bitmap, 0, _bitmap.Length);
            bool usesBitmap = false;
            foreach ((object key, Color color) in dataSet)
            {
                (LedId id, LogitechLedId customData) = ((LedId, LogitechLedId))key;

                // DarthAffe 26.03.2017: This is only needed since update by name doesn't work as expected for all keys ...
                if (BitmapMapping.BitmapOffset.TryGetValue(id, out int bitmapOffset))
                {
                    BitmapMapping.SetColor(_bitmap, bitmapOffset, color);
                    usesBitmap = true;
                }
                else
                    _LogitechGSDK.LogiLedSetLightingForKeyWithKeyName((int)customData,
                                                                      (int)MathF.Round(color.R * 100),
                                                                      (int)MathF.Round(color.G * 100),
                                                                      (int)MathF.Round(color.B * 100));
            }

            if (usesBitmap)
                _LogitechGSDK.LogiLedSetLightingFromBitmap(_bitmap);
        }

        #endregion
    }
}
