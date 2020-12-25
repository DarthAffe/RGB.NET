// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.Bitwizard
{
    // ReSharper disable once InconsistentNaming
    /// <inheritdoc />
    /// <summary>
    /// Represents an bitwizard WS2812 USB device.
    /// </summary>
    public class BitwizardWS2812USBDevice : AbstractRGBDevice<BitwizardWS2812USBDeviceInfo>, ILedStripe
    {
        #region Properties & Fields

        private readonly int _ledOffset;

        /// <summary>
        /// Gets the update queue performing updates for this device.
        /// </summary>
        public BitwizardWS2812USBUpdateQueue UpdateQueue { get; }

        /// <inheritdoc />
        public override BitwizardWS2812USBDeviceInfo DeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BitwizardWS2812USBDevice"/> class.
        /// </summary>
        /// <param name="deviceInfo">The update trigger used by this queue.</param>
        /// <param name="updateQueue">The update queue performing updates for this device.</param>
        public BitwizardWS2812USBDevice(BitwizardWS2812USBDeviceInfo deviceInfo, BitwizardWS2812USBUpdateQueue updateQueue, int ledOffset)
        {
            this.DeviceInfo = deviceInfo;
            this.UpdateQueue = updateQueue;

            this._ledOffset = ledOffset;
        }

        #endregion

        #region Methods

        internal void Initialize(int ledCount)
        {
            for (int i = 0; i < ledCount; i++)
                InitializeLed(LedId.LedStripe1 + i, new Rectangle(i * 10, 0, 10, 10));

            //TODO DarthAffe 23.12.2018: Allow to load a layout.

            if (Size == Size.Invalid)
            {
                Rectangle ledRectangle = new Rectangle(this.Select(x => x.LedRectangle));
                Size = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
            }
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => _ledOffset + ((int)ledId - (int)LedId.LedStripe1);

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(ledsToUpdate.Where(x => x.Color.A > 0));

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
