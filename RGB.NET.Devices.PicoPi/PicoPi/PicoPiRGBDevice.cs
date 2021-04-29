using RGB.NET.Core;

namespace RGB.NET.Devices.PicoPi
{
    public class PicoPiRGBDevice : AbstractRGBDevice<PicoPiRGBDeviceInfo>
    {
        #region Properties & Fields

        private readonly LedMapping<int> _ledMapping;

        #endregion

        #region Constructors

        public PicoPiRGBDevice(PicoPiRGBDeviceInfo deviceInfo, IUpdateQueue updateQueue, LedMapping<int> ledMapping)
            : base(deviceInfo, updateQueue)
        {
            this._ledMapping = ledMapping;
        }

        #endregion

        #region Methods

        internal void Initialize()
        {
            for (int i = 0; i < DeviceInfo.LedCount; i++)
                AddLed(_ledMapping[i], new Point(i * 10, 0), new Size(10, 10), i);
        }

        /// <inheritdoc />
        protected override object GetLedCustomData(LedId ledId) => _ledMapping.TryGetValue(ledId, out int index) ? index : -1;

        #endregion
    }
}
