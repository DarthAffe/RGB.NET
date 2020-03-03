using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <inheritdoc cref="IRazerRGBDevice" />
    /// <summary>
    /// Represents a generic razer-device. (keyboard, mouse, headset, mousepad).
    /// </summary>
    public abstract class RazerRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IRazerRGBDevice
        where TDeviceInfo : RazerRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        /// <summary>
        /// Gets information about the <see cref="T:RGB.NET.Devices.Razer.RazerRGBDevice" />.
        /// </summary>
        public override TDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets or sets the update queue performing updates for this device.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected RazerUpdateQueue UpdateQueue { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RazerRGBDevice{TDeviceInfo}"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by razer for the device.</param>
        protected RazerRGBDevice(TDeviceInfo info)
        {
            this.DeviceInfo = info;

            RequiresFlush = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the device.
        /// </summary>
        public void Initialize(IDeviceUpdateTrigger updateTrigger)
        {
            InitializeLayout();

            if (Size == Size.Invalid)
            {
                Rectangle ledRectangle = new Rectangle(this.Select(x => x.LedRectangle));
                Size = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
            }

            UpdateQueue = CreateUpdateQueue(updateTrigger);
        }

        /// <summary>
        /// Creates a specific <see cref="RazerUpdateQueue"/> for this device.
        /// </summary>
        /// <param name="updateTrigger">The trigger used to update the queue.</param>
        /// <returns>The <see cref="RazerUpdateQueue"/> for this device.</returns>
        protected abstract RazerUpdateQueue CreateUpdateQueue(IDeviceUpdateTrigger updateTrigger);

        /// <summary>
        /// Initializes the <see cref="Led"/> and <see cref="Size"/> of the device.
        /// </summary>
        protected abstract void InitializeLayout();

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(ledsToUpdate.Where(x => x.Color.A > 0));

        /// <summary>
        /// Resets the device.
        /// </summary>
        public void Reset() => UpdateQueue.Reset();

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
