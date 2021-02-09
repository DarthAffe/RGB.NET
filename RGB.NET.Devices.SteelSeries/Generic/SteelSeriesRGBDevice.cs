using System.Collections.Generic;
using System.Linq;
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

        private Dictionary<LedId, SteelSeriesLedId> _ledMapping = new();

        /// <inheritdoc />
        /// <summary>
        /// Gets information about the <see cref="SteelSeriesRGBDevice" />.
        /// </summary>
        public override SteelSeriesRGBDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets or sets the update queue performing updates for this device.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected UpdateQueue? UpdateQueue { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SteelSeriesRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by SteelSeries for the device.</param>
        internal SteelSeriesRGBDevice(SteelSeriesRGBDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the device.
        /// </summary>
        void ISteelSeriesRGBDevice.Initialize(UpdateQueue updateQueue, Dictionary<LedId, SteelSeriesLedId> ledMapping)
        {
            _ledMapping = ledMapping;

            int counter = 0;
            foreach (KeyValuePair<LedId, SteelSeriesLedId> mapping in ledMapping)
                AddLed(mapping.Key, new Point((counter++) * 10, 0), new Size(10, 10));
            
            UpdateQueue = updateQueue;
        }

        /// <inheritdoc />
        protected override object GetLedCustomData(LedId ledId) => _ledMapping[ledId];

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue?.SetData(ledsToUpdate.Where(x => x.Color.A > 0));

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
