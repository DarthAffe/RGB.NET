// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.Arduino
{
    // ReSharper disable once InconsistentNaming
    /// <inheritdoc />
    /// <summary>
    /// Represents an arduino WS2812 device.
    /// </summary>
    public class ArduinoWS2812USBDevice : AbstractRGBDevice<ArduinoWS2812USBDeviceInfo>
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the update queue performing updates for this device.
        /// </summary>
        public ArduinoWS2812USBUpdateQueue UpdateQueue { get; }

        /// <inheritdoc />
        public override ArduinoWS2812USBDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets the channel (as defined in the arduino-sketch) this device is attached to.
        /// </summary>
        public int Channel { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArduinoWS2812USBDevice"/> class.
        /// </summary>
        /// <param name="deviceInfo">The update trigger used by this queue.</param>
        /// <param name="updateQueue">The update queue performing updates for this device.</param>
        /// <param name="channel">The channel (as defined in the arduino-sketch) this device is attached to.</param>
        public ArduinoWS2812USBDevice(ArduinoWS2812USBDeviceInfo deviceInfo, ArduinoWS2812USBUpdateQueue updateQueue, int channel)
        {
            this.DeviceInfo = deviceInfo;
            this.UpdateQueue = updateQueue;
            this.Channel = channel;
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
        protected override object CreateLedCustomData(LedId ledId) => (Channel, (int)ledId - (int)LedId.LedStripe1);

        /// <inheritdoc />
        protected override IEnumerable<Led> GetLedsToUpdate(bool flushLeds) => (flushLeds || LedMapping.Values.Any(x => x.IsDirty)) ? LedMapping.Values : null;

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(ledsToUpdate.Where(x => x.Color.A > 0));

        #endregion
    }
}
