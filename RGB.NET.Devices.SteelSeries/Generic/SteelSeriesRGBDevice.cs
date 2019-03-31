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
    public class SteelSeriesRGBDevice : AbstractRGBDevice<SteelSeriesRGBDeviceInfo>, ISteelSeriesRGBDevice
    {
        #region Properties & Fields

        private Dictionary<LedId, SteelSeriesLedId> _ledMapping;

        /// <inheritdoc />
        /// <summary>
        /// Gets information about the <see cref="SteelSeriesRGBDevice" />.
        /// </summary>
        public override SteelSeriesRGBDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets or sets the update queue performing updates for this device.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected UpdateQueue UpdateQueue { get; set; }

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
                InitializeLed(mapping.Key, new Rectangle((counter++) * 10, 0, 10, 10));

            InitializeLayout();

            if (Size == Size.Invalid)
            {
                Rectangle ledRectangle = new Rectangle(this.Select(x => x.LedRectangle));
                Size = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
            }

            UpdateQueue = updateQueue;
        }

        protected override object CreateLedCustomData(LedId ledId) => _ledMapping[ledId];

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(ledsToUpdate.Where(x => x.Color.A > 0));

        /// <summary>
        /// Initializes the <see cref="Led"/> and <see cref="Size"/> of the device.
        /// </summary>
        protected virtual void InitializeLayout()
        {
            if (!(DeviceInfo is SteelSeriesRGBDeviceInfo info)) return;

            string layout = info.ImageLayout;
            string layoutPath = info.LayoutPath;
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\SteelSeries\{layoutPath}.xml"), layout, true);
        }

        #endregion
    }
}
