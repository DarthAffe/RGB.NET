// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X.NodeMCU
{
    // ReSharper disable once InconsistentNaming
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents an NodeMCU WS2812 device.
    /// </summary>
    public class NodeMCUWS2812USBDevice : AbstractRGBDevice<NodeMCUWS2812USBDeviceInfo>, ILedStripe
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the update queue performing updates for this device.
        /// </summary>
        public NodeMCUWS2812USBUpdateQueue UpdateQueue { get; }

        /// <inheritdoc />
        public override NodeMCUWS2812USBDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets the channel (as defined in the NodeMCU-sketch) this device is attached to.
        /// </summary>
        public int Channel { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeMCUWS2812USBDevice"/> class.
        /// </summary>
        /// <param name="deviceInfo">The update trigger used by this queue.</param>
        /// <param name="updateQueue">The update queue performing updates for this device.</param>
        /// <param name="channel">The channel (as defined in the NodeMCU-sketch) this device is attached to.</param>
        public NodeMCUWS2812USBDevice(NodeMCUWS2812USBDeviceInfo deviceInfo, NodeMCUWS2812USBUpdateQueue updateQueue, int channel)
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
                AddLed(LedId.LedStripe1 + i, new Point(i * 10, 0), new Size(10, 10));

            if (Size == Size.Invalid)
            {
                Rectangle ledRectangle = new(this.Select(x => x.LedRectangle));
                Size = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
            }
        }

        /// <inheritdoc />
        protected override object GetLedCustomData(LedId ledId) => (Channel, (int)ledId - (int)LedId.LedStripe1);

        /// <inheritdoc />
        protected override IEnumerable<Led> GetLedsToUpdate(bool flushLeds) => (flushLeds || LedMapping.Values.Any(x => x.IsDirty)) ? LedMapping.Values : Enumerable.Empty<Led>();

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
