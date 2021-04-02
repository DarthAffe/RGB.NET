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
            foreach ((object key, Color color) in dataSet)
            {
                int offset = key as int? ?? -1;
                if (offset >= 0)
                    BitmapMapping.SetColor(_bitmap, offset, color);
            }

            _LogitechGSDK.LogiLedSetLightingFromBitmap(_bitmap);
        }

        #endregion
    }
}
