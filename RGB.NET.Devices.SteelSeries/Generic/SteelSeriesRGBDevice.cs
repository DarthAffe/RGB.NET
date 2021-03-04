using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.SteelSeries
{
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <inheritdoc cref="ISteelSeriesRGBDevice" />
    /// <summary>
    /// Represents a SteelSeries-device. (keyboard, mouse, headset, mousepad).
    /// </summary>
    public class SteelSeriesRGBDevice : AbstractRGBDevice<SteelSeriesRGBDeviceInfo>, ISteelSeriesRGBDevice, IUnknownDevice//TODO DarthAffe 18.04.2020: It's know which kind of device this is, but they would need to be separated
    {
        #region Properties & Fields

        private readonly Dictionary<LedId, SteelSeriesLedId> _ledMapping;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SteelSeriesRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by SteelSeries for the device.</param>
        internal SteelSeriesRGBDevice(SteelSeriesRGBDeviceInfo info, string apiName, Dictionary<LedId, SteelSeriesLedId> ledMapping, IDeviceUpdateTrigger updateTrigger)
            : base(info, new SteelSeriesDeviceUpdateQueue(updateTrigger, apiName))
        {
            this._ledMapping = ledMapping;

            InitializeLayout();
        }

        #endregion

        #region Methods

        private void InitializeLayout()
        {
            int counter = 0;
            foreach (KeyValuePair<LedId, SteelSeriesLedId> mapping in _ledMapping)
                AddLed(mapping.Key, new Point((counter++) * 10, 0), new Size(10, 10));
        }

        /// <inheritdoc />
        protected override object GetLedCustomData(LedId ledId) => _ledMapping[ledId];

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(GetUpdateData(ledsToUpdate));

        /// <inheritdoc />
        public override void Dispose()
        {
            try { UpdateQueue?.Dispose(); }
            catch { /* at least we tried */ }

            base.Dispose();
        }

        #endregion
    }
}
